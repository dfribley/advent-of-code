// Advent of Code challenge: https://adventofcode.com/2024/day/23

Console.WriteLine("AoC - Day 23\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var networkMap = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(ln =>
        {
            var parts = ln.Split("-");
            return (a: parts[0], b: parts[1]);
        })
        .ToArray();
    
    var adjacentNodes = new Dictionary<string, HashSet<string>>();

    foreach (var (a, b) in networkMap)
    {
        if (!adjacentNodes.ContainsKey(a))
        {
            adjacentNodes[a] = [];
        }
        if (!adjacentNodes.ContainsKey(b))
        {
            adjacentNodes[b] = [];
        }
        adjacentNodes[a].Add(b);
        adjacentNodes[b].Add(a);
    }

    var groups = new List<List<string>>();

    foreach (var kvp in adjacentNodes)
    {
        FindGroupOfThree(kvp.Key, [kvp.Key], 0);
    }

    var part1 = groups
        .Where(grp => grp.Any(node => node.StartsWith("t")))
        .Select(grp => string.Join("", grp.Order().Distinct()))
        .Distinct()
        .Count();
    
    Console.WriteLine($"Part 1: {part1}");

    var cliques = new List<HashSet<string>>();
    BronKerbosch([], [..adjacentNodes.Keys], [], adjacentNodes, cliques);
    
    var part2 = string.Join(",", cliques
        .OrderByDescending(c => c.Count)
        .First()
        .Order());
    
    Console.WriteLine($"Part 2: {part2}\n");
    
    continue;

    void FindGroupOfThree(string node, List<string> path, int hop)
    {
        if (hop == 3)
        {
            if (node == path[0])
            {
                groups.Add(path);
            }

            return;
        }

        foreach (var next in adjacentNodes[node])
        {
            FindGroupOfThree(next, [..path, next], hop + 1);
        }
    }
}

return;

static void BronKerbosch(HashSet<string> r, HashSet<string> p, HashSet<string> x, Dictionary<string, HashSet<string>> adjNodes, List<HashSet<string>> cliques)
{
    if (p.Count == 0 && x.Count == 0)
    {
        cliques.Add([..r]);
        return;
    }

    var p1 = new HashSet<string>(p);
    
    foreach (var v in p1)
    {
        var nP = p.Intersect(adjNodes[v]).ToHashSet();
        var nX = x.Intersect(adjNodes[v]).ToHashSet();
        
        BronKerbosch([..r, v], nP, nX, adjNodes, cliques);
        
        p.Remove(v);
        x.Add(v);
    }
}
