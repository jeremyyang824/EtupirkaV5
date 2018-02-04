using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Etupirka.Application.Portal.Notifications.Dto;

namespace Etupirka.Application.Portal.Notifications
{
    public interface INotificationAppService : IApplicationService
    {
        /// <summary>
        /// 取得用户通知信息
        /// </summary>
        Task<NotificationsPagedResultDto> GetUserNotifications(GetUserNotificationsInput input);

        /// <summary>
        /// 将所有通知置为已读
        /// </summary>
        Task SetAllNotificationsAsRead();

        /// <summary>
        /// 将通知置为已读
        /// </summary>
        /// <param name="id">通知Id</param>
        Task SetNotificationAsRead(Guid id);

        /// <summary>
        /// 将通知置为已读
        /// </summary>
        Task SetNotificationsAsRead(Guid[] ids);

        /// <summary>
        /// 删除指定通知
        /// </summary>
        Task DeleteNotifications(Guid[] ids);

        /// <summary>
        /// 取得通知设置
        /// </summary>
        Task<NotificationSettingsOutput> GetNotificationSettings();

        /// <summary>
        /// 更新通知设置
        /// </summary>
        Task UpdateNotificationSettings(UpdateNotificationSettingsInput input);
    }
}