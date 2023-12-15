using System.Text;

Console.WriteLine("AOC - Day 25\n");

static long toBase10(string snafu)
{
    var base10 = 0L;
    var reverse = new string(snafu.Reverse().ToArray());

    for (var i = 0; i < reverse.Length; i++)
    {
        var val = (long)Math.Pow(5, i);
        switch (reverse[i])
        {
            case '2':
                base10 += (val * 2);
                break;
            case '1':
                base10 += val;
                break;
            case '-':
                base10 -= val;
                break;
            case '=':
                base10 -= (val * 2);
                break;
        }
    }

    return base10;
};

static string toSnafu(long base10)
{
    var snafu = new StringBuilder();
    var length = 1;
    var currentValue = 0L;

    while (base10 > Math.Pow(5, length) / 2)
    {
        length++;
    }

    for (var i = length - 1; i >= 0; i--)
    {
        var positionValue = Math.Pow(5, i);
        var nextMaxValue = positionValue / 2;

        if (nextMaxValue >= Math.Abs(base10 - currentValue))
        {
            snafu.Append('0');
        }
        else if (currentValue <= base10)
        {
            if (currentValue + positionValue + nextMaxValue >= base10)
            {
                snafu.Append('1');
                currentValue += (long)positionValue;
            }
            else
            {
                snafu.Append('2');
                currentValue += (long)positionValue * 2;
            }
        }
        else
        {
            if (currentValue - positionValue - nextMaxValue <= base10)
            {
                snafu.Append('-');
                currentValue -= (long)positionValue;
            }
            else
            {
                snafu.Append('=');
                currentValue -= (long)positionValue * 2;
            }
        }
    }

    return snafu.ToString();   
};

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");
    
    var part1 = toSnafu(File.ReadLines(inputFile).Select(s => toBase10(s)).Sum());
    Console.WriteLine($"Part 1: {part1}\n");
}