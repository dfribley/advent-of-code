Console.WriteLine("AOC - Day 3\n\n");

static char getMostCommonBit(IList<string> list, int index)
{
    int onCount = 0;

    foreach (var number in list)
    {
        if (number[index] == '1')
        {
            onCount++;
        }
    }

    return onCount / (float)list.Count >= .5 ? '1' : '0';
}

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var report = File.ReadAllLines(inputFile).ToList();

    var gamma = string.Concat(Enumerable.Range(0, report[0].Length).Select(i => getMostCommonBit(report, i)));
    var xor = new string('1', report[0].Length);

    var part1 = Convert.ToInt32(gamma, 2) * (Convert.ToInt32(gamma, 2) ^ Convert.ToInt32(xor, 2));
    Console.WriteLine($"Part 1: {part1}");

    var i = 0;
    var reportCopy = report;
    while (reportCopy.Count > 1)
    {
        reportCopy = reportCopy
            .Where(number => number[i] == getMostCommonBit(reportCopy, i))
            .ToList();

        i++;
    }

    i = 0;
    while (report.Count > 1)
    {
        report = report
            .Where(number => number[i] != getMostCommonBit(report, i))
            .ToList();

        i++;
    }

    var part2 = Convert.ToInt32(reportCopy.Single(), 2) * Convert.ToInt32(report.Single(), 2);
    Console.WriteLine($"Part 2: {part2}\n");
}
