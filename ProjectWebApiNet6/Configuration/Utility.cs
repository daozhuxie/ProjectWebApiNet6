using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ProjectWebApi.Configuration
{
    /// <summary>
    /// 工具方法集合
    /// </summary>
    public static class Utility
    {
        private static String? _StrValue = null;//全局值
        /// <summary>
        /// <para>获取当前调用的api路径</para>
        /// <para>net core中使用</para>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetAbsoluteUri(this HttpRequest request)
        {
            return new StringBuilder()
                .Append(request.Scheme)
                .Append("://")
                .Append(request.Host)
                .Append(request.PathBase)
                .Append(request.Path)
                .Append(request.QueryString)
                .ToString();
        }

        /// <summary>
        /// 获取文件信息
        /// <para>适用于Windows、Linux</para>
        /// </summary>
        /// <param name="fileFullPath">文件的绝对路径或完整路径</param>
        /// <param name="webRootPath">根目录，必须是一个明确的文件夹路径</param>
        /// <returns></returns>
        public static IFileInfo GetFileInfo(string fileFullPath, string webRootPath = "")
        {
            /*
             * 格式说明：
             * WINDOWS环境下示例：
             *      fileFullPath：E:\test\Doc\demo\123.txt
             *      webRootPath：可使用E:\、E:\test\等作为根目录
             * LINUX环境下示例：
             *      fileFullPath：/app/wwwroot/wordtemplate/test/
             *      webRootPath：可使用/app/、/app/wwwroot/、/app/wwwroot/wordtemplate/作为根目录
             */

            if (string.IsNullOrEmpty(webRootPath))
            {
                //webRootPath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}wwwroot";
                webRootPath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}";
            }

            Console.WriteLine(webRootPath);

            var fileSubPath = fileFullPath;
            if (fileFullPath.StartsWith(webRootPath))
            {
                var index = fileFullPath.IndexOf(webRootPath);
                Console.WriteLine(index);
                fileSubPath = fileFullPath.Substring(index + webRootPath.Length).TrimStart(Path.DirectorySeparatorChar);
            }
            Console.WriteLine(fileSubPath);

            //指明根目录
            var provider = new PhysicalFileProvider(webRootPath);
            //获取根目录{webRootPath}下所有的内容，即当前文件夹下所有文件及文件夹
            //var contents = provider.GetDirectoryContents("");
            //根据文件绝对路径获取指定文件信息
            var fileInfo = provider.GetFileInfo(fileSubPath);

            return fileInfo;
        }

        /// <summary>
        /// 根据文件流，获取文件MD5值
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileMD5(Stream file)
        {
            try
            {
                //FileStream file = new FileStream(filePath, FileMode.Open);
#pragma warning disable SYSLIB0021 // 类型或成员已过时
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
#pragma warning restore SYSLIB0021 // 类型或成员已过时
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        /// <summary>  
        /// 根据文件全路径，获取文件MD5值
        /// </summary>  
        /// <param name="path">文件地址</param>  
        /// <returns>MD5Hash</returns>  
        public static string GetFileMD5(string path)
        {
            string md5 = "";
            try
            {
                if (File.Exists(path) == false)
                {
                    return "";
                }

                int bufferSize = 1024 * 16;//自定义缓冲区大小16K  
                byte[] buffer = new byte[bufferSize];
                Stream inputStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
#pragma warning disable SYSLIB0021 // 类型或成员已过时
                System.Security.Cryptography.HashAlgorithm hashAlgorithm = new System.Security.Cryptography.MD5CryptoServiceProvider();
#pragma warning restore SYSLIB0021 // 类型或成员已过时
                int readLength = 0;//每次读取长度  
                var output = new byte[bufferSize];
                while ((readLength = inputStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    //计算MD5  
                    hashAlgorithm.TransformBlock(buffer, 0, readLength, output, 0);
                }
                //完成最后计算，必须调用(由于上一部循环已经完成所有运算，所以调用此方法时后面的两个参数都为0)  
                hashAlgorithm.TransformFinalBlock(buffer, 0, 0);
                md5 = BitConverter.ToString(hashAlgorithm.Hash);
                hashAlgorithm.Clear();
                inputStream.Close();
                md5 = md5.Replace("-", "").ToLower();
            }
            catch (Exception ex)
            {
                Console.WriteLine("EDoc2.Vdrive.UI.Core.Common.TransferUtils.GetMD5ByFile()", ex);
            }
            return md5;
        }

        /// <summary>
        /// <para>读取文件字节</para>
        /// <para>*该方法net core、net formwork均适用</para>
        /// <para>注意：File.ReadAllBytes(filePath);不适用于net core</para>
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static byte[] ReadFileBytes(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    byte[] buffur = new byte[fs.Length];
                    fs.Read(buffur, 0, (int)fs.Length);
                    return buffur;
                }
                catch (Exception ex)
                {
                    _StrValue = ex.Message;
                    throw ;
                }
            }
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string SHA1(string source)
        {
            //创建SHA1类的实例，SHA1是抽象类(abstract)所以不能直接实例化     
            System.Security.Cryptography.SHA1 sha1 = System.Security.Cryptography.SHA1.Create();

            //Unicode.GetBytes获得string的Unicode(双字节字符)字节流 
            byte[] hashedPassword = sha1.ComputeHash(Encoding.UTF8.GetBytes(source));
            return BitConverter.ToString(hashedPassword).ToLower().Replace("-", "");
        }

        /// <summary>
        /// HMAC-SHA256加密
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string HmacSHA256(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] s = hmacsha256.ComputeHash(messageBytes);
                var sb = new StringBuilder();
                for (int i = 0; i < s.Length; i++)
                {
                    sb.Append(s[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 计算MD5
        /// </summary>
        /// <param name="source"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string MD5(string source, Encoding? encode = null)
        {
            encode = encode ?? Encoding.UTF8;
#pragma warning disable SYSLIB0021 // 类型或成员已过时
            var md5 = new MD5CryptoServiceProvider();
#pragma warning restore SYSLIB0021 // 类型或成员已过时
            byte[] s = md5.ComputeHash(encode.GetBytes(source));
            var sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                sb.Append(s[i].ToString("x2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// MD5 字节格式
        /// </summary>
        /// <param name="source"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static byte[] MD5Byte(string source, Encoding? encode = null)
        {
            encode = encode ?? Encoding.UTF8;
#pragma warning disable SYSLIB0021 // 类型或成员已过时
            var md5 = new MD5CryptoServiceProvider();
#pragma warning restore SYSLIB0021 // 类型或成员已过时
            byte[] s = md5.ComputeHash(encode.GetBytes(source));

            return s;
        }

        /// <summary>
        /// 计算Base64
        /// </summary>
        /// <param name="source"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string Base64(string source, Encoding? encode = null)
        {
            var md5 = MD5Byte(source, encode);

            return Convert.ToBase64String(md5);
        }

        /// <summary>
        ///  AES 加密
        /// </summary>
        /// <param name="str">明文（待加密）</param>
        /// <param name="key">密文</param>
        /// <returns></returns>
        public static string AESEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

#pragma warning disable SYSLIB0022 // 类型或成员已过时
            var rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
#pragma warning restore SYSLIB0022 // 类型或成员已过时

            var cTransform = rm.CreateEncryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        ///  AES 解密
        /// </summary>
        /// <param name="str">明文（待解密）</param>
        /// <param name="key">密文</param>
        /// <returns></returns>
        public static string AESDecrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

#pragma warning disable SYSLIB0022 // 类型或成员已过时
            var rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
#pragma warning restore SYSLIB0022 // 类型或成员已过时

            var cTransform = rm.CreateDecryptor();
            Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Encoding.UTF8.GetString(resultArray);
        }

        /// <summary>
        /// 读取Stream到byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadStream(Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static long GetTimeStamp(DateTime d)
        {
            TimeSpan ts = d.ToUniversalTime() - new DateTime(1970, 1, 1);
            return (long)ts.TotalMilliseconds;     //精确到毫秒
        }

        /// <summary>
        /// 获取当前时间戳-精确到毫秒
        /// </summary>
        /// <returns></returns>
        public static string GetNowTimeStamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }
        /// <summary>
        /// 获取当前格式化时间
        /// </summary>
        /// <returns></returns>
        public static string GetNowFormatDatetime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static long GetLocalGetTimeStamp(DateTime d)
        {
            TimeSpan ts = d - new DateTime(1970, 1, 1);
            return (long)ts.TotalMilliseconds;     //精确到毫秒
        }

        /// <summary>
        /// 格式化时间
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static string FormatDatetime(DateTime d)
        {
            return Convert.ToString(d);// d.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 获取秒级时间戳
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static long GetSecondTimeStamp(DateTime d)
        {
            TimeSpan ts = d - new DateTime(1970, 1, 1);
            return (long)ts.TotalSeconds;
        }

        /// <summary>
        /// 时间戳转日期
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static DateTime TimeStampToDate(long d)
        {
            //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));// 提示TimeZone 方法过时
            DateTime dtStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            long lTime = long.Parse(d + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);
            return dtResult;
        }

        /// <summary>
        /// 时间戳转为时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(long timeStamp)
        {
            //return TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1)).AddMilliseconds(timeStamp);// 提示TimeZone 方法过时
            return TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local).AddMilliseconds(timeStamp);
        }

        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp">Unix时间戳格式</param>
        /// <returns>C#格式时间</returns>
        public static DateTime GetTime(string timeStamp)
        {
            //DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));// 提示TimeZone 方法过时
            DateTime dtStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);

            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>
        /// 将时间整数转为字符串
        /// </summary>
        /// <param name="longTime"></param>
        /// <returns></returns>
        public static string ConvertLongTime(long longTime)
        {
            string time = "";
            var hour = Math.Floor((decimal)longTime / 3600).ToString();
            if (hour.Length == 1)
            {
                hour = "0" + hour;
            }
            var second = (longTime % 3600);
            var minute = Math.Floor((decimal)second / 60).ToString();
            if (minute.Length == 1)
            {
                minute = "0" + minute;
            }
            time = hour + ":" + minute;
            return time;
        }

        /// <summary>
        /// Unicode转字符串
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string UnicodeToGB(string text)
        {
            if (string.IsNullOrEmpty(text) == true)
            {
                return "";
            }
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(text, "\\\\u([\\w]{4})");
            if (mc != null && mc.Count > 0)
            {
                foreach (System.Text.RegularExpressions.Match m2 in mc)
                {
                    string v = m2.Value;
                    string word = v.Substring(2);
                    byte[] codes = new byte[2];
                    int code = Convert.ToInt32(word.Substring(0, 2), 16);
                    int code2 = Convert.ToInt32(word.Substring(2), 16);
                    codes[0] = (byte)code2;
                    codes[1] = (byte)code;
                    text = text.Replace(v, Encoding.Unicode.GetString(codes));
                }
            }
            return text;
        }

        /// <summary>
        /// 对象转Json数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Serialize(object data)
        {
            var setting = new JsonSerializerSettings();
            setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            return JsonConvert.SerializeObject(data, setting);
        }

        /// <summary>
        /// Json数据转对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data);
        }

        /// <summary>
        /// DataTable 转 Json
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="tbname">表名</param>
        /// <returns></returns>
        public static string DataTableToString(DataTable dt, string tbname)
        {
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"" + tbname + "\":[");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (i > 0)
                    jsonBuilder.Append(",");
                jsonBuilder.Append("{");
                jsonBuilder.Append("\"no\":\"" + i + "\",");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (j > 0)
                        jsonBuilder.Append(",");
                    if (dt.Columns[j].DataType.Equals(typeof(DateTime)) && dt.Rows[i][j].ToString() != "")
                        jsonBuilder.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\": \"" + Convert.ToDateTime(dt.Rows[i][j].ToString()).ToString("yyyy-MM-dd HH:mm:ss") + "\"");
                    else if (dt.Columns[j].DataType.Equals(typeof(String)))
                        jsonBuilder.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\": \"" + dt.Rows[i][j].ToString().Replace("\\", "\\\\").Replace("\'", "\\\'").Replace("\t", " ").Replace("\r", " ").Replace("\n", "<br/>") + "\"");
                    else
                        jsonBuilder.Append("\"" + dt.Columns[j].ColumnName.ToLower() + "\": \"" + dt.Rows[i][j].ToString() + "\"");
                }
                jsonBuilder.Append("}");
            }
            jsonBuilder.Append("]}");

            return jsonBuilder.ToString();
        }

        #region 获取 本周、本月、本季度、本年 的开始时间或结束时间
        /// <summary>
        /// 获取开始时间
        /// 周：一周的开始时间到结束时间为：周一到周日
        ///    国际惯例为：周日到周六
        /// </summary>
        /// <param name="timeType">Week、Month、Season、Year</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime? TimeStart(TimeType timeType, DateTime now)
        {
            switch (timeType)
            {
                case TimeType.Week:
                    if ((int)now.DayOfWeek == 0)//周日
                    {
                        return now.AddDays(-6).Date;
                    }
                    return now.AddDays(-(int)now.DayOfWeek + 1).Date;
                case TimeType.Month:
                    return now.AddDays(-now.Day + 1).Date;
                case TimeType.Season:
                    var time = now.AddMonths(0 - ((now.Month - 1) % 3));
                    return time.AddDays(-time.Day + 1).Date;
                case TimeType.Year:
                    return now.AddDays(-now.DayOfYear + 1).Date;
                default:
                    return null;
            }
        }

        /// <summary>
        /// 获取结束时间
        /// </summary>
        /// <param name="timeType">Week、Month、Season、Year</param>
        /// <param name="now"></param>
        /// <returns></returns>
        public static DateTime? TimeEnd(TimeType timeType, DateTime now)
        {
            switch (timeType)
            {
                case TimeType.Week:
                    if ((int)now.DayOfWeek == 0)//周日
                    {
                        return now.Date.AddDays(1).AddMilliseconds(-1);
                    }
                    return now.AddDays(7 - (int)now.DayOfWeek).Date.AddDays(1).AddMilliseconds(-1);
                case TimeType.Month:
                    return now.AddMonths(1).AddDays(-now.AddMonths(1).Day + 1).Date.AddMilliseconds(-1);
                case TimeType.Season:
                    var time = now.AddMonths((3 - ((now.Month - 1) % 3) - 1));
                    return time.AddMonths(1).AddDays(-time.AddMonths(1).Day + 1).AddDays(-1).AddMilliseconds(-1);
                case TimeType.Year:
                    var time2 = now.AddYears(1);
                    return time2.AddDays(-time2.DayOfYear).AddMilliseconds(-1);
                default:
                    return null;
            }
        }
        #endregion

        /// <summary>
        /// 获取实体类的字段名称和值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetProperties<T>(T entity)
        {
            var ret = new Dictionary<string, string>();

            if (entity == null)
            {
                return null;
            }
            System.Reflection.PropertyInfo[] properties = entity.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return null;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;//实体类字段名称
                string value = item.GetValue(entity, null).ToString();//该字段的值

                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    ret.Add(name, value);//在此可转换value的类型
                }
                //else
                //{
                //    GetProperties(value);
                //}
            }

            return ret;
        }

        /// <summary>
        /// 已知属性设置成实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static T? SetProperties<T>(T entity, Dictionary<string, string> dic)
        {
            if (entity == null || dic == null)
            {
                return default(T);
            }
            System.Reflection.PropertyInfo[] properties = entity.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return default(T);
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;//名称
                string value = item.GetValue(entity, null).ToString();//值

                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    object val = dic.Where(c => c.Key == name).FirstOrDefault().Value;
                    if (val != null && val.ToString() != value)
                    {
                        if (item.PropertyType.Name.StartsWith("Nullable`1"))
                        {
                            item.SetValue(entity, val, null);
                        }
                        else
                        {
                            item.SetValue(entity, val, null);
                        }
                    }
                }
            }

            return entity;
        }

    }

    /// <summary>
    /// 时间格式枚举
    /// Week、Month、Season、Year
    /// </summary>
    public enum TimeType
    {
        /// <summary>
        /// 周
        /// </summary>
        Week,
        /// <summary>
        /// 月
        /// </summary>
        Month,
        /// <summary>
        /// 季度
        /// </summary>
        Season,
        /// <summary>
        /// 年
        /// </summary>
        Year
    }
}
