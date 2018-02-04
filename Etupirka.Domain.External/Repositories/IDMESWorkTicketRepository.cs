using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Etupirka.Domain.External.Entities.Dmes;

namespace Etupirka.Domain.External.Repositories
{
    public interface IDMESWorkTicketRepository : IRepository
    {
        Task<IPagedResult<DmesOrderOutput>> FindDispatchedOrderPagerByWorkCenter(DmesFindDispatchedOrderByWorkCenterInput input);

        Task<IList<DmesDispatchedIdOutput>> GetDispatchedWorkTicketIDsLatest(DmesGetDispatchedIdsInput input);

        Task<IList<DmesOrderOutput>> FindDispatchedOrdersByDispatchWorkID(DmesFindDispatchedOrderByTicketInput input);
    }
}
