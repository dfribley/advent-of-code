namespace AoC.Shared.Enumerable;

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
}
