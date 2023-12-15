namespace AoC.Shared.Collections;

public static class ListExtensions
{
	public static int IndexOf<T>(this IList<T> list, Func<T, bool> condition)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (condition(list[i]))
			{
				return i;
			}
		}

		return -1;
	}

	public static bool RemoveFirst<T>(this IList<T> list, Func<T, bool> condition)
	{
        for (int i = 0; i < list.Count; i++)
        {
            if (condition(list[i]))
            {
				list.RemoveAt(i);
				return true;
            }
        }

        return false;
    }

	public static void ReplaceAt<T>(this IList<T> list, int i, T item)
	{
		list.RemoveAt(i);
		list.Insert(i, item);
	}
}
