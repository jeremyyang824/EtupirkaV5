(function () {
    'use strict';

    angular.module('app.main')
        .controller('layoutTopBarController', [
            '$rootScope', '$state', 'appSessionService', 'appUserNotificationHelper', 'abp.services.portal.notification',
            function ($rootScope, $state, appSessionService, appUserNotificationHelper, notificationService) {
                var vm = this;

                vm.languages = abp.localization.languages;
                vm.currentLanguage = abp.localization.currentLanguage;
                vm.notifications = [];
                vm.unreadNotificationCount = 0;

                //获取当前用户信息
                vm.userName = appSessionService.currentUser.surname;
                vm.companyName = appSessionService.currentTenant === null ?
                    App.consts.hostInfo.fullName : appSessionService.currentTenant.name;
                vm.organizationName = appSessionService.organizationUnit.displayName;
                
                vm.loadNotifications = function () {
                    notificationService.getUserNotifications({
                        maxResultCount: 3,
                        state: abp.notifications.userNotificationState.UNREAD
                    }).success(function (result) {
                        vm.unreadNotificationCount = result.unreadCount;
                        vm.notifications = [];
                        $.each(result.items, function (index, item) {
                            vm.notifications.push(appUserNotificationHelper.format(item));
                        });
                    });
                }

                vm.readNotification = function (notification) {
                    if (notification.state == 'UNREAD') {
                        appUserNotificationHelper.setAsRead(notification.userNotificationId);
                    }
                    if (notification.url) {
                        $state.go(notification.url);
                    }
                }

                abp.event.on('abp.notifications.received', function (userNotification) {
                    appUserNotificationHelper.show(userNotification);
                    vm.loadNotifications();
                });

                abp.event.on('app.notifications.refresh', function () {
                    vm.loadNotifications();
                });

                abp.event.on('app.notifications.read', function (userNotificationId) {
                    vm.loadNotifications();
                });


                function init() {
                    vm.loadNotifications();
                }
                init();
            }
        ])
        .directive('layoutTopBarComponent', function () {
            return {
                restrict: 'E',
                replace: true,
                scope: true,
                controller: 'layoutTopBarController',
                controllerAs: 'vm',
                templateUrl: '~/App/Main/views/layout/topBar.cshtml'
            };
        });

})();