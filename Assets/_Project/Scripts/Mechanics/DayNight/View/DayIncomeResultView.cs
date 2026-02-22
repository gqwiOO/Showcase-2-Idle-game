using TMPro;
using UnityEngine;

namespace Mechanics.DayNight
{
    public class DayIncomeResultView : DayResultView
    {
        [SerializeField] private TMP_Text _earnedText;
        [SerializeField] private TMP_Text _spentText;
        [SerializeField] private TMP_Text _deltaText;

        public override bool SupportsData(IDayResultData data) => data is DayIncomeResult;

        public override void Init(IDayResultData data)
        {
            if (data is not DayIncomeResult income)
                return;

            if (_earnedText != null)
                _earnedText.text = FormatCurrency(income.Earned);
            if (_spentText != null)
                _spentText.text = FormatCurrency(income.Spent);
            if (_deltaText != null)
            {
                float delta = income.BalanceDelta;
                string sign = delta >= 0 ? "+" : "";
                _deltaText.text = $"{sign}{FormatCurrency(delta)}";
            }

            gameObject.SetActive(true);
        }

        private static string FormatCurrency(float value) => value.ToString("F0");
    }
}
