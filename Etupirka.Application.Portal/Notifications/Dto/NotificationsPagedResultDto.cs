using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Notifications;

namespace Etupirka.Application.Portal.Notifications.Dto
{
    public class NotificationsPagedResultDto : PagedResultDto<UserNotification>
    {
        public int UnreadCount { get; set; }

        public NotificationsPagedResultDto(int totalCount, int unreadCount, List<UserNotification> notifications)
            : base(totalCount, notifications)
        {
            UnreadCount = unreadCount;
        }
    }
}
