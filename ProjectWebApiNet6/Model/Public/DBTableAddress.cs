using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebApi.Model
{//WebCoreTemplateApi.Model.Config
    /// <summary>
    /// 数据库访问地址公用模板
    /// </summary>
    public class DBTableAddress
    {
        /// <summary>
        /// 完整的地址访问字符串地址
        /// </summary>
        public string DBSettings { get; set; }
        /// <summary>
        /// 登录具体的Ip地址
        /// </summary>
        public string ServerUrl { get; set; }
        /// <summary>
        /// 登录名称
        /// </summary>
        public string User { get; set; }
        /// <summary>
        /// 登录密码
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// 登录数据库名
        /// </summary>
        public string Database { get; set; }
        /// <summary>
        /// 端口号
        /// </summary>
        public string Port { get; set; }
        /// <summary>
        /// 登录数据库类型
        /// </summary>
        public string DbType { get; set; }
    }
}
