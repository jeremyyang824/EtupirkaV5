(function () {
    'use strict';

    angular.module('app.main')
        /**
         * ajax下载文件，配合传入FileDto使用
         */
        .factory('appFileDownload', [
            function () {
                var appFileDownload = {
                    download: download,
                    downloadTempFile: downloadTempFile
                };

                function download(url, data, method) {
                    jQuery('<form action="' + url + '" method="' + (method || 'post') + '">' +
                             '<input name="__RequestVerificationToken" type="hidden" value="' + abp.security.antiForgery.getToken() + '" />' +
                             '<input type="text" name="fileName" value="' + data.fileName + '"/>' +
                             '<input type="text" name="fileType" value="' + data.fileType + '"/>' +
                             '<input type="text" name="fileToken" value="' + data.fileToken + '"/>' +
                           '</form>')
                    .appendTo('body').submit().remove();
                };

                function downloadTempFile(data, method) {
                    return download(abp.toAbsAppPath('File/DownloadTempFile'), data, method);
                }

                return appFileDownload;
            }
        ]);
})();