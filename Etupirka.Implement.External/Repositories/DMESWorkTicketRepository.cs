using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using Dapper;
using Etupirka.Domain.External.Entities.Dmes;
using Etupirka.Domain.External.Repositories;

namespace Etupirka.Implement.External.Repositories
{
    public class DMESWorkTicketRepository : EtupirkaExternalRepositoryBase, IDMESWorkTicketRepository
    {
        /// <summary>
        /// 取得电气机台工票                                
        /// </summary>
        //[UnitOfWork(TransactionScopeOption.Suppress)]
        public async Task<IPagedResult<DmesOrderOutput>> FindDispatchedOrderPagerByWorkCenter(DmesFindDispatchedOrderByWorkCenterInput input)
        {
            string sql = this.getDispatchTicketSql() + this.getJqWorkTicketsCondition() + this.getDispatchedTicketsCondition();
            sql += "  AND cwc.ID=@Id";  //机台Id

            var sqlParams = new { Id = input.WorkCenterId };
            string countSql = $@"SELECT COUNT(1) FROM ({sql})T";
            string pagerSql = getPaggerWapperSql(sql, input); //分页包装

            using (var scope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            using (var conn = this.ConnectionManager.GetDMESSqlConnection())
            {
                var count = await conn.QueryFirstAsync<int>(countSql, (object)sqlParams);
                var orders = await conn.QueryAsync<DmesOrderOutput>(pagerSql, (object)sqlParams);

                return new PagedResultDto<DmesOrderOutput>(count, orders.ToList());
            }
        }

        /// <summary>
        /// 金切有效机床过滤条件
        /// </summary>
        private string getJqWorkTicketsCondition()
        {
            //return @" AND dt.MES_STATUS in (0,1) AND o.MRP_CONTROLLER = '100' ";
            return @" AND o.MRP_CONTROLLER = '100' ";
        }

        /// <summary>
        /// 派工单已下发过滤条件
        /// MES_STATUS	派工单状态
        /// MANUFACTURE_STATUS 加工状态
        /// </summary>
        private string getDispatchedTicketsCondition()
        {
            //return @" AND dt.MES_STATUS in (0,1) AND o.MRP_CONTROLLER = '100' ";
            return @" AND dt.MES_STATUS in (1) AND ISNULL(dt.MANUFACTURE_STATUS,0) < 3   ";
        }

        /// <summary>
        /// 派工单已下发过滤条件
        /// MES_STATUS	派工单状态
        /// MANUFACTURE_STATUS 加工状态
        /// </summary>
        private string getDispatchedTicketIdsCondition(IList<string> ids)
        {
            string idStr = string.Format("'{0}'", string.Join("','", ids));
            //return @" AND dt.MES_STATUS in (0,1) AND o.MRP_CONTROLLER = '100' ";
            return string.Format(@" AND dwr.ID in ({0})  ", idStr);
        }

        private string getDispatchTicketSql()
        {
            return
                @"SELECT ROW_NUMBER() Over(Order by o.ORDER_NUMBER,p.OPERATION_NUMBER,dwr.DISPATCHTICKET_ID,dwr.CREATEDATE DESC) RowID,
	                dwr.ID DispatchWorKTicketID,o.ORDER_NUMBER OrderNumber, o.WBS_ELEMENT, o.MATERIAL MaterialNumber, o.MATERIAL_TEXT MaterialDescription,  
	                p.OPERATION_NUMBER RoutingNumber,o.[START_DATE] StartDate, o.FINISH_DATE FinishDate, o.ACTUAL_RELEASE_DATE ActualReleaseDate, o.MES_STATUS OMesStatus,o.TARGET_QUANTITY TargetQuantity,
	                p.QUANTITY Quantity, p.OPR_CNTRL_KEY OprCntrl, p.WORK_CENTER OWorkCenter,
	                p.AFVV_VGW01, p.AFVV_VGE01, p.AFVV_VGW02, p.AFVV_VGE02, p.AFVV_VGW03, p.AFVV_VGE03, 
	                p.SCHED_START_DATE SchedStartDate, p.SCHED_FINISH_DATE SchedFinishDate, p.ACTUAL_START_DATE ActualStartDate, p.ACTUAL_FINISH_DATE ActualFinishDate,
	                dwr.[WORKCENTER_ID] AS ActualWorkCenterID,dwr.DISPATCHTICKET_ID DispatchTicketID,dwr.CREATEDATE AS ActualDispatchDate,
	                dt.MES_STATUS AS DispatchMesStatus,dt.MANUFACTURE_STATUS AS DispatchManufStatus,
	                cwc.WORKID AS ActualWorkID, cwc.WORKNAME AS ActualWorkName,dwr.MANUFNUM ManufNum,dwr.ID AS DW_ID
                    ----,stp.[REQUIRE_DATE] RequireDate,0 ReachStatus,ISNULL(stp.[STATUS],-1) ToolStatus,stp.[REQUIRE_DATE] RequireDate,0 DiStatus,0 TechStatus,0 NCStatus 
                FROM [dbo].[MES_PROORDER] o
	                INNER JOIN [dbo].[MES_PROORDER_AFVC] p ON p.[PROORDER_ID] = o.ID
	                INNER JOIN [dbo].[MES_WORKTICKET] wt ON wt.PROORDER_AFVC_ID = p.ID
	                INNER JOIN [dbo].[MES_DISPTICKET_WORKTICKET] dwr ON  dwr.WORKTICKET_ID = wt.ID
	                INNER JOIN [dbo].[MES_DISPATCHTICKET] dt ON dt.ID = dwr.DISPATCHTICKET_ID
	                INNER JOIN [dbo].[BASE_WORKCENTER] cwc ON  cwc.ID = dwr.WORKCENTER_ID
	                ----LEFT OUTER JOIN [dbo].[SJTU_TOOLING_PREPARE] stp ON stp.DISPATCH_WORK_ID = dwr.ID 
                WHERE 1=1";
        }


        /// <summary>
        /// 取的最新的派工单号   
        /// </summary>
        //[UnitOfWork(TransactionScopeOption.Suppress)]
        public async Task<IList<DmesDispatchedIdOutput>> GetDispatchedWorkTicketIDsLatest(DmesGetDispatchedIdsInput input)
        {
            string sql = @"SELECT dwr.ID DispatchWorKTicketID,o.MATERIAL MaterialNumber,cwc.WORKID AS ActualWorkID
                            FROM [dbo].[MES_PROORDER] o
                                 INNER JOIN [dbo].[MES_PROORDER_AFVC] p ON p.[PROORDER_ID] = o.ID
                                 INNER JOIN [dbo].[MES_WORKTICKET] wt ON wt.PROORDER_AFVC_ID = p.ID
                                 INNER JOIN [dbo].[MES_DISPTICKET_WORKTICKET] dwr ON  dwr.WORKTICKET_ID = wt.ID
                                 INNER JOIN [dbo].[MES_DISPATCHTICKET] dt ON dt.ID = dwr.DISPATCHTICKET_ID
                                 INNER JOIN [dbo].[BASE_WORKCENTER] cwc ON  cwc.ID = dwr.WORKCENTER_ID
                            WHERE 1=1" + this.getJqWorkTicketsCondition() + this.getDispatchedTicketsCondition();//+ " GROUP BY dwr.ID,o.MATERIAL ";
            sql += "  AND dt.RELEASEDATE >= @lastWorkerDate";

            var sqlParams = new { lastWorkerDate = input.lastWorkerDate };

            using (var scope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            using (var conn = this.ConnectionManager.GetDMESSqlConnection())
            {
                var ids = await conn.QueryAsync<DmesDispatchedIdOutput>(sql, (object)sqlParams);

                return await Task.FromResult(ids.ToList());
            }
        }

        public async Task<IList<DmesOrderOutput>> FindDispatchedOrdersByDispatchWorkID(DmesFindDispatchedOrderByTicketInput input)
        {

            string sql = this.getDispatchTicketSql() + this.getDispatchedTicketIdsCondition(input.DispatchWorkIDs);

            using (var scope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            using (var conn = this.ConnectionManager.GetDMESSqlConnection())
            {
                var orders = await conn.QueryAsync<DmesOrderOutput>(sql);

                return await Task.FromResult(orders.ToList());
            }
        }

    }
}
