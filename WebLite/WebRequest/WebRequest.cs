using ExtendsLite;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebLite.WebRequest
{
    enum HttpMethod
    {
        GET,
        POST
    }

    /// <summary>
    ///  基于WebRequest发起请求
    /// </summary>
    public class WebRequest
    {
        private static readonly string GET = HttpMethod.GET.StringValue();
        private static readonly string POST = HttpMethod.POST.StringValue();

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

            HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
            request.KeepAlive = true;
            request.Method = GET;
            request.ContentType = "application/json";

            string responseContent = null;
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseContent = await reader.ReadToEndAsync();
                }
                return JsonConvert.DeserializeObject<T>(responseContent);
            }

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

            HttpWebRequest request = System.Net.WebRequest.Create(url) as HttpWebRequest;
            request.KeepAlive = true;
            request.Method = POST;
            request.ContentType = contentType;

            if (param != null)
            {
                var data = JsonConvert.SerializeObject(param);
                byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                using (Stream requestStream = await request.GetRequestStreamAsync())
                {
                    requestStream.Write(dataBytes, 0, dataBytes.Length);
                    requestStream.Flush();
                }

                string responseContent = null;
                using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        responseContent = await reader.ReadToEndAsync();
                    }
                    return JsonConvert.DeserializeObject<T>(responseContent);
                }
            }
            else
            {
                return await HttpGet<T>(url);
            }
        }
    }
}
