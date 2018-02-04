using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.UI;
using Abp.Web.Models;
using Abp.Web.Mvc.Authorization;
using Etupirka.Application.Portal;
using Etupirka.Application.Portal.Common;

namespace Etupirka.Web.Controllers
{
    /// <summary>
    /// 物料图片管理
    /// </summary>
    [AbpMvcAuthorize]
    public class ItemImageController : EtupirkaControllerBase
    {
        private readonly IAppFolders _appFolders;

        public ItemImageController(IAppFolders appFolders)
        {
            _appFolders = appFolders;
        }

        /// <summary>
        /// 获取物料图片
        /// </summary>
        /// <param name="filename">物料文件名（不含文件后缀）</param>
        /// <param name="showUnknow">是否显示未知零件图片</param>
        [DisableAuditing]
        public FileResult GetItemImage(string filename, bool showUnknow = false)
        {
            if (string.IsNullOrWhiteSpace(filename))
            {
                return this.getEmptyItemImage();
            }
            filename = filename.Trim();

            var directory = new DirectoryInfo(_appFolders.ItemImagesFolder);
            var files = directory.GetFiles(filename + ".*", SearchOption.TopDirectoryOnly).ToList();
            if (files.Count > 0)
            {
                return File(files[0].FullName, MimeTypeNames.ImageJpeg);
            }

            if (showUnknow)
                return this.getUnknowItemImage();
            return this.getEmptyItemImage();
        }

        private FileResult getEmptyItemImage()
        {
            var filePath = Path.Combine(_appFolders.ItemImagesFolder, "default.png");
            return File(filePath, MimeTypeNames.ImagePng);
        }

        private FileResult getUnknowItemImage()
        {
            var filePath = Path.Combine(_appFolders.ItemImagesFolder, "unknowitem.png");
            return File(filePath, MimeTypeNames.ImagePng);
        }

        /// <summary>
        /// 保存上传物料图片
        /// </summary>
        /// <param name="filename">物料文件名（不含文件后缀）</param>
        public JsonResult UploadItemImage(string filename)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filename))
                    throw new ArgumentNullException("filename");
                filename = filename.Trim();

                //Check input
                if (Request.Files.Count <= 0 || Request.Files[0] == null)
                {
                    throw new UserFriendlyException("图片上传失败！");
                }

                var file = Request.Files[0];
                if (file.ContentLength > 1024 * 1024) //1MB.
                {
                    throw new UserFriendlyException("图片尺寸过大（不得超过1M）。");
                }

                //Check file type & format
                var fileImage = Image.FromStream(file.InputStream);
                if (!fileImage.RawFormat.Equals(ImageFormat.Jpeg)
                    && !fileImage.RawFormat.Equals(ImageFormat.Png)
                    && !fileImage.RawFormat.Equals(ImageFormat.Bmp))
                {
                    throw new UserFriendlyException("图片格式不正确（jpeg、png、bmp）!");
                }

                //Delete exists pictures from ItemImagesFolder
                AppFileHelper.DeleteFilesInFolderIfExists(_appFolders.ItemImagesFolder, filename);

                //Save new picture to ItemImagesFolder
                var fileInfo = new FileInfo(file.FileName);
                var fileNameWithExtension = filename + fileInfo.Extension;
                var filePath = Path.Combine(_appFolders.ItemImagesFolder, fileNameWithExtension);
                file.SaveAs(filePath);

                return Json(new AjaxResponse(new { fileName = fileNameWithExtension }));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }
        }
    }
}