/*-------------------------------------------------------
 *	Copyright (c)  , All rights reserved.
 *	Author: Xiexiangying
 *	Date:2022-02
 *	Description:接口帮助类
 *------------------------------------------------------*/
using ProjectWebApi.DBIService.Config;
using ProjectWebApi.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using DbType = SqlSugar.DbType;

namespace Configuration
{
    /// <summary>
    /// 数据库连接
    /// </summary>
    public class DBInfoService : IDBInfoService
    {
        private String _StrValue = null;//全局值
        /// <summary>
        /// 数据库Db 连接
        /// </summary>
        public SqlSugarClient Db { get; set; }
        private string oracleServiceName { get; set; }
        /// <summary>
        /// SqlSugar 配置
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="providerType"></param>
        public void GetNewInstance(string connectionString, DbType providerType)
        {
            this.Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = providerType,
                InitKeyType = InitKeyType.Attribute,
                IsAutoCloseConnection = true,
            });
        }
        /// <summary>
        /// 暂未使用
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        private string GetConnectionString(DbType dbType, string serverUrl, string user, string pwd)
        {
            if (dbType == DbType.SqlServer)
            {
                return string.Format("server={0};User ID={1};Password={2};Connection Reset=FALSE", serverUrl, user, pwd);
            }
            else if (dbType == DbType.Oracle)
            {
                string serviceName = "orcl";

                if (!string.IsNullOrEmpty(oracleServiceName))
                {
                    serviceName = oracleServiceName;
                }


                string oracleStr = @"Data Source =
                                     (DESCRIPTION =
                                     (ADDRESS_LIST =
                                       (ADDRESS = (PROTOCOL = TCP)(HOST = {0})(PORT = {4}))
                                     )
                                     (CONNECT_DATA =
                                       (SERVER = DEDICATED)
                                       (SERVICE_NAME = {3})
                                     )
                                   ); User ID = {1}; Password = {2};";

                return string.Format(oracleStr, serverUrl, user, pwd, serviceName);
            }



            return "";
        }
        /// <summary>
        /// 暂未使用
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="database"></param>
        /// <param name="Port"></param>
        /// <returns></returns>
        private string GetConnectionString(DbType dbType, string serverUrl, string user, string pwd, string database,string Port)
        {
            if (dbType == DbType.SqlServer)
            {
                return string.Format("server={0};User ID={1};Password={2};database={3};pooling=true;min pool size=20;max pool size=50", serverUrl, user, pwd, database);
            }
            else if (dbType == DbType.MySql)
            {
                return string.Format("server={0};User ID={1};Password={2};Database={3};Port={4};CharSet=utf8;pooling=true;Convert Zero Datetime=False;Allow Zero Datetime=True;Allow User Variables=True;", serverUrl, user, pwd, database, Port);
            }
            else if (dbType == DbType.Oracle)
            {
                string serviceName = "orcl";
                string host = serverUrl;
                string defaltPort = "1521";
                string[] arr = serverUrl.Split(',');
                if (arr.Length > 1)
                {
                    host = arr[0];
                    serviceName = arr[2];
                    defaltPort = arr[1];
                }

                return string.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={4})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={3})));User Id={1};Password={2};", host, user, pwd, serviceName, defaltPort);
            }
            return "";
        }
        /// <summary>
        /// 暂未使用
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        private string GetDataBaseString(DbType dbType)
        {
            if (dbType == DbType.SqlServer)
            {
                return "select name from sysdatabases";
            }
            else if (dbType == DbType.MySql)
            {
                return "show databases;";
            }
            else if (dbType == DbType.Oracle)
            {
                return "  select username as name from all_users order by username "; //select name from v$tablespace
            }
            return "";
        }
        /// <summary>
        /// 暂未使用
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        private string GetTableString(DbType dbType, string database)
        {
            if (dbType == DbType.SqlServer)
            {
                return string.Format("use {0};select name from sysobjects where xtype='U'", database);
            }
            else if (dbType == DbType.MySql)
            {
                return string.Format("select table_name as name from information_schema.tables where table_schema='{0}' and table_type='base table'", database);
            }
            else if (dbType == DbType.Oracle)
            {
                return string.Format("select * from {0};", database);
            }
            return "";
        }
        /// <summary>
        /// 暂未使用
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        private string GetProcString(DbType dbType, string database)
        {
            if (dbType == DbType.SqlServer)
            {
                return string.Format("use {0};select name from sysobjects where xtype='P'", database);
            }
            else if (dbType == DbType.MySql)
            {
                return string.Format("select name from mysql.proc where db='{0}' and type = 'PROCEDURE'", database);
            }
            else if (dbType == DbType.Oracle)
            {
                return string.Format("select object_name as name from user_objects where object_type='PROCEDURE'");
            }
            return "";
        }
        /// <summary>
        /// 暂未使用
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="database"></param>
        /// <param name="objName"></param>
        /// <returns></returns>
        private string GetColumnString(DbType dbType, string database, string objName)
        {
            if (dbType == DbType.SqlServer)
            {
                string str = @"select  name, type_name(xusertype) as [dbType],length from syscolumns 
                            where id=object_id('{0}')";

                return string.Format(str, objName);
            }
            else if (dbType == DbType.MySql)
            {
                return string.Format("select PARAMETER_NAME as name, DATA_TYPE as dbType,CHARACTER_MAXIMUM_LENGTH from information_schema.PARAMETERS where SPECIFIC_NAME = '{0}'", objName);
            }
            else if (dbType == DbType.Oracle)
            {
                // return string.Format("select column_name, data_type from user_tab_columns where table_name = '{0}'", objName);

                return string.Format("select dbms_metadata.get_ddl('PROCEDURE','P_KTMED_INFO') from dual");
            }
            return "";
        }
        /// <summary>
        /// 暂未使用
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public DataTable GetDataBase(DbType dbType, string serverUrl, string user, string pwd)
        {
            string defDB = "";
            if (dbType == DbType.SqlServer)
            {
                defDB = "master";
            }
            else if (dbType == DbType.MySql)
            {
                defDB = "sys";
            }

            string connectionStr = GetConnectionString(dbType, serverUrl, user, pwd, defDB, "");
            GetNewInstance(connectionStr, dbType);

            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                DataTable dt = new DataTable();
                //LogHelper.WriteLog("sqlConnection:" + connectionStr);
                string dbStr = GetDataBaseString(dbType);

                sw.Start();
                dt = new DataTable("MyTable");
                dt = Db.Ado.GetDataTable(dbStr, new List<SugarParameter>());
                sw.Stop();
                // LogHelper.WriteLog("sqlData:" + sw.Elapsed);
                sw.Restart();

                DataTable resultDt = new DataTable();
                resultDt.Columns.Add("name");
                foreach (DataRow dr in dt.Rows)
                {
                    DataRow newRow = resultDt.NewRow();
                    string? val = dr[0].ToString();
                    newRow["name"] = val;
                    resultDt.Rows.Add(newRow);
                }
                resultDt.DefaultView.Sort = "name asc";
                resultDt = resultDt.DefaultView.ToTable();

                sw.Stop();
                // LogHelper.WriteLog("DataProcess:" + sw.Elapsed);

                return resultDt;
            }
            catch (Exception ex)
            {
                throw new Exception("Sql语句错误:" + ex);
            }


        }
        /// <summary>
        /// 暂未使用
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public DataTable GetTable(DbType dbType, string serverUrl, string user, string pwd, string database)
        {
            string connectionStr = GetConnectionString(dbType, serverUrl, user, pwd, database, "");
            GetNewInstance(connectionStr, dbType);
            try
            {
                DataTable dt = new DataTable();

                string dbStr = GetTableString(dbType, database);
                Stopwatch sw = new Stopwatch();

                sw.Start();
                dt = new DataTable("MyTable");
                dt = Db.Ado.GetDataTable(dbStr, new List<SugarParameter>());
                sw.Stop();

                return dt;
            }
            catch (Exception ex)
            {
                _StrValue = ex.Message;
                throw new Exception("Sql语句错误");
            }

        }
        /// <summary>
        /// 暂未使用
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        public DataTable GetProc(DbType dbType, string serverUrl, string user, string pwd, string database)
        {
            string connectionStr = GetConnectionString(dbType, serverUrl, user, pwd, database, "");
            GetNewInstance(connectionStr, dbType);

            try
            {
                DataTable dt = new DataTable();
                string dbStr = GetProcString(dbType, database);
                Stopwatch sw = new Stopwatch();

                sw.Start();
                dt = new DataTable("MyTable");
                dt = Db.Ado.GetDataTable(dbStr, new List<SugarParameter>());
                sw.Stop();
                //LogHelper.WriteLog("sqlData:" + sw.Elapsed);

                return dt;
            }
            catch (Exception ex)
            {
                _StrValue = ex.Message;
                throw new Exception("Sql语句错误");
            }

        }
        /// <summary>
        /// 暂未使用
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="database"></param>
        /// <param name="objName"></param>
        /// <returns></returns>
        public DataTable GetColumn(DbType dbType, string serverUrl, string user, string pwd, string database, string objName)
        {
            string connectionStr = GetConnectionString(dbType, serverUrl, user, pwd, database, "");
            GetNewInstance(connectionStr, dbType);

            try
            {
                DataTable dt = new DataTable();

                string dbStr = GetColumnString(dbType, database, objName);
                Stopwatch sw = new Stopwatch();

                sw.Start();
                dt = new DataTable("MyTable");
                dt = Db.Ado.GetDataTable(dbStr, new List<SugarParameter>());
                sw.Stop();
                //LogHelper.WriteLog("sqlData:" + sw.Elapsed);

                return dt;
            }
            catch (Exception ex)
            {
                _StrValue = ex.Message;
                throw new Exception("Sql语句错误");
            }

        }
        /// <summary>
        /// 连接数据库执行Sql脚本 -暂未使用
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="serverUrl">连接Ip地址</param>
        /// <param name="user">登录名称</param>
        /// <param name="pwd">密码</param>
        /// <param name="database">库名</param>
        /// <param name="sqlStr">sql脚本</param>
        /// <returns></returns>
        public DataTable ExecSql(DbType dbType, string serverUrl, string user, string pwd, string database, string sqlStr)
        {
            string connectionStr = GetConnectionString(dbType, serverUrl, user, pwd, database, "");
            GetNewInstance(connectionStr, dbType);

            try
            {
                Stopwatch sw = new Stopwatch();
                DataTable dt = new DataTable();

                sw.Start();
                dt = new DataTable("MyTable");
                dt = Db.Ado.GetDataTable(sqlStr, new List<SugarParameter>());
                sw.Stop();
                //LogHelper.WriteLog("sqlData:" + sw.Elapsed);

                return dt;
            }
            catch (Exception ex)
            {
                _StrValue = ex.Message;
                throw new Exception("Sql语句错误");
            }
        }
        /// <summary>
        /// 往数据插入数据时使用
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="serverUrl">数据库地址</param>
        /// <param name="user">登录账号</param>
        /// <param name="pwd">登录密码</param>
        /// <param name="database">数据库名称</param>
        /// <param name="port">请求端口</param>
        /// <param name="sqlStr">sql脚本</param>
        /// <param name="paraLst">请求参数</param>
        /// <returns></returns>
        public DataTable ExecSql(DbType dbType, string serverUrl, string user, string pwd, string database,string port, string sqlStr, List<SugarParameter> paraLst)
        {
            string connectionStr = GetConnectionString(dbType, serverUrl, user, pwd, database, port);
            GetNewInstance(connectionStr, dbType);
            try
            {
                Stopwatch sw = new Stopwatch();
                DataTable dt = new DataTable();

                sw.Start();
                if (paraLst == null)
                    paraLst = new List<SugarParameter>();

                foreach (var p in paraLst)
                {
                    if (p.Value == null)
                        p.Value = "";
                    //LogHelper.WriteLog("para:" + p.ParameterName + " || [" + p.Value + "]");
                }

                dt = new DataTable("MyTable");
                dt = Db.Ado.GetDataTable(sqlStr, paraLst);
                //Db.Updateable();
                sw.Stop();
                //LogHelper.WriteLog("sqlData:" + sw.Elapsed);

                return dt;
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog(ex.Message);
                throw new Exception("Sql语句错误"+ ex);
            }
        }
        /// <summary>
        /// 执行查询操作-根据数据库完整的配置-亲测可行
        /// </summary>
        /// <param name="dbType">数据库类型</param>
        /// <param name="connectionStr">数据库连接字符串</param>
        /// <param name="sqlStr">执行的Sql脚本</param>
        /// <returns></returns>
        public DataTable ExecSql(DbType dbType, string connectionStr, string sqlStr)
        {
            GetNewInstance(connectionStr, dbType);

            try
            {
                Stopwatch sw = new Stopwatch();
                DataTable dt = new DataTable();

                sw.Start();
                dt = new DataTable("MyTable");
                dt = Db.Ado.GetDataTable(sqlStr, new List<SugarParameter>());

                sw.Stop();
                //LogHelper.WriteLog("sqlData:" + sw.Elapsed);

                return dt;
            }
            catch (Exception ex)
            {
                //LogHelper.WriteLog(ex.Message);
                Console.WriteLine($"{ex.Message}|{ex.InnerException}");
                //throw new Exception("Sql语句错误");
                throw new Exception("Sql语句错误" + ex.Message);
            }
        }
        /// <summary>
        /// 暂未使用
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <param name="pwd"></param>
        /// <param name="database"></param>
        /// <param name="spName"></param>
        /// <param name="paraLst"></param>
        /// <returns></returns>
        public DataTable ExecProc(DbType dbType, string serverUrl, string user, string pwd, string database, string spName, List<SugarParameter> paraLst)
        {
            string connectionStr = GetConnectionString(dbType, serverUrl, user, pwd, database, "");
            GetNewInstance(connectionStr, dbType);

            try
            {
                Stopwatch sw = new Stopwatch();
                DataTable dt = new DataTable();

                sw.Start();
                dt = new DataTable("MyTable");

                foreach (var p in paraLst)
                {
                    if (p.Value == null)
                        p.Value = "";

                    if (p.DbType == System.Data.DbType.Date)
                        p.Value = DateTime.Parse(p.Value.ToString());
                    //LogHelper.WriteLog("para:" + p.ParameterName + " || [" + p.Value + "]");
                }

                if (dbType == DbType.Oracle)
                {
                    SugarParameter para = new SugarParameter("C_RES", null,true);
                    para.IsRefCursor = true;
                    paraLst.Add(para);

                    dt = Db.Ado.UseStoredProcedure().GetDataTable(spName, paraLst);
                    sw.Stop();
                    //LogHelper.WriteLog("sqlData:" + sw.Elapsed);
                }


                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Sql语句错误" + ex);
            }
        }
        /// <summary>
        /// 暂未使用
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
        public int ExecSqlNonQuery(DbType dbType, string serverUrl, string user, string pwd, string database, string port,string sqlStr, List<SugarParameter> paraLst)
        {
            string connectionStr = GetConnectionString(dbType, serverUrl, user, pwd, database, port);
            GetNewInstance(connectionStr, dbType);

            try
            {
                int count = 0;
                Stopwatch sw = new Stopwatch();
                sw.Start();

                foreach (var p in paraLst)
                {
                    if (p.Value == null)
                        p.Value = "";
                    //LogHelper.WriteLog("para:" + p.ParameterName + " || [" + p.Value + "]");
                }

                count = Db.Ado.ExecuteCommand(sqlStr, paraLst);
                sw.Stop();
                //LogHelper.WriteLog("sqlData:" + sw.Elapsed);
                return count;
            }
            catch (Exception ex)
            {
                _StrValue = ex.Message;
                throw new Exception("Sql语句错误");
            }
        }
        /// <summary>
        /// 拆分访问的数据库字段
        /// </summary>
        /// <param name="DBSettings">连接字符串</param>
        public DBTableAddress SetDBTableAddress(string DBSettings)
        {
            DBTableAddress model = new DBTableAddress();
            string[] strdbvalue = DBSettings.Split(';');
            model.DBSettings = DBSettings;
            model.ServerUrl = strdbvalue[1].Split('=')[1];
            model.User = strdbvalue[3].Split('=')[1];
            model.Pwd = strdbvalue[4].Split('=')[1];
            model.Port = strdbvalue[2].Split('=')[1];
            model.Database = strdbvalue[0].Split('=')[1];
            return model;
        }

    }
}