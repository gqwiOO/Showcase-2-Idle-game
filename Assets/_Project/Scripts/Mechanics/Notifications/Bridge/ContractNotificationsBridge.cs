using Mechanics.Product;
using Zenject;

namespace Mechanics.Notifications
{
    public class ContractNotificationsBridge : IInitializable
    {
        private readonly IContractService _contractService;
        private readonly INotificationService _notificationService;

        [Inject]
        public ContractNotificationsBridge(
            IContractService contractService,
            INotificationService notificationService)
        {
            _contractService = contractService;
            _notificationService = notificationService;
        }

        public void Initialize()
        {
            _contractService.OnContractCompleted += OnContractCompleted;
            _contractService.OnContractFailed += OnContractFailed;
        }

        private void OnContractCompleted(ContractCompletionResult _) =>
            _notificationService.Show(NotificationType.ContractCompleted);

        private void OnContractFailed(IContractData _) =>
            _notificationService.Show(NotificationType.ContractFailed);
    }
}
