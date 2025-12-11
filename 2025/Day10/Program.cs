// Advent of Code challenge: https://adventofcode.com/2025/day/10

using System.Text.RegularExpressions;
using AoC.Shared.BinaryString;

Console.WriteLine("AoC - Day 10\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var machines = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var lightMatch = Regex.Match(line, @"\[(.+)]");
            var buttonMatch = Regex.Match(line, @"\(([\d,]+)\)");
            var joltageMatch = Regex.Match(line, "{(.+)}");
            
            var lights = lightMatch.Groups[1].Value.Select(c => c == '#' ? "1" : "0").ToBinaryString().ToInt32();
            var joltages = joltageMatch.Groups[1].Value.Split(',').Select(int.Parse).ToArray();
            var buttons = new List<int>();

            while (buttonMatch.Success)
            {
                buttons.Add(
                    buttonMatch.Groups[1].Value
                        .Split(',')
                        .Select(num => lightMatch.Groups[1].Value.Length - 1 - int.Parse(num))
                        .ToBinaryString()
                        .ToInt32()
                    );

                buttonMatch = buttonMatch.NextMatch();
            }
            
            return (lights, buttons, joltages);
        })
        .ToList();
    
    var part1 = machines
        .Select(m => InitializeMachineForLights(m.lights, m.buttons))
        .Sum();
    
    Console.WriteLine($"Part 1: {part1}");
    Console.WriteLine($"Part 2:\n");
}

return;

static long InitializeMachineForLights(int desiredLights, List<int> buttons)
{
    var queue = new Queue<(int turn, int lights, int button)>();
    var knownStates = new HashSet<int>();

    foreach (var button in buttons)
    {
        queue.Enqueue((0, 0, button));
    }

    while (queue.TryDequeue(out var buttonPress))
    {
        var (turn, lights, button) = buttonPress;

        if (desiredLights == lights)
        {
            return turn;
        }
        
        var stateKey = lights * 31 + button;
        if (!knownStates.Add(stateKey))
        {
            continue;
        }

        foreach (var b in buttons)
        {
            queue.Enqueue((turn + 1, lights ^ button, b));
        }
    }

    throw new Exception("No solution found!");
}
