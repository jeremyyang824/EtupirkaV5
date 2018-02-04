(function () {
    'use strict';

    angular.module('app.main')
        .controller('layoutSideBarController', [
            '$rootScope', '$state',
            function ($rootScope, $state) {
                var vm = this;

                //构建菜单模型，并附加到$rootScope
                var menu = abp.nav.menus.MainMenu;
                var currentMenuName = $state.current.menu;
                extendMenu(menu, currentMenuName, function (menuItem) {
                    if ($rootScope.title == null || $rootScope.title === '') {
                        $rootScope.title = menuItem.displayName;
                        $rootScope.subTitle = '';
                    }
                });
                $rootScope.menu = menu;
                //触发事件：菜单改变
                $rootScope.$broadcast('etuMenuChanged', menu);

                //路由改变完成事件
                $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
                    var toMenuName = toState.menu;
                    if (toMenuName) {
                        extendMenu($rootScope.menu, toMenuName, function (menuItem) {
                            $rootScope.setViewTitle(menuItem.displayName, '');  //设置页标题
                        });
                        //触发事件：菜单改变
                        $rootScope.$broadcast('etuMenuChanged', menu);
                    }
                });

                /**
                 * 设置菜单选中状态
                 */
                function extendMenu(menu, currentMenuName, fnActiveMenu) {
                    angular.forEach(menu.items, function (menuItem) {
                        //一层菜单
                        menuItem.active = (menuItem.name === currentMenuName);
                        if (menuItem.active && fnActiveMenu && typeof fnActiveMenu === 'function') {
                            fnActiveMenu(menuItem);
                        }

                        //二层菜单
                        if (menuItem.items.length) {
                            angular.forEach(menuItem.items, function (menuSubItem) {
                                menuSubItem.active = (menuSubItem.name === currentMenuName);
                                if (menuSubItem.active) {
                                    menuItem.active = true;     //同时激活上级（一级）菜单
                                    if (fnActiveMenu && typeof fnActiveMenu === 'function') {
                                        fnActiveMenu(menuSubItem);
                                    }
                                }
                            });
                        }
                    });
                };

            }
        ])
        .directive('layoutSideBarComponent', function () {
            return {
                restrict: 'E',
                replace: true,
                scope: true,
                controller: 'layoutSideBarController',
                controllerAs: 'vm',
                templateUrl: '~/App/Main/views/layout/sideBar.cshtml'
            };
        });

})();