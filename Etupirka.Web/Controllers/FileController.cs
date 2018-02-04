using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.UI;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Etupirka.Application.Portal;
using Etupirka.Application.Portal.Dto;

namespace Etupirka.Web.Controllers
{
    public class FileController : EtupirkaControllerBase
    {
        private readonly IAppFolders _appFolders;

        public FileController(IAppFolders appFolders)
        {
            _appFolders = appFolders;
        }

        /// <summary>
        /// 下载临时文件
        /// </summary>
        [AbpMvcAuthorize]
        [DisableAuditing]
        public ActionResult DownloadTempFile(FileDto file)
        {
            CheckModelState();

            var filePath = Path.Combine(_appFolders.TempFileFolder, file.FileToken);
            if (!System.IO.File.Exists(filePath))
            {
                throw new UserFriendlyException(L("RequestedFileDoesNotExists"));
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            System.IO.File.Delete(filePath);
            return File(fileBytes, file.FileType, file.FileName);
        }

        /// <summary>
        /// 上传临时文件
        /// </summary>
        [AbpMvcAuthorize]
        [DisableAuditing]
        public JsonResult UploadTempFile([Required]string fileName, [Required]string fileType)
        {
            try
            {
                CheckModelState();

                //Check input
                if (Request.Files.Count <= 0 || Request.Files[0] == null)
                {
                    throw new UserFriendlyException("文件上传失败！");
                }

                var file = Request.Files[0];
                if (file.ContentLength > 1024 * 1024 * 10) //10MB.
                {
                    throw new UserFriendlyException("文件尺寸过大（不得超过10M）。");
                }

                FileDto fileDto = new FileDto(fileName, fileType);
                string fullPath = Path.Combine(_appFolders.TempFileFolder, fileDto.FileToken);
                file.SaveAs(fullPath);

                return Json(new AjaxResponse(fileDto));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}