using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Auditing;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.Domain.Manufacture.Services
{
    /// <summary>
    /// SAP制造订单管理领域服务
    /// </summary>
    public class SapMOrderManager : DomainService
    {
        private readonly IRepository<SapMOrder, Guid> _sapMOrderRepository;
        private readonly IRepository<SapMOrderProcess, Guid> _sapMOrderProcessRepository;
        private readonly IRepository<SapMOrderProcessCooperate, int> _sapMOrderProcessCooperateRepository;

        public SapMOrderManager(
            IRepository<SapMOrder, Guid> sapMOrderRepository,
            IRepository<SapMOrderProcess, Guid> sapMOrderProcessRepository,
            IRepository<SapMOrderProcessCooperate, int> sapMOrderProcessCooperateRepository)
        {
            this._sapMOrderRepository = sapMOrderRepository;
            this._sapMOrderProcessRepository = sapMOrderProcessRepository;
            this._sapMOrderProcessCooperateRepository = sapMOrderProcessCooperateRepository;
        }

        /// <summary>
        /// 取得关联外协信息的SAP道序源
        /// </summary>
        [DisableAuditing]
        public IQueryable<SapMOrderProcessWithCooperate> GetSapMOrderProcessListWithCooperateQuery()
        {
            var sapProcessWithCooperateQuery =
                from p in _sapMOrderProcessRepository.GetAll().Include(p => p.SapMOrder)
                join c in _sapMOrderProcessCooperateRepository.GetAll() on p.Id equals c.SapMOrderProcessId into t
                from c in t.DefaultIfEmpty()   //left join
                select new SapMOrderProcessWithCooperate
                {
                    ProcessLine = p,
                    CooperateLine = c
                };
            return sapProcessWithCooperateQuery;
        }

        /// <summary>
        /// 获取订单工序及外协信息
        /// </summary>
        /// <param name="orderNumber">SAP生产订单号</param>
        /// <param name="processNumber">SAP生产工序号</param>
        [DisableAuditing]
        public async Task<SapMOrderProcessWithCooperate> GetSapMOrderProcess(string orderNumber, string processNumber)
        {
            var sapOrderProcess = (await this._sapMOrderProcessRepository.GetAll()
                .Include(p => p.SapMOrder)
                .FirstAsync(o => o.SapMOrder.OrderNumber == orderNumber && o.OperationNumber == processNumber));
            if (sapOrderProcess == null)
                return null;

            //外协信息
            SapMOrderProcessCooperate cooperateLine = null;
            if (sapOrderProcess.CanCooperate())
            {
                cooperateLine = await this._sapMOrderProcessCooperateRepository
                    .FirstOrDefaultAsync(c => c.SapMOrderProcessId == sapOrderProcess.Id);
            }

            return new SapMOrderProcessWithCooperate
            {
                ProcessLine = sapOrderProcess,
                CooperateLine = cooperateLine
            };
        }

        /// <summary>
        /// 取得SAP下一个道序
        /// </summary>
        /// <param name="currentSapProcessId">当前道序ID</param>
        /// <returns>下个道序（含外协信息）</returns>
        [DisableAuditing]
        public async Task<SapMOrderProcessWithCooperate> GetNextSapMOrderProcess(Guid currentSapProcessId)
        {
            var sapOrder = (await this._sapMOrderProcessRepository.GetAll()
                .Include(p => p.SapMOrder)
                .FirstAsync(o => o.Id == currentSapProcessId)).SapMOrder;

            var sapProcessList = sapOrder.OrderProcess.OrderBy(o => o.OperationNumber).ToList();

            for (int i = 0; i < sapProcessList.Count - 1 /*忽略最后道序(无nextProcess)*/; i++)
            {
                if (sapProcessList[i].Id == currentSapProcessId)
                {
                    var nextProcess = sapProcessList[i + 1];
                    var sapCooperate = await this._sapMOrderProcessCooperateRepository.GetAll()
                        .Include(co => co.SapMOrderProcess)
                        .FirstOrDefaultAsync(co => co.SapMOrderProcessId == nextProcess.Id);

                    return new SapMOrderProcessWithCooperate
                    {
                        ProcessLine = nextProcess,
                        CooperateLine = sapCooperate
                    };
                }
            }
            return null;
        }

        /// <summary>
        /// 取得首个道序
        /// </summary>
        /// <param name="orderId">订单ID</param>
        [DisableAuditing]
        public async Task<SapMOrderProcessWithCooperate> GetFirstSapMOrderProcess(Guid orderId)
        {
            var firstProcess = await this._sapMOrderProcessRepository.GetAll()
                .Include(p => p.SapMOrder)
                .Where(p => p.SapMOrderId == orderId)
                .OrderBy(p => p.OperationNumber)
                .FirstOrDefaultAsync();

            if (firstProcess != null)
            {
                var sapCooperate = await this._sapMOrderProcessCooperateRepository.GetAll()
                    .Include(co => co.SapMOrderProcess)
                    .FirstOrDefaultAsync(co => co.SapMOrderProcessId == firstProcess.Id);

                return new SapMOrderProcessWithCooperate
                {
                    ProcessLine = firstProcess,
                    CooperateLine = sapCooperate
                };
            }
            return null;
        }

        /// <summary>
        /// 封装了SAP工艺及外协信息
        /// </summary>
        public sealed class SapMOrderProcessWithCooperate
        {
            public SapMOrderProcess ProcessLine { get; set; }
            public SapMOrderProcessCooperate CooperateLine { get; set; }
        }

    }
}
