/*-------------------------------------------------------
 *	Copyright (c)  , All rights reserved.
 *	Author: Xiexiangying
 *	Date:2017-11
 *	Description:API接口消息帮助类
 *------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWebApi.Configuration
{
    /// <summary>
    /// 封装访问HttpPostApi访问形式
    /// </summary>
    public class HttpPostApi
    {
        #region HttpPost批量接口请求
        /// <summary>
        /// 接口请求模板
        /// </summary>
        public class GetHttpPostApiModel1
        {
            /// <summary>
            /// url
            /// </summary>
            public string url { get; set; }
            /// <summary>
            /// 请求类型
            /// </summary>
            public string Method { get; set; }
            /// <summary>
            /// Body传参 字符串转json
            /// </summary>
            public string strjson { get; set; }
            /// <summary>
            /// Headers传参 
            /// 列如： key1:admin,key2:123,key3:1234
            /// </summary>
            //public string headersArray { get; set; }
            public List<headersArray> headersArray { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public GetHttpPostApiModel1()
            {
                headersArray = new List<headersArray>();
            }

        }
        /// <summary>
        /// 键值对
        /// </summary>
        public class headersArray
        {
            /// <summary>
            /// 
            /// </summary>
            public string headersKey { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string headersValue { get; set; }
        }

        /// <summary>
        /// HttpPost 手动配置，可多个url 连调
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static DataResult<Object> GetHttpPostApi(GetHttpPostApiModel1 viewModel)
        {
            string Authorization = "", Data = "";//接收变量 token,接收返回结果集
            Init();
            //LogHelper.WriteLog("请求url:" + viewModel.Data.logurl);
            DataResult<Object> results = new DataResult<object>();
            List<headersArray> headersArraylist = viewModel.headersArray;
#pragma warning disable SYSLIB0014 // 类型或成员已过时
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(viewModel.url.Split(',')[0]);
#pragma warning restore SYSLIB0014 // 类型或成员已过时
            req.Accept = "*/*";
            req.Method = "POST";
            req.ContentType = "application/json; charset=utf-8";

            //body 传参 获取登录token 
            if (string.IsNullOrEmpty(viewModel.strjson))
                viewModel.strjson = "";
            byte[] data = Encoding.UTF8.GetBytes(viewModel.strjson.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            //HttpWebRequest resps = (HttpWebRequest)HttpWebRequest.Create(viewModel.Data.url);
            //ServicePointManager.ServerCertificatidationCallback = new System.Net.Security.RemoteCertificatidationCallback(true);//验证服务器证书回调自动验证
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                //LogHelper.WriteLog("请求token_resp =" + resp);
                //LogHelper.WriteLog("请求token_reader =" + reader);
                Data = reader.ReadToEnd();
                Authorization = resp.GetResponseHeader("Authorization");
            }
            //-----------------------------------------------------------------------------------------------------------------------
#pragma warning disable SYSLIB0014 // 类型或成员已过时
            req = (HttpWebRequest)WebRequest.Create(viewModel.url.Split(',')[1]);
#pragma warning restore SYSLIB0014 // 类型或成员已过时
            req.Accept = "*/*";
            req.Method = "POST";
            //req.Method = viewModel.Data.Method;
            req.ContentType = "application/json; charset=utf-8";
            req.Headers.Add("Authorization", Authorization);//headers 传参token 获取科室
            req.ContentLength = 0;

            resp = (HttpWebResponse)req.GetResponse();
            stream = resp.GetResponseStream();
            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                Data = reader.ReadToEnd();
                //LogHelper.WriteLog("请求科室Data =" + Data);
            }

            GetHttpPostApiModel1 MMMMs = JsonHelper.DeserializeJsonToObject<GetHttpPostApiModel1>(Data);

            results.Data = Data;
            results.ResultCode = 200;

            return results;
        }

        private static void Init()
        {
#pragma warning disable CS8622 // 参数类型中引用类型的为 Null 性与目标委托不匹配(可能是由于为 Null 性特性)。
            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
#pragma warning restore CS8622 // 参数类型中引用类型的为 Null 性与目标委托不匹配(可能是由于为 Null 性特性)。
        }

        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors error)
        {
            System.Console.WriteLine("Warning, trust any certificate");
            //为了通过证书验证，总是返回true
            return true;
        }

        #endregion

        #region HttpPostApi正式可用
        /// <summary>
        /// 传统的HttpPost接口调用
        /// </summary>
        /// <param name="url">指定调用接口的Ip地址</param>
        /// <param name="jsonStr">传参的Json格式</param>
        /// <returns></returns>
        public static string HttpPost(string url, string jsonStr)
        {
            string result = "";
#pragma warning disable SYSLIB0014 // 类型或成员已过时
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
#pragma warning restore SYSLIB0014 // 类型或成员已过时
            req.Method = "POST";
            req.ContentType = "application/json;charset=utf-8";
            req.Accept = "*/*";
            req.Timeout = 10000;// 超时10秒
            req.ReadWriteTimeout = 10000;// 超时10秒
            req.KeepAlive = false;
            req.Proxy = null;
            req.AllowAutoRedirect = true;

            #region 添加Post 参数
            StringBuilder builder = new StringBuilder();

            byte[] data = Encoding.UTF8.GetBytes(jsonStr.ToString());
            req.ContentLength = data.Length;
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(data, 0, data.Length);
                reqStream.Close();
            }
            #endregion
            try
            {
                //强制回收垃圾
                //System.GC.Collect();
                
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                //req.UseDefaultCredentials = true;
                Stream stream = resp.GetResponseStream();
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
        #endregion

        #region HttpGetApi正式可用

        /// <summary>
        /// HttpGet
        /// </summary>
        /// <param name="url">Get请求的Url地址</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
#pragma warning disable SYSLIB0014 // 类型或成员已过时
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
#pragma warning restore SYSLIB0014 // 类型或成员已过时
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.UserAgent = null;
            request.Timeout = 500000;
            string retString = "";
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
            }
            catch (Exception ex)
            {
                retString = ex.ToString();
            }
            
            return retString;
        }
        #endregion

        #region HttpPostApi返回Text 正式可用
        /// <summary>
        /// 传统的HttpPost接口调用 application/x-www-form-urlencoded
        /// </summary>
        /// <param name="url">指定调用接口的Ip地址</param>
        /// <param name="postdata">传参appid=ssss并且loginid=shuy格式</param>
        /// <returns></returns>
        public static string HttpPostTest(string url, string postdata)
        {
            string EndResult = "";
            string SendMessageAddress = url;
#pragma warning disable SYSLIB0014 // 类型或成员已过时
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SendMessageAddress);
#pragma warning restore SYSLIB0014 // 类型或成员已过时
            request.Method = "POST";
            request.AllowAutoRedirect = true;
            request.Timeout = 5000; //5 秒
            request.ReadWriteTimeout = 5000;//5 秒
            request.ContentType = "application/x-www-form-urlencoded";
            request.Headers.Add("x-cherun-auth-key", "LarxMbndsxfGwoYAqsfJSPPU42l04cb3");
            //string PostData = "appid=ssss&loginid=shuy";
            byte[] byteArray = Encoding.Default.GetBytes(postdata);
            request.ContentLength = byteArray.Length;
            using (Stream newStream = request.GetRequestStream())
            {
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream rspStream = response.GetResponseStream();
            using (StreamReader reader = new StreamReader(rspStream, Encoding.UTF8))
            {
                EndResult = reader.ReadToEnd();
                rspStream.Close();
            }
            response.Close();

            return EndResult;
        }
        #endregion

    }

}
