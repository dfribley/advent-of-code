namespace AoC.Shared.Ranges;

public class Range64
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
}
