using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Abp.Configuration;
using Abp.Dependency;

namespace Etupirka.Application.Manufacture.Cooperate
{
    /// <summary>
    /// 交接过程配置信息
    /// </summary>
    public class CooperateConfigurations : ITransientDependency
    {
        private readonly ISettingManager _settingManager;
        public CooperateConfigurations(ISettingManager settingManager)
        {
            this._settingManager = settingManager;
        }

        /// <summary>
        /// FS中东厂区客户ID
        /// </summary>
        public string Cooperate_EastCustomerId => this._settingManager.GetSettingValue("Cooperate_EastCustomerId");

        /// <summary>
        /// FS中西厂区客户ID
        /// </summary>
        public string Cooperate_WestCustomerId => this._settingManager.GetSettingValue("Cooperate_WestCustomerId");

        /// <summary>
        /// FS中西厂区工艺外协物料编码
        /// </summary>
        public string Cooperate_WestCooperateItemNumber => this._settingManager.GetSettingValue("Cooperate_WestCooperateItemNumber");

        /// <summary>
        /// FS中西厂区工艺外协计划员
        /// </summary>
        public string Cooperate_Planner => this._settingManager.GetSettingValue("Cooperate_Planner");

        /// <summary>
        /// FS中西厂区工艺外协工作中心
        /// </summary>
        public string Cooperate_WorkCenter => this._settingManager.GetSettingValue("Cooperate_WorkCenter");

        /// <summary>
        /// FS中西厂区工艺外协默认使用点
        /// </summary>
        public string Cooperate_DefaultPointOfUse => this._settingManager.GetSettingValue("Cooperate_DefaultPointOfUse");


        /// <summary>
        /// FS中西厂区工艺外协半成品库
        /// </summary>
        public string Cooperate_FsMoStockRoom => this._settingManager.GetSettingValue("Cooperate_FsMoStockRoom");

        /// <summary>
        /// FS中西厂区工艺外协半成品位
        /// </summary>
        public string Cooperate_FsMoStockBin => this._settingManager.GetSettingValue("Cooperate_FsMoStockBin");

        /// <summary>
        /// FS中西厂区工艺外协发运库
        /// </summary>
        public string Cooperate_FsShipStockRoom => this._settingManager.GetSettingValue("Cooperate_FsShipStockRoom");

        /// <summary>
        /// FS中西厂区工艺外协发运位
        /// </summary>
        public string Cooperate_FsShipStockBin => this._settingManager.GetSettingValue("Cooperate_FsShipStockBin");


        /// <summary>
        /// SAP西厂区采购组织
        /// </summary>
        public string Cooperate_SapWestEKORG => this._settingManager.GetSettingValue("Cooperate_SapWestEKORG");

        /// <summary>
        /// SAP西厂区工艺外协采购组
        /// </summary>
        public string Cooperate_SapWestEKGRP => this._settingManager.GetSettingValue("Cooperate_SapWestEKGRP");

        /// <summary>
        /// SAP西厂区公司代码
        /// </summary>
        public string Cooperate_SapWestBUKRS => this._settingManager.GetSettingValue("Cooperate_SapWestBUKRS");

        /// <summary>
        /// SAP西厂区工厂代码
        /// </summary>
        public string Cooperate_SapWestWERKS => this._settingManager.GetSettingValue("Cooperate_SapWestWERKS");

        /// <summary>
        /// SAP西厂区销售税代码
        /// </summary>
        public string Cooperate_SapWestMWSKZ => this._settingManager.GetSettingValue("Cooperate_SapWestMWSKZ");

        /// <summary>
        /// SAP西厂区工艺外协物料组
        /// </summary>
        public string Cooperate_SapWestMATKL => this._settingManager.GetSettingValue("Cooperate_SapWestMATKL");

        /// <summary>
        /// SAP西厂区工艺外协总帐科目
        /// </summary>
        public string Cooperate_SapWestSAKTO => this._settingManager.GetSettingValue("Cooperate_SapWestSAKTO");

        /// <summary>
        /// SAP采购申请单审批部门代码
        /// 50  计划部领导
        /// 51  综合计划部审批
        /// 52  设备动力部审批
        /// 53  行政事业部审批
        /// 54  安全保卫部审批
        /// 55  生产管理部审批
        /// 56  财务会计部审批
        /// </summary>
        public string Cooperate_SapPoRequestReleaseRelCode => this._settingManager.GetSettingValue("Cooperate_SapPoRequestReleaseRelCode");

        /// <summary>
        /// SAP采购订单审批部门代码
        /// </summary>
        public string Cooperate_SapPoReleaseRelCode => this._settingManager.GetSettingValue("Cooperate_SapPoReleaseRelCode");

        /// <summary>
        /// 新场公司在SAP中的供方代码
        /// </summary>
        public string Cooperate_SapXcSupplierCode => this._settingManager.GetSettingValue("Cooperate_SapXcSupplierCode");
    }
}
