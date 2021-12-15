using Manager.Core.Communication.Messages.Notifications;
using System.Threading.Tasks;

namespace Manager.Core.Communication.Mediator.Interfaces
{
    public interface IMediatorHandler
    {
        Task PublishDomainNotificationAsync<T>(T appNotification)
            where T : DomainNotification;
    }
}
