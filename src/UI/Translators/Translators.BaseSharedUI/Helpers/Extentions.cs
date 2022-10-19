using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Translators
{
    public static class Extentions
    {
        public static bool TryGetValue(this IDictionary dictionary, string key, out object color)
        {
#if (NETSTANDARD)
            return dictionary.TryGetValue(key, out color);
#else
            if (dictionary.Contains(key))
            {
                color = dictionary[key];
                return true;
            }
            else
            {
                color = null;
                return false;
            }
#endif
        }
    }
}
