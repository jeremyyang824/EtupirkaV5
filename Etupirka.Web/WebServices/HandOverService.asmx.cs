using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using Abp.Dependency;
using Etupirka.Application.Manufacture.Cooperate;
using Etupirka.Application.Manufacture.Cooperate.Dto;
using Nito.AsyncEx;

namespace Etupirka.Web.WebServices
{
    /// <summary>
    /// 交接单相关WebService
    /// </summary>
    [WebService(Namespace = "STMC.MES")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class HandOverService : System.Web.Services.WebService
    {
        private readonly ICooperateAppService cooperateAppService;

        public HandOverService()
        {
            cooperateAppService = IocManager.Instance.Resolve<ICooperateAppService>();
        }

        /// <summary>
        /// SAP外协到FS的生产订单道序质检完成反写接口
        /// </summary>
        /// <param name="fsMOrderNumber">FS生产订单号</param>
        /// <param name="inspectQualified">质检合格数量</param>
        [WebMethod]
        public bool SapCooperFsProcessFinished(string fsMOrderNumber, decimal inspectQualified)
        {
            var param = new SapCooperInspectedInput
            {
                FsMOrderNumber = fsMOrderNumber,
                InspectQualified = inspectQualified,
            };
            bool result = AsyncContext.Run(async () =>
            {
                return await this.cooperateAppService.SapCooperFsProcessFinished(param);
            });
            return result;
        }
    }
}
