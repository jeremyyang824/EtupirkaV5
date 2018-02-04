using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Abp.Configuration;
using Abp.Runtime.Caching;
using Etupirka.Domain.External.Fsti;

#if IsPublishVersion
using Etupirka.Implement.External.STMC.FSTI;
#else
using Etupirka.Implement.External.STMC.FSTI.Test;
#endif


namespace Etupirka.Implement.External.Infrasturctures
{
    /// <summary>
    /// ForthShift接口帮助类
    /// </summary>
    public class FSTIHelper : IFSTIHelper
    {
        private readonly ISettingManager _settingManager;
        private readonly ICacheManager _cacheManager;
        private readonly ITypedCache<string, FstiToken> _fstiTokenCache;

        #region FS配置参数

        /// <summary>
        /// 域名
        /// </summary>
        public string FSTI_NetDomain { get; private set; }

        /// <summary>
        /// 域用户名
        /// </summary>
        public string FSTI_NetUserName { get; private set; }

        /// <summary>
        /// 域密码
        /// </summary>
        public string FSTI_NetPassword { get; private set; }

        /// <summary>
        /// FS系统用户名
        /// </summary>
        public string FSTI_Interface_SystemUid { get; private set; }

        /// <summary>
        /// FS系统用户密码
        /// </summary>
        public string FSTI_Interface_SystemPwd { get; private set; }

        #endregion

        public FSTIHelper(ISettingManager settingManager, ICacheManager cacheManager)
        {
            this._settingManager = settingManager;
            this._cacheManager = cacheManager;
            _fstiTokenCache = _cacheManager.GetCache<string, FstiToken>("FSTI_ConfigurationCache");

            this.FSTI_NetDomain = this._settingManager.GetSettingValue("FSTI_NetDomain");
            this.FSTI_NetUserName = this._settingManager.GetSettingValue("FSTI_NetUserName");
            this.FSTI_NetPassword = this._settingManager.GetSettingValue("FSTI_NetPassword");
            this.FSTI_Interface_SystemUid = this._settingManager.GetSettingValue("FSTI_Interface_SystemUid");
            this.FSTI_Interface_SystemPwd = this._settingManager.GetSettingValue("FSTI_Interface_SystemPwd");
        }

        /// <summary>
        /// 登录并开始执行FSTI相关接口
        /// 通过using方式使用，自动注销FSTI
        /// </summary>
        public FstiContext BeginFsti()
        {
            FstiToken token = this.FSLogin();
            FstiDisposer disposer = new FstiDisposer(token, this);
            return disposer;
        }

        /// <summary>
        /// 登录FS系统
        /// </summary>
        public FstiToken FSLogin(string username, string password)
        {
            username = username?.Trim();
            password = password?.Trim();
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException("username");
            if (password == null)
                throw new ArgumentNullException("password");

            string cackeKey = FstiToken.GetCacheItemKey(username, password);
            FstiToken tokenCacheItem = _fstiTokenCache.GetOrDefault(cackeKey);    //从缓存获取token

            using (FSTI_GUID fstiService = this.CreateFSTIService())
            {
                if (tokenCacheItem != null)
                {
                    //验证Token是否有效
                    string validateResult = fstiService.logonPool(tokenCacheItem.Token.ToString());
                    if (FstiResultParser.IsLoginTokenValidate(validateResult))
                        return tokenCacheItem;
                    else
                        _fstiTokenCache.Remove(tokenCacheItem.CacheItemKey);   //移除失效token的缓存
                }
                return this.LoginCore(fstiService, username, password);
            }
        }

        /// <summary>
        /// 使用系统默认用户名密码登录
        /// </summary>
        public FstiToken FSLogin()
        {
            if (string.IsNullOrWhiteSpace(this.FSTI_Interface_SystemUid))
                throw new ApplicationException("找不到配置[FSTI_Interface_SystemUid]！");
            if (string.IsNullOrWhiteSpace(this.FSTI_Interface_SystemPwd))
                throw new ApplicationException("找不到配置[FSTI_Interface_SystemPwd]！");

            return this.FSLogin(this.FSTI_Interface_SystemUid, this.FSTI_Interface_SystemPwd);
        }

        /// <summary>
        /// 注销FS系统
        /// </summary>
        public void FSLogout(Guid fstiToken)
        {
            if (fstiToken != Guid.Empty)
            {
                using (FSTI_GUID fstiService = this.CreateFSTIService())
                {
                    //注销ERP登录
                    fstiService.logout(fstiToken.ToString());
                }
            }
        }

        /// <summary>
        /// 注销FS系统
        /// </summary>
        public void FSLogout(FstiToken fstiToken)
        {
            this.FSLogout(fstiToken.Token);
            _fstiTokenCache.Remove(fstiToken.CacheItemKey);   //移除缓存
        }


        protected virtual FstiToken LoginCore(FSTI_GUID fstiService, string username, string password)
        {
            if (fstiService == null)
                throw new ArgumentNullException("fstiService");

            //登录ERP并获得登录GUID
            string loginResult = fstiService.logon(username, password);
            Guid token = new Guid(loginResult);

            //更新缓存
            FstiToken cacheItem = new FstiToken(username, password, token);
            _fstiTokenCache.Set(cacheItem.CacheItemKey, cacheItem);
            return cacheItem;
        }


        /// <summary>
        /// 创建FSTI WebService实例
        /// </summary>
        /// <returns></returns>
        public virtual FSTI_GUID CreateFSTIService()
        {
            FSTI_GUID fsti = new FSTI_GUID();
            fsti.Credentials = new NetworkCredential(this.FSTI_NetUserName, this.FSTI_NetPassword, this.FSTI_NetDomain);
            return fsti;
        }

        /// <summary>
        /// 用于注销FSTI接口
        /// </summary>
        public sealed class FstiDisposer : FstiContext
        {
            public FSTIHelper FSTIHelper { get; private set; }

            public FstiDisposer(FstiToken fstiToken, FSTIHelper fstiHelper)
                : base(fstiToken)
            {
                if (fstiHelper == null)
                    throw new ArgumentNullException("fstiHelper");

                this.FSTIHelper = fstiHelper;
            }

            public override void Dispose()
            {
                this.FSTIHelper.FSLogout(this.FstiToken);
            }
        }
    }
}
