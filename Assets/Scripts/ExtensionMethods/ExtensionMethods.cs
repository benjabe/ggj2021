using System.Collections.Generic;
using System.Linq;

public static class ExtensionMethods
{
    public static T Random<T>(this IEnumerable<T> enumerable)
    {
        return enumerable.ElementAt(UnityEngine.Random.Range(0, enumerable.Count()));
    }
    public static TValue RandomValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
    {
        return dictionary.Values.Random();
    }
    public static TKey RandomKey<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
    {
        return dictionary.Keys.Random();
    }
}