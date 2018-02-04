using System;
using System.Collections.Generic;
using System.Linq;
using Abp.Application.Services.Dto;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Etupirka.Implement.External.Infrasturctures;

namespace Etupirka.Implement.External.Repositories
{
    /// <summary>
    /// 外部数据Repository基类
    /// </summary>
    public abstract class EtupirkaExternalRepositoryBase : IRepository
    {
        /// <summary>
        /// 数据库连接管理器
        /// </summary>
        public virtual ConnectionManager ConnectionManager { get; set; }

        protected EtupirkaExternalRepositoryBase()
        {
        }

        /// <summary>
        /// 将SQL包装为分页SQL
        /// </summary>
        /// <param name="rawSql">原始SQL(必须包含RowID的序号列)</param>
        /// <param name="paggerInput">分页输入</param>
        /// <returns>分页SQL</returns>
        protected virtual string getPaggerWapperSql(string rawSql, IPagedResultRequest paggerInput)
        {
            if (string.IsNullOrWhiteSpace(rawSql))
                throw new ArgumentNullException(nameof(rawSql));

            string sql =
                $@"SELECT * FROM ({rawSql}) AS _T WHERE _T.RowID BETWEEN {paggerInput.SkipCount + 1} AND {(paggerInput.SkipCount + paggerInput.MaxResultCount)} ";
            return sql;
        }
    }
}
