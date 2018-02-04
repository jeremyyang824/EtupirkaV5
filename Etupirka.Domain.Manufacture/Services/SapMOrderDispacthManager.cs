using Abp.Auditing;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Etupirka.Domain.External.Entities.Dmes;
using Etupirka.Domain.External.Repositories;
using Etupirka.Domain.Manufacture.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.Manufacture.Services
{
    public class SapMOrderDispacthManager : DomainService
    {
        private readonly IRepository<SapMOrderProcessDispatchPrepare, int> _sapMOrderProcessDispatchRepository;
        private readonly IDMESWorkTicketRepository _dMESWorkTicketRepository;

        public SapMOrderDispacthManager(
            IRepository<SapMOrderProcessDispatchPrepare, int> sapMOrderProcessDispatchRepository, IDMESWorkTicketRepository dMESWorkTicketRepository)
        {
            this._sapMOrderProcessDispatchRepository = sapMOrderProcessDispatchRepository;
            this._dMESWorkTicketRepository = dMESWorkTicketRepository;
        }



        /// <summary>
        /// 封装了SAP工艺及加工准备信息
        /// </summary>
        public sealed class SapMOrderProcessPreparation
        {
            public DmesOrderOutput DmesOrder { get; set; }
            public SapMOrderProcessDispatchPrepare DispatchPrepare { get; set; }
        }

    }
}
