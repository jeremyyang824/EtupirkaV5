using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Etupirka.Application.Manufacture.Cooperate.Dto;

namespace Etupirka.Application.Manufacture.Cooperate
{
    /// <summary>
    /// 工艺外协管理
    /// </summary>
    public interface ICooperateAppService : IApplicationService
    {
        /// <summary>
        /// 同步SAP订单到本地
        /// </summary>
        Task SapMOrderSync(SapMOrderSyncInput input);

        /// <summary>
        /// 创建工艺外协ERP相关内容
        /// </summary>
        Task<bool> SapCooperSendOut(SapCooperSendInput input);

        /// <summary>
        /// SAP工艺外协创建的FS制造订单道序完工
        /// </summary>
        Task<bool> SapCooperFsProcessFinished(SapCooperInspectedInput input);

        /// <summary>
        /// 取得SAP订单中外协工艺的接口日志
        /// </summary>
        /// <param name="sapMOrderNumber">SAP制造订单号</param>
        /// <returns>日志列表</returns>
        Task<ListResultDto<SapCooperProcessLogOutput>> GetSapMOrderCooperLogs(string sapMOrderNumber);
    }
}