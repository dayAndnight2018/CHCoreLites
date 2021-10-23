using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebLite.WebRequest
{
    /// <summary>
    /// 基于HttpClient的网络请求
    /// </summary>
    public class WebClient
    {

        /// <summary>
        ///  发起Get请求
        /// </summary>
        /// <typeparam name="T">期望返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <returns>期望请求类型返回</returns>
        public static async Task<T> HttpGet<T>(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("The request url is null or empty");
            }

            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            return default(T);
        }

        /// <summary>
        ///  发起Post请求
        /// </summary>
        /// <typeparam name="T">期望响应类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="contentType">内容协商</param>
        /// <param name="param">发送参数</param>
        /// <returns>期望请求类型返回</returns>
        public static async Task<T> HttpPost<T>(string url, string contentType = "application/json", object param = null)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new Exception("The request url is null or empty");
            }

            HttpClient client = new HttpClient();
            if (param != null)
            {
                var data = JsonConvert.SerializeObject(param);
                HttpContent httpContent = new StringContent(data);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                var responseContent = await client.PostAsync(url, httpContent).Result.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseContent);
            }
            else
            {
                return await HttpGet<T>(url);
            }
        }
    }
}
