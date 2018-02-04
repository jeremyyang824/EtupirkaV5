using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Etupirka.Application.Manufacture.MetaManage.Dto;
using Etupirka.Application.Portal;
using Etupirka.Domain.Manufacture.Entities;

namespace Etupirka.Application.Manufacture.MetaManage
{
    /// <summary>
    /// SAP/FS工艺映射管理
    /// </summary>
    [AbpAuthorize]
    public class ProcessCodeMapAppService : EtupirkaAppServiceBase, IProcessCodeMapAppService
    {
        private readonly IRepository<ProcessCodeMap, int> _processCodeMapRepository;

        public ProcessCodeMapAppService(
            IRepository<ProcessCodeMap, int> processCodeMapRepository)
        {
            this._processCodeMapRepository = processCodeMapRepository;
        }

        /// <summary>
        /// 取得所有工艺映射
        /// </summary>
        public async Task<ListResultDto<ProcessCodeMapOutput>> GetAllProcessCodeMap()
        {
            var list = await this._processCodeMapRepository.GetAll().ToListAsync();
            return new ListResultDto<ProcessCodeMapOutput>(list.MapTo<List<ProcessCodeMapOutput>>());
        }
    }
}
