namespace AoC.Shared.MathUtils;

public static class MathExtensions
{
    public static long LCM(this IEnumerable<int> values)
    {
        return values.Select(Convert.ToInt64).LCM();
    }

    public static long LCM(this IEnumerable<long> values)
    {
        return values.Aggregate((S, val) => S * val / GCD(S, val));
    }

    static long GCD(long n1, long n2)
    {
        return n2 == 0 ? n1 : GCD(n2, n1 % n2);
    }
}
