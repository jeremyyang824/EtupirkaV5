using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Notifications;
using Etupirka.Domain.Portal.Users;

namespace Etupirka.Domain.Portal.Notifications
{
    public class EtupirkaPortalNotifier : EtupirkaDomainServiceBase, IEtupirkaPortalNotifier
    {
        private readonly INotificationPublisher _notificationPublisher;

        public EtupirkaPortalNotifier(INotificationPublisher notificationPublisher)
        {
            _notificationPublisher = notificationPublisher;
        }


        public async Task WelcomeToTheApplicationAsync(SysUser user)
        {
            await _notificationPublisher.PublishAsync(
                EtupirkaPortalNotificationNames.WelcomeToTheApplication,
                new MessageNotificationData($"欢迎您：{user.Name}！"),
                severity: NotificationSeverity.Success,
                userIds: new[] { user.ToUserIdentifier() });
        }
    }
}
