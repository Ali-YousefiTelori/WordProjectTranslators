using Newtonsoft.Json;
using SignalGo.Client;
using SignalGo.Shared.Models;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Models.Storages;

namespace Translators.ServiceManagers
{
    public class TranslatorNoCacheHttpClient : HttpClient
    {
        public TranslatorNoCacheHttpClient()
        {
            Encoding = System.Text.Encoding.UTF8;
        }

        public override async Task<HttpClientResponse> PostAsync(string url, ParameterInfo[] parameterInfoes, BaseStreamInfo streamInfo = null)
        {
            var result = await base.PostAsync(url, parameterInfoes, streamInfo);
            _ = SaveLocal(url, parameterInfoes, result);
            return result;
        }

        protected static string GetKey(string url, ParameterInfo[] parameterInfoes)
        {
            return $"{url}_{(parameterInfoes == null ? "Null" : string.Join("#", parameterInfoes.Select(p => $"{p.Name}_{p.Value}")))}";
        }

        protected static async Task SaveLocal(string url, ParameterInfo[] parameterInfoes, HttpClientResponse httpClientResponse)
        {
            var value = System.Text.Encoding.UTF8.GetString(httpClientResponse.Data.Data);
            var messageContract = JsonConvert.DeserializeObject<MessageContract>(value);
            if (messageContract.IsSuccess)
            {
                var key = GetKey(url, parameterInfoes);
                await ApplicationBookData.Add(key, value);
            }
        }
    }

    public class TranslatorHttpClient : TranslatorNoCacheHttpClient
    {
        public override async Task<HttpClientResponse> PostAsync(string url, ParameterInfo[] parameterInfoes, BaseStreamInfo streamInfo = null)
        {
            if (ApplicationBookData.TryGet(GetKey(url, parameterInfoes), out var value))
                return new HttpClientResponse()
                {
                    Data = new HttpClientDataResponse(value),
                    Status = System.Net.HttpStatusCode.OK
                };
            return await base.PostAsync(url, parameterInfoes, streamInfo);
        }
    }
}
