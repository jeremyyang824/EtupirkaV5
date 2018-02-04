using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Web.Mvc.Authorization;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using ZXing.QrCode.Internal;

namespace Etupirka.Web.Controllers
{
    /// <summary>
    /// 条形码生成
    /// </summary>
    //[AbpMvcAuthorize]
    public class BarCodeImageController : EtupirkaControllerBase
    {
        private const int barCodeWidth = 200;
        private const int barCodeHeidht = 25;

        private const int qrCodeWidth = 80;
        private const int qrCodeHeidht = 80;

        /// <summary>
        /// 取得条形码图片
        /// </summary>
        [DisableAuditing]
        public FileResult GetBarCode(string content, int w = barCodeWidth, int h = barCodeHeidht)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException(nameof(content));
            content = content.Trim().ToUpper();

            if (w < barCodeWidth)
                w = barCodeWidth;
            if (h < barCodeHeidht)
                h = barCodeHeidht;

            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    PureBarcode = true,
                    Width = w,
                    Height = h
                }
            };
            Bitmap bitmap = writer.Write(content);

            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return File(ms.GetBuffer(), "image/png");
            }
        }

        /// <summary>
        /// 取得二维码图片
        /// </summary>
        [DisableAuditing]
        public FileResult GetQrCode(string content, int w = qrCodeWidth, int h = qrCodeHeidht)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentNullException(nameof(content));
            content = content.Trim().ToUpper();

            if (w < qrCodeWidth)
                w = qrCodeWidth;
            if (h < qrCodeHeidht)
                h = qrCodeHeidht;

            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    DisableECI = true,
                    CharacterSet = "UTF-8",
                    Width = w,
                    Height = h,
                    ErrorCorrection = ErrorCorrectionLevel.H
                }
            };
            Bitmap bitmap = writer.Write(content);

            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return File(ms.GetBuffer(), "image/png");
            }
        }
    }
}