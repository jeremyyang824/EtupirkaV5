using System;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace Etupirka.Implement.External.Infrasturctures
{
    /// <summary>
    /// 连接管理
    /// </summary>
    public class ConnectionManager : Abp.Dependency.ISingletonDependency
    {
        /// <summary>
        /// 取得FS ERP Ole连接
        /// </summary>
        public IDbConnection GetFsOleConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["ERP_OLEClient"].ConnectionString;
            return new OleDbConnection(connStr);
        }

        /// <summary>
        /// 取得 DMES Sql连接
        /// </summary>
        public IDbConnection GetDMESSqlConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["DMES_SqlClient"].ConnectionString;
            return new SqlConnection(connStr);
        }

        /// <summary>
        /// 取得 Winchill Sql连接
        /// </summary>
        public IDbConnection GetWinchillSqlConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["WinChill_SqlClient"].ConnectionString;
            return new SqlConnection(connStr);
        }

        /// <summary>
        /// 取得可视化系统Sql连接
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetVMESSqlConnection()
        {
            string connStr = ConfigurationManager.ConnectionStrings["VMES_SqlClient"].ConnectionString;
            return new SqlConnection(connStr);
        }
    }
}
