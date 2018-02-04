using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Configuration;

namespace Etupirka.Application.Manufacture.Configuration
{
    public class ManufactureSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition("Cooperate_EastCustomerId", "100000"),                    //FS中西厂区客户ID
                new SettingDefinition("Cooperate_WestCustomerId", "200000"),                    //FS中西厂区客户ID
                new SettingDefinition("Cooperate_WestCooperateItemNumber", "FXZXC0000002"),     //FS中西厂区工艺外协物料编码

                new SettingDefinition("Cooperate_Planner", "ZYR"),                              //FS中西厂区工艺外协计划员
                new SettingDefinition("Cooperate_WorkCenter", "19"),                            //SAP工艺外协成本科目号
                new SettingDefinition("Cooperate_DefaultPointOfUse", "1"),                      //FS中西厂区工艺外协默认使用点

                new SettingDefinition("Cooperate_FsMoStockRoom", "WZ"),                         //FS中西厂区工艺外协半成品库
                new SettingDefinition("Cooperate_FsMoStockBin", "001001"),                      //FS中西厂区工艺外协半成品位
                new SettingDefinition("Cooperate_FsShipStockRoom", "XC"),                       //FS中西厂区工艺外协发运库
                new SettingDefinition("Cooperate_FsShipStockBin", "000001"),                    //FS中西厂区工艺外协发运位

                new SettingDefinition("Cooperate_SapWestEKORG", "2000"),    //SAP西厂区采购组织
                new SettingDefinition("Cooperate_SapWestEKGRP", "251"),     //SAP西厂区工艺外协采购组
                new SettingDefinition("Cooperate_SapWestBUKRS", "2000"),    //SAP西厂区公司代码
                new SettingDefinition("Cooperate_SapWestWERKS", "2000"),    //SAP西厂区工厂代码

                new SettingDefinition("Cooperate_SapWestMWSKZ", "J1"),              //SAP西厂区销售税代码
                new SettingDefinition("Cooperate_SapWestMATKL", "908930"),          //SAP西厂区工艺外协物料组
                new SettingDefinition("Cooperate_SapWestSAKTO", "5001040000"),      //SAP西厂区工艺外协总帐科目

                new SettingDefinition("Cooperate_SapPoRequestReleaseRelCode", "55"),    //采购申请单审批部门代码(55: 生产管理部审批)
                new SettingDefinition("Cooperate_SapPoReleaseRelCode", "50"),           //SAP采购订单审批部门代码(50: 计划部领导)

                new SettingDefinition("Cooperate_SapXcSupplierCode", "0000200144"),    //新场公司在SAP中的供方代码
            };
        }
    }
}
