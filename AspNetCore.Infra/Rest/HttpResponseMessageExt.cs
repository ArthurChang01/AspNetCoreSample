using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetCore.Infra.Rest
{
    public static class HttpResponseMessageExt
    {
        public async static Task<T> GetResult<T>(this HttpResponseMessage resp)
        {
            T result = default(T);

            if (resp.IsSuccessStatusCode)
            {
                string strResult = await resp.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<T>(strResult);
            }

            return result;
        }
    }
}