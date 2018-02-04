using Etupirka.Domain.External.Repositories;
using System;
using System.Linq;
using Etupirka.Domain.External.Entities.Dmes;
using System.Dynamic;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using Dapper;
using System.Collections.Generic;

namespace Etupirka.Implement.External.Repositories
{
    public class DMESWorkCenterRepository : EtupirkaExternalRepositoryBase, IDMESWorkCenterRepository
    {
        private readonly string workCenterFields = @"[ID] as WorkCenterId, [WORKID] as WorkCenterCode, [WORKNAME] as WorkCenterName";

        public async Task<DmesWorkCenterOutput> GetWorkCenter(int id)
        {
            string sql = $"SELECT {workCenterFields} FROM [dbo].[BASE_WORKCENTER] WHERE Id=@Id";
            using (var scope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            using (var conn = this.ConnectionManager.GetDMESSqlConnection())
            {
                var wCenter = await conn.QueryFirstOrDefaultAsync<DmesWorkCenterOutput>(sql, new { Id = id });
                return wCenter;
            }
        }

        /// <summary>
        /// 取得电气机台（分页）
        /// </summary>
        ///[UnitOfWork(TransactionScopeOption.Suppress)]
        public async Task<IPagedResult<DmesWorkCenterOutput>> GetWorkCenters(DmesGetWorkCenterInput input)
        {
            string countSql = @"SELECT COUNT(1) FROM [dbo].[BASE_WORKCENTER] WHERE 1=1";
            string pagerSql = $@"SELECT ROW_NUMBER() Over(Order by WORKID,WORKNAME) RowID, {workCenterFields} FROM [dbo].[BASE_WORKCENTER] WHERE 1=1";

            //有效机台
            string condition = $@"{getX1X2WorkCenterCondition()}";
            dynamic sqlParams = new ExpandoObject();
            //条件过滤
            if (!string.IsNullOrWhiteSpace(input.WorkCenterCode))
            {
                condition += " AND WORKID like @WORKID";
                sqlParams.WORKID = $"%{input.WorkCenterCode}%";
            }
            if (!string.IsNullOrWhiteSpace(input.WorkCenterName))
            {
                condition += " AND WORKNAME like @WORKNAME";
                sqlParams.WORKNAME = $"%{input.WorkCenterName}%";
            }

            countSql += condition;
            pagerSql = getPaggerWapperSql(pagerSql + condition, input); //分页包装

            using (var scope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            using (var conn = this.ConnectionManager.GetDMESSqlConnection())
            {
                var count = await conn.QueryFirstAsync<int>(countSql, (object)sqlParams);
                var wCenter = await conn.QueryAsync<DmesWorkCenterOutput>(pagerSql, (object)sqlParams);

                return new PagedResultDto<DmesWorkCenterOutput>(count, wCenter.ToList());
            }
        }

        private string getX1X2WorkCenterCondition()
        {
            return @" AND LEFT(WORKID,2) IN ('X1','X2')";
        }


        public async Task<IList<DmesWorkcenterMapOutput>> GetWorkCenterWinToolMaps()
        {
            string sql = @"SELECT * FROM T_SJTU_WORKCENTER_MAP ";

            using (var scope = new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            using (var conn = this.ConnectionManager.GetDMESSqlConnection())
            {
                var result = await conn.QueryAsync<DmesWorkcenterMapOutput>(sql);

                return await Task.FromResult(result.ToList());
            }
        }

    }
}
