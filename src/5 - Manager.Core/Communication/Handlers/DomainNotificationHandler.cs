using Manager.Core.Communication.Messages.Notifications;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.Core.Communication.Handlers
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        private List<DomainNotification> _notifications;

        public IReadOnlyCollection<DomainNotification> Notifications
            => _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new List<DomainNotification>();
        }

        public Task Handle(DomainNotification notification, CancellationToken cancellationToken = default)
        {
            _notifications.Add(notification);
            return Task.CompletedTask;
        }

        public bool HasNotifications()
           => _notifications.Count > 0;

        public int NotificationsCount()
            => _notifications.Count;
    }
}
