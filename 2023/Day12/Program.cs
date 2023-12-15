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
