using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Castle.Core.Logging;
using Dapper;
using Etupirka.Domain.External.Entities.Vmes;
using Etupirka.Domain.External.Repositories;
using Etupirka.Implement.External.Infrasturctures;

namespace Etupirka.Implement.External.Repositories
{
    /// <summary>
    /// 可视化MES接口
    /// </summary>
    public class VMESRepository : EtupirkaExternalRepositoryBase, IVMESRepository
    {
        private readonly VMESHelper _vmesHelper;

        public ILogger Logger { get; set; }

        public VMESRepository(VMESHelper vmesHelper)
        {
            this._vmesHelper = vmesHelper;
            Logger = NullLogger.Instance;
        }

        /// <summary>
        /// 从FS同步PICK到可视化MES
        /// </summary>
        public void SyncPickToVmes(SyncPickToVmesInput input)
        {
            using (var mesService = this._vmesHelper.SyncPickVMESService())
            {
                mesService.PickMultipleChange(input.MONumber, input.MOLineNumber.ToString());
            }
        }

        /// <summary>
        /// 指定生产订单行是否在MES中质检过
        /// </summary>
        public bool IsInspected(IsInspectedInput input)
        {
            string sql =
                @"SELECT COUNT(1)
                FROM [FSDBMR].[dbo].[z_sjtu_t_quality]
                WHERE [InspectingStatus]=1 AND [InspectingEndTime] IS NOT NULL
	                AND [MONumber]=@MONumber AND [MOLineNumber]=@MOLineNumber AND [ProcessID]=@ProcessID";

            var sqlParams = new { MONumber = input.MONumber, MOLineNumber = input.MOLineNumber, ProcessID = input.ProcessNumber };

            this.Logger.Info(
                $@"VMESRepository.IsInspected: MONumber: {input.MONumber}, MOLineNumber: {input.MOLineNumber}, ProcessNumber: {input.ProcessNumber}");

            //不包含在分布式事务中
            using (new TransactionScope(TransactionScopeOption.Suppress))
            using (var conn = this.ConnectionManager.GetVMESSqlConnection())
            {
                var count = conn.QueryFirst<int>(sql, (object)sqlParams);
                return count > 0;
            }
        }
    }
}
