using System.Collections.Generic;

namespace DM.Web.Core.Extensions.DictionaryExtensions
{
    public static class DictionaryExtensions
    {
        public static TValue SafeGet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.ContainsKey(key)
                       ? dictionary[key]
                       : default;
        }

        public static void SafeAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
        }
    }
}