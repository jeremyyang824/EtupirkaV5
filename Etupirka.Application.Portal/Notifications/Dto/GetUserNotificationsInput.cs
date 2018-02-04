using Abp.Notifications;
using Etupirka.Application.Portal.Dto;

namespace Etupirka.Application.Portal.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInput
    {
        public UserNotificationState? State { get; set; }
    }
}
