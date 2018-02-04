using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Application.Portal.Notifications.Dto
{
    public class NotificationSettingsOutput
    {
        /// <summary>
        /// 是否接收通知
        /// </summary>
        public bool ReceiveNotifications { get; set; }

        public List<NotificationSubscriptionWithDisplayNameDto> Notifications { get; set; }
    }
}
