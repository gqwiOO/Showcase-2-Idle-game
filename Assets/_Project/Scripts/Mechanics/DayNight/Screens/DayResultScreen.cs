using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Services.Screen;
using UnityEngine;
using UnityEngine.UI;

namespace Mechanics.DayNight
{
    public class DayResultScreen : BaseScreen
    {
        [SerializeField] private List<DayResultView> _resultViews = new();

        [SerializeField]
        private Button closeButton;

        private void Awake()
        {
            closeButton.onClick.AddListener(Close_Clicked);
        }

        private void Close_Clicked()
        {
            Hide().Forget();
        }

        private void OnDestroy()
        {
            closeButton.onClick.RemoveListener(Close_Clicked);
        }

        public void Init(DaySummaryData data)
        {
            foreach (var view in _resultViews)
                view.gameObject.SetActive(false);

            foreach (var resultData in data.Results)
            {
                var view = FindViewFor(resultData);
                if (view != null)
                    view.Init(resultData);
            }
        }

        private DayResultView FindViewFor(IDayResultData data)
        {
            foreach (var view in _resultViews)
            {
                if (view.SupportsData(data))
                    return view;
            }

            return null;
        }
    }
}
