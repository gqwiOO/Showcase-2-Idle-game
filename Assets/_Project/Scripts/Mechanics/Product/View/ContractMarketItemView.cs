using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI.Buttons;

namespace Mechanics.Product
{
    public class ContractMarketItemView : MonoBehaviour
    {
        [Header("Common")]
        [SerializeField] private TMP_Text _typeText;
        [SerializeField] private TMP_Text _tierText;
        [SerializeField] private TMP_Text _rewardText;
        [SerializeField] private TMP_Text _reputationText;
        [SerializeField] private TMP_Text _heatText;
        [SerializeField] private TMP_Text _requiredRolesText;

        [Header("Available state")]
        [SerializeField] private UIButton _takeButton;
        [SerializeField] private GameObject _takeButtonRoot;

        [Header("In progress state")]
        [SerializeField] private Slider _progressSlider;
        [SerializeField] private GameObject _progressRoot;

        private IContractData _offer;
        private IContractData _contractData;
        private IContractMarketService _marketService;
        private ContractMarketScreen _screen;

        public void InitAsOffer(IContractData offer, IContractMarketService marketService, ContractMarketScreen screen)
        {
            _offer = offer;
            _contractData = null;
            _marketService = marketService;
            _screen = screen;

            SetAvailableState(active: true);
            SetInProgressState(active: false);

            if (_typeText != null) _typeText.text = offer.Type.ToString();
            if (_tierText != null) _tierText.text = $"Tier {(int)offer.Tier}";
            if (_rewardText != null) _rewardText.text = $"{offer.RewardAmount:F0}$";
            if (_reputationText != null) _reputationText.text = $"+{offer.ReputationReward}";
            if (_heatText != null) _heatText.text = $"+{offer.HeatReward}";
            if (_requiredRolesText != null)
                _requiredRolesText.text = offer.RequiredRoles != null ? string.Join(", ", offer.RequiredRoles) : "";

            _takeButton.OnClicked -= OnTakeClicked;
            _takeButton.OnClicked += OnTakeClicked;
            _takeButton.SetInteractableState(marketService.CanTakeOffer(offer));
        }

        public void InitAsContract(IContractData contract, ContractMarketScreen screen)
        {
            _offer = null;
            _contractData = contract;
            _marketService = null;
            _screen = screen;

            SetAvailableState(active: false);
            SetInProgressState(active: true);

            if (_typeText != null) _typeText.text = contract.Type != ContractType.None ? contract.Type.ToString() : contract.Name;
            if (_tierText != null) _tierText.text = "";
            if (_rewardText != null) _rewardText.text = $"{contract.RewardAmount:F0}$";
            if (_reputationText != null) _reputationText.text = "";
            if (_heatText != null) _heatText.text = "";
            if (_requiredRolesText != null)
                _requiredRolesText.text = "";

            _takeButton.OnClicked -= OnTakeClicked;

            if (contract.ExecutionData != null)
            {
                contract.ExecutionData.OnProgressUpdated -= OnProgressUpdated;
                contract.ExecutionData.OnProgressUpdated += OnProgressUpdated;
                UpdateProgress(contract.ExecutionData.CurrentProgress);
            }
        }

        private void SetAvailableState(bool active)
        {
            if (_takeButtonRoot != null)
                _takeButtonRoot.SetActive(active);
        }

        private void SetInProgressState(bool active)
        {
            if (_progressRoot != null)
                _progressRoot.SetActive(active);
            if (_progressSlider != null)
                _progressSlider.gameObject.SetActive(active);
        }

        private void OnProgressUpdated(float progress)
        {
            UpdateProgress(progress);
        }

        private void UpdateProgress(float progress)
        {
            if (_progressSlider != null)
                _progressSlider.value = progress;
        }

        private void OnTakeClicked()
        {
            _screen?.OnTakeOfferClicked(_offer);
        }

        private void OnDestroy()
        {
            if (_takeButton != null)
                _takeButton.OnClicked -= OnTakeClicked;
            if (_contractData?.ExecutionData != null)
                _contractData.ExecutionData.OnProgressUpdated -= OnProgressUpdated;
        }
    }
}
