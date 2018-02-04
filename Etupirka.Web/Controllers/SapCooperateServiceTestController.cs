using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Abp.Auditing;
using Abp.Domain.Uow;
using Abp.Web.Mvc.Authorization;
using Etupirka.Application.Manufacture.Cooperate;
using Etupirka.Application.Manufacture.Cooperate.Dto;
using Etupirka.Domain.External.Repositories;

namespace Etupirka.Web.Controllers
{
    [AbpMvcAuthorize]
    public class SapCooperateServiceTestController : EtupirkaControllerBase
    {
        private readonly ICooperateAppService cooperateAppService;
        private readonly IFSTIRepository fstiRepository;
        private readonly IFSRepository fsRepository;

        public SapCooperateServiceTestController(
            ICooperateAppService cooperateAppService,
            IFSTIRepository fstiRepository,
            IFSRepository fsRepository)
        {
            this.cooperateAppService = cooperateAppService;
            this.fstiRepository = fstiRepository;
            this.fsRepository = fsRepository;
        }

        [DisableAuditing]
        [HttpGet]
        public ActionResult CooperateTest()
        {
            return View();
        }

        [DisableAuditing]
        [UnitOfWork(isTransactional: false)]
        [HttpPost]
        public async Task<ActionResult> SapCooperSendOut(string SapMOrderNumber, string SapMOrderProcessNumber)
        {
            var param = new SapCooperSendInput
            {
                SapMOrderNumber = SapMOrderNumber,
                SapMOrderProcessNumber = SapMOrderProcessNumber,
                Direction = SapCooperSendInput.SapCooperSendDirection.SendOut
            };
            bool result = await this.cooperateAppService.SapCooperSendOut(param);
            return View("CooperateTest", (object)result.ToString());
        }

        [DisableAuditing]
        [UnitOfWork(isTransactional: false)]
        [HttpPost]
        public async Task<ActionResult> SapCooperFsProcessFinished(string FsMOrderNumber, decimal InspectQualified)
        {
            var param = new SapCooperInspectedInput
            {
                FsMOrderNumber = FsMOrderNumber,
                InspectQualified = InspectQualified,
            };
            bool result = await this.cooperateAppService.SapCooperFsProcessFinished(param);
            return View("CooperateTest", (object)result.ToString());
        }

        //[DisableAuditing]
        //[UnitOfWork(isTransactional: false)]
        //[HttpPost]
        //public async Task<ActionResult> SapCooperSendBack(string SapMOrderNumber, string SapMOrderProcessNumber)
        //{
        //    var param = new SapCooperSendInput
        //    {
        //        SapMOrderNumber = SapMOrderNumber,
        //        SapMOrderProcessNumber = SapMOrderProcessNumber,
        //        Direction = SapCooperSendInput.SapCooperSendDirection.SendBack
        //    };
        //    bool result = await this.cooperateAppService.SapCooperSendBack(param);
        //    return View("CooperateTest", (object)result.ToString());
        //}

        [DisableAuditing]
        [HttpPost]
        public ActionResult DoTest()
        {
            var cost = fsRepository.GetItemCost0ByItemNumber("WC[R]E611");
            //this.cooperateAppService.DoTest();
            var item = fsRepository.GetItemByItemNumber("11BBG9310100");
            string result = $"cost:{cost}; fsitem:{item.ItemNumber}";
            return View("CooperateTest", (object)result);
        }
    }
}