using System;
using Core.Mechanics.Shops;
using Core.Scripts.Services.UpdateService;
using Core.Storage.Bank;
using Mechanics.Income;
using UnityEngine;
using Zenject;
using static Mechanics.DayNight.DayNightPhase;

namespace Mechanics.DayNight
{
    public class GameTimeService : IGameTimeService, IUpdatable
    {
        private readonly DayNightConfig _config;
        private readonly IUpdateService _updateService;
        private readonly IBanksProvidersProvider _banksProvidersProvider;

        private float _accumulatedRealSeconds;
        private int _currentGameHour = 7;
        private DayNightPhase _currentPhase = Day;
        private int _dayNumber = 1;
        private float _balanceAtDayStart;
        private float _previousBalance;
        private float _earnedDuringDay;
        private float _spentDuringDay;
        private IGameEconomyService _gameEconomyService;
        private float _timeSpeed = 1f;

        public DayNightPhase CurrentPhase => _currentPhase;
        public float TimeSpeed { get => _timeSpeed; set => _timeSpeed = Mathf.Clamp(value, 1f, 10f); }
        public int CurrentGameHour => _currentGameHour;
        public int DayNumber => _dayNumber;

        public float ProgressInPhase
        {
            get
            {
                int hoursInPhase;
                int phaseDuration;
                if (_currentPhase == Day)
                {
                    hoursInPhase = _currentGameHour - _config.DayStartHour;
                    phaseDuration = _config.DayDurationHours;
                }
                else
                {
                    hoursInPhase = _currentGameHour >= _config.NightStartHour
                        ? _currentGameHour - _config.NightStartHour
                        : 24 - _config.NightStartHour + _currentGameHour;
                    phaseDuration = _config.NightDurationHours;
                }

                float partOfCurrentHour = _accumulatedRealSeconds / _config.SecondsPerGameHour;
                return (hoursInPhase + partOfCurrentHour) / phaseDuration;
            }
        }

        public float ProgressInDayCycle
        {
            get
            {
                float partOfCurrentHour = _accumulatedRealSeconds / _config.SecondsPerGameHour;
                return ((_currentGameHour + partOfCurrentHour) % 24f) / 24f;
            }
        }

        public event Action<DayNightPhase> OnPhaseChanged;
        public event Action<int> OnHourChanged;
        public event Action<DaySummaryData> OnDaySummaryReady;

        Core.Scripts.Services.UpdateService.UpdateType IUpdatable.UpdateType =>
            Core.Scripts.Services.UpdateService.UpdateType.Update;

        [Inject]
        public GameTimeService(DayNightConfig config, IUpdateService updateService,
            IBanksProvidersProvider banksProvidersProvider, IGameEconomyService gameEconomyService)
        {
            _gameEconomyService = gameEconomyService;
            _config = config;
            _updateService = updateService;
            _banksProvidersProvider = banksProvidersProvider;
        }

        public void Init()
        {
            _balanceAtDayStart = GetPlayerBalance();
            _previousBalance = _balanceAtDayStart;
            _gameEconomyService.OnPlayerBalanceChanged += OnBalanceChanged;
            _updateService.Add(this);
        }

        private float GetPlayerBalance()
        {
            try
            {
                var bank = _banksProvidersProvider.GetFloatBankProvider().Get(BankId.FruitsBank);
                return bank.GetValue();
            }
            catch
            {
                return 0f;
            }
        }

        private void OnBalanceChanged(float newBalance)
        {
            if (_currentPhase != Day) return;

            float delta = newBalance - _previousBalance;
            if (delta > 0)
                _earnedDuringDay += delta;
            else if (delta < 0)
                _spentDuringDay += Math.Abs(delta);
            _previousBalance = newBalance;
        }

        public void Tick(float deltaTime)
        {
            _accumulatedRealSeconds += deltaTime * _timeSpeed;

            while (_accumulatedRealSeconds >= _config.SecondsPerGameHour)
            {
                _accumulatedRealSeconds -= _config.SecondsPerGameHour;
                AdvanceHour();
            }
        }

        private void AdvanceHour()
        {
            _currentGameHour = (_currentGameHour + 1) % 24;
            OnHourChanged?.Invoke(_currentGameHour);

            if (_currentPhase == Day && _currentGameHour == _config.DayEndHour)
            {
                SwitchToPhase(Night);
            }
            else if (_currentPhase == Night && _currentGameHour == _config.NightEndHour)
            {
                CompleteDay();
                SwitchToPhase(Day);
            }
        }

        private void SwitchToPhase(DayNightPhase newPhase)
        {
            _currentPhase = newPhase;
            OnPhaseChanged?.Invoke(_currentPhase);

            if (newPhase == Day)
            {
                _balanceAtDayStart = GetPlayerBalance();
                _previousBalance = _balanceAtDayStart;
                _earnedDuringDay = 0f;
                _spentDuringDay = 0f;
            }
        }

        private void CompleteDay()
        {
            float endBalance = GetPlayerBalance();
            var summary = new DaySummaryData
            {
                Results =
                {
                    new DayIncomeResult
                    {
                        StartBalance = _balanceAtDayStart,
                        EndBalance = endBalance,
                        Earned = _earnedDuringDay,
                        Spent = _spentDuringDay
                    }
                }
            };
            OnDaySummaryReady?.Invoke(summary);
            _dayNumber++;
        }

        public void SetGameHour(int hour)
        {
            hour = Math.Clamp(hour, 0, 23);
            _currentGameHour = hour;
            _accumulatedRealSeconds = 0f;
            OnHourChanged?.Invoke(_currentGameHour);

            if (IsDayHour(hour))
            {
                if (_currentPhase != Day)
                    SwitchToPhase(Day);
            }
            else
            {
                if (_currentPhase != Night)
                    SwitchToPhase(Night);
            }
        }

        public void SetDay()
        {
            SetGameHour(_config.DayStartHour);
        }

        public void SetNight()
        {
            SetGameHour(_config.NightStartHour);
        }

        private bool IsDayHour(int hour)
        {
            if (_config.DayStartHour <= _config.DayEndHour)
                return hour >= _config.DayStartHour && hour < _config.DayEndHour;
            return hour >= _config.DayStartHour || hour < _config.DayEndHour;
        }
    }
}
