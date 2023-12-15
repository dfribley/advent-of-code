using AoC.Shared.Strings;
using System.Diagnostics;
using System.Text.RegularExpressions;

Console.WriteLine("AOC - Puzzle 16\n");

var inputRegEx = new Regex(@"Valve (..) has flow rate=(\d+); tunnels? leads? to valves? (.+)");

static long getMaxPressureRelease(IDictionary<int, (string id, int flowRate, IEnumerable<string> tunnels)> valves, int minutes, int totalPlayers)
{
    var knownStates = new Dictionary<int, long>();
    var valveLookup = valves.ToDictionary(kvp => kvp.Value.id, kvp => kvp.Key);
    var startPosition = valveLookup["AA"];

    long maxPressureForState(int position, int openValves, int minute, int players)
    {
        if (minute == 0)
        {
            return players > 1 ? maxPressureForState(startPosition, openValves, minutes, players - 1) : 0;
        }

        var stateKey = openValves * valves.Count * 31 * 2 + position * 31 * 2 + minute * 2 + players;

        if (knownStates.ContainsKey(stateKey))
        {
            return knownStates[stateKey];
        }

        var maxPressure = 0L;

        if (valves[position].flowRate > 0 && (openValves & (1 << position)) != (1 << position))
        {
            maxPressure = valves[position].flowRate * (minute - 1) + maxPressureForState(position, (openValves | (1 << position)), minute - 1, players);
        }

        foreach (var newPosition in valves[position].tunnels)
        {
            maxPressure = Math.Max(maxPressure, maxPressureForState(valveLookup[newPosition], openValves, minute - 1, players));
        }

        knownStates[stateKey] = maxPressure;
        return maxPressure;
    }

    return maxPressureForState(startPosition, 0, minutes, totalPlayers);
};

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    var valves = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var match = inputRegEx.Match(line);

            return (
                id: match.Groups[1].Value,
                flowRate: match.Groups[2].Value.ToInt32(),
                tunnels: match.Groups[3].Value.Split(',').Select(k => k.Trim()));
        })
        .OrderByDescending(t => t.flowRate)
        .Select((valve, i) => (valve, i))
        .ToDictionary(t => t.i, t => t.valve);

    Console.WriteLine($"{inputFile}\n");

    var sw = new Stopwatch();
    sw.Start();
    var part1 = getMaxPressureRelease(valves, 30, 1);
    sw.Stop();

    Console.WriteLine($"Part 1: ({string.Format("{0:00}:{1:00}.{2:00}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10)}) {part1}");

    sw.Restart();
    var part2 = getMaxPressureRelease(valves, 26, 2);
    sw.Stop();

    Console.WriteLine($"Part 2: ({string.Format("{0:00}:{1:00}.{2:00}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds / 10)}) {part2}\n");
}
