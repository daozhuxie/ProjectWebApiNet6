using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DbType = SqlSugar.DbType;


namespace ProjectWebApi.DBIService.Config
{
    /// <summary>
    /// 映射数据库
    /// </summary>
    public interface IDBInfoService
    {
        /// <summary>
        /// 获取所有数据库
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        DataTable GetDataBase(DbType dbType, string serverUrl, string user, string pwd);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        DataTable GetTable(DbType dbType, string serverUrl, string user, string pwd, string database);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        DataTable GetProc(DbType dbType, string serverUrl, string user, string pwd, string database);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="database"></param>
        /// <param name="tbName"></param>
        /// <returns></returns>
        DataTable GetColumn(DbType dbType, string serverUrl, string user, string pwd, string database, string tbName);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="database"></param>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        DataTable ExecSql(DbType dbType, string serverUrl, string user, string pwd, string database, string sqlStr);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="database"></param>
        /// <param name="port"></param>
        /// <param name="sqlStr"></param>
        /// <param name="paraLst"></param>
        /// <returns></returns>
        DataTable ExecSql(DbType dbType, string serverUrl, string user, string pwd, string database,string port,string sqlStr, List<SugarParameter> paraLst);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="connectionStr"></param>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        DataTable ExecSql(DbType dbType, string connectionStr, string sqlStr);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="database"></param>
        /// <param name="spName"></param>
        /// <param name="paraLst"></param>
        /// <returns></returns>
        DataTable ExecProc(DbType dbType, string serverUrl, string user, string pwd, string database, string spName, List<SugarParameter> paraLst);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="database"></param>
        /// <param name="port"></param>
        /// <param name="sqlStr"></param>
        /// <param name="paraLst"></param>
        /// <returns></returns>
        int ExecSqlNonQuery(DbType dbType, string serverUrl, string user, string pwd, string database, string port,string sqlStr, List<SugarParameter> paraLst);

    }
}
