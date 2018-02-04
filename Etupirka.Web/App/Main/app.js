(function () {
    'use strict';

    //app.main module
    angular.module('app.main', [
        'angular-loading-bar',
        'cfp.loadingBar', //loading-bar事件监听
        'ngAnimate',
        'ngSanitize',   //净化HTML标签:$sce
        //'ngTouch',

        'ui.router',
        'ui.bootstrap',
        'ui.jq',

        'ui.grid',
        'ui.grid.pagination',
        'ui.grid.selection',
        'ui.grid.pinning',

        'ngFileUpload',

        'abp',

        'angular.ui.etupirka'   //自定义UI 
    ])

    .constant('errorsConst', {
        'test': App.localize('testA')
    })

    .factory('settings', ['$rootScope',
        function ($rootScope) {
            //AngularJs 设置信息
            var settings = {

            };

            $rootScope.settings = settings;
            return settings;
        }
    ])

    .config(['$stateProvider', '$urlRouterProvider',
        function ($stateProvider, $urlRouterProvider) {
            //路由配置
            $urlRouterProvider.otherwise('/');

            $stateProvider
                .state('home', {
                    url: '/',
                    templateUrl: '~/App/Main/views/home/home.cshtml',
                    menu: 'Home'
                });

            //租户管理
            $stateProvider
                .state('tenants', {
                    abstract: true,
                    url: '/tenants',
                    template: '<div ui-view ></div>'
                })
                .state('tenants.list', {
                    url: '?filterText&filterIsActive',
                    params: { filterIsActive: '1' }, //默认仅显示启用的租户
                    templateUrl: '~/App/Main/views/tenants/list.cshtml',
                    controller: 'tenantsListController as vm',
                    menu: 'SystemConfig_ListTenants'
                })
                //租户人员
                .state('tenants.userlist', {
                    url: '/:tenantId/users?&filterText&filterIsActive',
                    params: { filterIsActive: '1' }, //默认仅显示启用的用户
                    templateUrl: '~/App/Main/views/users/list.cshtml',
                    controller: 'usersListController as vm'
                })
                //租户角色
                .state('tenants.rolelist', {
                    url: '/:tenantId/roles',
                    templateUrl: '~/App/Main/views/roles/list.cshtml',
                    controller: 'rolesListController as vm'
                });

            //用户管理
            $stateProvider
                .state('users', {
                    abstract: true,
                    url: '/users',
                    template: '<div ui-view ></div>'    //class="shuffle-animation"
                })
                .state('users.list', {
                    url: '?filterText&filterIsActive',
                    params: { filterIsActive: '1' },    //默认仅显示启用的用户
                    templateUrl: '~/App/Main/views/users/list.cshtml',
                    controller: 'usersListController as vm',
                    menu: 'SystemConfig_ListUsers'
                });

            //角色管理
            $stateProvider
                .state('roles', {
                    abstract: true,
                    url: '/roles',
                    template: '<div ui-view ></div>'
                })
                .state('roles.list', {
                    url: '',
                    templateUrl: '~/App/Main/views/roles/list.cshtml',
                    controller: 'rolesListController as vm',
                    menu: 'SystemConfig_ListRoles'
                });

            //通知管理
            $stateProvider
                .state('notifications', {
                    abstract: true,
                    url: '/notifications',
                    template: '<div ui-view ></div>'
                })
                .state('notifications.list', {
                    url: '',
                    templateUrl: '~/App/Main/views/notifications/list.cshtml',
                    controller: 'notificationsListController as vm'
                });

            //工艺维护
            $stateProvider
                .state('sapProcessManage', {
                    abstract: true,
                    url: '/sapProcessManage',
                    template: '<div ui-view ></div>'
                })
                .state('sapProcessManage.list', {
                    url: '',
                    templateUrl: '~/App/Main/views/sapProcessManage/list.cshtml',
                    controller: 'sapProcessListController as vm',
                    menu: 'SapOrder_ListProcess'
                });

            //交接单管理
            $stateProvider
                .state('handOverBills', {
                    abstract: true,
                    url: '/handOverBills',
                    template: '<div ui-view ></div>'
                })
                .state('handOverBills.list', {
                    url: '',
                    templateUrl: '~/App/Main/views/handovers/list.cshtml',
                    controller: 'handOverBillListController as vm',
                    menu: 'HandOver_ListHandOverBills'
                })
                .state('handOverBills.detail', {
                    url: '/detail/:billId',
                    templateUrl: '~/App/Main/views/handovers/detail.cshtml',
                    controller: 'handOverBillDetailController as vm'
                });

            //机台左右管理
            $stateProvider
                .state('dispatchedOrders', {
                    abstract: true,
                    url: '/dispatchedOrders',
                    template: '<div ui-view ></div>'
                })
                .state('dispatchedOrders.workcenters', {
                    url: '',
                    templateUrl: '~/App/Main/views/orders/workcenters.cshtml',
                    controller: 'dispatchedWorkCentersController as vm',
                    menu: 'DispatchedOrder_ListWorkCenter'
                })
                .state('dispatchedOrders.orders', {
                    url: '/orders/:workCenterId',
                    templateUrl: '~/App/Main/views/orders/orders.cshtml',
                    controller: 'dispatchedOrdersController as vm'
                }).state('dispatchedOrders.orderPrepares', {
                    url: '/prepares',
                    templateUrl: '~/App/Main/views/orders/orderPrepares.cshtml',
                    controller: 'dispatchedOrderPreparesController as vm'
                });


            //测试
            $stateProvider
                .state('tests', {
                    abstract: true,
                    url: '/tests',
                    template: '<div ui-view ></div>'
                })
                .state('tests.testDrawingPdf', {
                    url: '/testDrawingPdf',
                    templateUrl: '~/App/Main/views/tests/testDrawingPdf.cshtml',
                    controller: 'testDrawingPdfController as vm'
                });
        }
    ])
    .config(['cfpLoadingBarProvider', configLoadingBar])
    .config(['$sceDelegateProvider',
function ($sceDelegateProvider) {
    $sceDelegateProvider.resourceUrlWhitelist([
        // Allow same origin resource loads.
        "self",
        // Allow loading from our assets domain.  Notice the difference between * and **.
        "https://172.16.2.31/**"]);
}])

.run(["$rootScope", "settings", "$state", "i18nService",
    function ($rootScope, settings, $state, i18nService) {
        $rootScope.$state = $state;
        $rootScope.$settings = settings;

        //Set Ui-Grid language
        if (i18nService.get(abp.localization.currentCulture.name)) {
            i18nService.setCurrentLang(abp.localization.currentCulture.name);
        } else {
            i18nService.setCurrentLang("zh-cn");
        }

        //替代Angular的$scope.$apply()
        $rootScope.safeApply = function (fn) {
            var phase = this.$root.$$phase;
            if (phase === '$apply' || phase === '$digest') {
                if (fn && (typeof (fn) === 'function')) {
                    fn();
                }
            } else {
                this.$apply(fn);
            }
        };
    }
])

.run(["$templateCache",
    function ($templateCache) {
        //ui-grid 行首模板（显示行号）
        $templateCache.put('etu-ui-grid/rowHeader',
            "<div class=\"etu-ui-grid-row-header\">#{{rowRenderIndex+1}}</div>");
    }
]);

    function configLoadingBar(cfpLoadingBarProvider) {
        cfpLoadingBarProvider.includeSpinner = false;
        cfpLoadingBarProvider.includeBar = true;
        cfpLoadingBarProvider.latencyThreshold = 500;   //500ms后出现进度条
        if (abp.setting.getInt('Etupirka.DisplayLevel') >= 2)
            cfpLoadingBarProvider.parentSelector = '#split-bar';
    }

    //DOM Loaded
    $(document).ready(function () {
        //App.AdminLTE.changeLayout('fixed');
    });

})();