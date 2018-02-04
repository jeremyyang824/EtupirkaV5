using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Etupirka.Application.Manufacture.SapMOrderManage.Dto;

namespace Etupirka.Application.Manufacture.SapMOrderManage
{
    /// <summary>
    /// SAP制造订单信息管理
    /// </summary>
    public interface ISapMOrderAppService : IApplicationService
    {
        /// <summary>
        /// 取得SAP制造订单
        /// </summary>
        Task<SapMOrderOutput> GetSapMOrder(Guid id);

        /// <summary>
        /// 取得SAP制造订单
        /// </summary>
        Task<SapMOrderOutput> FindSapMOrderByOrderNumber(string orderNumber);

        /// <summary>
        /// 取得SAP制造订单工艺列表
        /// </summary>
        /// <param name="orderId">SAP制造订单ID</param>
        Task<ListResultDto<SapMOrderProcessOutput>> GetSapMOrderProcessList(Guid orderId);

        /// <summary>
        /// 取得SAP制造订单工艺列表（含外协信息）
        /// </summary>
        /// <param name="orderId">SAP制造订单ID</param>
        Task<ListResultDto<SapMOrderProcessWithCooperateOutput>> GetSapMOrderProcessListWithCooperate(Guid orderId);
    }
}