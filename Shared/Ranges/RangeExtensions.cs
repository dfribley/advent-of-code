namespace AoC.Shared.Ranges
{
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
    }
}
