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
    public class RestContext
    {
        private string _baseUrl = string.Empty;

        private IDictionary<string, object> _qryString = new Dictionary<string, object>();
        private IDictionary<string, object> _header = new Dictionary<string, object>();

        private StringContent _body = null;
        private CookieContainer _coockie = null;
        private HttpClientHandler _handler = null;
        private HttpClient _client = null;

        public RestContext(string baseUrl)
        {
            this._baseUrl = baseUrl;

            this._coockie = new CookieContainer();
            this._handler = new HttpClientHandler() { CookieContainer = this._coockie };

            this._client = new HttpClient(this._handler);
            this._client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private async Task<T> SendRequest<T>(string resourceUrl, HttpMethod method, bool hasResult = true)
        {
            T result = default(T);

            HttpRequestMessage req = CreateRequest(resourceUrl, method);

            using (this._client)
            {
                HttpResponseMessage resp = await this._client.SendAsync(req);
                if (resp.IsSuccessStatusCode && hasResult)
                {
                    string strResult = await resp.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<T>(strResult);
                }
                else if (hasResult)
                    throw new Exception(string.Format("{0} operation is fail!", method));
            }

            return result;
        }

        protected HttpRequestMessage CreateRequest(string resourceUrl, HttpMethod httpMethod)
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

        public RestContext SetBody(object content)
        {
            this._body = new StringContent(JsonConvert.SerializeObject(content));
            return this;
        }

        public RestContext SetQueryString(string name, object value)
        {
            this._qryString.Add(name, value);
            return this;
        }

        public RestContext SetHeader(string name, string value)
        {
            this._header.Add(name, value);
            return this;
        }

        public RestContext SetCoockies(string name, string value)
        {
            this._coockie.Add(new Uri(this._baseUrl), new Cookie(name, value));
            return this;
        }

        public async Task<T> Get<T>(string resourceUrl = "")
        {
            T result = default(T);

            try
            {
                result = await this.SendRequest<T>(resourceUrl, HttpMethod.Put);
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        public async Task<T> Post<T>(string resourceUrl = "")
        {
            T result = default(T);

            try
            {
                result = await this.SendRequest<T>(resourceUrl, HttpMethod.Put);
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        public async Task<T> Put<T>(string resourceUrl = "")
        {
            T result = default(T);

            try
            {
                result = await this.SendRequest<T>(resourceUrl, HttpMethod.Put);
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }

        public async Task Delete(string resourceUrl = "")
        {
            try
            {
                await this.SendRequest<object>(resourceUrl, HttpMethod.Delete, false);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}