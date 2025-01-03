// Advent of Code challenge: https://adventofcode.com/2024/day/24

using AoC.Shared.BinaryString;
using AoC.Shared.Enumerable;
using AoC.Shared.Strings;
using QuikGraph;
using QuikGraph.Graphviz;

Console.WriteLine("AoC - Day 24\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .ToList();

    var states = input[0].Values
        .Select(ln =>
        {
            var parts = ln.Split(": ");
            return (wire: parts[0], value: parts[1].ToInt32() == 1);
        })
        .ToDictionary(t => t.wire, t => t.value);
    var gates = input[1].Values
        .Select((ln, i) =>
        {
            var parts = ln.Split(" -> ");
            var operands = parts[0].Split(" ");

            return (left: operands[0], right: operands[2], op: operands[1], target: parts[1]);
        })
        .ToArray();
    
    ExportToGraphViz(gates, $"{inputFile}.dot");
    
    var gateLookup = new Dictionary<string, List<(string left, string right, string op, string target)>>();
    foreach (var gate in gates)
    {
        if (!gateLookup.ContainsKey(gate.left))
        {
            gateLookup[gate.left] = new List<(string, string, string, string)>();
        }
        
        if (!gateLookup.ContainsKey(gate.right))
        {
            gateLookup[gate.right] = new List<(string, string, string, string)>();
        }

        gateLookup[gate.left].Add(gate);
        gateLookup[gate.right].Add(gate);
    }
    
    var queue = new Queue<string>();
    foreach (var w in states.ToArray())
    {
        queue.Enqueue(w.Key);
    }
    
    while (queue.TryDequeue(out var wire))
    {
        if (!gateLookup.TryGetValue(wire, out var gs))
        {
            continue;
        }
        
        foreach (var (l, r, op, target) in gs)
        {
            if (!states.ContainsKey(l) || !states.ContainsKey(r))
            {
                continue;
            }
            
            var left = states[l];
            var right = states[r];
        
            states[target] = op switch
            {
                "AND" => left && right,
                "OR" => left || right,
                _ => left ^ right
            };

            queue.Enqueue(target);
        }
    }

    var part1 = states
        .Where(w => w.Key[0] == 'z')
        .OrderByDescending(w => w.Key[1..].ToInt32())
        .Select(w => w.Value)
        .ToBinaryString()
        .ToLong();
    Console.WriteLine($"Part 1:{part1}");
    
    if (inputFile.Contains("sample"))
    {
        Console.WriteLine();
        continue;
    }
    
    // For part 2, I used an online viewer for GraphViz files to find the solution
    // I have learned that this is a RCA (Ripple Carry Adder) circuit
    // TODO: Implement a solution for part 2 that identifies the problems in the circuit
    var part2 = new[] { "vvf", "z19", "z37", "nvh", "z12", "qdg", "dck", "fgn" };
    Console.WriteLine($"Part 2:{string.Join(",", part2.Order())}\n");
}

static void ExportToGraphViz(IEnumerable<(string left, string right, string op, string target)> gates, string filePath)
{
    var graph = new AdjacencyGraph<string, TaggedEdge<string, string>>();

    // Add vertices and edges
    foreach (var gate in gates)
    {
        var (left, right, op, target) = gate;

        graph.AddVertex(left);
        graph.AddVertex(right);
        graph.AddVertex(target);

        graph.AddEdge(new TaggedEdge<string, string>(left, target, op[0].ToString()));
        graph.AddEdge(new TaggedEdge<string, string>(right, target, op[0].ToString()));
    }

    // Generate GraphViz file
    var graphviz = new GraphvizAlgorithm<string, TaggedEdge<string, string>>(graph);
    graphviz.FormatVertex += (sender, args) =>
    {
        args.VertexFormat.Label = args.Vertex;
    };
    graphviz.FormatEdge += (sender, args) =>
    {
        args.EdgeFormat.Label.Value = args.Edge.Tag;
    };

    var dot = graphviz.Generate();
    File.WriteAllText(filePath, dot);
}
