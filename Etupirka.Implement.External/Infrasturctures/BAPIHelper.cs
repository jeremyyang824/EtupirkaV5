using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Abp.Configuration;
using Abp.Runtime.Caching;
using Etupirka.Domain.External.Bapi;

#if IsPublishVersion
using Etupirka.Implement.External.STMC.BAPI;
#else
using Etupirka.Implement.External.STMC.BAPI.Test;
#endif

namespace Etupirka.Implement.External.Infrasturctures
{
    /// <summary>
    /// SAP BAPI接口帮助类
    /// </summary>
    public class BAPIHelper : IBAPIHelper
    {
        private readonly ISettingManager _settingManager;
        private readonly ICacheManager _cacheManager;

        #region BAPI配置参数

        /// <summary>
        /// 域名
        /// </summary>
        public string BAPI_NetDomain { get; private set; }

        /// <summary>
        /// 域用户名
        /// </summary>
        public string BAPI_NetUserName { get; private set; }

        /// <summary>
        /// 域密码
        /// </summary>
        public string BAPI_NetPassword { get; private set; }

        #endregion

        public BAPIHelper(ISettingManager settingManager, ICacheManager cacheManager)
        {
            this._settingManager = settingManager;
            this._cacheManager = cacheManager;

            this.BAPI_NetDomain = this._settingManager.GetSettingValue("BAPI_NetDomain");
            this.BAPI_NetUserName = this._settingManager.GetSettingValue("BAPI_NetUserName");
            this.BAPI_NetPassword = this._settingManager.GetSettingValue("BAPI_NetPassword");
        }

        #region 创建服务

        /// <summary>
        /// 创建采购订单服务
        /// </summary>
        public virtual ZMM_PO_WS CreatePoBAPIService()
        {
            ZMM_PO_WS bapi = new ZMM_PO_WS();
            bapi.Credentials = new NetworkCredential(this.BAPI_NetUserName, this.BAPI_NetPassword, this.BAPI_NetDomain);
            return bapi;
        }

        /// <summary>
        /// 获取生产订单服务
        /// </summary>
        public virtual ZBAPI_PRODORD_GET_LIST_YJ SyncMoBAPIService()
        {
            ZBAPI_PRODORD_GET_LIST_YJ bapi = new ZBAPI_PRODORD_GET_LIST_YJ();
            bapi.Credentials = new NetworkCredential(this.BAPI_NetUserName, this.BAPI_NetPassword, this.BAPI_NetDomain);
            return bapi;
        }

        /// <summary>
        /// 获取生产订单工序服务
        /// </summary>
        public virtual ZBAPI_PRODORD_GET_OPERATION_YJ SyncMoProcessBAPIService()
        {
            ZBAPI_PRODORD_GET_OPERATION_YJ bapi = new ZBAPI_PRODORD_GET_OPERATION_YJ();
            bapi.Credentials = new NetworkCredential(this.BAPI_NetUserName, this.BAPI_NetPassword, this.BAPI_NetDomain);
            return bapi;
        }

        /// <summary>
        /// 获取生产订单质检检查服务
        /// </summary>
        public virtual ZQM_DISPATCH QmBAPIService()
        {
            ZQM_DISPATCH bapi = new ZQM_DISPATCH();
            bapi.Credentials = new NetworkCredential(this.BAPI_NetUserName, this.BAPI_NetPassword, this.BAPI_NetDomain);
            return bapi;
        }

        /// <summary>
        /// 采购订单查询服务
        /// </summary>
        /// <returns></returns>
        public virtual BAPI_PO_GETDETAIL SyncPoBAPIService()
        {
            BAPI_PO_GETDETAIL bapi = new BAPI_PO_GETDETAIL();
            bapi.Credentials = new NetworkCredential(this.BAPI_NetUserName, this.BAPI_NetPassword, this.BAPI_NetDomain);
            return bapi;
        }

        /// <summary>
        /// 采购订单审批服务
        /// </summary>
        public virtual ZPRRelease ReleasePoBAPIService()
        {
            ZPRRelease bapi = new ZPRRelease();
            bapi.Credentials = new NetworkCredential(this.BAPI_NetUserName, this.BAPI_NetPassword, this.BAPI_NetDomain);
            return bapi;
        }

        #endregion

    }
}
