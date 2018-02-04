(function () {
    'use strict';

    //面包屑组件
    angular.module('angular.ui.etupirka')
        .factory('etuBreadcrumbsService', [
            '$rootScope',
            function ($rootScope) {
                var crumbs = [];

                return {
                    get: function () {
                        return angular.copy(crumbs);
                    },
                    push: function (item, isTriggerEvent) {
                        isTriggerEvent = angular.isUndefined(isTriggerEvent) ? true : isTriggerEvent;   //默认true
                        if (item) {
                            crumbs.push(item);
                            if (isTriggerEvent === true)
                                $rootScope.$broadcast('etuBreadcrumbsChanged', this.get());
                        }
                    },
                    pop: function (idx) {
                        if (crumbs.length > idx) {
                            crumbs.splice(idx, crumbs.length - idx);
                            $rootScope.$broadcast('etuBreadcrumbsChanged', this.get());
                        }
                    }
                };
            }
        ])
        .controller('etuBreadcrumbsController', [
            '$rootScope', 'etuBreadcrumbsService',
            function ($rootScope, etuBreadcrumbsService) {
                var vm = this;

                //面包屑改变事件
                $rootScope.$on('etuBreadcrumbsChanged', function (event, crumbs) {
                    vm.breadcrumbs = crumbs;
                });

                if ($rootScope.menu) {
                    initialCrumbsFromMenu($rootScope.menu);
                }

                //菜单改变事件
                $rootScope.$on('etuMenuChanged', function (event, menu) {
                    initialCrumbsFromMenu($rootScope.menu);
                });

                /**
                 * 是否定义了指定属性成员
                 */
                vm.isDefined = function (member) {
                    return angular.isDefined(member);
                };

                /**
                 * 定位到指定位置的面包屑
                 */
                vm.gotoBreadCrumb = function (index) {
                    etuBreadcrumbsService.pop(index + 1);
                };

                /**
                 * 根据菜单激活项初始化面包屑
                 * @param {} 系统菜单 
                 * @returns [] 面包屑数组 
                 */
                function initialCrumbsFromMenu(menu) {
                    etuBreadcrumbsService.pop(0);

                    if (menu && menu.items) {
                        angular.forEach(menu.items, function (menuItem) {
                            if (menuItem.active === true
                                /*忽略主页*/
                                && (menuItem.url !== '#/' && menuItem.url !== '/')) {

                                //第一层菜单
                                etuBreadcrumbsService.push({
                                    label: menuItem.displayName,
                                    url: menuItem.url
                                });

                                if (menuItem.items && menu.items.length > 0) {
                                    angular.forEach(menuItem.items, function (menuSubItem) {
                                        if (menuSubItem.active === true) {

                                            //第二层菜单
                                            etuBreadcrumbsService.push({
                                                label: menuSubItem.displayName,
                                                url: menuSubItem.url
                                            });
                                        }
                                    }); //end for
                                }
                            }
                        }); //end for
                    }
                    return etuBreadcrumbsService.get();
                };

            }
        ])
        .directive('etuBreadcrumbsComponent', function () {
            return {
                restrict: 'E',
                replace: true,
                scope: {},
                controller: 'etuBreadcrumbsController',
                controllerAs: 'vm',
                templateUrl: '~/App/Main/components/etuBreadcrumbs.cshtml'
            };
        })
        /**
         * 与ui-sref配合使用，点击链接时自动加入breadcrumb
         */
        .directive('etuBreadcrumbsPush', [
            '$compile', 'etuBreadcrumbsService',
            function ($compile, etuBreadcrumbsService) {
                return {
                    restrict: 'A',
                    link: function (scope, elem, attr) {
                        var srefVal = attr.uiSref;
                        if (angular.isDefined(srefVal)) {
                            var labelText = '[新标签]';
                            var params = attr.etuBreadcrumbsPush;
                            if (angular.isUndefined(params) || angular === null || params === '') {
                                labelText = '[新标签]';
                            }
                            else if (angular.isString(params)) {
                                labelText = params;
                            }
                            else if (angular.isObject(params)) {
                                labelText = params.label || labelText;
                                //TODO：添加对现有url修改的功能参数
                            } else {
                                throw new 'arguments error exception';
                            }

                            //点击指令所在链接时，加入面包屑
                            var onLinkClick = function () {
                                etuBreadcrumbsService.push({
                                    label: labelText,
                                    sref: srefVal
                                });
                            }

                            elem.bind("click", onLinkClick);
                            scope.$on('$destroy', function () {
                                elem.unbind("click", onLinkClick);
                            });
                        }
                    } //end link
                };
            }
        ]);

})();