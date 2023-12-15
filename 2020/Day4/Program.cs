using System.Text.RegularExpressions;
using AoC.Shared.Enumerable;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 4\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var credentials = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .Select(grp => grp
            .Values
            .SelectMany(line => line
                .Split(' ')
                .Select(pair => {
                    var parts = pair.Split(':');
                    return new KeyValuePair<string, string>(parts[0], parts[1]);
                })
            )
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
        );

    var validations = new Dictionary<string, Func<string, bool>>
    {
        { "byr", v => v.IsIntBetween(1920, 2002) },
        { "iyr", v => v.IsIntBetween(2010, 2020) },
        { "eyr", v => v.IsIntBetween(2020, 2030) },
        { "hgt", v => {
            var matches = new Regex(@"(\d+)(cm|in)").Match(v);

            return matches.Success && matches.Groups[2].Value switch
            {
                "cm" => matches.Groups[1].Value.IsIntBetween(150, 193),
                _ => matches.Groups[1].Value.IsIntBetween(59, 76)
            };
        }},
        { "hcl", new Regex(@"^#([0-9]|[a-f]){6}$").IsMatch },
        { "ecl", new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains },
        { "pid", new Regex(@"^\d{9}$").IsMatch }
    };

    var part1 = credentials
        .Where(cred => validations.Keys.All(cred.ContainsKey))
        .Count();

    Console.WriteLine($"Part 1: {part1}");

    var part2 = credentials
        .Where(cred => validations.All(v => cred.ContainsKey(v.Key) && v.Value(cred[v.Key])))
        .Count();

    Console.WriteLine($"Part 2: {part2}\n");
}
