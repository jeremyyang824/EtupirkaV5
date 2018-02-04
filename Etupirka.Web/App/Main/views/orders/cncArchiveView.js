(function () {
    'use strict';

    angular.module('app.main')
        .controller('dispatchedCncArchiveController', [
            '$scope', '$state', '$stateParams', '$uibModalInstance', 'etuBreadcrumbsService', 'abp.services.manufacture.arragement', 'partNumber', 'archiveType',
            function ($scope, $state, $stateParams, $uibModalInstance, etuBreadcrumbsService, arragementService, partNumber, archiveType) {

                var vm = this;
                vm.loading = false;
                vm.partNumber = partNumber;
                vm.archiveType = archiveType;
                vm.isShowMenu = vm.archiveType == '1' ? false : true;
                vm.fileData = [];
                vm.fileUrl = "";

                vm.cancel = function () {
                    $uibModalInstance.dismiss();
                };

                vm.viewFile = function (file) {
                    if (!file.urlPath)
                        return;

                    vm.drawingUrl = getDrawingUrl(file.urlPath);
                };

                function getDrawingUrl(rawUrl) {
                    var url = abp.toAbsAppPath("WinToolFile/GetNetworkFile?filepath=" + rawUrl);
                    return url;
                }

                (function init() {
                    if (!vm.partNumber) {
                        abp.notify.error('物料编码获取失败！');
                        return;
                    }


                    vm.loading = true;
                    arragementService.getCncArchives(vm.partNumber, vm.archiveType)
                        .then(function (result) {
                            vm.fileData = result.data.items;
                            //构建展示对象
                            //vm.drawingUrl = getDrawingUrl(urlData);
                            if (vm.archiveType == '1') {
                                vm.viewFile(vm.fileData[0]);
                            }
                        }).finally(function () {
                            vm.loading = false;
                        });

                })();
            }
        ]);
})();