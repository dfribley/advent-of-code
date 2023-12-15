using AoC.Day19;
using AoC.Shared.Strings;
using System.Diagnostics;
using System.Text.RegularExpressions;

Console.WriteLine("AOC - Puzzle 19\n");

var inputRegEx = new Regex(@"Blueprint (\d+): Each ore robot costs (\d+) ore. Each clay robot costs (\d+) ore. Each obsidian robot costs (\d+) ore and (\d+) clay. Each geode robot costs (\d+) ore and (\d+) obsidian.");

static IDictionary<BluePrint, GameState> processBlueprints(IEnumerable<BluePrint> blueprints, int minutes)
{
    var results = new Dictionary<BluePrint, GameState>();

    foreach (var blueprint in blueprints)
    {
        var knownStates = new HashSet<GameState>();
        var states = new Stack<GameState>();
        states.Push(new GameState { OreRobots = 1, Minute = minutes });

        var maxOreNeed = Math.Max(blueprint.GeodeRobotOreCost, Math.Max(blueprint.ObsidianRobotOreCost, Math.Max(blueprint.OreRobotCost, blueprint.ClayRobotCost)));

        while (states.Any())
        {
            var state = states.Pop();

            if (knownStates.Contains(state))
            {
                continue;
            }

            if (state.Minute == 0)
            {
                if (!results.ContainsKey(blueprint) || state.Geodes > results[blueprint].Geodes)
                {
                    results[blueprint] = state;
                }

                continue;
            }

            knownStates.Add(state);

            var newState = new GameState(state);
            newState.Minute--;

            if (newState.Ore > (maxOreNeed * newState.Minute) - (newState.OreRobots * (newState.Minute - 1)))
            {
                newState.Ore = (maxOreNeed * newState.Minute) - (newState.OreRobots * (newState.Minute - 1));
            }

            if (newState.Clay > (blueprint.ObsidianRobotClayCost * newState.Minute) - (newState.ClayRobots * (newState.Minute - 1)))
            {
                newState.Clay = (blueprint.ObsidianRobotClayCost * newState.Minute) - (newState.ClayRobots * (newState.Minute - 1));
            }

            if (newState.Obsidian > (blueprint.GeodeRobotObsidianCost * newState.Minute) - (newState.ObsidianRobots * (newState.Minute - 1)))
            {
                newState.Obsidian = (blueprint.GeodeRobotObsidianCost * newState.Minute) - (newState.ObsidianRobots * (newState.Minute - 1));
            }

            newState.Ore += newState.OreRobots;
            newState.Clay += newState.ClayRobots;
            newState.Obsidian += newState.ObsidianRobots;
            newState.Geodes += newState.GeodeRobots;

            if (state.Ore >= blueprint.GeodeRobotOreCost && state.Obsidian >= blueprint.GeodeRobotObsidianCost)
            {
                var sState = new GameState(newState);

                sState.GeodeRobots++;
                sState.Ore -= blueprint.GeodeRobotOreCost;
                sState.Obsidian -= blueprint.GeodeRobotObsidianCost;

                states.Push(sState);
            }

            if (state.Ore >= blueprint.ObsidianRobotOreCost && state.Clay >= blueprint.ObsidianRobotClayCost && state.ObsidianRobots < blueprint.GeodeRobotObsidianCost)
            {
                var sState = new GameState(newState);

                sState.ObsidianRobots++;
                sState.Ore -= blueprint.ObsidianRobotOreCost;
                sState.Clay -= blueprint.ObsidianRobotClayCost;

                states.Push(sState);
            }

            if (state.Ore >= blueprint.ClayRobotCost && state.ClayRobots < blueprint.ObsidianRobotClayCost)
            {
                var sState = new GameState(newState);

                sState.ClayRobots++;
                sState.Ore -= blueprint.ClayRobotCost;

                states.Push(sState);
            }

            if (state.Ore >= blueprint.OreRobotCost && state.OreRobots < maxOreNeed)
            {
                var sState = new GameState(newState);

                sState.OreRobots++;
                sState.Ore -= blueprint.OreRobotCost;

                states.Push(sState);
            }

            states.Push(newState);
        }
    }

    return results;
};

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var blueprints = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var match = inputRegEx.Match(line);

            return new BluePrint
            {
                Id = match.Groups[1].Value.ToInt32(),
                OreRobotCost = match.Groups[2].Value.ToInt32(),
                ClayRobotCost = match.Groups[3].Value.ToInt32(),
                ObsidianRobotOreCost = match.Groups[4].Value.ToInt32(),
                ObsidianRobotClayCost = match.Groups[5].Value.ToInt32(),
                GeodeRobotOreCost = match.Groups[6].Value.ToInt32(),
                GeodeRobotObsidianCost = match.Groups[7].Value.ToInt32()
            };
        })
        .ToList();

    var sw = new Stopwatch();
    sw.Start();
    var part1 = processBlueprints(blueprints, 24)
        .Select(result => result.Key.Id * result.Value.Geodes)
        .Sum();
    sw.Stop();

    Console.WriteLine($"Part 1: ({string.Format("{0:00}:{1:00}.{2:00}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10)}) {part1}");

    sw.Restart();
    var part2 = 1;
    processBlueprints(blueprints.Take(3), 32)
        .ToList()
        .ForEach(result => { part2 *= result.Value.Geodes; });
    sw.Stop();
    Console.WriteLine($"Part 2: ({string.Format("{0:00}:{1:00}.{2:00}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10)}) {part2}\n");
}

