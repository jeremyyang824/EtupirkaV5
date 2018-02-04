(function () {
    'use strict';

    angular.module('app.main')
        .controller('drawingViewerController', [
            '$scope', '$uibModalInstance', '$http', 'abp.services.manufacture.arragement', 'partNumber', 
            function ($scope, $uibModalInstance, $http, arragementService, partNumber) {

                var vm = this;
                vm.loading = false;
                vm.partNumber = partNumber;
                //vm.partVersion = partVersion;
                vm.isShowMenu = true;
                vm.drawingData = null;
                vm.drawingUrl = "";

                vm.cancel = function () {
                    $uibModalInstance.dismiss();
                };

                vm.toggle = function () {
                    vm.isShowMenu = !vm.isShowMenu;
                };

                vm.view2d = function (docItem) {
                    if (!docItem.downloadUrl2D)
                        return;

                    vm.drawingUrl = getDrawingUrl(docItem.downloadUrl2D);
                };

                vm.view3d = function (docItem) {
                    if (!docItem.downloadUrl3D)
                        return;

                    vm.drawingUrl = getDrawingUrl(docItem.downloadUrl3D);
                };

                function getDrawingUrl(rawUrl) {
                    var url = abp.toAbsAppPath("WinChillDrawing/GetClientDrawing?url=" + rawUrl);
                    return url;
                }

                (function init() {
                    if (!vm.partNumber) {
                        abp.notify.error('物料编码获取失败！');
                        return;
                    }

                    vm.loading = true;
                    arragementService.getPartDrawingLastVersion(vm.partNumber)
                        .then(function (result) {
                            var rawData = result.data;
                            if (!rawData) {
                                abp.notify.warn('物料['+vm.partName+']图纸不存在！');
                                return;
                            }
                            //构建展示对象
                            var tempArr = [];
                            _.each(rawData.partDocs, function (partDoc) {
                                _.each(partDoc.versions, function (partVersion) {
                                    tempArr.push({
                                        docNumber: partDoc.docNumber,
                                        docName: partDoc.docName,
                                        docVersion: partVersion.docVersion,
                                        downloadUrl3D: partVersion.downloadUrl3D,
                                        downloadUrl2D: partVersion.downloadUrl2D,
                                        publishTime: partVersion.publishTime,
                                        docModifier: partVersion.docModifier,
                                        flag: partVersion.flag
                                    });
                                });
                            });
                            vm.drawingData = {
                                partNumber: rawData.partNumber,
                                partName: rawData.partName,
                                partVersion: rawData.partVersion,
                                partDocs: tempArr
                            };
                        }).finally(function () {
                            vm.loading = false;
                        });
                })();
            }
        ]);
})();