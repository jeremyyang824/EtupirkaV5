using System;
using System.Collections.Generic;
using System.Linq;
using Etupirka.Domain.External.Bapi;
using Etupirka.Domain.External.Entities.Bapi;
using Etupirka.Domain.External.Repositories;
using Etupirka.Implement.External.Infrasturctures;
using Etupirka.Domain.Portal.Utils;

#if IsPublishVersion
using Etupirka.Implement.External.STMC.BAPI;
#else
using Etupirka.Implement.External.STMC.BAPI.Test;
#endif

namespace Etupirka.Implement.External.Repositories
{
    /// <summary>
    /// SAP BAPI
    /// </summary>
    public class BAPIRepository : EtupirkaExternalRepositoryBase, IBAPIRepository
    {
        private readonly BAPIHelper _bapiHelper;

        public BAPIRepository(BAPIHelper bapiHelper)
        {
            this._bapiHelper = bapiHelper;
        }

        /// <summary>
        /// 创建SAP采购订单
        /// </summary>
        /// <returns>执行结果（含采购订单号）</returns>
        public BapiResult<string> PurcharseOrderCreate(IList<PoCreateInput> inputs)
        {
            if (inputs.Count < 1)
                throw new ArgumentNullException("inputs");

            using (var poBapiService = this._bapiHelper.CreatePoBAPIService())
            {
                //构建参数
                ZncrKeyvalue[] zncr = new ZncrKeyvalue[0];
                Bapiret2[] ret2 = new Bapiret2[0];

                ZmmPodata[] zmmPodata = inputs.Select(input => new ZmmPodata
                {
                    Bsart = input.BSART,
                    Lifnr = input.LIFNR,
                    Ekorg = input.EKORG,
                    Ekgrp = input.EKGRP,
                    Bukrs = input.BUKRS,
                    Ihrez = input.IHREZ,
                    Ebelp = input.EBELP,
                    Knttp = input.KNTTP,
                    Matnr = input.MATNR,
                    Txz01 = input.TXZ01,
                    Menge = Convert.ToDecimal(input.MENGE.ToString("0.00")),
                    Meins = input.MEINS,
                    Eeind = input.EEIND?.ToString("yyyyMMdd"),
                    Netpr = Convert.ToDecimal(input.NETPR.ToString("0.00")),
                    Waers = input.WAERS,
                    Matkl = input.MATKL,
                    Werks = input.WERKS,
                    Bednr = input.BEDNR,
                    Afnam = input.AFNAM,
                    Mwskz = input.MWSKZ,
                    Sakto = input.SAKTO,
                    Kostl = input.KOSTL,
                    Anln1 = input.ANLN1,
                    Aufnr = input.AUFNR,
                    Str1 = input.STR1,
                    Str2 = input.STR2,
                    Str3 = input.STR3,
                    Bstae = input.BSTAE,
                    Preqno = input.PREQNO,
                    Preqitem = input.PREQITEM,
                    Epstyp = input.EPSTP,
                    WbsElement = input.WbsElement,
                }).ToArray();

                //执行接口
                poBapiService.ZmmFunPoCreate(ref zncr, ref ret2, ref zmmPodata);
                if (zncr?.Length > 0)
                {
                    var resultMessage = zncr[0].Value;  //执行结果信息
                    var poNumber = zncr[0].Keyid;   //采购订单

                    if (string.IsNullOrWhiteSpace(poNumber))
                    {
                        return new BapiResult<string>(false, resultMessage);
                    }
                    return new BapiResult<string>(true, resultMessage) { ExtensionData = poNumber };
                }
                return new BapiResult<string>(false, "BAPI ZmmFunPoCreate执行失败!");
            }
        }

        /// <summary>
        /// SAP采购订单入库
        /// </summary>
        public BapiResult<string> PurcharseOrderFinish(PoFinishInput input)
        {
            using (var poBapiService = this._bapiHelper.CreatePoBAPIService())
            {
                //构建参数
                Bapiret2[] ret2 = new Bapiret2[0];
                Bapi2017GmItemCreate[] gmItems = new Bapi2017GmItemCreate[]
                {
                    new Bapi2017GmItemCreate
                    {
                        Material = input.MATERIAL,
                        Plant = input.PLANT,
                        StgeLoc = input.STGE_LOC,
                        Batch = input.BATCH,
                        MoveType = input.MOVE_TYPE,
                        StckType = input.STCK_TYPE,
                        Vendor = input.VENDOR,
                        Customer = input.CUSTOMER,
                        EntryQnt = Convert.ToDecimal(input.ENTRY_QNT.ToString("0.00")),
                        EntryUom = input.ENTRY_UOM,
                        PoNumber = input.PO_NUMBER,
                        PoItem = input.PO_ITEM,
                        MvtInd = input.MVT_IND
                    }
                };

                //执行接口
                poBapiService.ZmmFunPoGr(ref ret2, ref gmItems);
                string message = string.Empty;
                if (ret2?.Length > 0)
                {
                    message = ret2[0].Message ?? "";
                    if (ret2[0].Type?.ToUpper() == "E")
                    {
                        return new BapiResult<string>(false, message);
                    }
                }
                return new BapiResult<string>(true, message);
            }
        }

        /// <summary>
        /// 取得SAP订单信息
        /// </summary>
        public GetSapOrdersOutput GetSapOrders(GetSapOrdersInput input)
        {
            using (var moBapiService = this._bapiHelper.SyncMoBAPIService())
            {
                string clto = "";   //是否显示汇总订单
                string mktx = "";   //物料描述（短文本）
                string orfb = "";   //基本完成日期
                string orfe = "";   //基本完成日期
                string orsb = "";   //基本开始日期
                string orse = "";   //基本开始日期
                int pageCurrent = 0;    //当期页号
                bool pageCurrentSpecified = false;
                int pageRow = 0;        //页行数
                bool pageRowSpecified = false;
                string wmpf = "";   //收货方/运达方

                var materialRange = new ZbapiOrderMaterialRange[0]; //查询物料范围
                var mrpCtrlRange = new OrderMrpCntrlRange[0];   //查询MRP控制者范围

                var orderRange = new ZbapiPpOrderrange[0];  //查询生产订单范围
                if (!input.IsNoneOrderNumberRangeBegin && !input.IsNoneOrderNumberRangeEnd)
                    orderRange = new[] { new ZbapiPpOrderrange { Sign = "I", Option = "BT", Low = input.OrderNumberRangeBegin, High = input.OrderNumberRangeEnd } };
                else if (!input.IsNoneOrderNumberRangeBegin)
                    orderRange = new[] { new ZbapiPpOrderrange { Sign = "I", Option = "GE", Low = input.OrderNumberRangeBegin } };
                else if (!input.IsNoneOrderNumberRangeEnd)
                    orderRange = new[] { new ZbapiPpOrderrange { Sign = "I", Option = "LE", High = input.OrderNumberRangeEnd } };

                var perioRange = new OrderPrioRange[0]; //查询订单优先级范围
                var typeRange = new OrderOrderTypeRange[0]; //查询订单类型范围
                var planPlantRange = new[] { new OrderPlanplantRange { Sign = "I", Option = "EQ", Low = input.Plant, High = input.Plant } };    //查询计划工厂范围
                var scheduleRange = new OrderProdSchedRange[0];     //暂时不用
                var prodPlantRange = new OrderProdplantRange[0];    //查询生产工厂范围
                var salesOrderRange = new ZbapiOrderSalesOrdRange[0];   //查询销售订单范围
                var salesOrderItemRange = new OrderSalesOrdItmRange[0]; //查询销售订单行项目范围
                var orderSeqnRange = new OrderSeqNoRange[0];    //暂时不用
                var statusRange = new ZbapiPpStatus[0]; //查询订单状态范围
                var wbsRange = new ZbapiOrderWbsElementRange[0];    //查询WBS范围

                //output
                int totalRows = 0;
                var orderHeads = new ZbapiOrderHeader1[0];

                //获取订单信息
                Bapiret2 result = moBapiService.ZbapiProdordGetListYj(clto, mktx,
                    ref materialRange, ref mrpCtrlRange, orfb, orfe, ref orderHeads, ref orderRange, ref perioRange,
                    orsb, orse, ref typeRange, pageCurrent, pageCurrentSpecified, pageRow, pageRowSpecified,
                    ref planPlantRange, ref scheduleRange, ref prodPlantRange, ref salesOrderItemRange,
                    ref salesOrderRange, ref orderSeqnRange, ref statusRange, ref wbsRange, wmpf, out totalRows);

                if (result?.Type == "E")
                    throw new ApplicationException($"SAP订单[{input.OrderNumberRangeBegin}-{input.OrderNumberRangeEnd}]读取失败:" + result.Message);

                List<BapiOrderOutput> orderDataList = new List<BapiOrderOutput>();
                foreach (var orderBean in orderHeads)
                {
                    BapiOrderOutput orderDto = new BapiOrderOutput
                    {
                        OrderNumber = orderBean.OrderNumber,
                        ProductionPlant = orderBean.ProductionPlant,
                        MRPController = orderBean.MrpController,
                        ProductionScheduler = orderBean.ProductionScheduler,
                        MaterialNumber = orderBean.Material,
                        MaterialDescription = orderBean.MaterialText,
                        MaterialExternal = orderBean.MaterialExternal,
                        MaterialGuid = orderBean.MaterialGuid,
                        MaterialVersion = orderBean.MaterialVersion,
                        RoutingNumber = orderBean.RoutingNo,
                        ScheduleReleaseDate = orderBean.SchedReleaseDate.TryParse<DateTime?>(),
                        ActualReleaseDate = orderBean.ActualReleaseDate.TryParse<DateTime?>(),
                        StartDate = orderBean.StartDate.TryParse<DateTime?>(),
                        FinishDate = orderBean.FinishDate.TryParse<DateTime?>(),
                        ProductionStartDate = orderBean.ProductionStartDate.TryParse<DateTime?>(),
                        ProductionFinishDate = orderBean.ProductionFinishDate.TryParse<DateTime?>(),
                        ActualStartDate = orderBean.ActualStartDate.TryParse<DateTime?>(),
                        ActualFinishDate = orderBean.ActualFinishDate.TryParse<DateTime?>(),
                        TargetQuantity = orderBean.TargetQuantity,
                        ScrapQuantity = orderBean.Scrap,
                        ConfirmedQuantity = orderBean.ConfirmedQuantity,
                        Unit = orderBean.Unit,
                        UnitISO = orderBean.UnitIso,
                        Priority = orderBean.Priority,
                        OrderType = orderBean.OrderType,
                        WBSElement = orderBean.WbsElement,
                        SystemStatus = orderBean.SystemStatus,
                        Batch = orderBean.Batch,
                        ABLAD = orderBean.Ablad,
                        WEMPF = orderBean.Wempf,
                        AufkAenam = orderBean.AufkAenam,
                        AufkAedat = orderBean.AufkAedat.TryParse<DateTime?>(),
                        AufkPhas0 = orderBean.AufkPhas0,
                        AufkPhas1 = orderBean.AufkPhas1,
                        AufkPhas2 = orderBean.AufkPhas2,
                        AufkPhas3 = orderBean.AufkPhas3,
                        SapId = orderBean.Id
                    };

                    //填充道序信息
                    orderDto.BapiOrderProcessList = this.GetSapOrderProcessList(orderDto.RoutingNumber, orderDto.ProductionPlant);
                    orderDataList.Add(orderDto);
                }
                return new GetSapOrdersOutput(totalRows, orderDataList);
            }
        }

        /// <summary>
        /// 取得SAP生产订单工序质检信息
        /// </summary>
        public GetSapMoInspectStateOutput GetSapMoInspectState(GetSapMoInspectStateInput input)
        {
            using (var qmBapiService = this._bapiHelper.QmBAPIService())
            {
                int state = qmBapiService.Zqmmogxexists(input.MOrderNumber, input.OperationSeqnNumber);
                return new GetSapMoInspectStateOutput
                {
                    InspectState = state
                };
            }
        }

        /// <summary>
        /// 采购请求审批
        /// </summary>
        public BapiResult<string> PurcharseOrderRequestRelease(PoRequestReleaseInput input)
        {
            using (var poBapiService = this._bapiHelper.ReleasePoBAPIService())
            {
                //采购申请号若不足10位, 补0
                string requestNumber = input.RequestNumber.Trim().PadLeft(10, '0');
                Bapireturn[] result = new Bapireturn[0];
                string relState = null;
                poBapiService.RequisitionReleaseGen("", requestNumber, input.RelCode, ref result, out relState);

                string message = string.Empty;
                if (result?.Length > 0)
                {
                    message = result[0].Message ?? "";
                    if (result[0].Type?.ToUpper() == "E"
                        && !(message.Contains("批准已生效") || message.Contains("已经批准")))
                    {
                        return new BapiResult<string>(false, message);
                    }
                }
                return new BapiResult<string>(true, message);
            }
        }

        /// <summary>
        /// 采购审批
        /// </summary>
        public BapiResult<string> PurcharseOrderRelease(PoReleaseInput input)
        {
            using (var poBapiService = this._bapiHelper.ReleasePoBAPIService())
            {
                Bapireturn[] result = new Bapireturn[0];
                string relState = null;
                int retCode = 0;
                poBapiService.PoRelease("", input.RelCode, input.PoNumber, ref result, "", out relState, out retCode);

                string message = string.Empty;
                if (result?.Length > 0)
                {
                    message = result[0].Message ?? "";
                    if (result[0].Type?.ToUpper() == "E"
                        && !(message.Contains("批准已生效") || message.Contains("已经批准")))
                    {
                        return new BapiResult<string>(false, message);
                    }
                }
                return new BapiResult<string>(true, message);
            }
        }

        /// <summary>
        /// 取得SAP订单工序
        /// </summary>
        /// <param name="routingNumber">工艺路线号</param>
        /// <param name="werks">工厂</param>
        private List<BapiOrderProcessOutput> GetSapOrderProcessList(string routingNumber, string werks)
        {
            if (string.IsNullOrWhiteSpace(routingNumber))
                throw new ArgumentNullException("routingNumber");
            if (string.IsNullOrWhiteSpace(werks))
                throw new ArgumentNullException("werks");

            using (var mopBapiService = this._bapiHelper.SyncMoProcessBAPIService())
            {
                //input
                var aplzlRange = new ZbapiAplzlRange[0];    //订单的通用计数器选择范围
                var aufplRange = new[] { new ZbapiAufplRange { Sign = "I", Option = "EQ", Low = routingNumber, High = routingNumber } };    //订单中工序的工艺路线号选择范围
                var orderOper = new ZbapiOrderOperationYj[0]; //生产订单工序
                string ltxt = "";   //长文本
                string mastx = "X"; //主数据
                string prtx = "";   //工装工具

                //output
                var docs = new ZbapiDocument[0];        //文档
                var ekGrp = new ZbapiEkgrp[0];          //采购组
                var material = new ZbapiMaterial[0];    //物料清单
                var matlGrp = new ZbapiMatlGroup[0];    //物料组
                var operLtxt = new ZbapiOperationLtxt[0];   //工序长文本
                var oper = new ZbapiOrderOperationYj[0];  //生产订单工序
                var part = new ZbapiOrderPrt[0];         //生产订单资源工具
                var partD = new ZbapiOrderPrtD[0];      //生产订单工装工具-文档
                var partM = new ZbapiOrderPrtM[0];      //生产订单工装工具-物料
                var workCenter = new ZbapiWorkcenter[0];    //工作中心数据

                //获取订单工艺信息
                Bapiret2 result = mopBapiService.ZbapiProdordGetOperationYj(
                    ref docs, ref ekGrp, ref material, ref matlGrp, ref operLtxt, ref oper,
                    ref part, ref partD, ref partM, ref workCenter,
                    ref aplzlRange, ref aufplRange, ref orderOper,
                    ltxt, mastx, prtx, werks);

                if (result?.Type == "E")
                    throw new ApplicationException($"SAP工序[{routingNumber}]读取失败:" + result.Message);

                //构建工作中心索引
                var wcDic = workCenter.ToDictionary(wc => wc.Objid);

                //构建道序
                List<BapiOrderProcessOutput> orderProcessDataList = new List<BapiOrderProcessOutput>();
                foreach (var operBean in oper)
                {
                    var wc = wcDic.TryGetValue(operBean.Arbid);
                    BapiOrderProcessOutput operDto = new BapiOrderProcessOutput
                    {
                        RoutingNumber = operBean.Aufpl,
                        OrderCounter = operBean.Aplzl.TryParse<int>(),
                        OperationNumber = operBean.Vornr,
                        OperationCtrlCode = operBean.Steus,
                        ProductionPlant = operBean.Werks,
                        WorkCenterObjId = operBean.Arbid,
                        WorkCenterCode = wc?.Arbpl ?? "",
                        WorkCenterName = wc?.Ktext ?? "",
                        StandardText = operBean.Ktsch,
                        ProcessText1 = operBean.Ltxa1,
                        ProcessText2 = operBean.Ltxa2,
                        UMREN = operBean.Umren,
                        UMREZ = operBean.Umrez,
                        Unit = operBean.Meinh,
                        BaseQuantity = operBean.Bmsch,
                        ProcessQuantity = operBean.Mgvrg,
                        ScrapQuantity = operBean.Asvrg,
                        ProcessPrice = operBean.Preis,
                        ProcessPriceUnit = operBean.Peinh,
                        VGE01 = operBean.Vge01,
                        VGW01 = operBean.Vgw01,
                        VGE02 = operBean.Vge02,
                        VGW02 = operBean.Vgw02,
                        VGE03 = operBean.Vge03,
                        VGW03 = operBean.Vgw03,
                        VGE04 = operBean.Vge04,
                        VGW04 = operBean.Vgw04,
                        VGE05 = operBean.Vge05,
                        VGW05 = operBean.Vgw05,
                        VGE06 = operBean.Vge06,
                        VGW06 = operBean.Vgw06,
                        ScheduleStartDate = operBean.Ssavd.TryParse<DateTime?>(),
                        ScheduleFinishDate = operBean.Ssedd.TryParse<DateTime?>(),
                        BANFN = operBean.Banfn,
                        BNFPO = operBean.Bnfpo,
                        LIFNR = operBean.Lifnr
                    };
                    orderProcessDataList.Add(operDto);
                }
                return orderProcessDataList.OrderBy(p => p.OrderCounter).ToList();  //按工序计数器排序
            }
        }

    }
}
