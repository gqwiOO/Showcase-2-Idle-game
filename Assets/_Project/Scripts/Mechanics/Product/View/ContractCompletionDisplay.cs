using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace Mechanics.Product
{
    /// <summary>Shows contract completion rewards: dirty money, reputation, heat.</summary>
    public class ContractCompletionDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private float _displayDuration = 5f;

        private IContractService _contractService;

        [Inject]
        private void Construct(IContractService contractService)
        {
            _contractService = contractService;
        }

        private void OnEnable()
        {
            _contractService.OnContractCompleted += OnContractCompleted;
        }

        private void OnDisable()
        {
            _contractService.OnContractCompleted -= OnContractCompleted;
        }

        private void OnContractCompleted(ContractCompletionResult result)
        {
            var msg = $"Контракт завершено!\n" +
                      $"Dirty: +{result.DirtyMoneyReceived:F0}$\n" +
                      $"Репутація: +{result.ReputationAdded:F0}\n" +
                      $"Heat: +{result.HeatAdded:F0}";
            ShowMessage(msg);
        }

        private void ShowMessage(string message)
        {
            if (_text != null)
            {
                _text.text = message;
                _text.gameObject.SetActive(true);
                StopAllCoroutines();
                StartCoroutine(HideAfterDelay());
            }
        }

        private IEnumerator HideAfterDelay()
        {
            yield return new WaitForSeconds(_displayDuration);
            if (_text != null)
                _text.gameObject.SetActive(false);
        }
    }
}
