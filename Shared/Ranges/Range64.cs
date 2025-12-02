using System.Collections;
using AoC.Shared.Strings;

namespace AoC.Shared.Ranges;

public class Range64(long start, long length) : IEnumerable<long>
{
    public long Start => start;

    public long End => start + length - 1;

    public long Length => length;

    public bool Contains(long value)
    {
        return Start <= value && value <= End;
    }

    public bool Contains(Range64 range)
    {
        return Start <= range.Start && range.Start <= End;
    }

    public bool Overlaps(Range64 range)
    {
        return
            (Start <= range.Start && range.Start <= End) ||
            (range.Start <= Start && Start <= range.End);
    }
    
    public IEnumerator<long> GetEnumerator()
    {
        for (var i = Start; i <= End; i++)
            yield return i;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static Range64 Parse(string input)
    {
        if (!input.Contains('-'))
        {
            throw new ArgumentException("Range string must match format 'x-y'");
        }
        
        var parts = input.Split('-');
        var start = parts[0].ToInt64();
        var end = parts[1].ToInt64();
        
        return new Range64(start, end - start + 1);
    }
}
