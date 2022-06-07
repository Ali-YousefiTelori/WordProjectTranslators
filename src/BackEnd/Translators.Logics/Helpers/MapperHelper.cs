using Newtonsoft.Json;

namespace System
{
    public static class MapperHelper
    {
        static MapperHelper()
        {
            InitializeMappers();
        }

        public static Dictionary<Type, Func<object, string, string, object[], object>> Mappers { get; set; } = new Dictionary<Type, Func<object, string, string, object[], object>>();

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
                            Mappers.Add(method.ReturnType, (toMap, uniqueRecordId, language, parameters) =>
                            {
                                return method.Invoke(null, new object[] { toMap, uniqueRecordId, language, parameters });
                            });
                        }
                    }
                }
            }
        }

        public static T Map<T>(this object data)
        {
            if (Mappers.TryGetValue(typeof(T), out Func<object, string, string, object[], object> func))
            {
                return (T)func(data, null, null, null);
            }
            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(data));
        }
    }
}
