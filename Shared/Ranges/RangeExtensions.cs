namespace AoC.Shared.Ranges;

public static class RangeExtensions
{
    public static bool FullyContains(this Range range1, Range range2)
    {
        return range2.Start.Value >= range1.Start.Value && range2.End.Value <= range1.End.Value;
    }

    public static bool Overlaps(this Range range1, Range range2)
    {
        return (range1.Start.Value >= range2.Start.Value && range1.Start.Value <= range2.End.Value) ||
            (range1.End.Value <= range2.End.Value && range1.End.Value >= range2.Start.Value);
    }

    public static int Count(this Range range)
    {
        return range.End.Value - range.Start.Value + 1;
    }

    public static (Range low, Range high) Split(this Range range, int lowEnd)
    {
        return (new Range(range.Start, lowEnd), new Range(lowEnd + 1, range.End));
    }
}
