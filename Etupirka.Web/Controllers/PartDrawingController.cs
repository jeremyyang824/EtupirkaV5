using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using Abp.Auditing;
using Etupirka.Application.Manufacture.Arragement;
using Etupirka.Application.Manufacture.Arragement.Dto;
using Etupirka.Web.Mvc;

namespace Etupirka.Web.Controllers
{
    [RoutePrefix("PartDrawings")]
    public class PartDrawingController : EtupirkaControllerBase
    {
        private readonly IArragementAppService _arragementAppService;

        public PartDrawingController(IArragementAppService arragementAppService)
        {
            this._arragementAppService = arragementAppService;
        }

        [DisableAuditing]
        [Route("{partNumber}")]
        [HttpGet]
        public async Task<XmlResult> GetPartDrawing(string partNumber)
        {
            var list = await _arragementAppService.GetPartDrawingAll(partNumber);
            return this.Xml(list.Items, XmlRequestBehavior.AllowGet);
        }

        [DisableAuditing]
        [Route("{partNumber}/{partVersion}")]
        [HttpGet]
        public async Task<XmlResult> GetPartDrawing(string partNumber, string partVersion)
        {
            var item = await _arragementAppService.GetPartDrawing(partNumber, partVersion);
            return this.Xml(item, XmlRequestBehavior.AllowGet);
        }

        [DisableAuditing]
        [Route("createAll")]
        [HttpGet]
        public async Task<JsonResult> CreateAllDrawings()
        {
            string basePath = System.Configuration.ConfigurationManager.AppSettings["WinChillDrawingDirectory"];

            var items = await _arragementAppService.GetAllDrawings();
            var count = 0;
            foreach (var data in items.Items)
            {
                string filename = $"{data.PartNumber.ToUpper().Trim()}@{data.PartVersion.ToUpper().Trim()}.xml";
                filename = $"{basePath}\\{filename}";

                using (FileStream fs = new FileStream(filename, FileMode.Create))
                {
                    XmlSerializer xmlser = new XmlSerializer(typeof(PartDrawingDto));
                    xmlser.Serialize(fs, data);
                }
                count++;
            }
            return Json(new
            {
                XMLCount = count
            }, JsonRequestBehavior.AllowGet);
        }
    }
}