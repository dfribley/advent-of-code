using System.Drawing;
using AoC.Shared.Grid;
using AoC.Shared.Points;

// Ray casting https://en.wikipedia.org/wiki/Point_in_polygon
Console.WriteLine("AOC - Day 10\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var field = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToGrid();

    var pipes = new Dictionary<char, (Point n1, Point n2)>()
    {
        { '|', (new Point(0, 1), new Point(0, -1)) },
        { '-', (new Point(-1, 0), new Point(1, 0)) },
        { 'L', (new Point(0, 1), new Point(1, 0)) },
        { 'J', (new Point(-1, 0), new Point(0, 1)) },
        { '7', (new Point(-1, 0), new Point(0, -1)) },
        { 'F', (new Point(0, -1), new Point(1, 0)) },
    };

    var start = field.First(c => c == 'S');
    var loop = new List<Point>() { start };
    var position = field.Neighbors(start)
        .First(c => c.value != '.'
            && (c.coordinate.Add(pipes[c.value].n1) == start ||
                c.coordinate.Add(pipes[c.value].n2) == start))
        .coordinate;

    while (position != start)
    {
        loop.Add(position);

        var neighbor1 = position.Add(pipes[field[position]].n1);
        var neighbor2 = position.Add(pipes[field[position]].n2);

        position = loop[^2] == neighbor1
            ? neighbor2
            : neighbor1;
    }

    Console.WriteLine($"Part 1: {loop.Count / 2}");

    var startNeighbors = new[] { loop[1], loop.Last() };
    var startShape = pipes
        .First(kvp =>
            startNeighbors.Contains(kvp.Value.n1.Add(start)) && 
            startNeighbors.Contains(kvp.Value.n2.Add(start)))
        .Key;

    field[start] = startShape;

    var flipParts = new[] { 'L', '|', 'J' };
    var enclosed = 0;
    
    for (var y = 0; y <= field.MaxY; y++)
    {
        var outside = true;
        for (var x = 0; x <= field.MaxX; x++)
        {
            var coords = new Point(x, y);

            if (loop.Contains(coords))
            {
                if (flipParts.Contains(field[coords]))
                {
                    outside = !outside;
                }
            }
            else if (!outside)
            {
                enclosed++;
            }
        }
    }

    Console.WriteLine($"Part 2: {enclosed}\n");
}
