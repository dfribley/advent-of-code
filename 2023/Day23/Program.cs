// Advent of Code challenge: https://adventofcode.com/2023/day/23

using System.Diagnostics;
using System.Drawing;
using System.Text;
using AoC.Shared.Grid;
using AoC.Shared.Points;

Console.WriteLine("AoC - Day 23\n\n");

var graph = new Graph();
graph.AddEdge("A", "B");
graph.AddEdge("A", "C");
graph.AddEdge("B", "E");
graph.AddEdge("C", "F");
graph.AddEdge("B", "D");
graph.AddEdge("C", "D");
graph.AddEdge("D", "E");
graph.AddEdge("D", "F");
graph.AddEdge("E", "G");
graph.AddEdge("F", "G");

var sortedOrder = graph.TopologicalSort();
Console.WriteLine(string.Join(" -> ", sortedOrder));



foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var map = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToGrid();

    // var start = new Point(1, map.MaxY);
    var end = new Point(map.MaxX - 1, 0);
    var crossRoads = new List<Point> { new(1, map.MaxY), end };

    foreach (var xy in map)
    {
        if (xy.value != '.')
        {
            continue;
        }
        
        if (GridDirections.SideNeighbors.All(n =>
            {
                var next = xy.coordinate.Add(n);

                return map.IsValid(next) && map[next] != '.';
            }))
        {
            crossRoads.Add(xy.coordinate);
        }
    }
    
    var distances = new Dictionary<Point, List<(Point, int)>>();
    var queue = new Queue<(Point, List<Point>)>();
    
    foreach (var xy in crossRoads)
    {
        queue.Enqueue((xy, [xy]));
        
        while (queue.TryDequeue(out var element))
        {
            var (pnt, path) = element;

            if (crossRoads.Contains(pnt) && pnt != xy)
            {
                if (!distances.ContainsKey(xy))
                {
                    distances[xy] = new List<(Point, int)>();
                }
                distances[xy].Add((pnt, path.Count - 1));
                continue;
            }
            
            foreach (var dir in GridDirections.SideNeighbors)
            {
                var next = pnt.Add(dir);
                
                if (!map.IsValid(next) || path.Contains(next) || map[next] == '#')
                {
                    continue;
                }

                if (map[next] != '.')
                {
                    var nextDir = map[next] switch
                    {
                        '>' => GridDirections.East,
                        '<' => GridDirections.West,
                        '^' => GridDirections.North,
                        'v' => GridDirections.South,
                    };
                
                    if (nextDir != dir)
                    {
                        continue;
                    }
                    
                    var newpath = new List<Point>(path);
                    newpath.Add(next);
                    next = next.Add(dir);
                    newpath.Add(next);
                    queue.Enqueue((next, newpath));
                    continue;
                }
                
                queue.Enqueue((next, [..path, next]));
            }
        }
    }
    
    GenerateGraphVizDotFile(distances, "graph.dot");
    
    var stopWatch = Stopwatch.StartNew();
    
    var part2 = FindLongestPathSteps(distances, new Point(1, map.MaxY), end);
    
    stopWatch.Stop();
    Console.WriteLine($"Part 2: {part2} ({stopWatch.ElapsedMilliseconds}ms)");
    
    stopWatch.Start();
    
    var part2Start = new Point(1, map.MaxY);
    var part2End = new Point(map.MaxX - 1, 0);

    var part2Queue = new Queue<(Point pnt, int steps, List<Point> path)>();
    var endDist = new List<int>();
    
    part2Queue.Enqueue((part2Start, 0, [part2Start]));

    while (part2Queue.TryDequeue(out var element))
    {
        var (pnt, steps, path) = element;
        
        if (pnt == part2End)
        {
            endDist.Add(steps);
            continue;
        }

        foreach (var (next, distance) in distances[pnt])
        {
            if (path.Contains(next))
            {
                continue;
            }
            
            part2Queue.Enqueue((next, steps + distance, [..path, next]));
        }
    }
    
    stopWatch.Stop();
    Console.WriteLine($"Part 2: {endDist.Max()} ({stopWatch.ElapsedMilliseconds}ms)");
    Console.WriteLine($"Part 2: {endDist.Max()}\n");
}

static void GenerateGraphVizDotFile(Dictionary<Point, List<(Point, int)>> map, string outputFilePath)
{
    var sb = new StringBuilder();
    sb.AppendLine("digraph G {");

    foreach (var kvp in map)
    {
        var from = kvp.Key;
        foreach (var (to, weight) in kvp.Value)
        {
            sb.AppendLine($"    \"{from}\" -> \"{to}\" [label=\"{weight}\"];");
        }
    }

    sb.AppendLine("}");

    File.WriteAllText(outputFilePath, sb.ToString());
}

// static int FindLongestPathSteps(Dictionary<Point, List<(Point, int)>> map, Point start, Point end)
// {
//     var maxWeight = 0;
//     var visited = new HashSet<Point>();
//
//     DFS(map, start, end, visited, 0, ref maxWeight);
//
//     return maxWeight;
// }

static void DFS(Dictionary<Point, List<(Point, int)>> map, Point current, Point end, HashSet<Point> visited, int currentWeight, ref int maxWeight)
{
    if (current.Equals(end))
    {
        if (currentWeight > maxWeight)
        {
            maxWeight = currentWeight;
        }
        return;
    }

    visited.Add(current);

    foreach (var (next, weight) in map[current])
    {
        if (!visited.Contains(next))
        {
            DFS(map, next, end, visited, currentWeight + weight, ref maxWeight);
        }
    }

    visited.Remove(current);
}

static int FindLongestPathSteps(Dictionary<Point, List<(Point, int)>> map, Point start, Point end)
{
    var topologicalOrder = TopologicalSort(map);
    var distances = new Dictionary<Point, int>();

    foreach (var point in map.Keys)
    {
        distances[point] = int.MinValue;
    }
    distances[start] = 0;

    foreach (var point in topologicalOrder)
    {
        if (point == end)
        {
            continue;
        }
        
        if (distances[point] != int.MinValue)
        {
            foreach (var (next, weight) in map[point])
            {
                if (next == end)
                {
                    if (!distances.TryGetValue(next, out var currentDistance))
                    {
                        currentDistance = 0;
                    }
                    
                    distances[next] = Math.Max(currentDistance, distances[point] + weight);
                }
                
                if (distances[next] < distances[point] + weight)
                {
                    distances[next] = distances[point] + weight;
                }
            }
        }
    }

    return distances[end];
}

static List<Point> TopologicalSort(Dictionary<Point, List<(Point, int)>> map)
{
    var visited = new HashSet<Point>();
    var stack = new Stack<Point>();
    var result = new List<Point>();

    // foreach (var point in map.Keys)
    // {
    //     if (!visited.Contains(point))
    //     {
    //         TopologicalSortUtil(point, visited, stack, map);
    //     }
    // }
    
    TopologicalSortUtil(map.First().Key, visited, stack, map);

    while (stack.Count > 0)
    {
        result.Add(stack.Pop());
    }

    return result;
}

static void TopologicalSortUtil(Point point, HashSet<Point> visited, Stack<Point> stack, Dictionary<Point, List<(Point, int)>> map)
{
    visited.Add(point);

    if (map.TryGetValue(point, out var value))
    {
        foreach (var (next, _) in value)
        {
            if (!visited.Contains(next))
            {
                TopologicalSortUtil(next, visited, stack, map);
            }
        }
    }

    stack.Push(point);
}

public class Graph
{
    private readonly Dictionary<string, List<string>> _adjacencyList;

    public Graph()
    {
        _adjacencyList = new Dictionary<string, List<string>>();
    }

    public void AddEdge(string from, string to)
    {
        if (!_adjacencyList.ContainsKey(from))
        {
            _adjacencyList[from] = [];
        }
        _adjacencyList[from].Add(to);
    }

    public List<string> TopologicalSort()
    {
        var visited = new HashSet<string>();
        var stack = new Stack<string>();
        var result = new List<string>();

        foreach (var vertex in _adjacencyList.Keys)
        {
            if (!visited.Contains(vertex))
            {
                TopologicalSortUtil(vertex, visited, stack);
            }
        }

        while (stack.Count > 0)
        {
            result.Add(stack.Pop());
        }

        return result;
    }

    private void TopologicalSortUtil(string vertex, HashSet<string> visited, Stack<string> stack)
    {
        visited.Add(vertex);

        if (_adjacencyList.TryGetValue(vertex, out var neighbors))
        {
            foreach (var neighbor in neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    TopologicalSortUtil(neighbor, visited, stack);
                }
            }
        }

        stack.Push(vertex);
    }
}