using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetCore.Infra.Rest
{
    public interface IRestContext : IDisposable
    {
        /// <summary>
        /// 設定請求基礎位址
        /// </summary>
        /// <param name="baseUrl">基礎位址</param>
        /// <returns>RestContext實體</returns>
        RestContext SetBaseAddress(string baseUrl);

        /// <summary>
        /// 清除QueryString/Body
        /// </summary>
        /// <returns>RestContext實體</returns>
        RestContext CleanParameter();

        /// <summary>
        /// Http Delete方法
        /// </summary>
        /// <typeparam name="T">Response資料型別</typeparam>
        /// <param name="resourceUrl">資源Url</param>
        /// <returns>Response</returns>
        Task<HttpResponseMessage> Delete(string resourceUrl = "");

        /// <summary>
        /// Http Get方法
        /// </summary>
        /// <typeparam name="T">Response資料型別</typeparam>
        /// <param name="resourceUrl">資源Url</param>
        /// <returns>Response</returns>
        Task<HttpResponseMessage> Get(string resourceUrl = "");

        /// <summary>
        /// Http Post方法
        /// </summary>
        /// <typeparam name="T">Response資料型別</typeparam>
        /// <param name="resourceUrl">資源Url</param>
        /// <returns>Response</returns>
        Task<HttpResponseMessage> Post(string resourceUrl = "");

        /// <summary>
        /// Http Put方法
        /// </summary>
        /// <typeparam name="T">Response資料型別</typeparam>
        /// <param name="resourceUrl">資源Url</param>
        /// <returns>Response</returns>
        Task<HttpResponseMessage> Put(string resourceUrl = "");

        /// <summary>
        /// 設定Body資訊
        /// </summary>
        /// <param name="content">參數物件</param>
        /// <returns>RestContext實體</returns>
        RestContext SetBody(object content);

        /// <summary>
        /// 設定Coockie
        /// </summary>
        /// <param name="name">名稱</param>
        /// <param name="value">值</param>
        /// <returns>RestContext實體</returns>
        RestContext SetCoockies(string name, string value);

        /// <summary>
        /// 設定標頭
        /// </summary>
        /// <param name="name">參數名稱</param>
        /// <param name="value">參數值</param>
        /// <returns>RestContext實體</returns>
        RestContext SetHeader(string name, string value);

        /// <summary>
        /// 設定QueryString資訊
        /// </summary>
        /// <param name="name">參數名稱</param>
        /// <param name="value">參數值</param>
        /// <returns>RestContext實體</returns>
        RestContext SetQueryString(string name, object value);
    }
}