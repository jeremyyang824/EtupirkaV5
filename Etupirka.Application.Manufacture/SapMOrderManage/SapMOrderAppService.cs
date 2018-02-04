using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Etupirka.Application.Manufacture.SapMOrderManage.Dto;
using Etupirka.Application.Portal;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.Domain.Manufacture.Services;
using Etupirka.Domain.Portal;

namespace Etupirka.Application.Manufacture.SapMOrderManage
{
    /// <summary>
    /// SAP制造订单信息管理
    /// </summary>
    [AbpAuthorize]
    public class SapMOrderAppService : EtupirkaAppServiceBase, ISapMOrderAppService
    {
        private readonly IRepository<SapMOrder, Guid> _sapMOrderRepository;
        private readonly IRepository<SapMOrderProcess, Guid> _sapMOrderProcessRepository;
        private readonly IRepository<SapMOrderProcessCooperate, int> _sapMOrderProcessCooperateRepository;
        private readonly SapMOrderManager _sapMOrderManager;

        public SapMOrderAppService(
            IRepository<SapMOrder, Guid> sapMOrderRepository,
            IRepository<SapMOrderProcess, Guid> sapMOrderProcessRepository,
            IRepository<SapMOrderProcessCooperate, int> sapMOrderProcessCooperateRepository,
            SapMOrderManager sapMOrderManager)
        {
            this._sapMOrderRepository = sapMOrderRepository;
            this._sapMOrderProcessRepository = sapMOrderProcessRepository;
            this._sapMOrderProcessCooperateRepository = sapMOrderProcessCooperateRepository;
            this._sapMOrderManager = sapMOrderManager;
        }

        #region 订单信息

        /// <summary>
        /// 取得SAP制造订单
        /// </summary>
        public async Task<SapMOrderOutput> GetSapMOrder(Guid id)
        {
            var order = await this._sapMOrderRepository.FirstOrDefaultAsync(id);
            return order.MapTo<SapMOrderOutput>();
        }

        /// <summary>
        /// 取得SAP制造订单
        /// </summary>
        public async Task<SapMOrderOutput> FindSapMOrderByOrderNumber(string orderNumber)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
                throw new ArgumentNullException("orderNumber");

            var order = await this._sapMOrderRepository.GetAll()
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
            return order.MapTo<SapMOrderOutput>();
        }

        #endregion

        #region 工艺信息

        /// <summary>
        /// 取得SAP制造订单工艺列表
        /// </summary>
        /// <param name="orderId">SAP制造订单ID</param>
        public async Task<ListResultDto<SapMOrderProcessOutput>> GetSapMOrderProcessList(Guid orderId)
        {
            var sapOrder = await this._sapMOrderRepository.GetAll()
                .Include(o => o.OrderProcess)
                .FirstAsync(o => o.Id == orderId);

            var processList = sapOrder.OrderProcess.OrderBy(p => p.OperationNumber);
            return new ListResultDto<SapMOrderProcessOutput>(processList.MapTo<List<SapMOrderProcessOutput>>());
        }

        /// <summary>
        /// 取得SAP制造订单工艺列表（含外协信息）
        /// </summary>
        /// <param name="orderId">SAP制造订单ID</param>
        public async Task<ListResultDto<SapMOrderProcessWithCooperateOutput>> GetSapMOrderProcessListWithCooperate(Guid orderId)
        {
            bool orderExists = await this._sapMOrderRepository.GetAll()
                .AnyAsync(o => o.Id == orderId);
            if (!orderExists)
                throw new DomainException($"SAP订单[{orderId}]不存在！");

            var processList = await this._sapMOrderManager.GetSapMOrderProcessListWithCooperateQuery()
                .Where(p => p.ProcessLine.SapMOrderId == orderId)
                .OrderBy(p => p.ProcessLine.OperationNumber)
                .ToListAsync();

            return new ListResultDto<SapMOrderProcessWithCooperateOutput>(
                processList.Select(this.mapSapMOrderProcessWithCooperateToDto).ToList());
        }

        public SapMOrderProcessWithCooperateOutput mapSapMOrderProcessWithCooperateToDto(
            SapMOrderManager.SapMOrderProcessWithCooperate bean)
        {
            if (bean == null)
                return null;

            var dto = bean.ProcessLine.MapTo<SapMOrderProcessWithCooperateOutput>();
            if (bean.CooperateLine != null)
            {
                dto.ProcessCooperateId = bean.CooperateLine.Id;
                dto.CooperateType = bean.CooperateLine.CooperateType;
                dto.CooperaterCode = bean.CooperateLine.CooperaterCode;
                dto.CooperaterName = bean.CooperateLine.CooperaterName;
                dto.CooperaterPrice = bean.CooperateLine.CooperaterPrice;
                dto.CooperaterFsPointOfUse = bean.CooperateLine.CooperaterFsPointOfUse;
                dto.FsCoNumber = bean.CooperateLine.FsCoNumber;
                dto.FsMoNumber = bean.CooperateLine.FsMoNumber;
                dto.IsMesInspectFinished = bean.CooperateLine.IsMesInspectFinished;
                dto.MesInspectQualified = bean.CooperateLine.MesInspectQualified;
            }
            return dto;
        }

        #endregion
    }
}
