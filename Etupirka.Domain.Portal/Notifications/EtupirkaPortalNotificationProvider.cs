using Abp.Authorization;
using Abp.Localization;
using Abp.Notifications;
using Etupirka.Domain.Portal.Authorization;

namespace Etupirka.Domain.Portal.Notifications
{
    public class EtupirkaPortalNotificationProvider : NotificationProvider
    {
        public override void SetNotifications(INotificationDefinitionContext context)
        {
            context.Manager.Add(
                new NotificationDefinition(
                    EtupirkaPortalNotificationNames.WelcomeToTheApplication,
                    displayName: L("WelcomeToTheApplication"),
                    permissionDependency: new SimplePermissionDependency(AppPermissions.Root)));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, EtupirkaPortalConsts.LocalizationSourceName);
        }
    }
}
