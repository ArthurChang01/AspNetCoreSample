using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspNetCore.Infra.Rest
{
    public interface IRestContext
    {
        Task<T> Get<T>(string resourceUrl, IDictionary<string, object> param = null, IDictionary<string, object> header = null);

        Task<Tout> Post<Tin, Tout>(string resourceUrl, Tin param, IDictionary<string, object> header = null);

        Task<Tout> Put<Tin, Tout>(string resourceUrl, Tin param, IDictionary<string, object> queryString = null, IDictionary<string, object> header = null);
    }
}