using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Etupirka.Application.Manufacture.DispatchedManage.Dto;
using Etupirka.Application.Portal;
using Etupirka.Domain.External.Entities.Dmes;
using Etupirka.Domain.External.Repositories;
using Etupirka.Domain.Manufacture.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using Etupirka.Domain.External.Wintool;

namespace Etupirka.Application.Manufacture.DispatchedManage
{

    /// 工作中心映射管理

    [AbpAuthorize]
    public class DispatchedOrderAppService : EtupirkaAppServiceBase, IDispatchedOrderAppService
    {
        private readonly IRepository<SapMOrderProcessDispatchPrepare, int> _sapMOrderProcessDispatchPrepareRepository;
        private readonly IRepository<SapMOrderProcessDispatchPrepareStep, int> _sapMOrderProcessDispatchPrepareStepRepository;
        
        public readonly IDMESWorkTicketRepository _dmesWorkTicketRepository;


        public DispatchedOrderAppService(IRepository<SapMOrderProcessDispatchPrepare, int> sapMOrderProcessDispatchPrepareRepository, IRepository<SapMOrderProcessDispatchPrepareStep, int> sapMOrderProcessDispatchPrepareStepRepository,
            IDMESWorkTicketRepository dmesWorkTicketRepository)
        {
            this._sapMOrderProcessDispatchPrepareRepository = sapMOrderProcessDispatchPrepareRepository;
            this._sapMOrderProcessDispatchPrepareStepRepository = sapMOrderProcessDispatchPrepareStepRepository;
            this._dmesWorkTicketRepository = dmesWorkTicketRepository;           
        }

        public async Task<PagedResultDto<DispatchedOrderOutput>> FindDispatchOrderPagerByWorkCenter(FindOrdersInput input)
        {
            var query = await _dmesWorkTicketRepository.FindDispatchedOrderPagerByWorkCenter(new DmesFindDispatchedOrderByWorkCenterInput
            {
                WorkCenterId = input.WorkCenterID,
                SkipCount = input.SkipCount,
                MaxResultCount = input.MaxResultCount
            });

            var count = (int)query.TotalCount;
            //var result = query.Items.MapTo<List<DispatchedOrderOutput>>();

            List<DispatchedOrderOutput> result = new List<DispatchedOrderOutput>();
            foreach (var dOrder in query.Items.ToList())
            {
                var orderOut = dOrder.MapTo<DispatchedOrderOutput>();
                var prepareDefault = await _sapMOrderProcessDispatchPrepareRepository.GetAll()
                .Include(p => p.PrepareSteps)
                .Where(p => p.DispatchWorKTicketID == orderOut.DispatchWorKTicketID).FirstOrDefaultAsync();

                if (prepareDefault != null && prepareDefault.Id > 0)
                {
                    //if (prepareDefault.PrepareSteps == null)
                    //    prepareDefault.PrepareSteps = new List<SapMOrderProcessDispatchPrepareStep>();

                    var prepare = prepareDefault.MapTo<DispatchOrderPrepareOutput>();
                    orderOut.PrepareInfo = prepare;
                }
                result.Add(orderOut);
            }

            return await Task.FromResult(new PagedResultDto<DispatchedOrderOutput>(count, result));
        }


    }
}
