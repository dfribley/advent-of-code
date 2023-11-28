namespace AoC.Shared.Strings;

public static class StringExtensions
{
    public static int ToInt32(this string str)
    {
        return Convert.ToInt32(str);
    }

    public static int ToInt32(this char c)
    {
        return c.ToString().ToInt32();
    }

    public static long ToInt64(this string str)
    {
        return Convert.ToInt64(str);
    }

    public static IEnumerable<int> AllIndexesOf(this string str, string value)
    {
        var i = str.IndexOf(value);
        while (i != -1)
        {
            yield return i;
            i = str.IndexOf(value, i + value.Length);
        }
    }

    public static bool IsIntBetween(this string value, int low, int high)
    {
        if (!int.TryParse(value, out var number))
        {
            return false;
        }

        return low <= number && number <= high;
    }
}
