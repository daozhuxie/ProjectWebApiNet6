/*-------------------------------------------------------
 *	Copyright (c)  , All rights reserved.
 *	Author: Xiexiangying
 *	Date:2022-02
 *	Description:日志帮助类
 *------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectWebApiNet5.Configuration
{
    /// <summary>
    /// 往指定文件夹下，每天写入日志信息
    /// </summary>
    public class LogHelperText
    {
        /// <summary>
        /// 在网站根目录下创建日志目录(bin文件夹→debug文件夹→logs文件夹)
        /// </summary>
        public static string path = AppDomain.CurrentDomain.BaseDirectory + "logs";

        /// <summary>
        /// 死锁-锁住队列
        /// </summary>
        public static object loglock = new object();
        /// <summary>
        /// 调试提示
        /// </summary>
        /// <param name="content"></param>
        public static void Debug(string content)
        {
            WriteLog("DEBUG", content);
        }
        /// <summary>
        /// 消息提示
        /// </summary>
        /// <param name="content"></param>
        public static void Info(string content)
        {
            WriteLog("INFO", content);
        }
        /// <summary>
        /// 问题提示
        /// </summary>
        /// <param name="content"></param>
        public static void Error(string content)
        {
            WriteLog("ERROR", content);
        }
        /// <summary>
        /// 开始写入日志
        /// </summary>
        /// <param name="type"></param>
        /// <param name="content"></param>
        protected static void WriteLog(string type, string content)
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
    }
}
