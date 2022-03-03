using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebApi.Configuration
{
    /// <summary>
    /// 打印日志信息
    /// </summary>
    public class LogFactory
    {
        /// <summary>
        /// 
        /// </summary>
        public static Logger log;
        private string filename;

        /// <summary>
        /// 日志类
        /// </summary>
        /// <param name="filename">文件夹名称</param>
        public LogFactory(string filename)
        {
            this.filename = filename;
            log = LogManager.GetCurrentClassLogger();
        }
        /// <summary>
        /// 信息
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            log.WithProperty("filename", filename).Info(message);
        }
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            log.WithProperty("filename", filename).Error(message);
        }
        /// <summary>
        /// 系统错误
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            log.WithProperty("filename", filename).Debug(message);
        }
         
    }
}
