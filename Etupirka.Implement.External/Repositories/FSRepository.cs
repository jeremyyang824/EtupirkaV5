using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Abp.Domain.Uow;
using Dapper;
using Etupirka.Domain.External.Entities.Fs;
using Etupirka.Domain.External.Repositories;

namespace Etupirka.Implement.External.Repositories
{
    /// <summary>
    /// FS ERP数据仓储
    /// </summary>
    public class FSRepository : EtupirkaExternalRepositoryBase, IFSRepository
    {
        /// <summary>
        /// 取得物料0套成本
        /// </summary>
        /// <param name="itemNumber">物料编码</param>
        /// <returns>0套成本</returns>
        //[UnitOfWork(TransactionScopeOption.Suppress)]
        public decimal GetItemCost0ByItemNumber(string itemNumber)
        {
            if (string.IsNullOrWhiteSpace(itemNumber))
                throw new ArgumentNullException("itemNumber");

            string sql =
                @"SELECT c.TotalRolledCost
                FROM dbo._NoLock_FS_Item i
                    INNER JOIN dbo._NoLock_FS_ItemCost c on c.ItemKey=i.ItemKey
                WHERE i.ItemStatus <> 'O' AND c.CostType=0 AND i.ItemNumber=?";

            //不包含在分布式事务中
            using (new TransactionScope(TransactionScopeOption.Suppress))
            using (var conn = this.ConnectionManager.GetFsOleConnection())
            {
                var cost0 = conn.Query<decimal>(sql, new { itemNumber }).FirstOrDefault();
                return cost0;
            }
        }

        /// <summary>
        /// 取得入库批次号
        /// </summary>
        //[UnitOfWork(TransactionScopeOption.Suppress)]
        public string GetLotNumber(string orderNumber, int lineNumber, string itemNumber)
        {
            if (string.IsNullOrWhiteSpace(orderNumber))
                throw new ArgumentNullException("orderNumber");
            if (lineNumber <= 0)
                throw new ArgumentNullException("lineNumber");
            if (string.IsNullOrWhiteSpace(itemNumber))
                throw new ArgumentNullException("itemNumber");

            string sql =
                @"SELECT TOP 1 LotNumber FROM ZCTMC_LotNumberInfo 
                WHERE OrderNumber=? AND LineNumber=? AND ItemNumber=?
                ORDER BY LastActivityDate DESC,LotTraceKey DESC";

            using (new TransactionScope(TransactionScopeOption.Suppress))
            using (var conn = this.ConnectionManager.GetFsOleConnection())
            {
                var lotNumber = conn.Query<string>(sql, new { orderNumber, lineNumber, itemNumber }).FirstOrDefault();
                return lotNumber;
            }
        }

        /// <summary>
        /// 根据物料编码取得物料信息
        /// </summary>
        /// <param name="itemNumber">物料编码</param>
        /// <returns>物料信息</returns>
        //[UnitOfWork(TransactionScopeOption.Suppress)]
        public FSItem GetItemByItemNumber(string itemNumber)
        {
            if (string.IsNullOrWhiteSpace(itemNumber))
                throw new ArgumentNullException("itemNumber");

            string sql = 
                $@"SELECT TOP 1 {_fsItemField}
                FROM dbo._NoLock_FS_Item 
                WHERE ItemStatus <> 'O' AND ItemNumber=?";

            using (new TransactionScope(TransactionScopeOption.Suppress))
            using (var conn = this.ConnectionManager.GetFsOleConnection())
            {
                var fsItem = conn.Query<FSItem>(sql, new { itemNumber }).FirstOrDefault();
                return fsItem;
            }
        }

        private readonly string _fsItemField =
            "ItemNumber, ItemDescription, ItemUM, MakeBuyCode, ItemType, FamilySubgroup, DrawingNumber, PreferredStockroom, PreferredBin, ItemKey";
    }
}
