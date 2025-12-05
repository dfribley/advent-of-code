using System.Collections;
using AoC.Shared.Strings;

namespace AoC.Shared.Ranges;

public class Range64(long start, long length) : IEnumerable<long>
{
    private long _start = start;
    private long _end = start + length - 1;
    
    public long Start
    {
        get => _start;
        set => _start = value;
    }

    public long End
    {
        get => _end;
        set => _end = value;
    }
    
    public long Size => End - Start + 1;

    public bool Contains(long value)
    {
        return Start <= value && value <= End;
    }

    public bool FullyContains(Range64 range)
    {
        return Start <= range.Start && End >= range.End;
    }

    public bool Intersects(Range64 range)
    {
        return (Start < range.Start && End >= range.Start) || (End > range.End && Start <= range.End);
    }

    public void ShrinkToExclude(Range64 range)
    {
        if (!Intersects(range))
        {
            return;
        }

        if (range.Start < Start) Start = range.End + 1;
        if (range.End > End) End = range.Start - 1;
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

    public override bool Equals(object? obj)
    {
        if (obj is Range64 range)
        {
            return Start == range.Start && End == range.End;
        }
    
        return false;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(_start, _end);
    }
    
    public override string ToString() => $"{Start}-{End}";
}
