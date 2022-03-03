/*-------------------------------------------------------
 *	Copyright (c)  , All rights reserved.
 *	Author: Xiexiangying
 *	Date:2022-02
 *	Description:Xml转换帮助类
 *------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;

namespace ProjectWebApi.Configuration
{
    /// <summary>
    /// xml 文件序列化
    /// </summary>
    public class XmlSerializeHelper
    {
        private static String _StrValue = null;//全局值
        /// <summary>
        /// 将内容序列号成字符串
        /// </summary>
        /// <typeparam name="T">对象</typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj)
        {
            return Serialize<T>(obj, Encoding.UTF8);
        }

        /// <summary>
        /// 对象转XML
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ObjToXml(object obj)
        {
            using (MemoryStream Stream = new MemoryStream())
            {
                XmlSerializer xml = new XmlSerializer(obj.GetType());
                xml.Serialize(Stream, obj);
                Stream.Position = 0;
                StreamReader sr = new StreamReader(Stream);
                string str = sr.ReadToEnd();
                return str;
            }

        }

        /// <summary>
        /// 实体对象序列化成xml字符串  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Serialize<T>(T obj, Encoding encoding)
        {
            try
            {

                if (obj == null)
                    throw new ArgumentNullException("obj");

                var ser = new XmlSerializer(obj.GetType());
                using (var ms = new MemoryStream())
                {
                    using (var writer = new XmlTextWriter(ms, encoding))
                    {
                        writer.Formatting = Formatting.Indented;
                        ser.Serialize(writer, obj);
                    }
                    var xml = encoding.GetString(ms.ToArray());
                    xml = xml.Replace("xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"", "");
                    xml = xml.Replace("xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", "");
                    xml = Regex.Replace(xml, @"\s{2}", "");
                    xml = Regex.Replace(xml, @"\s{1}/>", "/>");
                    return xml;
                }
            }
            catch (Exception ex)
            {
                _StrValue = ex.Message;
                throw ;
            }
        }

        /// <summary>  
        /// 反序列化xml字符为对象，默认为Utf-8编码  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="xml"></param>  
        /// <returns></returns>  
        public static T DeSerialize<T>(string xml)
            where T : new()
        {
#pragma warning disable CS8603 // 可能返回 null 引用。
            return DeSerialize<T>(xml, Encoding.UTF8);
#pragma warning restore CS8603 // 可能返回 null 引用。
        }

        /// <summary>  
        /// 反序列化xml字符为对象  
        /// </summary>  
        /// <typeparam name="T"></typeparam>  
        /// <param name="xml"></param>  
        /// <param name="encoding"></param>  
        /// <returns></returns>  
        public static T? DeSerialize<T>(string xml, Encoding encoding)
            where T : new()
        {
            try
            {
                var mySerializer = new XmlSerializer(typeof(T));
                using (var ms = new MemoryStream(encoding.GetBytes(xml)))
                {
                    using (var sr = new StreamReader(ms, encoding))
                    {
#pragma warning disable CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
                        return (T)mySerializer.Deserialize(sr);
#pragma warning restore CS8600 // 将 null 字面量或可能为 null 的值转换为非 null 类型。
                    }
                }
            }
            catch (Exception ex)
            {
                _StrValue = ex.Message;
                return default(T);
            }

        }
    }
}