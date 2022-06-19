using Newtonsoft.Json;
using SignalGo.Client;
using SignalGo.Shared.Models;
using System.Linq;
using System.Threading.Tasks;
using Translators.Contracts.Common;
using Translators.Models.Storages;

namespace Translators.ServiceManagers
{
    public static class ClientConnectionManager
    {
        public static string GetUrl(string serviceName, string methodName)
        {
            return $"{TranslatorService.ServiceAddress}/{serviceName}/{methodName}".ToLower();
        }

        public static string GetKey(string url, ParameterInfo[] parameterInfoes)
        {
            return $"{url.ToLower().Trim('/')}_{(parameterInfoes == null ? "Null" : string.Join("#", parameterInfoes.Select(p => $"{p.Name}_{p.Value}")))}";
        }

        public static async Task SaveLocal(string url, ParameterInfo[] parameterInfoes, HttpClientResponse httpClientResponse)
        {
            var value = System.Text.Encoding.UTF8.GetString(httpClientResponse.Data.Data);
            await SaveLocal(url, parameterInfoes, value);
        }

        public static async Task SaveLocal(string url, ParameterInfo[] parameterInfoes, string json)
        {
            var messageContract = JsonConvert.DeserializeObject<MessageContract>(json);
            if (messageContract.IsSuccess)
            {
                var key = GetKey(url, parameterInfoes);
                await ApplicationBookData.Add(key, json);
            }
        }

        public static bool TakeData(string url, ParameterInfo[] parameterInfoes, out string result)
        {
            if (ApplicationBookData.TryGet(ClientConnectionManager.GetKey(url, parameterInfoes), out var value))
            {
                result = value;
                return true;
            }
            result = null;
            return false;
        }
    }

    public class TranslatorNoCacheHttpClient : HttpClient
    {
        public TranslatorNoCacheHttpClient()
        {
            Encoding = System.Text.Encoding.UTF8;
        }

        public override async Task<HttpClientResponse> PostAsync(string url, ParameterInfo[] parameterInfoes, BaseStreamInfo streamInfo = null)
        {
            var result = await base.PostAsync(url, parameterInfoes, streamInfo);
            _ = ClientConnectionManager.SaveLocal(url, parameterInfoes, result);
            return result;
        }


    }

    public class TranslatorHttpClient : TranslatorNoCacheHttpClient
    {
        public override async Task<HttpClientResponse> PostAsync(string url, ParameterInfo[] parameterInfoes, BaseStreamInfo streamInfo = null)
        {
            if (ClientConnectionManager.TakeData(url, parameterInfoes, out var value))
                return new HttpClientResponse()
                {
                    Data = new HttpClientDataResponse(value),
                    Status = System.Net.HttpStatusCode.OK
                };
            return await base.PostAsync(url, parameterInfoes, streamInfo);
        }
    }
}
