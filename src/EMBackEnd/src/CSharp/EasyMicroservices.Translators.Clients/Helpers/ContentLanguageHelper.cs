using Translators.GeneratedServices;
using EasyMicroservices.TranslatorsMicroservice.Clients.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MessageContract = Translators.GeneratedServices.MessageContract;

namespace EasyMicroservices.TranslatorsMicroservice.Clients.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentLanguageHelper
    {
        private readonly ContentClient _contentClient;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contentClient"></param>
        public ContentLanguageHelper(ContentClient contentClient)
        {
            _contentClient = contentClient;
        }

        string GetUniqueIdentity(object contract)
        {
            var type = contract.GetType();
            var uniqueIdentity = type.GetProperty("UniqueIdentity", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (uniqueIdentity == null)
                throw new Exception($"Property UniqueIdentity not found in type {type.FullName} to ResolveContentLanguage!");
            return uniqueIdentity.GetValue(contract) as string;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public async Task ResolveContentAllLanguage(object contract)
        {
            if (contract == null)
                return;
            string uniqueIdentity = default;

            var uidProperty = contract.GetType().GetProperty("UniqueIdentity", BindingFlags.Instance | BindingFlags.Public);
            if (uidProperty != null)
                uniqueIdentity = uidProperty.GetValue(contract) as string;
            foreach (var property in contract.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                if (property.GetCustomAttribute<ContentLanguageAttribute>() != null)
                {
                    var instance = Activator.CreateInstance(property.PropertyType);
                    if (instance is IList list)
                    {
                        var genericType = property.PropertyType.GetGenericArguments()[0];
                        var Translators = await _contentClient.GetAllByKeyAsync(new GetAllByKeyRequestContract
                        {
                            Key = $"{GetPropertyName(property)}",
                            UniqueIdentity = uniqueIdentity
                        });
                        if (Translators.IsSuccess)
                        {
                            foreach (var item in Translators.Result)
                            {
                                var itemInstance = Activator.CreateInstance(genericType);
                                var languageProperty = itemInstance.GetType().GetProperty(nameof(LanguageDataContract.Language), BindingFlags.Public | BindingFlags.Instance);
                                if (languageProperty == null)
                                    throw new Exception($"Property {nameof(LanguageDataContract.Language)} not found in type {itemInstance.GetType()}");
                                var dataProperty = itemInstance.GetType().GetProperty(nameof(LanguageDataContract.Data), BindingFlags.Public | BindingFlags.Instance);
                                if (dataProperty == null)
                                    throw new Exception($"Property {nameof(LanguageDataContract.Data)} not found in type {itemInstance.GetType()}");
                                languageProperty.SetValue(itemInstance, item.Language.Name);
                                dataProperty.SetValue(itemInstance, item.Data);
                                list.Add(itemInstance);
                            }
                        }
                    }
                    property.SetValue(contract, instance);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public async Task ResolveContentLanguage(IEnumerable items, string language)
        {
            if (items == null)
                return;
            List<Task> tasks = new List<Task>();
            foreach (var item in items)
            {
                tasks.Add(ResolveContentLanguage(item, language, new HashSet<object>()));
            }
            await Task.WhenAll(tasks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contract"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        public Task ResolveContentLanguage(object contract, string language)
        {
            return ResolveContentLanguage(contract, language, new HashSet<object>());
        }

        async Task ResolveContentLanguage(object contract, string language, HashSet<object> mappedItems)
        {
            if (contract.Equals(default) || mappedItems.Contains(contract))
                return;
            var type = contract.GetType();
            mappedItems.Add(contract);
            if (IsClass(type) && typeof(IEnumerable).IsAssignableFrom(type))
            {
                foreach (var item in (IEnumerable)contract)
                {
                    await ResolveContentLanguage(item, language, mappedItems);
                }
            }
            else
            {
                foreach (var property in type.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
                {
                    if (property.GetCustomAttribute<ContentLanguageAttribute>() != null)
                    {
                        var contentResult = await _contentClient.GetByLanguageAsync(new GetByLanguageRequestContract()
                        {
                            Key = property.Name,
                            UniqueIdentity = GetUniqueIdentity(contract),
                            Language = language
                        });
                        if (contentResult.IsSuccess)
                            property.SetValue(contract, contentResult.Result.Data);
                    }
                    else if (IsClass(property.PropertyType) && typeof(IEnumerable).IsAssignableFrom(property.PropertyType))
                    {
                        var items = property.GetValue(contract);
                        if (items == null)
                            continue;
                        foreach (var item in (IEnumerable)items)
                        {
                            await ResolveContentLanguage(item, language, mappedItems);
                        }
                    }
                    else if (IsClass(property.PropertyType))
                    {
                        var value = property.GetValue(contract);
                        if (value != null)
                            await ResolveContentLanguage(value, language, mappedItems);
                    }
                }
            }
        }

        bool IsClass(Type type)
        {
            return type.GetTypeInfo().IsClass && typeof(string) != type && typeof(char[]) != type;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<CategoryContractMessageContract> AddToContentLanguage(object item)
        {
            return SaveToContentLanguage(item, AddToContent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public Task<List<Task<CategoryContractMessageContract>>> AddToContentLanguage(IEnumerable items)
        {
            List<Task<CategoryContractMessageContract>> tasks = new List<Task<CategoryContractMessageContract>>();
            foreach (var item in items)
            {
                tasks.Add(AddToContentLanguage(item));
            }
            return Task.FromResult(tasks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Task<MessageContract> UpdateToContentLanguage(object item)
        {
            return SaveToContentLanguageUpdate(item, UpdateToContent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public async Task UpdateToContentLanguage(IEnumerable items)
        {
            List<Task> tasks = new List<Task>();
            foreach (var item in items)
            {
                tasks.Add(UpdateToContentLanguage(item));
            }
            await Task.WhenAll(tasks);
        }

        async Task<CategoryContractMessageContract> SaveToContentLanguage(object item, Func<(string UniqueIdentity, string Name, IEnumerable<LanguageDataContract> Languages)[], Task<CategoryContractMessageContract>> saveData)
        {
            if (item.Equals(default))
                return new CategoryContractMessageContract()
                {
                    IsSuccess = true,
                };
            string uniqueIdentity = default;

            var uidProperty = item.GetType().GetProperty("UniqueIdentity", BindingFlags.Instance | BindingFlags.Public);
            if (uidProperty != null)
                uniqueIdentity = uidProperty.GetValue(item) as string;
            var request = new List<(string UniqueIdentity, string Name, IEnumerable<LanguageDataContract> Languages)>();
            foreach (var property in item.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                if (TryGetPropertyName(property, out string propertyName))
                {
                    if (property.GetValue(item) is IEnumerable items)
                    {
                        request.Add((uniqueIdentity, propertyName, Map(items)));
                    }
                }
            }
            var result = await saveData(request.ToArray());
            if (!result.IsSuccess)
                return result;
            return new CategoryContractMessageContract()
            {
                IsSuccess = true,
            };
        }


        async Task<MessageContract> SaveToContentLanguageUpdate(object item, Func<(string UniqueIdentity, string Name, IEnumerable<LanguageDataContract> Languages)[], Task<MessageContract>> saveData)
        {
            if (item.Equals(default))
                return new MessageContract()
                {
                    IsSuccess = true,
                };
            string uniqueIdentity = default;

            var uidProperty = item.GetType().GetProperty("UniqueIdentity", BindingFlags.Instance | BindingFlags.Public);
            if (uidProperty != null)
                uniqueIdentity = uidProperty.GetValue(item) as string;
            var request = new List<(string UniqueIdentity, string Name, IEnumerable<LanguageDataContract> Languages)>();
            foreach (var property in item.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                if (TryGetPropertyName(property, out string propertyName))
                {
                    if (property.GetValue(item) is IEnumerable items)
                    {
                        request.Add((uniqueIdentity, propertyName, Map(items)));
                    }
                }
            }
            var result = await saveData(request.ToArray());
            if (!result.IsSuccess)
                return result;
            return new MessageContract()
            {
                IsSuccess = true,
            };
        }

        bool TryGetPropertyName(PropertyInfo property, out string propertyName)
        {
            var contentLanguageAttribute = property.GetCustomAttribute<ContentLanguageAttribute>();
            propertyName = GetPropertyName(property);
            return contentLanguageAttribute != null;
        }

        string GetPropertyName(PropertyInfo property)
        {
            var contentLanguageAttribute = property.GetCustomAttribute<ContentLanguageAttribute>();
            if (contentLanguageAttribute != null)
                return contentLanguageAttribute.PropertyName ?? property.Name;
            else
                return property.Name;
        }

        IEnumerable<LanguageDataContract> Map(IEnumerable objects)
        {
            if (objects != null)
            {
                foreach (var item in objects)
                {
                    var languageProperty = item.GetType().GetProperty(nameof(LanguageDataContract.Language), BindingFlags.Public | BindingFlags.Instance);
                    if (languageProperty == null)
                        throw new Exception($"Property {nameof(LanguageDataContract.Language)} not found in type {item.GetType()}");
                    var dataProperty = item.GetType().GetProperty(nameof(LanguageDataContract.Data), BindingFlags.Public | BindingFlags.Instance);
                    if (dataProperty == null)
                        throw new Exception($"Property {nameof(LanguageDataContract.Data)} not found in type {item.GetType()}");
                    yield return new LanguageDataContract()
                    {
                        Language = languageProperty.GetValue(item) as string,
                        Data = dataProperty.GetValue(item) as string,
                    };
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniqueIdentity"></param>
        /// <param name="name"></param>
        /// <param name="languages"></param>
        /// <returns></returns>
        async Task<CategoryContractMessageContract> AddToContent(string uniqueIdentity, string name, IEnumerable<LanguageDataContract> languages)
        {
            var addNames = await _contentClient.AddContentWithKeyAsync(new Translators.GeneratedServices.AddContentWithKeyRequestContract
            {
                Key = $"{name}",
                UniqueIdentity = uniqueIdentity,
                LanguageData = languages.Select(o => new LanguageDataContract
                {
                    Data = o.Data,
                    Language = o.Language
                }).ToList(),
            });
            return addNames;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        async Task<CategoryContractMessageContract> AddToContent(params (string UniqueIdentity, string Name, IEnumerable<LanguageDataContract> Languages)[] items)
        {
            CategoryContractMessageContract result = default;
            foreach (var item in items)
            {
                result = await AddToContent(item.UniqueIdentity, item.Name, item.Languages);
                if (!result.IsSuccess)
                    return result;
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniqueIdentity"></param>
        /// <param name="name"></param>
        /// <param name="languages"></param>
        /// <returns></returns>
        async Task<MessageContract> UpdateToContent(string uniqueIdentity, string name, IEnumerable<LanguageDataContract> languages)
        {
            var addNames = await _contentClient.UpdateContentWithKeyAsync(new Translators.GeneratedServices.AddContentWithKeyRequestContract
            {
                Key = $"{name}",
                UniqueIdentity = uniqueIdentity,
                LanguageData = languages.Select(o => new LanguageDataContract
                {
                    Data = o.Data,
                    Language = o.Language
                }).ToList(),
            });

            return addNames;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        async Task<MessageContract> UpdateToContent(params (string UniqueIdentity, string Name, IEnumerable<LanguageDataContract> Languages)[] items)
        {
            MessageContract result = default;
            foreach (var item in items)
            {
                result = await UpdateToContent(item.UniqueIdentity, item.Name, item.Languages);
                if (!result.IsSuccess)
                    return result;
            }
            return result;
        }
    }
}
