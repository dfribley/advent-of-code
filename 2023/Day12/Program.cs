using System.Text.RegularExpressions;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 12\n\n");

foreach (var inputFile in new[] { "sample.txt" , "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var conditions = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            return (report: parts[0], groups: parts[1].Split(",").Select(n => n.ToInt32()).ToList());
        })
        .ToList();

    var test = conditions
        .Select(t =>
        {
            var newReport = MultiplyPattern(t.report, 5);
            var newGroups = MultiplySequences(t.groups, 5);
            
            return FindValidArrangements(newReport, newGroups);
        })
        .Sum();
    
    Console.WriteLine($"Answer: {test}");
    
    continue;

    bool isValid(string condition, IList<int> groups)
    {
        var regex = new Regex(@"(#+)");
        var match = regex.Match(condition);

        int i = 0;

        while (match.Success)
        {
            if (i >= groups.Count || match.Value.Length != groups[i++])
            {
                return false;
            }

            match = match.NextMatch();
        }

        return i == groups.Count;
    }

    var ans = conditions
        .Select(t =>
        {
            var validChars = new List<char>() { '.', '#' };
            var variations = new List<string>();

            void GeneratePermutations(string mask, string combination)
            {
                if (mask.Length <= 0)
                {
                    variations.Add(combination);
                    return;
                }

                if (mask[0] != '?')
                {
                    GeneratePermutations(mask[1..], combination + mask[0]);
                }
                else
                {
                    validChars.ForEach(c => GeneratePermutations(mask[1..], combination + c));
                }
            }

            GeneratePermutations(t.report, String.Empty);

            return variations.Where(s => isValid(s, t.groups)).Count();


            //var newmask = "?" + t.report + "?";
            //if (t.report.EndsWith("#"))
            //{ 
            //    int k = 0;

            //    for (int i = t.report.Length - 1; i >= 0; i--)
            //    {
            //        if (t.report[i] != '#')
            //        {
            //            break;
            //        }

            //        k++;
            //    }

            //    if (k >= t.groups.Last())
            //    {
            //        newmask = t.report + "?";
            //    }
            //}


            //GenerateCombos(newmask, String.Empty);
            ////GenerateCombos(t.report, String.Empty);

            //var test = (double) combos.Where(s => isValid(s, t.groups)).Count();
            //var td = Math.Pow(test, 4);

            //combos.Clear();

            //GenerateCombos(t.report, String.Empty);


            //var x = combos.Where(s => isValid(s, t.groups)).Count();


            //var ans = td * x;

            //return ans;
            //return new BigInteger(td * x);

            //return combos.Where(s => isValid(s, t.groups)).Count();
        })

    .Sum();

    Console.WriteLine($"Part 1: {ans}");

    // TODO: NEEDS WORK
    Console.WriteLine($"Part 2:\n");
}

static string MultiplyPattern(string pattern, int times)
{
    return string.Join("?", Enumerable.Repeat(pattern, times));
}

static List<int> MultiplySequences(List<int> sequences, int times)
{
    return Enumerable.Repeat(sequences, times).SelectMany(x => x).ToList();
}

static long FindValidArrangements(string pattern, List<int> expectedLengths)
{
    int n = pattern.Length;
    int m = expectedLengths.Count;
    var dp = new long[n + 1, m + 1];
    dp[0, 0] = 1;

    for (int i = 1; i <= n; i++)
    {
        for (int j = 0; j <= m; j++)
        {
            if (pattern[i - 1] == '?')
            {
                dp[i, j] += dp[i - 1, j];
                if (j > 0 && i >= expectedLengths[j - 1] && pattern.Substring(i - expectedLengths[j - 1], expectedLengths[j - 1]).All(c => c == '#' || c == '?'))
                {
                    dp[i, j] += dp[i - expectedLengths[j - 1], j - 1];
                }
            }
            else
            {
                dp[i, j] += dp[i - 1, j];
                if (j > 0 && i >= expectedLengths[j - 1] && pattern.Substring(i - expectedLengths[j - 1], expectedLengths[j - 1]).All(c => c == '#'))
                {
                    dp[i, j] += dp[i - expectedLengths[j - 1], j - 1];
                }
            }
        }
    }

    return dp[n, m];
}