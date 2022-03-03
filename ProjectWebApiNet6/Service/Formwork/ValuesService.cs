using Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ProjectWebApi.Configuration;
using ProjectWebApi.Model;
using SqlSugar;
using SqlSugar.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebApiNet5.Service
{
    /// <summary>
    /// 测试 Service
    /// </summary>
    public class ValuesService
    {
        private DataTable dt;
        //private DBInfoService dbInfoService = new DBInfoService();

        #region 测试直接查询数据库数据
        /// <summary>
        /// 测试直接查询数据库数据
        /// </summary>
        /// <returns></returns>
        public DataTable GetEformEdrmsBorrowInfo()
        {
            dt = new DataTable();
            string sql = string.Format("select * from eform_edrms_LogMessages limit 10");
            try
            {
                var aaaaaa1 = SiteConfigOutput._siteConfig.ConectionConfig.ProjectMySql;
                dt = SiteConfigOutput._dbInfoService.ExecSql(SqlSugar.DbType.MySql, SiteConfigOutput._siteConfig.ConectionConfig.ProjectMySql, sql);
                //dt = dbInfoService.ExecSql(SqlSugar.DbType.MySql, SiteConfigOutput._siteConfig.ConectionConfig.ProjectMySql, sql);
                return dt;
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                dt.Columns.Add("result");
                DataRow newRow = dt.NewRow();
                newRow["result"] = ex.Message;
                dt.TableName = "result";//表名称
                dt.Rows.Add(newRow);
            }
            return dt;
        }
        #endregion

    }
}
