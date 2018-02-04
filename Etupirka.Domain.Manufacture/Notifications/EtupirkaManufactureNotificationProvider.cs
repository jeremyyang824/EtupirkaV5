using System;
using Abp.Authorization;
using Abp.Localization;
using Abp.Notifications;
using Etupirka.Domain.Portal;
using Etupirka.Domain.Portal.Authorization;

namespace Etupirka.Domain.Manufacture.Notifications
{
    public class EtupirkaManufactureNotificationProvider : NotificationProvider
    {
        public override void SetNotifications(INotificationDefinitionContext context)
        {
           
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, EtupirkaPortalConsts.LocalizationSourceName);
        }
    }
}
