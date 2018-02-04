using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Etupirka.Domain.Manufacture.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etupirka.Domain.Manufacture.Services
{
    public class SapWorkCenterManager : DomainService
    {
        private readonly IRepository<SapWorkCenter, Guid> _workcenterRepository;

        public SapWorkCenterManager(IRepository<SapWorkCenter, Guid> workcenterRepository)
        {
            this._workcenterRepository = workcenterRepository;
        }

        public async Task<List<SapWorkCenter>> GetAllWorkCenters()
        {
            var list = await this._workcenterRepository.GetAll().ToListAsync();
            return list;
        }

        public async Task<List<SapWorkCenter>> FindWorkCenterByCondition()
        {
            var list = await this._workcenterRepository.GetAll().ToListAsync();
            return list;
        }
    }
}
