(function () {
    'use strict';

    angular.module('app.main')
        .controller('notificationsListController', [
            '$scope', '$rootScope', '$stateParams', 'appUserNotificationHelper', 'abp.services.portal.notification',
            function ($scope, $rootScope, $stateParams, appUserNotificationHelper, notificationService) {

                $rootScope.setViewTitle('通知列表');

                var vm = this;
                var requestParams = {
                    skipCount: 0,
                    maxResultCount: App.consts.grid.defaultPageSize
                };

                vm.loading = false;
                vm.selectedNotifications = [];
                vm.readStateFilter = null;

                //通知列表
                vm.notificationGridOptions = {
                    //菜单
                    enableColumnMenus: false,
                    //选中
                    enableRowSelection: true,
                    enableSelectAll: true,
                    multiSelect: true,
                    enableSorting: false,
                    paginationPageSizes: App.consts.grid.defaultPageSizes,
                    paginationPageSize: App.consts.grid.defaultPageSize,
                    useExternalPagination: true,
                    useExternalSorting: false,
                    appScopeProvider: vm,
                    data: [],
                    //rowTemplate: '<div ng-repeat="(colRenderIndex, col) in colContainer.renderedColumns track by col.colDef.name" class="ui-grid-cell" ng-class="{ \'ui-grid-row-header-cell\': col.isRowHeader, \'notification-read\' : row.entity.state == \'READ\' }"  ui-grid-cell></div>',
                    columnDefs: [{
                        name: '通知内容',
                        field: 'text',
                        minWidth: 500,
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <a href="" ng-click="grid.appScope.readNotification(row.entity)" class="notification-content" ng-class="{\'notification-content-unreaded\': row.entity.state == \'UNREAD\' }" title="{{row.entity.text}}">{{row.entity.text}}</a>' +
                            '</div>'
                    }, {
                        name: '通知时间',
                        field: 'time',
                        cellTemplate:
                            '<div class=\"ui-grid-cell-contents\">' +
                            '  <span title="{{row.entity.time | momentFormat: \'LLL\'}}">{{COL_FIELD CUSTOM_FILTERS}}</span> ' +
                            '</div>',
                        cellFilter: 'momentFormat: \'YYYY-MM-DD HH:mm\'',
                        width: 150
                    }],
                    onRegisterApi: function (gridApi) {
                        vm.notificationGridOptions.gridApi = gridApi;

                        //分页
                        vm.notificationGridOptions.gridApi.pagination.on.paginationChanged(null, function (pageNumber, pageSize) {
                            requestParams.skipCount = (pageNumber - 1) * pageSize;
                            requestParams.maxResultCount = pageSize;

                            vm.bindNotifications();
                        });
                        //行选中事件
                        vm.notificationGridOptions.gridApi.selection.on.rowSelectionChanged(null, function (row, event) {
                            vm.selectedNotifications = vm.notificationGridOptions.gridApi.selection.getSelectedRows();
                        });
                        vm.notificationGridOptions.gridApi.selection.on.rowSelectionChangedBatch(null, function (row, event) {
                            vm.selectedNotifications = vm.notificationGridOptions.gridApi.selection.getSelectedRows();
                        });
                    }
                };

                vm.bindNotifications = function () {
                    vm.loading = true;
                    notificationService.getUserNotifications({
                        skipCount: requestParams.skipCount,
                        maxResultCount: requestParams.maxResultCount,
                        state: vm.readStateFilter
                    }).success(function (result) {
                        vm.notificationGridOptions.totalItems = result.totalCount;
                        vm.notificationGridOptions.data = _.map(result.items, function (item) {
                            return appUserNotificationHelper.format(item, false);
                        });
                    }).finally(function () {
                        vm.loading = false;
                    });
                };

                vm.readNotification = function (notification) {
                    if (notification.state == 'UNREAD') {
                        appUserNotificationHelper.setAsRead(notification.userNotificationId, function () {
                            vm.bindNotifications();
                        });
                    }

                    if (notification.url) {
                        $state.go(notification.url);
                    }
                };

                vm.setNotificationsAsRead = function () {
                    var selectedIds = _.chain(vm.selectedNotifications)
                        .filter(function (notification) {
                            return notification.state == 'UNREAD';
                        })
                        .map(function (notification) {
                            return notification.userNotificationId;
                        })
                        .value();

                    if (selectedIds.length > 0) {
                        appUserNotificationHelper.setSelectedAsRead(selectedIds, function () {
                            vm.bindNotifications();
                        });
                    }
                };

                vm.deleteNotifications = function () {
                    abp.message.confirm('确定要删除选中通知吗？')
                        .then(function (isConfirmed) {
                            if (!isConfirmed)
                                return;

                            var selectedIds = _.map(vm.selectedNotifications, function (notification) {
                                return notification.userNotificationId;
                            });
                            if (selectedIds.length > 0) {
                                appUserNotificationHelper.deleteSelected(selectedIds, function () {
                                    vm.bindNotifications();
                                });
                            }
                        });
                };

                vm.bindNotifications();

            } //end controller
        ]);
})();