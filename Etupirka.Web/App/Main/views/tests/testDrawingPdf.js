(function () {
    'use strict';

    angular.module('app.main')
        .controller('testDrawingPdfController', [
            '$scope', '$rootScope', '$state', '$stateParams', '$uibModal', 'appSessionService',
            function ($scope, $rootScope, $state, $stateParams, $uibModal, appSessionService) {

                $rootScope.setViewTitle('图纸显示测试');  //设置页标题

                var vm = this;
                
                //显示该行接口日志
                vm.showDrawing = function () {
                    $uibModal.open({
                        templateUrl: '~/App/Main/views/tests/drawingViewer.cshtml',
                        controller: 'drawingViewerController as vm',
                        backdrop: 'static',
                        windowClass: 'modal-full-width',
                        resolve: {
                            partNumber: function () {
                                return '4BBH36001100';
                            }
                        }
                    });
                };
                
            }
        ]);
})();