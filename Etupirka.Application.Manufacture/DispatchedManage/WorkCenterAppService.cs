using Abp.Authorization;
using Etupirka.Application.Portal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Etupirka.Application.Manufacture.DispatchedManage.Dto;
using Etupirka.Domain.Manufacture.Entities;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.AutoMapper;
using Etupirka.Domain.External.Repositories;
using Etupirka.Domain.External.Entities.Dmes;
using System.Data.Entity;

namespace Etupirka.Application.Manufacture.DispatchedManage
{
    /// <summary>
    /// 工作中心映射管理
    /// </summary>
    [AbpAuthorize]
    public class WorkCenterAppService : EtupirkaAppServiceBase, IWorkCenterAppService
    {
        public readonly IRepository<SapWorkCenter, Guid> _workcenterRepository;
        public readonly IDMESWorkCenterRepository _dmesWorkCenterRepository;
        public readonly IDMESWorkTicketRepository _dmesWorkTicketRepository;

        public WorkCenterAppService(
            IRepository<SapWorkCenter, Guid> workcenterRepository, 
            IDMESWorkCenterRepository dmesWorkCenterRepository,
            IDMESWorkTicketRepository dmesWorkTicketRepository)
        {
            this._workcenterRepository = workcenterRepository;
            this._dmesWorkCenterRepository = dmesWorkCenterRepository;
            this._dmesWorkTicketRepository = dmesWorkTicketRepository;
        }

        //public async Task<PagedResultDto<WorkCenterOutput>> FindWorkCenterList(FindWorkCentersInput input)
        //{
        //    var query = this.getSapWorkCenterWithOrderQuery(input);
        //    var count = await query.CountAsync();
        //    var list = await query.PageBy(input).ToListAsync();
        //    var results = list.MapTo<List<WorkCenterOutput>>();
        //    //var results = (await query.PageBy(input).ToListAsync()).MapTo<List<WorkCenterOutput>>();
        //    return new PagedResultDto<WorkCenterOutput>(count, results);

        //}

        //private IQueryable<SapWorkCenter> getSapWorkCenterWithOrderQuery(FindWorkCentersInput input)
        //{
        //    var wquery = _workcenterRepository.GetAll()
        //         .WhereIf(!string.IsNullOrWhiteSpace(input.WorkCenterCode), w => w.WorkCenterCode.Contains(input.WorkCenterCode.Trim()))
        //         .WhereIf(!string.IsNullOrWhiteSpace(input.WorkCenterName), w => w.WorkCenterName.Contains(input.WorkCenterName.Trim()))
        //         .OrderBy(w => w.WorkCenterCode);

        //    return wquery;
        //}

        public async Task<IPagedResult<DmesWorkCenterOutput>> FindWorkCenterList(FindWorkCentersInput input)
        {
            return await _dmesWorkCenterRepository.GetWorkCenters(new DmesGetWorkCenterInput
            {
                WorkCenterCode = input.WorkCenterCode,
                WorkCenterName = input.WorkCenterName,
                SkipCount = input.SkipCount,
                MaxResultCount = input.MaxResultCount,
            });
        }

        public async Task<DmesWorkCenterOutput> GetWorkCenter(int id)
        {
            return await _dmesWorkCenterRepository.GetWorkCenter(id);
        }
    }
}
