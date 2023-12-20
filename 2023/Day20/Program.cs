using AoC.Shared.MathUtils;

Console.WriteLine("AOC - Day 20\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var modules = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var parts = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var destinations = parts.Skip(2).Select(d =>
            {
                if (d.EndsWith(","))
                {
                    return d[..^1];
                }

                return d;
            });

            return (module: parts[0][0] == 'b' ? parts[0] : parts[0][1..], type: parts[0][0], destinations);
        })
        .ToDictionary(m => m.module, m => m);

    void bootUp(Func<int, string, bool, bool> processPulse, Func<int, bool> postButtonPress)
    {
        var flipflops = modules
            .Where(m => m.Value.type == '%')
            .Select(m => (m.Key, state: false))
            .ToDictionary(m => m.Key, m => m.state);

        var conjunctions = modules
            .Where(m => m.Value.type == '&')
            .Select(m => (
                m.Key,
                inputs: modules
                    .Where(kvp => kvp.Value.destinations.Contains(m.Key))
                    .ToDictionary(m => m.Key, m => false)))
            .ToDictionary(m => m.Key, m => m.inputs);

        var queue = new Queue<(string, string, bool)>();
        var times = 0;

        while (true)
        {
            times++;
            queue.Enqueue(("broadcaster", "button", false));

            while (queue.TryDequeue(out var element))
            {
                var (toModule, fromModule, pulse) = element;

                if (processPulse(times, toModule, pulse))
                {
                    return;
                }

                if (toModule == "broadcaster")
                {
                    foreach (var next in modules[toModule].destinations)
                    {
                        queue.Enqueue((next, toModule, pulse));
                    }

                    continue;
                }

                if (!modules.ContainsKey(toModule))
                {
                    continue;
                }

                var (key, type, destinations) = modules[toModule];

                if (type == '%')
                {
                    if (!pulse)
                    {
                        flipflops[key] = !flipflops[key];

                        foreach (var next in destinations)
                        {
                            queue.Enqueue((next, toModule, flipflops[key]));
                        }
                    }
                }

                if (type == '&')
                {
                    conjunctions[toModule][fromModule] = pulse;

                    var allHigh = conjunctions[toModule].Values.All(pulse => pulse == true);

                    foreach (var next in destinations)
                    {
                        queue.Enqueue((next, toModule, !allHigh));
                    }
                }
            }

            if (postButtonPress(times))
            {
                return;
            }
        }
    }

    var lowSent = 0;
    var highSent = 0;

    bootUp(
        (time, module, pulse) =>
        {
            if (pulse)
            {
                highSent++;
            }
            else
            {
                lowSent++;
            }

            return false;
        },
        (time) => time == 1000);

    Console.WriteLine($"Part 1: {lowSent * highSent}");

    if (inputFile.StartsWith("sample"))
    {
        Console.WriteLine();
        continue;
    }

    var lastConjunction = modules.Single(m => m.Value.destinations.Contains("rx")).Key;
    var feederConjunctions = modules
        .Where(m => m.Value.destinations.Contains(lastConjunction))
        .Select(m => m.Key)
        .ToArray();
    var lastLowPulse = new Dictionary<string, int>();
    var cycles = new Dictionary<string, int>();

    bootUp(
        (time, module, pulse) =>
        {
            if (!pulse && feederConjunctions.Contains(module) && !cycles.ContainsKey(module))
            {
                if (lastLowPulse.ContainsKey(module))
                {
                    cycles.Add(module, time - lastLowPulse[module]);
                }
                else
                {
                    lastLowPulse.Add(module, time);
                }
            }

            return cycles.Count == feederConjunctions.Length;
        },
        (time) => false);

    Console.WriteLine($"Part 2: {cycles.Values.LCM()}\n");
}
