using Newtonsoft.Json.Linq;
using ProjectWebApi.Configuration;
using ProjectWebApi.Model;
using ProjectWebApiNet5.Model.Public;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebApiNet5.Service.LogMessages
{
    /// <summary>
    /// 封装操作日志信息记录
    /// </summary>
    public class LogMessagesService
    {
        private DataTable _dt = new DataTable();//内部变量
        private LogMessagesModel _logmessagesmodel;//局部全局对象
        private string _Str = "";//内部变量
        private string _StrTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //private int _InCon = 0;//内部变量

        #region 添加日志信息
        /// <summary>
        /// 添加借阅申请主表
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DataTable AddLogMessages(LogMessagesModel model)
        {
            
            _dt = new DataTable();
            //往数据插入数据，并且获取插入的主键Id
            string sql = string.Format("set @msgId=uuid();" +
                "insert into eform_edrms_LogMessages " +
                "(Id,createTime,modifiedTime,createId,createname,type,typeNote,methodName,inputParameter,outputParameter,contentMessages)" +
                " values(@msgId,@createTime,modifiedTime,@createId,@createname,@type,@typeNote,@methodName,@inputParameter,@outputParameter,@contentMessages);" +
                "select @msgId as Id;");
            try
            {
                DBTableAddress adderss = SiteConfigOutput._dbInfoService.SetDBTableAddress(SiteConfigOutput._siteConfig.ConectionConfig.ProjectMySql);
                List<SugarParameter> lst = new List<SugarParameter>();
                //lst.Add(new SugarParameter("@Id", model.Id)); // uuid() 随机数 36位;记录产品自动创建的流程ID
                lst.Add(new SugarParameter("@createTime", model.createTime));
                lst.Add(new SugarParameter("@modifiedTime", model.createTime));//系统自带的编辑时间
                lst.Add(new SugarParameter("@createId", model.createId));
                lst.Add(new SugarParameter("@createName", model.createName));
                lst.Add(new SugarParameter("@type", model.type));
                lst.Add(new SugarParameter("@typeNote", model.typeNote));
                lst.Add(new SugarParameter("@methodName", model.methodName));
                lst.Add(new SugarParameter("@inputParameter", model.inputParameter));
                lst.Add(new SugarParameter("@outputParameter", model.outputParameter));
                lst.Add(new SugarParameter("@contentMessages", model.contentMessages));
                _dt = SiteConfigOutput._dbInfoService.ExecSql(SqlSugar.DbType.MySql, adderss.ServerUrl, adderss.User, adderss.Pwd, adderss.Database, adderss.Port, sql, lst);
            }
            catch (Exception ex)
            {
                _Str = ex.Message;
                _dt = new DataTable();
            }
            return _dt;

        }
        #endregion

        #region 查询日志信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model">分页</param>
        /// <param name="errorMsg">回调变量</param>
        /// <returns></returns>
        public DataTable GetLogMessagesPageList(PagingModel model,out string errorMsg)
        {
            errorMsg = "";
            _Str = "";
            _dt = new DataTable();
            _logmessagesmodel = new LogMessagesModel();
            string strkey = "";//动态变量 字段 
            string strvalue = "";//动态变量 值
            model.SortField = string.IsNullOrEmpty(model.SortField) ? "createTime" : model.SortField;
            model.SortOrder = string.IsNullOrEmpty(model.SortOrder) ? "desc" : model.SortOrder;
            model.Page = model.Page == 0 ? 1 : model.Page;
            model.Size = model.Size == 0 ? 10 : model.Size;
            string sql = string.Format("select * from eform_edrms_LogMessages {0} order by {1} {2} limit {3},{4}"
                , _Str, model.SortField, model.SortOrder, ((model.Page - 1) * model.Size), model.Size);
            try
            {
                // 动态循环动态传参数组 - 精确查询添加Json
                if (!string.IsNullOrEmpty(model.SearchField))
                {
                    JObject objsearch = JsonHelper.DeserializeJsonToObject<JObject>(model.SearchField);
                    foreach (var item in objsearch)
                    {
                        if (string.IsNullOrEmpty(_Str))
                            _Str = string.Format(" where {0} = '{1}' ", item.Key, item.Value.ToString());
                        else
                            _Str += string.Format(" and {0} = '{1}' ", item.Key, item.Value.ToString());
                    }
                }
                // 动态循环动态传参数组 - 范围查询条件
                if (!string.IsNullOrEmpty(model.RangeField))
                {
                    string startvalue = "", endvalue = "";// 开始范围  - 结束访问
                    JObject objsearch = JsonHelper.DeserializeJsonToObject<JObject>(model.RangeField);
                    foreach (var item in objsearch)
                    {
                        strkey = item.Key;
                        strvalue = item.Value.ToString();
                        startvalue = strvalue.Split('$')[0].ToString();
                        endvalue = strvalue.Split('$')[1].ToString();
                        //日期格式-需要转换
                        if (strkey.ToLower().IndexOf("time") > -1)
                        {
                            if (string.IsNullOrEmpty(_Str))
                                _Str = string.Format(" where (DATE_FORMAT({0},'%Y-%c-%d') >= '{1}' and  DATE_FORMAT({0},'%Y-%c-%d') <= '{2}') ", item.Key, startvalue, endvalue);
                            else
                                _Str += string.Format(" and (DATE_FORMAT({0},'%Y-%c-%d') >= '{1}' and  DATE_FORMAT({0},'%Y-%c-%d') <= '{2}') ", item.Key, startvalue, endvalue);
                        }
                        // 数字格式
                        else
                        {
                            if (string.IsNullOrEmpty(_Str))
                                _Str = string.Format(" where (CAST({0} as SIGNED) >= '{1}'  and CAST({0} as SIGNED) <= '{2}')", item.Key, startvalue, endvalue);
                            else
                                _Str += string.Format(" and (CAST({0} as SIGNED) >= '{1}'  and CAST({0} as SIGNED) <= '{2}')", item.Key, startvalue, endvalue);
                        }
                    }
                }
                // 动态循环动态传参数组 - 模糊条件查询
                if (!string.IsNullOrEmpty(model.FuzzyField))
                {
                    JObject objsearch = JsonHelper.DeserializeJsonToObject<JObject>(model.FuzzyField);
                    foreach (var item in objsearch)
                    {
                        //strkey = item.Key;
                        //strvalue = item.Value.ToString();
                        if (string.IsNullOrEmpty(_Str))
                            _Str = string.Format(" where {0} like '%{1}%' ", item.Key, item.Value.ToString());
                        else
                            _Str += string.Format(" and {0} like '%{1}%' ", item.Key, item.Value.ToString());
                    }
                }
                var aaaaaa1 = SiteConfigOutput._siteConfig.ConectionConfig.ProjectMySql;
                _dt = SiteConfigOutput._dbInfoService.ExecSql(SqlSugar.DbType.MySql, SiteConfigOutput._siteConfig.ConectionConfig.ProjectMySql, sql);
                return _dt;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                //dt = new DataTable();
                //dt.Columns.Add("result");
                //DataRow newRow = dt.NewRow();
                //newRow["result"] = ex.Message;
                //dt.TableName = "result";//表名称
                //dt.Rows.Add(newRow);
                // 记录日志
                _dt = new DataTable();
                _logmessagesmodel.createTime = _StrTime;
                _logmessagesmodel.createId = "";
                _logmessagesmodel.createName = "";
                _logmessagesmodel.type = "2";
                _logmessagesmodel.typeNote = "LogMessagesService";
                _logmessagesmodel.methodName = "GetLogMessagesPageList";
                _logmessagesmodel.inputParameter = "";
                _logmessagesmodel.outputParameter = ex.Message;
                _logmessagesmodel.contentMessages = ex.ToString() ;
                AddLogMessages(_logmessagesmodel);
            }
            return _dt;
        }
        #endregion
    }
}
