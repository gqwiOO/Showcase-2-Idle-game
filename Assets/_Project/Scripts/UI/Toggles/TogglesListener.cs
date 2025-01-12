using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;

namespace UI.Toggles
{
    public class TogglesListener : MonoBehaviour
    {
        [SerializeField] private List<BaseToggle> _toggles;

        private void Start() => _toggles.ForEach(toggle => toggle.OnButtonClicked += Toggle_OnClicked);
        private void OnDestroy() => _toggles.ForEach(toggle => toggle.OnButtonClicked -= Toggle_OnClicked);

        private void Toggle_OnClicked(object sender, EventArgs e)
        {
            var clickedToggle = (BaseToggle)sender;
            clickedToggle.PerformEnable();
            _toggles
                .Where(toggle => toggle != clickedToggle)
                .ForEach(toggle => toggle.PerformDisable());
        }
    }
}