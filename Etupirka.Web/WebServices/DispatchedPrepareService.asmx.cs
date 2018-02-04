using Abp.Dependency;
using Etupirka.Application.Manufacture.DispatchedManage;
using Etupirka.Application.Manufacture.DispatchedManage.Dto;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Etupirka.Web.WebServices
{
    /// <summary>
    /// 齐备性相关WebService
    /// </summary>
    [WebService(Namespace = "STMC.MES")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class DispatchedPrepareService : System.Web.Services.WebService
    {
        private readonly IDispatchedPrepareAppService dispatchedPrepareAppService;

        public DispatchedPrepareService()
        {
            dispatchedPrepareAppService = IocManager.Instance.Resolve<IDispatchedPrepareAppService>();
        }

        /// <summary>
        /// 刀具配刀完成接口       
        /// </summary>
        /// <param name="taskId">机台任务ID 可转数字类型</param>
        /// <returns></returns>
        [WebMethod]
        public bool FinishJobForTool(string taskId)
        {
            var param = new SetPrepareStatusInput
            {
                PrepareInfoId = Convert.ToInt32(taskId.Trim())
            };
            bool result = AsyncContext.Run(async () =>
            {
                var finished = await this.dispatchedPrepareAppService.FinishDispatchPrepareStatus_Tooling(param);
                return finished.IsSuccess;
            });
            return result;
        }

        /// <summary>
        /// NC程序准备完成接口       
        /// </summary>
        /// <param name="taskId">机台任务ID 可转数字类型</param>
        /// <returns></returns>
        [WebMethod]
        public bool FinishJobForNC(string taskId)
        {
            var param = new SetPrepareStatusInput
            {
                PrepareInfoId = Convert.ToInt32(taskId.Trim())
            };
            bool result = AsyncContext.Run(async () =>
            {
                var finished = await this.dispatchedPrepareAppService.FinishDispatchPrepareStatus_NC(param);
                return finished.IsSuccess;
            });
            return result;
        }
    }
}
