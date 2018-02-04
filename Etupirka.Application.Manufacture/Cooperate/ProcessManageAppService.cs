using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.Linq.Extensions;
using Etupirka.Application.Manufacture.Cooperate.Dto;
using Etupirka.Application.Portal;
using Etupirka.Application.Portal.Dto;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.Domain.Manufacture.Services;
using Etupirka.Domain.Portal;
using Etupirka.Domain.Portal.Office;
using Etupirka.Domain.Portal.Utils;

namespace Etupirka.Application.Manufacture.Cooperate
{
    /// <summary>
    /// 工艺管理
    /// </summary>
    [AbpAuthorize]
    public class ProcessManageAppService : EtupirkaAppServiceBase, IProcessManageAppService
    {
        private readonly IRepository<SapMOrder, Guid> _sapMOrderRepository;
        private readonly IRepository<SapMOrderProcess, Guid> _sapMOrderProcessRepository;
        private readonly IRepository<SapMOrderProcessCooperate, int> _sapMOrderProcessCooperateRepository;
        private readonly IRepository<ProcessCodeMap, int> _processCodeMap;
        private readonly SapMOrderManager _sapMOrderManager;
        private readonly SapSupplierManager _sapSupplierManager;

        public ProcessManageAppService(
            IRepository<SapMOrder, Guid> sapMOrderRepository,
            IRepository<SapMOrderProcess, Guid> sapMOrderProcessRepository,
            IRepository<SapMOrderProcessCooperate, int> sapMOrderProcessCooperateRepository,
            IRepository<ProcessCodeMap, int> processCodeMap,
            SapMOrderManager sapMOrderManager,
            SapSupplierManager sapSupplierManager)
        {
            this._sapMOrderRepository = sapMOrderRepository;
            this._sapMOrderProcessRepository = sapMOrderProcessRepository;
            this._sapMOrderProcessCooperateRepository = sapMOrderProcessCooperateRepository;
            this._processCodeMap = processCodeMap;
            this._sapMOrderManager = sapMOrderManager;
            this._sapSupplierManager = sapSupplierManager;
        }

        /// <summary>
        /// 导出SAP订单工艺及外协信息到Excel文件，并返回文件Token
        /// </summary>
        public async Task<FileDto> GetSapOrderProcessWithCooperaterExcel(GetSapOrderProcessWithCooperaterInput input)
        {
            var processList = await this.getSapOrderProcessWithCooperQuery(input).ToListAsync();
            var resultList = processList.Select(this.mapToSapProcessOutput).ToList();
            var fileStream = ExcelOutput.RenderToStream(resultList, new Dictionary<string, DataTableExtension.PropertyConventer>
            {
                //订单
                { "OrderNumber", new DataTableExtension.PropertyConventer("订单号", null) },
                { "MRPController", new DataTableExtension.PropertyConventer("MRP控制者", null) },
                { "MaterialNumber", new DataTableExtension.PropertyConventer("物料编码", null) },
                { "MaterialDescription", new DataTableExtension.PropertyConventer("物料名称", null) },
                { "TargetQuantity", new DataTableExtension.PropertyConventer("订单总计数量", null) },
                { "WBSElement", new DataTableExtension.PropertyConventer("WBS元素", null) },
                //工艺
                { "OperationNumber", new DataTableExtension.PropertyConventer("工序号", null) },
                { "OperationCtrlCode", new DataTableExtension.PropertyConventer("控制码", null) },
                { "WorkCenterCode", new DataTableExtension.PropertyConventer("工作中心", null) },
                { "WorkCenterName", new DataTableExtension.PropertyConventer("工作中心描述", null) },
                { "VGW01", new DataTableExtension.PropertyConventer("准备工时", null) },
                { "VGW02", new DataTableExtension.PropertyConventer("机器工时", null) },
                { "VGW03", new DataTableExtension.PropertyConventer("人工工时", null) },
                //外协
                { "CooperateType", new DataTableExtension.PropertyConventer("外协类型", p => ((int?)(SapMOrderProcessCooperateType?)p)?.ToString()) },
                { "CooperaterCode", new DataTableExtension.PropertyConventer("供方代码", null) },
                { "CooperaterName", new DataTableExtension.PropertyConventer("供方名称", null) },
                { "CooperaterPrice", new DataTableExtension.PropertyConventer("外协价格", null) },
            });
            return this.SaveToTempFolder(fileStream, "SAP工艺列表.xlsx", "application/x-excel");
        }

        /// <summary>
        /// 将工艺Excel文件中的外协类型、供应商代码、供应商名称、外协价格写入本地SAP工艺记录
        /// </summary>
        public async Task ImportSapOrderProcessWithCooperater(FileDto importFile)
        {
            //取得临时文件
            using (var file = this.GetTempFile(importFile))
            {
                var dataTable = ExcelInput.GetExcel(file.File);
                var tempDataList =
                    (from DataRow dr in dataTable.Rows
                     select new
                     {
                         OrderNumber = dr["订单号"]?.ToString().Trim(),
                         OperationNumber = dr["工序号"]?.ToString().Trim(),
                         CooperateType = dr["外协类型"]?.ToString().TryParse<int?>(),
                         CooperaterCode = dr["供方代码"]?.ToString().Trim(),
                         CooperaterName = dr["供方名称"]?.ToString().Trim(),
                         CooperaterPrice = dr["外协价格"]?.ToString().TryParse<decimal?>(),
                     })
                     .Where(li => !string.IsNullOrEmpty(li.OrderNumber) && !string.IsNullOrEmpty(li.OperationNumber))
                     .OrderBy(li => li.OperationNumber)
                     .GroupBy(li => li.OrderNumber);    //按订单号分组

                //遍历每个导入订单
                foreach (var tempDataItem in tempDataList)
                {
                    //过滤出对应订单的工艺及外协记录
                    var orderProcessDic = await this._sapMOrderManager
                        .GetSapMOrderProcessListWithCooperateQuery()
                        .Where(li => li.ProcessLine.SapMOrder.OrderNumber == tempDataItem.Key && li.CooperateLine != null)
                        .OrderBy(li => li.ProcessLine.OperationNumber)
                        .ToDictionaryAsync(li => li.ProcessLine.OperationNumber);   //以工序号为索引

                    //遍历订单的导入工艺
                    foreach (var importProcessData in tempDataItem)
                    {
                        if (!orderProcessDic.ContainsKey(importProcessData.OperationNumber))
                            continue;   //跳过没有找到的工艺

                        var orderProcess = orderProcessDic[importProcessData.OperationNumber];
                        if (!orderProcess.ProcessLine.CanCooperate())
                            continue;   //跳过非外协工艺

                        var orderProcessCooper = orderProcess.CooperateLine;
                        orderProcessCooper.CooperateType = (SapMOrderProcessCooperateType)importProcessData.CooperateType;
                        orderProcessCooper.CooperaterCode = importProcessData.CooperaterCode;
                        orderProcessCooper.CooperaterName = importProcessData.CooperaterName;
                        orderProcessCooper.CooperaterPrice = importProcessData.CooperaterPrice ?? 0;

                        var sapSupplier = await this._sapSupplierManager.GetSupplierBySapCode(importProcessData.CooperaterCode);
                        orderProcessCooper.CooperaterFsPointOfUse = sapSupplier.FsPointOfUse;

                        await this._sapMOrderProcessCooperateRepository.UpdateAsync(orderProcessCooper);
                    }
                }
            }   //自动删除临时文件
        }

        /// <summary>
        /// 取得一条外协工艺
        /// </summary>
        /// <param name="cooperateId">外协ID</param>
        public async Task<GetSapOrderProcessWithCooperaterOutput> GetSapOrderProcessCooperater(int cooperateId)
        {
            var cooperateBean = await this._sapMOrderManager.GetSapMOrderProcessListWithCooperateQuery()
                .Where(p => p.CooperateLine.Id == cooperateId)
                .FirstOrDefaultAsync();

            return this.mapToSapProcessOutput(cooperateBean);
        }

        /// <summary>
        /// 更新一个工艺外协信息
        /// </summary>
        public async Task UpdateSapOrderProcessCooperate(UpdateSapOrderProcessCooperateInput input)
        {
            var cooperateBean = await this._sapMOrderProcessCooperateRepository.FirstOrDefaultAsync(input.CooperateId);
            if (cooperateBean == null)
                throw new DomainException($"外协记录[{input.CooperateId}]不存在！");

            cooperateBean.CooperateType = (SapMOrderProcessCooperateType)input.CooperateType;
            cooperateBean.CooperaterCode = input.CooperaterCode;
            cooperateBean.CooperaterName = input.CooperaterName;
            cooperateBean.CooperaterPrice = input.CooperaterPrice;

            var sapSupplier = await this._sapSupplierManager.GetSupplierBySapCode(input.CooperaterCode);
            cooperateBean.CooperaterFsPointOfUse = sapSupplier.FsPointOfUse;

            await this._sapMOrderProcessCooperateRepository.UpdateAsync(cooperateBean);
        }

        /// <summary>
        /// 获取SAP订单工艺及外协信息
        /// 用于维护外协使用点、价格
        /// </summary>
        public async Task<PagedResultDto<GetSapOrderProcessWithCooperaterOutput>>
            GetSapOrderProcessWithCooperaterPager(GetSapOrderProcessWithCooperaterPagerInput input)
        {
            var query = this.getSapOrderProcessWithCooperQuery(input);
            var count = await query.CountAsync();
            var results = (await query.PageBy(input).ToListAsync()).Select(this.mapToSapProcessOutput).ToList();
            return new PagedResultDto<GetSapOrderProcessWithCooperaterOutput>(count, results);
        }

        private IQueryable<SapMOrderManager.SapMOrderProcessWithCooperate>
            getSapOrderProcessWithCooperQuery(GetSapOrderProcessWithCooperaterInput input)
        {
            var query = this._sapMOrderManager.GetSapMOrderProcessListWithCooperateQuery()
                //条件
                .WhereIf(!string.IsNullOrWhiteSpace(input.OrderNumberRangeBegin),
                    p => String.Compare(p.ProcessLine.SapMOrder.OrderNumber, input.OrderNumberRangeBegin, StringComparison.Ordinal) >= 0)
                .WhereIf(!string.IsNullOrWhiteSpace(input.OrderNumberRangeEnd),
                    p => String.Compare(p.ProcessLine.SapMOrder.OrderNumber, input.OrderNumberRangeEnd, StringComparison.Ordinal) <= 0)
                .OrderBy(p => p.ProcessLine.SapMOrder.OrderNumber)
                .ThenBy(p => p.ProcessLine.OperationNumber);
            return query;
        }

        private GetSapOrderProcessWithCooperaterOutput
            mapToSapProcessOutput(SapMOrderManager.SapMOrderProcessWithCooperate bean)
        {
            return new GetSapOrderProcessWithCooperaterOutput
            {
                //订单
                OrderId = bean.ProcessLine.SapMOrderId,
                OrderNumber = bean.ProcessLine.SapMOrder.OrderNumber,
                MRPController = bean.ProcessLine.SapMOrder.MRPController,
                MaterialNumber = bean.ProcessLine.SapMOrder.MaterialNumber,
                MaterialDescription = bean.ProcessLine.SapMOrder.MaterialDescription,
                TargetQuantity = bean.ProcessLine.SapMOrder.TargetQuantity,
                WBSElement = bean.ProcessLine.SapMOrder.WBSElement,
                //工艺
                ProcessId = bean.ProcessLine.Id,
                OperationNumber = bean.ProcessLine.OperationNumber,
                OperationCtrlCode = bean.ProcessLine.OperationCtrlCode,
                WorkCenterCode = bean.ProcessLine.WorkCenterCode,
                WorkCenterName = bean.ProcessLine.WorkCenterName,
                VGE01 = bean.ProcessLine.VGE01,
                VGW01 = bean.ProcessLine.VGW01,
                VGE02 = bean.ProcessLine.VGE02,
                VGW02 = bean.ProcessLine.VGW02,
                VGE03 = bean.ProcessLine.VGE03,
                VGW03 = bean.ProcessLine.VGW03,
                //外协
                CooperateId = bean.CooperateLine?.Id,
                CooperateType = bean.CooperateLine?.CooperateType,
                CooperaterCode = bean.CooperateLine?.CooperaterCode,
                CooperaterName = bean.CooperateLine?.CooperaterName,
                CooperaterPrice = bean.CooperateLine?.CooperaterPrice ?? 0,
            };
        }

    }
}
