(function () {
    'use strict';

    angular.module('app.main')
        .controller('importProcessController', [
            '$scope', '$uibModalInstance', 'Upload', 'abp.services.manufacture.processManage',
            function ($scope, $uibModalInstance, $upload, processManageService) {
                var vm = this;
                vm.loading = false;
                vm.importFile = null;

                vm.cancel = function () {
                    $uibModalInstance.dismiss();
                };

                vm.saveImportProcess = function () {
                    if (!vm.importFile) {
                        abp.notify.error("请选择上传文件!");
                        return;
                    }

                    vm.loading = true;
                    $upload
                        .upload({
                            url: abp.toAbsAppPath('File/UploadTempFile'),
                            data: { file: vm.importFile, fileName: 'SAP工艺列表.xlsx', fileType: 'application/x-excel' }
                        })
                        .then(function (resp) {
                            abp.notify.info('上传成功, 开始解析文件...');
                            return processManageService.importSapOrderProcessWithCooperater(resp.data);
                        })
                        .then(function (result) {
                            abp.notify.success('工艺导入成功！');
                            $uibModalInstance.close();
                        })
                        .finally(function () {
                            vm.loading = false;
                        });
                };

            }
        ]);
})();