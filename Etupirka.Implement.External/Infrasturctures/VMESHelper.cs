using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Configuration;
using Abp.Runtime.Caching;
using Etupirka.Domain.External.Vmes;
using Etupirka.Implement.External.STMC.VMES;

namespace Etupirka.Implement.External.Infrasturctures
{
    /// <summary>
    /// MES接口帮助类
    /// </summary>
    public class VMESHelper : IVMESHelper
    {
        private readonly ISettingManager _settingManager;
        private readonly ICacheManager _cacheManager;

        public VMESHelper(ISettingManager settingManager, ICacheManager cacheManager)
        {
            this._settingManager = settingManager;
            this._cacheManager = cacheManager;
        }

        /// <summary>
        /// 创建PICK同步服务
        /// </summary>
        public virtual PickChange SyncPickVMESService()
        {
            PickChange vmes = new PickChange();
            return vmes;
        }
    }
}
