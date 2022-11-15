using Newtonsoft.Json;
using SignalGo.Client;
using SignalGo.Shared.Models;
using System.Security.Cryptography;
using System.Text;
using Translators.Contracts.Common;
using Translators.Helpers;
using Translators.Models.Storages;
using HttpClient = SignalGo.Client.HttpClient;

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
            if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
                url = uri.PathAndQuery;
            return Sha1Hash($"{url.ToLower().Trim('/')}_{(parameterInfoes == null || parameterInfoes.Length == 0 ? "Empty" : string.Join("#", parameterInfoes.Select(p => $"{p.Name}_{p.Value}")))}");
        }

        private static SHA1 _sha1 = SHA1.Create();
        //hex encoding of the hash, in uppercase.
        public static string Sha1Hash(this string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            data = _sha1.ComputeHash(data);
            return BitConverter.ToString(data).Replace("-", "");
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
                var saver = new ApplicationBookData();
                saver.Initialize(key);
                await saver.Add(json);
            }
        }

        public static async Task<(bool IsSuccess, string Result)> TakeData(string url, ParameterInfo[] parameterInfoes)
        {
            var saver = new ApplicationBookData();
            await saver.InitializeLoad(GetKey(url, parameterInfoes));
            if (saver.TryGet(out var value))
            {
                return (true, value);
            }
            return (false, null);
        }
    }

    public class TranslatorNoCacheHttpClient : HttpClient
    {
        public TranslatorNoCacheHttpClient()
        {
            Encoding = System.Text.Encoding.UTF8;
            UseJsonPost = true;
        }

        public override async Task<HttpClientResponse> PostAsync(string url, ParameterInfo[] parameterInfoes, BaseStreamInfo streamInfo = null)
        {
            var result = await base.PostAsync(url, parameterInfoes, streamInfo);
            _ = ClientConnectionManager.SaveLocal(url, parameterInfoes, result);
            return result;
        }

        public override T Deserialize<T>(HttpClientDataResponse data)
        {
            var contract = base.Deserialize<T>(data);
            if (contract is MessageContract messageContract)
            {
                if (!messageContract.IsSuccess && messageContract.Error?.FailedReasonType == FailedReasonType.SessionAccessDenied)
                {
                    _ = ApplicationProfileData.Login(ApplicationProfileData.Current.Value.Session);
                }
                else if (!messageContract && messageContract.Error?.FailedReasonType == FailedReasonType.InternalError)
                {
                    _ = AlertHelper.Alert("خطا", $"خطای نامشخص سرویس دهنده رخ داده است، ممکن است نسخه‌ی نرم افزار شما قدیمی باشد لطفا نسبت به بروزرسانی نرم افزار خود اقدام نمایید.");
                }
            }
            return contract;
        }
    }

    public class TranslatorHttpClient : TranslatorNoCacheHttpClient
    {
        public override async Task<HttpClientResponse> PostAsync(string url, ParameterInfo[] parameterInfoes, BaseStreamInfo streamInfo = null)
        {
            (bool Success, string Result) = await ClientConnectionManager.TakeData(url, parameterInfoes);
            if (Success)
                return new HttpClientResponse()
                {
                    Data = new HttpClientDataResponse(Result),
                    Status = System.Net.HttpStatusCode.OK
                };
            return await base.PostAsync(url, parameterInfoes, streamInfo);
        }
    }
}
