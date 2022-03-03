/*-------------------------------------------------------
 *	Copyright (c)  , All rights reserved.
 *	Author: XieXingYing
 *	Date:2022-2
 *	Description:Json帮助类
 *------------------------------------------------------*/
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace ProjectWebApi.Configuration
{
    /// <summary>
    /// Newtonsoft.Json 帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }

        /// <summary>
        /// 格式化 json 字符串的斜杠,特殊字符,
        /// </summary>
        /// <param name="jsonstr">存在特殊字符串的json 字符串</param>
        /// <returns></returns>
        public static object SerializeObjectJson(string jsonstr)
        {
            //string str = new JsonSerializer().Deserialize(new JsonTextReader(new StringReader(jsonstr))).ToString();
            //var json = new Newtonsoft.Json.JsonSerializer().Deserialize(new JsonTextReader(new System.IO.StringReader(JsonHelper.SerializeObject(obj))));
            var json = new Newtonsoft.Json.JsonSerializer().Deserialize(new JsonTextReader(new System.IO.StringReader(jsonstr)));
            //json = json.ToString().Substring(1, json.ToString().Length - 1);
            return json;
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T? t = o as T;
#pragma warning disable CS8603 // 可能返回 null 引用。
            return t;
#pragma warning restore CS8603 // 可能返回 null 引用。
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// 
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T>? list = o as List<T>;
            return o as List<T>;
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }

        /// <summary>
        /// DataTable 转 Json
        /// </summary>
        /// <param name="dtb"></param>
        /// <returns></returns>
        public static string DataTableToJson(DataTable dtb)
        {
            //JavaScriptSerializer jss = new JavaScriptSerializer();
            JsonSerializer jss = new JsonSerializer();
            System.Collections.ArrayList dic = new System.Collections.ArrayList();
            foreach (DataRow dr in dtb.Rows)
            {
                System.Collections.Generic.Dictionary<string, object> drow = new System.Collections.Generic.Dictionary<string, object>();
                foreach (DataColumn dc in dtb.Columns)
                {
                    drow.Add(dc.ColumnName.ToString(), dr[dc.ColumnName].ToString());
                }
                dic.Add(drow);
            }
            //序列化  
            return JsonConvert.SerializeObject(dic);
        }

        /// <summary>
        /// Json格式转Datable格式
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(string json)
        {
            DataTable dtt = JsonConvert.DeserializeObject<DataTable>(json);
            for (int i = 0; i < dtt.Rows.Count; i++)
            {
                DataRow dr = dtt.Rows[i];
                //Console.WriteLine("{0}	{1}	{2}	", dr[0], dr[1], dr[2]);
            }
            return dtt;
        }
        #region JavaScriptSerializer 格式 Json 转DataTable
        ///// <summary>
        ///// Json 字符串 转换为 DataTable数据集合
        ///// </summary>
        ///// <param name="json"></param>
        ///// <returns></returns>
        //public static DataTable ToDataTableTwo(string json)
        //{
        //    DataTable dataTable = new DataTable();  //实例化
        //    DataTable result;
        //    try
        //    {
        //        JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        //        javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
        //        ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
        //        if (arrayList.Count > 0)
        //        {
        //            foreach (Dictionary<string, object> dictionary in arrayList)
        //            {
        //                if (dictionary.Keys.Count<string>() == 0)
        //                {
        //                    result = dataTable;
        //                    return result;
        //                }
        //                //Columns
        //                if (dataTable.Columns.Count == 0)
        //                {
        //                    foreach (string current in dictionary.Keys)
        //                    {
        //                        dataTable.Columns.Add(current, dictionary[current].GetType());
        //                    }
        //                }
        //                //Rows
        //                DataRow dataRow = dataTable.NewRow();
        //                foreach (string current in dictionary.Keys)
        //                {
        //                    dataRow[current] = dictionary[current];
        //                }
        //                dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
        //            }
        //        }
        //    }
        //    catch
        //    {
        //    }
        //    result = dataTable;
        //    return result;
        //}
        #endregion

        /// <summary>
        /// list 转 DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            System.Reflection.PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

    }

    /// <summary>
    ///     数据表转换类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbTableConvertor<T> where T : new()
    {
        /// <summary>
        /// 将DataTable转换为实体列表
        /// </summary>
        /// <param name="dt">待转换的DataTable</param>
        /// <returns></returns>
        public static List<T> ConvertToList(DataTable dt)
        {
            // 定义集合  
            var list = new List<T>();

            if (0 == dt.Rows.Count)
            {
                return list;
            }

            // 获得此模型的可写公共属性  
            IEnumerable<PropertyInfo> propertys = new T().GetType().GetProperties().Where(u => u.CanWrite);
            list = ConvertToEntity(dt, propertys);


            return list;
        }

        /// <summary>
        /// 将DataTable的首行转换为实体
        /// </summary>
        /// <param name="dt">待转换的DataTable</param>
        /// <returns></returns>
        public static T ConvertToEntity(DataTable dt)
        {
            DataTable dtTable = dt.Clone();
            dtTable.Rows.Add(dt.Rows[0].ItemArray);
            return ConvertToList(dtTable)[0];
        }


        private static List<T> ConvertToEntity(DataTable dt, IEnumerable<PropertyInfo> propertys)
        {
            var list = new List<T>();
            //遍历DataTable中所有的数据行  
            foreach (DataRow dr in dt.Rows)
            {
                var entity = new T();

                //遍历该对象的所有属性  
                foreach (PropertyInfo p in propertys)
                {
                    //将属性名称赋值给临时变量
                    string tmpName = p.Name;

                    //检查DataTable是否包含此列（列名==对象的属性名）    
                    if (!dt.Columns.Contains(tmpName)) continue;
                    //取值  
                    object value = dr[tmpName];
                    //如果非空，则赋给对象的属性  
                    if (value != DBNull.Value)
                    {
                        p.SetValue(entity, value, null);
                    }
                }
                //对象添加到泛型集合中  
                list.Add(entity);
            }
            return list;
        }
    }
}
