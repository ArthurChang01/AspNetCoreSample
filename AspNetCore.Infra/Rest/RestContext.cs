using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCore.Infra.Rest
{
    public class RestContext
    {
        private string _baseUrl = string.Empty;
        private HttpClient _client = null;

        public RestContext(string baseUrl)
        {
            this._baseUrl = baseUrl;
            this._client = new HttpClient();
            this._client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected HttpRequestMessage CreateRequest(string resourceUrl, HttpMethod httpMethod, IDictionary<string, object> param = null, IDictionary<string, object> header = null)
        {
            HttpRequestMessage req = new HttpRequestMessage();

            string strUrlTemplate = param != null ?
                string.Format("{0}/{1}?{2}", this._baseUrl, resourceUrl, string.Join("&", param.Select(o => string.Format("{0}={1}", o.Key, o.Value.ToString())))) :
                string.Format("{0}/{1}", this._baseUrl, resourceUrl);

            req.RequestUri = new Uri(strUrlTemplate);
            req.Method = httpMethod;

            if (header == null || header.Count == 0)
                return req;

            foreach (KeyValuePair<string, object> kv in header)
            {
                req.Headers.Add(kv.Key, kv.Value.ToString());
            }

            return req;
        }

        public async Task<T> Get<T>(string resourceUrl, IDictionary<string, object> param = null, IDictionary<string, object> header = null)
        {
            T result = default(T);

            HttpRequestMessage req = CreateRequest(resourceUrl, HttpMethod.Get, param, header);

            using (this._client)
            {
                HttpResponseMessage resp = await this._client.SendAsync(req);
                if (resp.IsSuccessStatusCode)
                {
                    string strResult = await resp.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<T>(strResult);
                }
                else
                    throw new Exception("Http Get operation is fail");
            }

            return result;
        }

        public async Task<Tout> Post<Tin, Tout>(string resourceUrl, Tin param, IDictionary<string, object> header = null)
        {
            Tout result = default(Tout);

            HttpRequestMessage req = CreateRequest(resourceUrl, HttpMethod.Post, header: header);
            req.Content = new StringContent(JsonConvert.SerializeObject(param));

            using (this._client)
            {
                HttpResponseMessage resp = await this._client.SendAsync(req);
                if (resp.IsSuccessStatusCode)
                {
                    string strResult = await resp.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Tout>(strResult);
                }
                else
                    throw new Exception("Http Post operation is fail");
            }

            return result;
        }

        public async Task<Tout> Put<Tin, Tout>(string resourceUrl, Tin param, IDictionary<string, object> queryString = null, IDictionary<string, object> header = null)
        {
            Tout result = default(Tout);

            HttpRequestMessage req = CreateRequest(resourceUrl, HttpMethod.Put, queryString, header);
            req.Content = new StringContent(JsonConvert.SerializeObject(param));

            using (this._client)
            {
                HttpResponseMessage resp = await this._client.SendAsync(req);
                if (resp.IsSuccessStatusCode)
                {
                    string strResult = await resp.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<Tout>(strResult);
                }
                else
                    throw new Exception("Http Put operation is fail");
            }

            return result;
        }
    }
}