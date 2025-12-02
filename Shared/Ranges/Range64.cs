using System.Collections;
using AoC.Shared.Strings;

namespace AoC.Shared.Ranges;

public class Range64 : IEnumerable<long>
{
    private readonly long _start;
    private readonly long _end;

    public Range64(long start, long length)
	{
        _start = start;
        _end = start + length - 1;
    }

    public long Start => _start;

    public long End => _end;

    public long Length => _end - _start + 1;

    public bool Contains(long value)
    {
        return _start <= value && value <= _end;
    }

    public bool Contains(Range64 range)
    {
        return _start <= range.Start && range.Start <= _end;
    }

    public bool Overlaps(Range64 range)
    {
        return
            (_start <= range.Start && range.Start <= _end) ||
            (range.Start <= _start && _start <= range.End);
    }
    
    public IEnumerator<long> GetEnumerator()
    {
        for (var i = _start; i <= _end; i++)
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
