using System.Drawing;
using AoC.Shared.Grid;
using AoC.Shared.Points;

Console.WriteLine("AOC - Day 16\n\n");

var north = new Point(0, 1);
var south = new Point(0, -1);
var west = new Point(-1, 0);
var east = new Point(1, 0);

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

    int getTotalEnergizedTiles((Point, Point) start)
    {
        var beams = new Queue<(Point position, Point direction)>();
        var energized = new HashSet<Point>();
        var knownStates = new HashSet<(Point, Point)>();

        beams.Enqueue(start);

        while (beams.Any())
        {
            var beam = beams.Dequeue();

            if (knownStates.Contains(beam))
            {
                continue;
            }

            knownStates.Add(beam);

            var (position, direction) = beam;
            energized.Add(position);

            var next = position.Add(direction);
            if (!field.IsValid(next))
            {
                continue;
            }

            if (field[next] == '|' && (direction == west || direction == east))
            {
                beams.Enqueue((next, north));
                beams.Enqueue((next, south));
            }
            else if (field[next] == '-' && (direction == north || direction == south))
            {
                beams.Enqueue((next, east));
                beams.Enqueue((next, west));
            }
            else if (field[next] == '\\')
            {
                if (direction == north)
                {
                    beams.Enqueue((next, west));
                }
                else if (direction == south)
                {
                    beams.Enqueue((next, east));
                }
                else if (direction == east)
                {
                    beams.Enqueue((next, south));
                }
                else
                {
                    beams.Enqueue((next, north));
                }
            }
            else if (field[next] == '/')
            {
                if (direction == north)
                {
                    beams.Enqueue((next, east));
                }
                else if (direction == south)
                {
                    beams.Enqueue((next, west));
                }
                else if (direction == east)
                {
                    beams.Enqueue((next, north));
                }
                else
                {
                    beams.Enqueue((next, south));
                }
            }
            else
            {
                beams.Enqueue((next, direction));
            }
        }

        return energized.Count - 1;
    }
    
    Console.WriteLine($"Part 1: {getTotalEnergizedTiles((new Point(-1, field.MaxY), east))}");

    var part2 = Enumerable.Range(0, field.MaxY).Select(y => getTotalEnergizedTiles((new Point(-1, y), east)))
        .Concat(Enumerable.Range(0, field.MaxY).Select(y => getTotalEnergizedTiles((new Point(field.TotalX, y), west))))
        .Concat(Enumerable.Range(0, field.MaxX).Select(x => getTotalEnergizedTiles((new Point(x, field.TotalY), south))))
        .Concat(Enumerable.Range(0, field.MaxX).Select(x => getTotalEnergizedTiles((new Point(x, -1), north))))
        .Max();

    Console.WriteLine($"Part 2: {part2}\n");
}
