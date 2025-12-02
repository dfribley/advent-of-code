// Advent of Code challenge: https://adventofcode.com/2019/day/6
Console.WriteLine("AoC - Day 6\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line =>
        {
            var parts = line.Split(')');
            return (key: parts[0], value: parts[1]);
        })
        .ToList();
    
    var map = new Dictionary<string, List<string>>();
    foreach (var kvp in input)
    {
        if (!map.ContainsKey(kvp.key))
        {
            map.Add(kvp.key, []);
        }
        
        map[kvp.key].Add(kvp.value);
    }
    
    Console.WriteLine($"Part 1: {GetCounts("COM", map, 0)}");

    var test1 = GetNewPath("COM", ["YOU", "SAN"], map);
    
    var youPath = GetPath("COM", "YOU", map, []);
    var santaPath = GetPath("COM", "SAN", map, []);

    var commonBase = 0;

    for (; commonBase < youPath.Count; commonBase++)
    {
        if (youPath[commonBase] != santaPath[commonBase])
        {
            break;
        }
    }
    
    Console.WriteLine($"Part 2: {(youPath.Count - commonBase)+(santaPath.Count - commonBase)}\n");
}

return;

static int GetCounts(string key, Dictionary<string, List<string>> map, int pathCount)
{
    if (!map.ContainsKey(key))
    {
        return pathCount;
    }

    var childScore = 0;
    foreach (var child in map[key])
    {
        childScore += GetCounts(child, map, pathCount + 1);
    }
    
    return childScore + pathCount;
}

static int? GetNewPath(string key, string[] end, Dictionary<string, List<string>> map)
{
    if (end.Contains(key))
    {
        return 1;
    }

    if (!map.ContainsKey(key))
    {
        return null;
    }

    int? path1 = null;
    int? path2 = null;

    foreach (var child in map[key])
    {
        var result = GetNewPath(child, end, map);
        if (result != null)
        {
            if (path1 == null)
            {
                path1 = result;
            }
            else if (path2 == null)
            {
                Console.WriteLine($"Test: {path1 + result - 2}");
                break;
            }
        }
    }

    if (path1 != null)
    {
        return path1 + 1;
    }

    return null;
}

static List<string> GetPath(string key, string end, Dictionary<string, List<string>> map, List<string> path)
{
    if (key == end)
    {
        return path;
    }

    if (!map.ContainsKey(key))
    {
        return null;
    }

    var childPath = new List<string>(path) { key };
    foreach (var child in map[key])
    {
        var result = GetPath(child, end, map, childPath);

        if (result != null)
        {
            return result;
        }
    }

    return null;
}