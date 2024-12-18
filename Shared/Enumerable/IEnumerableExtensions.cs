﻿namespace AoC.Shared.Enumerable;

public static class IEnumerableExtensions
{
    public static IEnumerable<T> StartAt<T>(this IEnumerable<T> values, int index)
    {
        if (index == 0)
        {
            return values;
        }

        var newValues = values.Skip(index).ToList();
        newValues.AddRange(values.Take(index));

        return newValues;
    }

    public static IEnumerable<SplitGroup<T>> Split<T>(this IEnumerable<T> values, Func<T, bool> delimiterTest)
    {
        var id = 0;
        var group = new List<T>();

        foreach (var item in values)
        {
            if (delimiterTest(item))
            {
                yield return new SplitGroup<T>
                {
                    Id = id++,
                    Values = new List<T>(group)
                };

                group.Clear();
            }
            else
            {
                group.Add(item);
            }
        }

        if (group.Any())
        {
            yield return new SplitGroup<T>
            {
                Id = id,
                Values = group
            };
        }
    }

    public static IEnumerable<SplitGroup<T>> Split<T>(this IEnumerable<T> values, int count)
    {
        var i = 0;
        var id = 0;
        var group = new List<T>();

        foreach (var item in values)
        {
            group.Add(item);

            if (++i % count == 0)
            {
                yield return new SplitGroup<T>
                {
                    Id = id++,
                    Values = new List<T>(group)
                };

                group.Clear();
            }
        }

        if (group.Any())
        {
            yield return new SplitGroup<T>
            {
                Id = id,
                Values = group
            };
        }
    }

    public static IEnumerable<T> Intersect<T>(this IEnumerable<IEnumerable<T>> collection)
    {
        var result = collection.First();

        foreach(var values in collection.Skip(1))
        {
            result = result.Intersect(values);
        }

        return result;
    }

    public static long Product(this IEnumerable<long> values)
    {
        var product = 1L;

        foreach(var value in values)
        {
            product *= value;
        }

        return product;
    }

    public static int Product(this IEnumerable<int> values)
    {
        var product = 1;

        foreach (var value in values)
        {
            product *= value;
        }

        return product;
    }

    public static Wrapable<Type> AsWrapable<Type>(this IEnumerable<Type> collection)
    {
        return new Wrapable<Type>(collection);
    }
    
    public static IReadOnlyDictionary<T, int> ToCountsDictionary<T>(this IEnumerable<T> collection) 
        where T : notnull
    {
        return collection.GroupBy(val => val).ToDictionary(grp => grp.Key, grp => grp.Count());
    }
    
    public static IEnumerable<(T a,T b)> ToPairs<T>(this IEnumerable<T> collection)
    {
        var collectionArray = collection.ToArray();
        
        for (var i = 0; i < collectionArray.Length - 1; i++)
        {
            for (var j = i + 1; j < collectionArray.Length; j++)
            {
                yield return (collectionArray[i], collectionArray[j]);
            }
        }
    }
}
