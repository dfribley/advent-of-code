using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 2\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var games = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(game =>
        {
            var parts = game.Split(":");
            var gameId = parts[0].Split(" ")[1].ToInt32();
            var rounds = parts[1].Split(";");

            return (id: gameId, rounds: rounds.Select(r =>
            {
                var red = 0;
                var green = 0;
                var blue = 0;

                foreach (var dice in r.Split(","))
                {
                    var parts = dice.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                    switch (parts[1])
                    {
                        case "red":
                            red += parts[0].ToInt32();
                            break;
                        case "green":
                            green += parts[0].ToInt32();
                            break;
                        case "blue":
                            blue += parts[0].ToInt32();
                            break;
                    }
                }

                return (red, green, blue);
            }).ToArray());
        })
        .ToList();

    var maxCubes = (red: 12, green: 13, blue: 14);
    var part1 = games
        .Where(g => g.rounds.All(r =>
            r.red <= maxCubes.red &&
            r.green <= maxCubes.green &&
            r.blue <= maxCubes.blue))
        .Select(g => g.id)
        .Sum();

    Console.WriteLine($"Part 1: { part1 }");

    var part2 = games
        .Select(g =>
        {
            var red = 0;
            var green = 0;
            var blue = 0;

            foreach (var round in g.rounds)
            {
                red = Math.Max(red, round.red);
                green = Math.Max(green, round.green);
                blue = Math.Max(blue, round.blue);
            }

            return red * green * blue;
        })
        .Sum();

    Console.WriteLine($"Part 2: { part2 }\n");
}
