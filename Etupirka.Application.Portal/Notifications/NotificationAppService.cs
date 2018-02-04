using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Notifications;
using Abp.Runtime.Session;
using Etupirka.Application.Portal.Notifications.Dto;

namespace Etupirka.Application.Portal.Notifications
{
    [AbpAuthorize]
    public class NotificationAppService : EtupirkaAppServiceBase, INotificationAppService
    {
        private readonly INotificationDefinitionManager _notificationDefinitionManager;
        private readonly IUserNotificationManager _userNotificationManager;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IRepository<UserNotificationInfo, Guid> _userNotificationRepository;

        public NotificationAppService(
            INotificationDefinitionManager notificationDefinitionManager,
            IUserNotificationManager userNotificationManager,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IRepository<UserNotificationInfo, Guid> userNotificationRepository)
        {
            _notificationDefinitionManager = notificationDefinitionManager;
            _userNotificationManager = userNotificationManager;
            _notificationSubscriptionManager = notificationSubscriptionManager;

            _userNotificationRepository = userNotificationRepository;
        }


        /// <summary>
        /// 取得用户通知信息
        /// </summary>
        public async Task<NotificationsPagedResultDto> GetUserNotifications(GetUserNotificationsInput input)
        {
            var currentUserId = AbpSession.ToUserIdentifier();

            var totalCount = await _userNotificationManager
                .GetUserNotificationCountAsync(currentUserId, input.State);

            var unreadCount = await _userNotificationManager
                .GetUserNotificationCountAsync(currentUserId, UserNotificationState.Unread);

            var notifications = await _userNotificationManager
                .GetUserNotificationsAsync(currentUserId, input.State, input.SkipCount, input.MaxResultCount);

            return new NotificationsPagedResultDto(totalCount, unreadCount, notifications);
        }

        /// <summary>
        /// 将所有通知置为已读
        /// </summary>
        public async Task SetAllNotificationsAsRead()
        {
            var currentUserId = AbpSession.ToUserIdentifier();
            await _userNotificationManager.UpdateAllUserNotificationStatesAsync(currentUserId, UserNotificationState.Read);
        }

        /// <summary>
        /// 将通知置为已读
        /// </summary>
        /// <param name="id">通知Id</param>
        public async Task SetNotificationAsRead(Guid id)
        {
            var userNotification = await _userNotificationManager.GetUserNotificationAsync(AbpSession.TenantId, id);
            if (userNotification.UserId != AbpSession.GetUserId())
                throw new ApplicationException($"Given user notification id [{id}] is not belong to the current user [{AbpSession.GetUserId()}]");

            await _userNotificationManager.UpdateUserNotificationStateAsync(AbpSession.TenantId, id, UserNotificationState.Read);
        }

        /// <summary>
        /// 将通知置为已读
        /// </summary>
        public async Task SetNotificationsAsRead(Guid[] ids)
        {
            long currentUserId = AbpSession.GetUserId();
            var userNotifications = await _userNotificationRepository
                .GetAll()
                .Where(n => ids.Contains(n.Id) && n.UserId == currentUserId)
                .ToListAsync();

            if (userNotifications.Count > 0)
            {
                foreach (var userNotification in userNotifications)
                {
                    userNotification.State = UserNotificationState.Read;
                }
            }
        }

        /// <summary>
        /// 删除指定通知
        /// </summary>
        public async Task DeleteNotifications(Guid[] ids)
        {
            long currentUserId = AbpSession.GetUserId();
            await _userNotificationRepository.DeleteAsync(n => ids.Contains(n.Id) && n.UserId == currentUserId);
        }

        /// <summary>
        /// 取得通知设置
        /// </summary>
        public async Task<NotificationSettingsOutput> GetNotificationSettings()
        {
            var currentUserId = AbpSession.ToUserIdentifier();
            var output = new NotificationSettingsOutput();

            //是否接收通知配置
            output.ReceiveNotifications = await SettingManager.GetSettingValueAsync<bool>(NotificationSettingNames.ReceiveNotifications);

            //取得通知定义
            output.Notifications = (await _notificationDefinitionManager.GetAllAvailableAsync(currentUserId))
                .Where(nd => nd.EntityType == null) //Get general notifications, not entity related notifications.
                .MapTo<List<NotificationSubscriptionWithDisplayNameDto>>();

            //取得订阅的通知名
            var subscribedNotifications = (await _notificationSubscriptionManager
                .GetSubscribedNotificationsAsync(currentUserId))
                .Select(ns => ns.NotificationName)
                .ToList();

            output.Notifications.ForEach(n => n.IsSubscribed = subscribedNotifications.Contains(n.Name));

            return output;
        }

        /// <summary>
        /// 更新通知设置
        /// </summary>
        public async Task UpdateNotificationSettings(UpdateNotificationSettingsInput input)
        {
            var currentUserId = AbpSession.ToUserIdentifier();

            //是否接收通知
            await SettingManager.ChangeSettingForUserAsync(currentUserId, NotificationSettingNames.ReceiveNotifications, input.ReceiveNotifications.ToString());

            foreach (var notification in input.Notifications)
            {
                if (notification.IsSubscribed)
                {
                    //订阅通知
                    await _notificationSubscriptionManager.SubscribeAsync(currentUserId, notification.Name);
                }
                else
                {
                    //关闭通知订阅
                    await _notificationSubscriptionManager.UnsubscribeAsync(currentUserId, notification.Name);
                }
            }
        }

        
    }
}
