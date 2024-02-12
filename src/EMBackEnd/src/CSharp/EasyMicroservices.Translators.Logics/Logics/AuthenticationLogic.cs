//using Kavenegar;
//using SignalGo.Shared.Log;
//using System;
//using System.Threading.Tasks;
//using Translators.Contracts.Common;
//using Translators.Database.Contexts;
//using Translators.Database.Entities.Authentications;

//namespace Translators.Logics
//{
//    public class AuthenticationLogic
//    {
//        static Random random = new Random();
//        public static string GetRandomCode()
//        {
//            return random.Next(100000, 999999).ToString();
//        }

//        public static async Task<MessageContract> SendSms(string number, string code)
//        {
//            var smsUserEntity = await new LogicBase<TranslatorContext, SMSUserEntity, SMSUserEntity>().Find();
//            if (smsUserEntity == null)
//                return "سیستم پیامکی یافت نشد.";
//            Logger.LogText($"number: {number} code: {code}");
//            KavenegarApi kavenegar = new KavenegarApi(smsUserEntity.Result.ApiSession);
//            var result = await kavenegar.VerifyLookup(number, code, smsUserEntity.Result.PatternKey);
//            if (result.Status == 5)
//                return true;
//            return result.Message;
//        }
//    }
//}
