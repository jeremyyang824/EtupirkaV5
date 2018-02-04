using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Caching;
using Etupirka.Domain.Manufacture.Entities;
using Etupirka.Domain.Portal;

namespace Etupirka.Domain.Manufacture.Services
{
    /// <summary>
    /// 供应商管理
    /// </summary>
    public class SapSupplierManager : DomainService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<SapSupplierMaper, int> _sapSupplierMaperRepository;

        private readonly ITypedCache<string, SapSupplierMaper> _sapSupplierMaperCache;

        public SapSupplierManager(
            ICacheManager cacheManager,
            IRepository<SapSupplierMaper, int> sapSupplierMaperRepository)
        {
            this._cacheManager = cacheManager;
            this._sapSupplierMaperRepository = sapSupplierMaperRepository;

            _sapSupplierMaperCache = _cacheManager
                .GetCache<string, SapSupplierMaper>("SapSupplierManager._sapSupplierMaperCache");
        }

        /// <summary>
        /// 根据SAP供方代码获取映射信息
        /// 若为找到对应的映射信息，则返回"SapSupplierMaper.Empty"
        /// </summary>
        /// <param name="sapSupplierCode">SAP供方代码</param>
        public async Task<SapSupplierMaper> GetSupplierBySapCode(string sapSupplierCode)
        {
            if (sapSupplierCode == "100000")
                throw new DomainException($"供应商代码{sapSupplierCode}无效，请使用精确的东厂区代码！");

            if (string.IsNullOrWhiteSpace(sapSupplierCode))
                throw new DomainException("sapSupplierCode empty!");

            //return await _sapSupplierMaperCache.GetAsync(sapSupplierCode, async key =>
            //{
            //    var beanFromDb = await _sapSupplierMaperRepository.FirstOrDefaultAsync(m => m.SapSupplierCode == sapSupplierCode);
            //    return beanFromDb ?? SapSupplierMaper.Empty;
            //});
            var mapper = await _sapSupplierMaperRepository.FirstOrDefaultAsync(m => m.SapSupplierCode == sapSupplierCode);
            return mapper ?? SapSupplierMaper.Empty;
        }
    }
}
