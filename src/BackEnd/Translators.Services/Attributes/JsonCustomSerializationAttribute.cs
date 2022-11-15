using Newtonsoft.Json;
using SignalGo.Server.ServiceManager;
using SignalGo.Shared.DataTypes;
using SignalGo.Shared.Log;

namespace Translators.Attributes
{
    public class JsonCustomSerializationAttribute : CustomSerializerAttribute
    {
        public JsonCustomSerializationAttribute(LimitExchangeType limitExchangeType = LimitExchangeType.OutgoingCall) : base(limitExchangeType)
        {
            LimitExchangeType = limitExchangeType;
        }

        public override object Deserialize(Type type, string data, object serverBase, object client)
        {
            try
            {
                if (ServerProvider == null)
                {
                    return JsonConvert.DeserializeObject(data, type, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                        Formatting = Formatting.None,
                        PreserveReferencesHandling = PreserveReferencesHandling.All
                    });
                }
                else
                {
                    return JsonConvert.DeserializeObject(data, type, new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ServerProvider.ProviderSetting.IsEnabledReferenceResolver ? ReferenceLoopHandling.Serialize : ReferenceLoopHandling.Error,
                        Formatting = Formatting.None,
                        PreserveReferencesHandling = ServerProvider.ProviderSetting.IsEnabledReferenceResolverForArray ? PreserveReferencesHandling.All : PreserveReferencesHandling.None,
                        Converters = new List<JsonConverter>()
                        {
                            ServerProvider.JsonSettingHelper.CurrentDateTimeSetting
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                var myex = new Exception($"Json Deserialize has Error on object {data}", ex);
                AutoLogger.Default.LogError(myex, "JsonCustomSerializationAttribute");
                throw myex;
            }
        }

        public static ServerProvider ServerProvider { get; set; }
        public override string Serialize(object data, object serverBase, object client)
        {
            if (ServerProvider == null)
            {
                return JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
                    Formatting = Formatting.None,
                    PreserveReferencesHandling = PreserveReferencesHandling.All
                });
            }
            else
            {
                return JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ServerProvider.ProviderSetting.IsEnabledReferenceResolver ? ReferenceLoopHandling.Serialize : ReferenceLoopHandling.Error,
                    Formatting = Formatting.None,
                    PreserveReferencesHandling = ServerProvider.ProviderSetting.IsEnabledReferenceResolverForArray ? PreserveReferencesHandling.All : PreserveReferencesHandling.None,
                    Converters = new List<JsonConverter>()
                    {
                        ServerProvider.JsonSettingHelper.CurrentDateTimeSetting
                    }
                });
            }
        }
    }

    public class FastJsonSerializationAttribute : JsonCustomSerializationAttribute
    {
        public override object Deserialize(Type type, string data, object serverBase, object client)
        {
            return JsonConvert.DeserializeObject(data, type, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None,
            });
        }

        public override string Serialize(object data, object serverBase, object client)
        {
            return JsonConvert.SerializeObject(data, Formatting.None, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.None,
            });
        }
    }
}
