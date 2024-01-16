using System;

namespace EasyMicroservices.TranslatorsMicroservice.Clients.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public class ContentLanguageAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public ContentLanguageAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }

        /// <summary>
        /// 
        /// </summary>
        public ContentLanguageAttribute()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public string PropertyName { get; set; }
    }
}
