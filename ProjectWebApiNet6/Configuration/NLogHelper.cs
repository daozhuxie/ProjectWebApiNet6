/*-------------------------------------------------------
 *	Copyright (c)  , All rights reserved.
 *	Author: Xiexiangying
 *	Date:2022-02
 *	Description:日志帮助类
 *------------------------------------------------------*/
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebApi.Configuration
{
    /// <summary>
    /// Nlog日志帮助类
    /// Trace 包含大量的信息，例如 protocol payloads。一般仅在开发环境中启用, 仅输出不存文件。
    /// Debug  比 Trance 级别稍微粗略，一般仅在开发环境中启用, 仅输出不存文件。
    /// Info 一般在生产环境中启用。
    /// Warn 一般用于可恢复或临时性错误的非关键问题。
    /// Error 一般是异常信息。
    /// Fatal - 非常严重的错误！
    /// </summary>
    public class NLogHelper
    {
        readonly Logger logger;

        private NLogHelper(Logger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 自定义 ${logger} (我用于区分文件夹)
        /// </summary>
        /// <param name="name"></param>
        public NLogHelper(string name= "Default") : this(NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetLogger(name))
        {
        }

        /// <summary>
        /// 默认 ${logger} (Default 文件夹下)
        /// </summary>
        public static NLogHelper Default { get; private set; }
        static NLogHelper()
        {
            Default = new NLogHelper(NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetLogger("Default"));
        }
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public void Debug(string msg, params object[] args)
        {
            Console.WriteLine(msg);
            logger.Debug(msg, args);
        }
        /// <summary>
        /// 调试
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="err"></param>
        public void Debug(string msg, Exception err)
        {
            Console.WriteLine(msg);
            logger.Debug(err, msg);
        }
        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public void Info(string msg, params object[] args)
        {
            Console.WriteLine(msg);
            logger.Info(msg, args);
        }
        /// <summary>
        /// 消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="err"></param>
        public void Info(string msg, Exception err)
        {
            Console.WriteLine(msg);
            logger.Info(err, msg);
        }
        /// <summary>
        /// 查出问题
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public void Trace(string msg, params object[] args)
        {
            Console.WriteLine(msg);
            logger.Trace(msg, args);
        }
        /// <summary>
        /// 查出问题
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="err"></param>
        public void Trace(string msg, Exception err)
        {
            Console.WriteLine(msg);
            logger.Trace(err, msg);
        }
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public void Error(string msg, params object[] args)
        {
            Console.WriteLine(msg);
            logger.Error(msg, args);
        }
        /// <summary>
        /// 错误
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="err"></param>
        public void Error(string msg, Exception err)
        {
            Console.WriteLine(msg);
            logger.Error(err, msg);
        }
        /// <summary>
        /// 致命问题
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="args"></param>
        public void Fatal(string msg, params object[] args)
        {
            Console.WriteLine(msg);
            logger.Fatal(msg, args);
        }
        /// <summary>
        /// 致命问题
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="err"></param>
        public void Fatal(string msg, Exception err)
        {
            Console.WriteLine(msg);
            logger.Fatal(err, msg);
        }
    }
}
