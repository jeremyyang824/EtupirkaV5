using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Etupirka.Application.Manufacture.HandOver.Dto;

namespace Etupirka.Application.Manufacture.HandOver
{
    /// <summary>
    /// 交接单管理
    /// </summary>
    public interface IHandOverAppService : IApplicationService
    {
        /// <summary>
        /// 取得所有可交接部门
        /// </summary>
        /// <returns></returns>
        Task<ListResultDto<HandOverDepartmentDto>> GetAllHandOverDepartments();
        
        /// <summary>
        /// 获取交接单
        /// </summary>
        Task<HandOverBillOutput> GetHandOverBill(int id);

        /// <summary>
        /// 查询交接单（带交接行统计）
        /// </summary>
        Task<PagedResultDto<HandOverBillWithLineStatisticsOutput>> FindHandOverBills(FindHandOverBillsInput input);

        /// <summary>
        /// 创建一张交接单
        /// </summary>
        Task<HandOverBillOutput> CreateHandOverBill();

        /// <summary>
        /// 保存交接单信息
        /// </summary>
        Task SaveHandOverBill(SaveHandOverBillInput input);

        /// <summary>
        /// 发布交接单
        /// </summary>
        Task<bool> PublishHandOverBill(SaveHandOverBillInput input);

        /// <summary>
        /// 删除交接单
        /// </summary>
        Task DeleteHandOverBill(int id);

        /// <summary>
        /// 取得交接单明细行
        /// </summary>
        /// <param name="billId">交接单ID</param>
        Task<ListResultDto<HandOverBillLineOutput>> GetHandOverBillLines(int billId);

        /// <summary>
        /// 添加SAP交接单行
        /// </summary>
        Task AddSapHandOverBillLine(AddSapHandOverBillLineInput input);

        /// <summary>
        /// 删除交接单行
        /// </summary>
        /// <param name="billId">交接单ID</param>
        /// <param name="lineIds">交接单行ID集合</param>
        Task DeleteHandOverBillLines(int billId, int[] lineIds);

        /// <summary>
        /// 接收选中交接单行
        /// </summary>
        /// <param name="billId">交接单ID</param>
        /// <param name="lineIds">交接单行ID集合</param>
        /// <returns>交接单是否全部完成</returns>
        Task<bool> ReceiveHandOverBillLines(int billId, int[] lineIds);

        /// <summary>
        /// 退回选中交接单行
        /// </summary>
        /// <param name="billId">交接单ID</param>
        /// <param name="lineIds">交接单行ID集合</param>
        /// <returns>交接单是否全部完成</returns>
        Task<bool> RejectHandOverBillLines(int billId, int[] lineIds);
    }
}