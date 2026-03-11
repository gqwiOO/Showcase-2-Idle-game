using Zenject;

namespace Mechanics.Notifications
{
    public class NotificationsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<NotificationService>().AsSingle();
            Container.BindInterfacesTo<ContractNotificationsBridge>().AsSingle();
        }
    }
}
