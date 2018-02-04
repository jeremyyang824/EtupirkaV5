using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Etupirka.Domain.External.Entities.Winchill;
using Etupirka.Domain.External.Repositories;
using Dapper;
using System.Dynamic;
using System.Transactions;

namespace Etupirka.Implement.External.Repositories
{
    public class WinchillRepository : EtupirkaExternalRepositoryBase, IWinchillRepository
    {
        /// <summary>
        /// 取得零部件记录
        /// </summary>
        public async Task<IEnumerable<PartItemDoc>> GetByPartItem(GetByPartItemInput input)
        {
            string condition = $@"partnumber=@PartNumber";
            dynamic sqlParams = new ExpandoObject();
            sqlParams.PartNumber = input.PartNumber;
            //其他条件
            if (!string.IsNullOrWhiteSpace(input.PartVersion))
            {
                condition += " AND partversion=@PartVersion";
                sqlParams.PartVersion = input.PartVersion;
            }
            if (!string.IsNullOrWhiteSpace(input.DocVersion))
            {
                condition += " AND docversion=@DocVersion";
                sqlParams.DocVersion = input.DocVersion;
            }

            string sql =
                $@"SELECT {partItemFields} FROM [dbo].[z_winchill_t_SHEPM]
                WHERE {condition}
                ORDER BY partnumber, partversion, docversion";

            using (new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            using (var conn = this.ConnectionManager.GetWinchillSqlConnection())
            {
                var list = await conn.QueryAsync<PartItemDoc>(sql, (object)sqlParams);
                return list;
            }
        }

        /// <summary>
        /// 取得所有零件版本
        /// </summary>
        public async Task<IEnumerable<PartVersionOutput>> GetAllPartVersions()
        {
            string sql =
                @"SELECT DISTINCT [partnumber] as PartNumber, [partversion] as PartVersion FROM dbo.z_winchill_t_SHEPM
                ORDER BY partnumber, partversion";

            using (new TransactionScope(TransactionScopeOption.Suppress, TransactionScopeAsyncFlowOption.Enabled))
            using (var conn = this.ConnectionManager.GetWinchillSqlConnection())
            {
                var list = await conn.QueryAsync<PartVersionOutput>(sql);
                return list;
            }
        }

        private readonly string partItemFields =
            @"[partnumber] as PartNumber, [partversion] as PartVersion, [partname] as PartName, [docnumber] as DocNumber, 
              [docversion] as DocVersion,[docname] as DocName, [downloadurl3d] as DownloadUrl3D, [downloadurl2d] as DownloadUrl2D, 
              CAST([publishtime] AS DATETIME) as PublishTime, [docmodifier] as DocModifier, [flag] as Flag";
    }
}
