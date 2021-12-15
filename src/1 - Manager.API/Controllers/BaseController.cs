using Manager.API.ViewModes;
using Manager.Core.Communication.Handlers;
using Manager.Core.Communication.Messages.Notifications;
using Manager.Core.Enum;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Manager.API.Controllers
{
    [ApiController]
    public abstract class BaseController : ControllerBase
    {
        private readonly DomainNotificationHandler _domainNotificationHandler;

        protected BaseController(
           INotificationHandler<DomainNotification> domainNotificationHandler)
        {
            _domainNotificationHandler = domainNotificationHandler as DomainNotificationHandler;
        }

        protected bool HasNotifications()
           => _domainNotificationHandler.HasNotifications();

        protected ObjectResult Created(dynamic responseObject)
            => StatusCode(201, responseObject);

        protected ObjectResult Result()
        {
            var notification = _domainNotificationHandler
                .Notifications
                .FirstOrDefault();

            return StatusCode(GetStatusCodeByNotificationType(notification.Type),
                new ResultViewModel
                {
                    Message = notification.Message,
                    Success = false,
                    Data = new { }
                });
        }

        private int GetStatusCodeByNotificationType(DomainNotificationType errorType)
        {
            return errorType switch
            {
                //Conflict
                DomainNotificationType.UserAlreadyExists
                    => 409,

                //Unprocessable Entity
                DomainNotificationType.UserInvalid
                    => 422,

                //Not Found
                DomainNotificationType.UserNotFound
                    => 404,

                (_) => 500,
            };
        }
    }
}
