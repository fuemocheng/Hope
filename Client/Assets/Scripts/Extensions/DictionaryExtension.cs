using System.Collections;
using System.Collections.Generic;

public static class DictionaryExtension
{
    public static V Get<K, V>(this Dictionary<K, V> dic, K key) where V : class
    {
        if (dic.ContainsKey(key))
            return dic[key];

        return null;
    }

    public static void ForEach<K, V>(this Dictionary<K, V> dict, System.Action<object> action)
    {
        var enumerator = dict.GetEnumerator();
        while (enumerator.MoveNext())
            action?.Invoke(enumerator.Current);
    }

    public static void ForEachKey<K, V>(this Dictionary<K, V> dict, System.Action<object> action)
    {
        var enumerator = dict.GetEnumerator();
        while (enumerator.MoveNext())
            action?.Invoke(enumerator.Current.Key);
    }
    public static void ForEachValue<K, V>(this Dictionary<K, V> dict, System.Action<object> action)
    {
        var enumerator = dict.GetEnumerator();
        while (enumerator.MoveNext())
            action?.Invoke(enumerator.Current.Value);
    }
}
