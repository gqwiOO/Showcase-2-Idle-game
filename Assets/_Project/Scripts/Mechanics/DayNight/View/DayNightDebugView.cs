using TMPro;
using UI.Buttons;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Mechanics.DayNight
{
    public class DayNightDebugView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _hourInputField;
        [SerializeField] private UIButton _setHourButton;
        [SerializeField] private UIButton _setDayButton;
        [SerializeField] private UIButton _setNightButton;
        [SerializeField] private Slider _timeSpeedSlider;
        [SerializeField] private TMP_Text _timeSpeedLabel;

        private IGameTimeService _gameTimeService;

        [Inject]
        private void Construct(IGameTimeService gameTimeService)
        {
            _gameTimeService = gameTimeService;
        }

        private void OnEnable()
        {
            _hourInputField.text = _gameTimeService.CurrentGameHour.ToString();
            if (_timeSpeedSlider != null)
            {
                _timeSpeedSlider.minValue = 1f;
                _timeSpeedSlider.maxValue = 10f;
                _timeSpeedSlider.value = _gameTimeService.TimeSpeed;
                _timeSpeedSlider.onValueChanged.AddListener(OnTimeSpeedChanged);
                UpdateTimeSpeedLabel();
            }
            _setHourButton.OnClicked += OnSetHourClicked;
            _setDayButton.OnClicked += OnSetDayClicked;
            _setNightButton.OnClicked += OnSetNightClicked;
        }

        private void OnDisable()
        {
            if (_timeSpeedSlider != null)
                _timeSpeedSlider.onValueChanged.RemoveListener(OnTimeSpeedChanged);
            _setHourButton.OnClicked -= OnSetHourClicked;
            _setDayButton.OnClicked -= OnSetDayClicked;
            _setNightButton.OnClicked -= OnSetNightClicked;
        }

        private void OnTimeSpeedChanged(float value)
        {
            _gameTimeService.TimeSpeed = value;
            UpdateTimeSpeedLabel();
        }

        private void UpdateTimeSpeedLabel()
        {
            if (_timeSpeedLabel != null)
                _timeSpeedLabel.text = $"x{_gameTimeService.TimeSpeed:F1}";
        }

        private void OnSetHourClicked()
        {
            if (int.TryParse(_hourInputField.text, out int hour))
                _gameTimeService.SetGameHour(Mathf.Clamp(hour, 0, 23));
        }

        private void OnSetDayClicked() => _gameTimeService.SetDay();

        private void OnSetNightClicked() => _gameTimeService.SetNight();
    }
}
