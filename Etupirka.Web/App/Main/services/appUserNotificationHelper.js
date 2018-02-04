(function () {
    'use strict';

    angular.module('app.main')
        .factory('appUserNotificationHelper', [
            '$uibModal', '$location', 'abp.services.portal.notification',
            function ($uibModal, $location, notificationService) {

                var format = function (userNotification, truncateText) {
                    var formatted = {
                        userNotificationId: userNotification.id,
                        text: abp.notifications.getFormattedMessageFromUserNotification(userNotification),
                        time: userNotification.notification.creationTime,
                        icon: getUiIconBySeverity(userNotification.notification.severity),
                        state: abp.notifications.getUserNotificationStateAsString(userNotification.state),
                        data: userNotification.notification.data,
                        url: getUrl(userNotification)
                    };

                    if (truncateText || truncateText === undefined) {
                        formatted.text = abp.utils.truncateStringWithPostfix(formatted.text, 100);
                    }

                    return formatted;
                };

                var show = function (userNotification) {
                    abp.notifications.showUiNotifyForUserNotification(userNotification, {
                        'onclick': function () {
                            //Take action when user clicks to live toastr notification
                            var url = getUrl(userNotification);
                            if (url) {
                                location.href = url;
                            }
                        }
                    });
                };

                var setAllAsRead = function (callback) {
                    notificationService.setAllNotificationsAsRead().success(function () {
                        abp.event.trigger('app.notifications.refresh');
                        callback && callback();
                    });
                };

                var setSelectedAsRead = function (userNotificationIdArray, callback) {
                    notificationService.setNotificationsAsRead(userNotificationIdArray)
                        .success(function () {
                            abp.event.trigger('app.notifications.refresh');
                            callback && callback();
                        });
                };

                var setAsRead = function (userNotificationId, callback) {
                    notificationService.setNotificationAsRead(userNotificationId)
                        .success(function () {
                            abp.event.trigger('app.notifications.read', userNotificationId);
                            callback && callback(userNotificationId);
                        });
                };

                var deleteSelected = function (userNotificationIdArray, callback) {
                    notificationService.deleteNotifications(userNotificationIdArray)
                       .success(function () {
                           abp.event.trigger('app.notifications.refresh');
                           callback && callback();
                       });
                };

                var openSettingsModal = function () {
                    $uibModal.open({
                        templateUrl: '~/App/common/views/notifications/settingsModal.cshtml',
                        controller: 'common.views.notifications.settingsModal as vm',
                        backdrop: 'static'
                    });
                };


                function getUiIconBySeverity(severity) {
                    switch (severity) {
                        case abp.notifications.severity.SUCCESS:
                            return 'fa fa-check';
                        case abp.notifications.severity.WARN:
                            return 'fa fa-warning';
                        case abp.notifications.severity.ERROR:
                            return 'fa fa-bolt';
                        case abp.notifications.severity.FATAL:
                            return 'fa fa-bomb';
                        case abp.notifications.severity.INFO:
                        default:
                            return 'fa fa-info';
                    }
                }

                function getUrl(userNotification) {
                    switch (userNotification.notification.notificationName) {
                        case 'App.NewUserRegistered':
                            return '#/users?filterText=' + userNotification.notification.data.properties.emailAddress;
                        case 'App.NewTenantRegistered':
                            return '#/host/tenants?filterText=' + userNotification.notification.data.properties.tenancyName;
                            //Add your custom notification names to navigate to a URL when user clicks to a notification.
                    }

                    //No url for this notification
                    return null;
                }

                return {
                    format: format,
                    show: show,
                    setAllAsRead: setAllAsRead,
                    setSelectedAsRead: setSelectedAsRead,
                    setAsRead: setAsRead,
                    deleteSelected: deleteSelected,
                    openSettingsModal: openSettingsModal
                };
            }
        ]);

})();