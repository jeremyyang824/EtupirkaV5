using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Domain.Repositories;
using Etupirka.Domain.External.Entities.Fs;

namespace Etupirka.Domain.External.Repositories
{
    public interface IFSRepository : IRepository
    {
        /// <summary>
        /// 取得物料0套成本
        /// </summary>
        /// <param name="itemNumber">物料编码</param>
        /// <returns>0套成本</returns>
        decimal GetItemCost0ByItemNumber(string itemNumber);

        /// <summary>
        /// 取得入库批次号
        /// </summary>
        string GetLotNumber(string orderNumber, int lineNumber, string itemNumber);

        /// <summary>
        /// 根据物料编码取得物料信息
        /// </summary>
        /// <param name="itemNumber">物料编码</param>
        /// <returns>物料信息</returns>
        FSItem GetItemByItemNumber(string itemNumber);
    }
}
