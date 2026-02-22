using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using static Mechanics.DayNight.DayNightPhase;

namespace Mechanics.DayNight
{
    public class DayNightClockView : MonoBehaviour
    {
        [Header("Clock")]
        [SerializeField] private RectTransform _arrow;
        [SerializeField] private Image _backgroundImage;

        [Header("Colors")]
        [SerializeField] private Color _dayColor = new Color(0.9f, 0.85f, 0.5f);
        [SerializeField] private Color _nightColor = new Color(0.2f, 0.2f, 0.4f);

        [Header("Optional")]
        [SerializeField] private TMP_Text _phaseLabel;

        private IGameTimeService _gameTimeService;

        // Radial clock: up = 00:00, right = 06:00, down = 12:00, left = 18:00
        private const float FullCircleDegrees = 360f;

        [Inject]
        private void Construct(IGameTimeService gameTimeService)
        {
            _gameTimeService = gameTimeService;
        }

        private void OnEnable()
        {
            _gameTimeService.OnPhaseChanged += OnPhaseChanged;
            _gameTimeService.OnHourChanged += OnHourChanged;
            UpdateView();
        }

        private void OnDisable()
        {
            _gameTimeService.OnPhaseChanged -= OnPhaseChanged;
            _gameTimeService.OnHourChanged -= OnHourChanged;
        }

        private void Update()
        {
            UpdateArrowRotation();
        }

        private void OnPhaseChanged(DayNightPhase phase)
        {
            UpdateBackgroundColor(phase);
            UpdatePhaseLabel(phase);
        }

        private void OnHourChanged(int hour)
        {
            UpdateArrowRotation();
        }

        private void UpdateView()
        {
            UpdateBackgroundColor(_gameTimeService.CurrentPhase);
            UpdatePhaseLabel(_gameTimeService.CurrentPhase);
            UpdateArrowRotation();
        }

        private void UpdateArrowRotation()
        {
            if (_arrow == null) return;

            float progress = Mathf.Clamp01(_gameTimeService.ProgressInDayCycle);
            float angle = progress * FullCircleDegrees;
            _arrow.localEulerAngles = new Vector3(0, 0, -angle);
        }

        private void UpdateBackgroundColor(DayNightPhase phase)
        {
            if (_backgroundImage == null) return;

            _backgroundImage.color = phase == Day ? _dayColor : _nightColor;
        }

        private void UpdatePhaseLabel(DayNightPhase phase)
        {
            if (_phaseLabel == null) return;

            _phaseLabel.text = phase == Day ? "День" : "Ніч";
        }
    }
}
