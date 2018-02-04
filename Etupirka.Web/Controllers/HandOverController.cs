using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.UI;
using Abp.Web.Mvc.Authorization;
using Etupirka.Application.Manufacture.DispatchedManage;
using Etupirka.Application.Manufacture.HandOver;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.Web.Models.HandOvers;

namespace Etupirka.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HandOverController : EtupirkaControllerBase
    {
        private readonly IHandOverAppService _handOverAppService;
        private readonly IWorkCenterAppService _workCenterAppService;

        public HandOverController(IHandOverAppService handOverAppService, IWorkCenterAppService workCenterAppService)
        {
            this._handOverAppService = handOverAppService;
            this._workCenterAppService = workCenterAppService;
        }

        [DisableAuditing]
        [HttpGet]
        public async Task<ActionResult> Test()
        {
            await this._workCenterAppService.FindWorkCenterList(
                new Application.Manufacture.DispatchedManage.Dto.FindWorkCentersInput
                {
                    WorkCenterCode = "EB81",
                    MaxResultCount = 10,
                    SkipCount = 0
                });
            return View("Print");
        }

        [DisableAuditing]
        [HttpGet]
        public async Task<ActionResult> Print(int id)
        {
            var bill = await this._handOverAppService.GetHandOverBill(id);
            if (bill == null)
                throw new UserFriendlyException($"交接单[{id}]不存在！");

            var bilLines = await this._handOverAppService.GetHandOverBillLines(id);

            return View(
                new PrintHandOverViewModel
                {
                    HandOverBill = bill,
                    HandOverBillLines = bilLines.Items
                });
        }
    }
}