/*-------------------------------------------------------
 *	Copyright (c)  , All rights reserved.
 *	Author: Xiexiangying
 *	Date:2022-02
 *	Description:日志帮助类
 *------------------------------------------------------*/
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWebApi.Configuration
{
    /// <summary>
    /// 日志封装记录类
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 初始化时间
        /// </summary>
        private string strTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        #region 封装Microsoft.Extensions.Logging的写法抽象,需要先在类内构造调用
        /// <summary>
        /// 定义对象
        /// </summary>
        private readonly ILogger<LogHelper> _logger;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="logger"></param>
        public LogHelper(ILogger<LogHelper> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 封装日志 info/error/dug/
        /// </summary>
        /// <param name="logType">日志类型</param>
        /// <param name="LogString"></param>
        public void WriteLog(string logType, string LogString)
        {
            _logger.LogError(dateTime() + ":" + LogString);
        }
        /// <summary>
        /// 封装日志
        /// </summary>
        /// <param name="LogString"></param>
        public void WriteLog(String LogString)
        {
            _logger.LogError(dateTime() + ":" + LogString);
        }

        ///// <summary>
        ///// 日志调用实列
        ///// </summary>
        ///// <returns></returns>
        //public void  Test()
        //{
        //    string str = "我是日志内容";
        //    _logger.LogError(str);
        //}
        #endregion

        #region 封装Net5/Core-Logger的写法
        //readonly NLog.Logger logger;
        //private LogHelper(NLog.Logger logger)
        //{
        //    this.logger = logger;
        //}
        ///// <summary>
        ///// 自定义 ${logger} (我用于区分文件夹)
        ///// </summary>
        ///// <param name="name"></param>
        //public LogHelper(string name = "Default") : this(NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetLogger(name))
        //{
        //}
        ///// <summary>
        ///// 默认 ${logger} (Default 文件夹下)
        ///// </summary>
        //public static LogHelper Default { get; private set; }
        //static LogHelper()
        //{
        //    Default = new LogHelper(NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetLogger("Default"));
        //}

        //public void NLogInfo(string msg, params object[] args)
        //{
        //    Console.WriteLine(msg);
        //    logger.Info(msg, args);
        //}
        ////public object Nlogger = NLog.LogManager.GetCurrentClassLogger();
        ///// <summary>
        ///// 封装 NLog 的写法
        ///// </summary>
        //public static void NLogInfo()
        //{
        //    var logger = NLog.LogManager.GetCurrentClassLogger();
        //    logger.Info("Hello {Name}", "Info");
        //    logger.Debug("Hello {Name}", "Debug");
        //    logger.Error("Hello {Name}", "Error");
        //    logger.Info("Hello {Info}");
        //    logger.Debug("Hello {Debug}");
        //    logger.Error("Hello {Error}");
        //}

        #endregion

        #region 封装 Console消息记录的写法
        /// <summary>
        /// Console消息记录的写法
        /// </summary>
        public static void ConsoleWriteLine()
        {
            Console.WriteLine("消息弹窗：不带标签：ConsoleWriteLine");
        }
        #endregion

        #region 封装日志写入Text文件夹
        /// <summary>
        /// 在网站根目录下创建日志目录(bin文件夹→debug文件夹→logs文件夹)
        /// </summary>
        public static string path = AppDomain.CurrentDomain.BaseDirectory + "logs";
        /// <summary>
        /// 死锁-锁住队列
        /// </summary>
        public static object loglock = new object();
        /// <summary>
        /// Text写入调试提示
        /// </summary>
        /// <param name="content"></param>
        public static void TextDebug(string content)
        {
            TextWriteLog("DEBUG", content);
        }
        /// <summary>
        /// Text写入消息提示
        /// </summary>
        /// <param name="content"></param>
        public static void TextInfo(string content)
        {
            TextWriteLog("INFO", content);
        }
        /// <summary>
        /// Text写入问题提示
        /// </summary>
        /// <param name="content"></param>
        public static void TextError(string content)
        {
            TextWriteLog("ERROR", content);
        }
        /// <summary>
        /// 封装往Text记录
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        protected static void TextWriteLog(string type, string content)
        {
            lock (loglock)
            {
                if (!Directory.Exists(path))//如果日志目录不存在就创建
                {
                    Directory.CreateDirectory(path);
                }

                string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff");//获取当前系统时间
                string filename = path + "/" + DateTime.Now.ToString("yyyy-MM-dd") + ".log";//用日期对日志文件命名

                //创建或打开日志文件，向日志文件末尾追加记录
                StreamWriter mySw = File.AppendText(filename);

                //向日志文件写入内容
                string write_content = time + " " + type + ": " + content;
                mySw.WriteLine(write_content);

                //关闭日志文件
                mySw.Close();
            }
        }
        #endregion

        #region 获取实时时间
        /// <summary>
        /// 获取实时时间
        /// </summary>
        /// <returns></returns>
        public string dateTime()
        {
            strTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            return strTime;
        }
        #endregion
    }
}
