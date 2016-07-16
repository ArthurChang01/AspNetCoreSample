using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Infra.Rest
{
    public class RestContext : IRestContext
    {
        #region 私有成員

        //請求的基礎位址
        private string _baseUrl = string.Empty;

        //QueryString字典集合
        private IDictionary<string, object> _qryString = new Dictionary<string, object>();

        //Header字典集合
        private IDictionary<string, object> _header = new Dictionary<string, object>();

        //請求的Body
        private StringContent _body = null;

        //Coockie容器
        private CookieContainer _coockie = null;

        private HttpClientHandler _handler = null;
        private HttpClient _client = null;

        #endregion 私有成員

        #region 建構子

        public RestContext()
        {
        }

        public RestContext(string baseUrl)
        {
            this._baseUrl = baseUrl;

            ConcretHttpClient();
        }

        #endregion 建構子

        #region 私有方法

        /// <summary>
        /// 組態和創建HttpClient相關物件
        /// </summary>
        private void ConcretHttpClient()
        {
            this._coockie = new CookieContainer();
            this._handler = new HttpClientHandler() { CookieContainer = this._coockie };

            this._client = new HttpClient(this._handler);
            this._client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        /// <summary>
        /// 傳送請求
        /// </summary>
        /// <typeparam name="T">Response型別</typeparam>
        /// <param name="resourceUrl">資源Url</param>
        /// <param name="method">HttpMethod(Get, Post, Put, Delete)</param>
        /// <param name="hasResult">是否有回傳值</param>
        /// <returns>Response</returns>
        private async Task<HttpResponseMessage> SendRequest(string resourceUrl, HttpMethod method)
        {
            HttpResponseMessage resp = null;

            HttpRequestMessage req = CreateRequest(resourceUrl, method);

            try
            {
                resp = await this._client.SendAsync(req);
            }
            catch (Exception)
            {
                throw;
            }

            return resp;
        }

        /// <summary>
        /// 創建HttpRequestMessage物件
        /// </summary>
        /// <param name="resourceUrl">資源Url</param>
        /// <param name="httpMethod">Http Method(Get, Post, Put, Delete)</param>
        /// <returns>HttpRequestMessage物件</returns>
        private HttpRequestMessage CreateRequest(string resourceUrl, HttpMethod httpMethod)
        {
            HttpRequestMessage req = new HttpRequestMessage();

            string strUrlTemplate = _qryString.Count > 0 ?
                string.Format("{0}/{1}?{2}", this._baseUrl, resourceUrl, string.Join("&", _qryString.Select(o => string.Format("{0}={1}", o.Key, o.Value.ToString())))) :
                string.Format("{0}/{1}", this._baseUrl, resourceUrl);

            req.RequestUri = new Uri(strUrlTemplate);
            req.Method = httpMethod;

            foreach (KeyValuePair<string, object> kv in _header)
            {
                req.Headers.Add(kv.Key, kv.Value.ToString());
            }

            if (this._body != null)
                req.Content = this._body;

            return req;
        }

        #endregion 私有方法

        #region 公開方法

        #region 組態

        /// <summary>
        /// 設定請求基礎位址
        /// </summary>
        /// <param name="baseUrl">基礎位址</param>
        /// <returns>RestContext實體</returns>
        public RestContext SetBaseAddress(string baseUrl)
        {
            ConcretHttpClient();
            this._baseUrl = baseUrl;

            return this;
        }

        /// <summary>
        /// 設定Body資訊
        /// </summary>
        /// <param name="content">參數物件</param>
        /// <returns>RestContext實體</returns>
        public RestContext SetBody(object content)
        {
            this._body = new StringContent(
                JsonConvert.SerializeObject(content),
                Encoding.UTF8,
                "application/json");

            return this;
        }

        /// <summary>
        /// 設定QueryString資訊
        /// </summary>
        /// <param name="name">參數名稱</param>
        /// <param name="value">參數值</param>
        /// <returns>RestContext實體</returns>
        public RestContext SetQueryString(string name, object value)
        {
            this._qryString.Add(name, value);
            return this;
        }

        /// <summary>
        /// 設定標頭
        /// </summary>
        /// <param name="name">參數名稱</param>
        /// <param name="value">參數值</param>
        /// <returns>RestContext實體</returns>
        public RestContext SetHeader(string name, string value)
        {
            this._header.Add(name, value);
            return this;
        }

        /// <summary>
        /// 設定Coockie
        /// </summary>
        /// <param name="name">名稱</param>
        /// <param name="value">值</param>
        /// <returns>RestContext實體</returns>
        public RestContext SetCoockies(string name, string value)
        {
            this._coockie.Add(new Uri(this._baseUrl), new Cookie(name, value));
            return this;
        }

        /// <summary>
        /// 清除QueryString/Body
        /// </summary>
        /// <returns>RestContext實體</returns>
        public RestContext CleanParameter()
        {
            this._qryString.Clear();

            if (this._body != null)
                this._body.Dispose();

            return this;
        }

        #endregion 組態

        #region Http

        /// <summary>
        /// Http Get方法
        /// </summary>
        /// <typeparam name="T">Response資料型別</typeparam>
        /// <param name="resourceUrl">資源Url</param>
        /// <returns>Response</returns>
        public async Task<HttpResponseMessage> Get(string resourceUrl = "")
        {
            HttpResponseMessage resp = null;

            try
            {
                resp = await this.SendRequest(resourceUrl, HttpMethod.Get);
            }
            catch (Exception)
            {
                throw;
            }

            return resp;
        }

        /// <summary>
        /// Http Post方法
        /// </summary>
        /// <typeparam name="T">Response資料型別</typeparam>
        /// <param name="resourceUrl">資源Url</param>
        /// <returns>Response</returns>
        public async Task<HttpResponseMessage> Post(string resourceUrl = "")
        {
            HttpResponseMessage resp = null;

            try
            {
                resp = await this.SendRequest(resourceUrl, HttpMethod.Post);
            }
            catch (Exception)
            {
                throw;
            }

            return resp;
        }

        /// <summary>
        /// Http Put方法
        /// </summary>
        /// <typeparam name="T">Response資料型別</typeparam>
        /// <param name="resourceUrl">資源Url</param>
        /// <returns>Response</returns>
        public async Task<HttpResponseMessage> Put(string resourceUrl = "")
        {
            HttpResponseMessage resp = null;

            try
            {
                resp = await this.SendRequest(resourceUrl, HttpMethod.Put);
            }
            catch (Exception)
            {
                throw;
            }

            return resp;
        }

        /// <summary>
        /// Http Delete方法
        /// </summary>
        /// <typeparam name="T">Response資料型別</typeparam>
        /// <param name="resourceUrl">資源Url</param>
        /// <returns>Response</returns>
        public async Task<HttpResponseMessage> Delete(string resourceUrl = "")
        {
            HttpResponseMessage resp = null;

            try
            {
                resp = await this.SendRequest(resourceUrl, HttpMethod.Delete);
            }
            catch (Exception)
            {
                throw;
            }

            return resp;
        }

        #endregion Http

        public void Dispose()
        {
            this.CleanParameter();

            this._handler.Dispose();

            this._client.Dispose();
        }

        #endregion 公開方法
    }
}