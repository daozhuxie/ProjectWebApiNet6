using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ProjectWebApi.Configuration
{
    /// <summary>
    /// 封装接口请求
    /// </summary>
    public class HttpClientHelper
    {
        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static string Post(string url, object requestData)
        {
            string jsonContent = JsonConvert.SerializeObject(requestData);
            string responseBody = string.Empty;
            //Logger.WriteLine($"POST(3)请求调用地址：{url} ，传参：{jsonContent}");
            using (HttpClient httpClient = new HttpClient())
            {
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("Method", "Post");
                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
                response.EnsureSuccessStatusCode();
                responseBody = response.Content.ReadAsStringAsync().Result;
            }
            //Logger.WriteLine($"返回值为：{responseBody}");

            return responseBody;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="requestData"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static T Post<T>(string url, object requestData, string token = "")
        {
            string jsonContent = JsonConvert.SerializeObject(requestData);
            string responseBody = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                if (string.IsNullOrEmpty(token) == false)
                    httpClient.DefaultRequestHeaders.Add("token", token);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("Method", "Post");
                var response = httpClient.PostAsync(url, content).Result;
                var code = response.EnsureSuccessStatusCode();
                responseBody = response.Content.ReadAsStringAsync().Result;

                return JsonConvert.DeserializeObject<T>(responseBody);
            }
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestData"></param>
        /// <param name="mediaType"></param>
        /// <returns></returns>
        public static string Post(string url, string requestData, string mediaType = "application/json")
        {
            string responseBody = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                var content = new StringContent(requestData, Encoding.UTF8, mediaType);
                httpClient.DefaultRequestHeaders.Add("Method", "Post");
                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
                var code = response.EnsureSuccessStatusCode();
                responseBody = response.Content.ReadAsStringAsync().Result;
            }

            return responseBody;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="jsonContent"></param>
        /// <returns></returns>
        public static string Post(string url, string jsonContent)
        {
            //string jsonContent = JsonConvert.SerializeObject(requestData);
            string responseBody = string.Empty;
            //Logger.WriteLine($"POST(3)请求调用地址：{url} ，传参：{jsonContent}");
            using (HttpClient httpClient = new HttpClient())
            {
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("Method", "Post");
                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
                response.EnsureSuccessStatusCode();
                responseBody = response.Content.ReadAsStringAsync().Result;
            }
            //Logger.WriteLine($"返回值为：{responseBody}");

            return responseBody;
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, object requestData)
        {
            string jsonContent = JsonConvert.SerializeObject(requestData);
            string responseBody = string.Empty;
            using (HttpClient httpClient = new HttpClient())
            {
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("Method", "Post");
                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }
            //Logger.WriteLine($"POST(1)请求调用地址：{url} ，传参：{jsonContent}，返回值为：{responseBody}");

            return responseBody;
        }

        /// <summary>
        /// Post请求
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="requestData"></param>
        /// <returns></returns>
        public static async Task<T> PostAsync<T>(string url, object requestData)
        {
            var jsonContent = JsonConvert.SerializeObject(requestData);

            using (HttpClient httpClient = new HttpClient())
            {
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("Method", "Post");
                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                var suc = response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();

                //Logger.WriteLine($"POST(2)请求调用地址：{url} ，传参：{jsonContent}，返回值为：{responseBody}");

                return JsonConvert.DeserializeObject<T>(responseBody);
            }
        }


        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> GetAsync(string url)
        {
            string responseBody = string.Empty;

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }

            //Logger.WriteLine($"GET(1)请求调用地址：{url} ，返回值为：{responseBody}");
            return responseBody;
        }

        /// <summary>
        /// GET 异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(string url)
        {
            string responseBody = string.Empty;

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                var suc = response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();

                //Logger.WriteLine($"GET(2)请求调用地址：{url} ，返回值为：{responseBody}");
                return JsonConvert.DeserializeObject<T>(responseBody);
            }
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static T Get<T>(string url)
        {
            string responseBody = string.Empty;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                responseBody = httpClient.GetStringAsync(url).Result;
            }

            //Logger.WriteLine($"GET(3)请求调用地址：{url} ，返回值为：{responseBody}");

            return JsonConvert.DeserializeObject<T>(responseBody);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string Get(string url)
        {
            string responseBody = string.Empty;

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
                responseBody = httpClient.GetStringAsync(url).Result;

                return responseBody;
            }

            //Logger.WriteLine($"GET(3)请求调用地址：{url} ，返回值为：{responseBody}");

            //return JsonConvert.DeserializeObject<T>(responseBody);
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="dicParams"></param>
        /// <returns></returns>
        public static async Task<string> Get(string baseUrl, Dictionary<string, string> dicParams)
        {
            var url = "";
            var param = "";
            if (dicParams != null && dicParams.Count > 0)
            {
                foreach (var item in dicParams)
                {
                    param += item.Key + "=" + item.Value + "&";
                }

                url = baseUrl + "?" + param.TrimEnd('&');
            }
            else
            {
                url = baseUrl;
            }

            string responseBody = string.Empty;

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }

            //Logger.WriteLine($"GET(4)请求调用地址：{url} ，返回值为：{responseBody}");
            return responseBody;
        }

        //private static void AddDefaultHeaders(HttpClient httpClient)
        //{
        //    httpClient.DefaultRequestHeaders.Add("x-www-foo", "123");
        //    httpClient.DefaultRequestHeaders.Add("x-www-bar", "456");
        //    httpClient.DefaultRequestHeaders.Add("x-www-baz", "789");
        //}


    }
}
