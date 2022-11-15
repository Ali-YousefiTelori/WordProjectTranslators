using Newtonsoft.Json;
using Translators.Contracts.Common;

namespace System
{
    public static class MapperHelper
    {
        static MapperHelper()
        {
            InitializeMappers();
        }

        public static Dictionary<string, Func<object, string, string, object[], object>> Mappers { get; set; } = new Dictionary<string, Func<object, string, string, object[], object>>();

        public static void InitializeMappers()
        {
            foreach (var type in typeof(MapperHelper).Assembly.GetTypes())
            {
                if (type.Namespace == "CompileTimeGoMapper")
                {
                    foreach (var method in type.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
                    {
                        if (method.Name == "Map")
                        {
                            string key = GetMapperKey(method.GetParameters()[0].ParameterType, method.ReturnType);
                            Console.WriteLine($"Added mapper {key}");
                            Mappers.Add(key, (toMap, uniqueRecordId, language, parameters) =>
                            {
                                return method.Invoke(null, new object[] { toMap, uniqueRecordId, language, parameters });
                            });
                        }
                    }
                }
            }
        }

        static string GetMapperKey(Type fromType, Type toType)
        {
            return $"{fromType}_{toType}";
        }

        public static T Map<T>(this object data)
        {
            if (data == null)
                return default;
            else if (data.GetType() == typeof(T))
                return (T)data;
            else if (Mappers.TryGetValue(GetMapperKey(data.GetType(), typeof(T)), out Func<object, string, string, object[], object> func))
            {
                return (T)func(data, null, null, null);
            }
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(data));
        }

        public static List<T> MapToList<T>(this IEnumerable<object> data)
        {
            if (data == null)
                return default;
            var result = new List<T>();
            foreach (var item in data)
            {
                result.Add(item.Map<T>());
            }
            return result;
        }

        public static T MapResult<T>(this MessageContract messageContract)
        {
            return Map<T>(messageContract.GetResult());
        }

        public static List<T> MapResultToList<T>(this MessageContract messageContract)
        {
            return MapToList<T>((IEnumerable<object>)messageContract.GetResult());
        }
    }
}
