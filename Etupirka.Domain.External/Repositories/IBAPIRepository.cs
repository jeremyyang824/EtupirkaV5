using System;
using System.Collections.Generic;
using Abp.Domain.Repositories;
using Etupirka.Domain.External.Bapi;
using Etupirka.Domain.External.Entities.Bapi;

namespace Etupirka.Domain.External.Repositories
{
    public interface IBAPIRepository : IRepository
    {
        /// <summary>
        /// 创建SAP采购订单
        /// </summary>
        /// <returns>执行结果（含采购订单号）</returns>
        BapiResult<string> PurcharseOrderCreate(IList<PoCreateInput> inputs);

        /// <summary>
        /// SAP采购订单入库
        /// </summary>
        BapiResult<string> PurcharseOrderFinish(PoFinishInput input);

        /// <summary>
        /// 取得SAP订单信息
        /// </summary>
        GetSapOrdersOutput GetSapOrders(GetSapOrdersInput input);

        /// <summary>
        /// 取得SAP生产订单工序质检信息
        /// </summary>
        GetSapMoInspectStateOutput GetSapMoInspectState(GetSapMoInspectStateInput input);

        /// <summary>
        /// 采购请求审批
        /// </summary>
        BapiResult<string> PurcharseOrderRequestRelease(PoRequestReleaseInput input);

        /// <summary>
        /// 采购审批
        /// </summary>
        BapiResult<string> PurcharseOrderRelease(PoReleaseInput input);
    }
}