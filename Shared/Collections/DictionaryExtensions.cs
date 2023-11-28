namespace AoC.Shared.Collections;

public static class DictionaryExtensions
{
    public static IDictionary<TKey, TValue> Clone<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        where TValue : ICloneable where TKey : notnull
    {
        return dictionary.ToDictionary(kvp => kvp.Key, kvp => (TValue)kvp.Value.Clone());
    }
}
