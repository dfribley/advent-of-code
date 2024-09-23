using System.Drawing;
using AoC.Shared.Grid;

Console.WriteLine("AOC - Day 11\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var seats = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToGrid();

    Console.WriteLine($"Part 1: {SimulateSeating()}");
    Console.WriteLine($"Part 2: {SimulateSeating(false)}\n");
    continue;

    int SimulateSeating(bool part1 = true)
    {
        var seatsCopy = seats.Clone();

        while (true)
        {
            var changes = new List<(Point, char)>();

            foreach (var seat in seatsCopy)
            {
                if (seat.value == '.')
                {
                    continue;
                }
                
                var neighbors = part1
                    ? seatsCopy.Neighbors(seat.coordinate)
                    : seatsCopy.NeighborsInSight(seat.coordinate, c => c == '.');

                if (seat.value == 'L')
                {
                    if (neighbors.All((n => n.value != '#')))
                    {
                        changes.Add((seat.coordinate, '#'));
                    }
                }
                else
                {
                    if (neighbors.Count(n => n.value == '#') >= (part1 ? 4 : 5))
                    {
                        changes.Add((seat.coordinate, 'L'));
                    }

                }
            }
        
            if (changes.Count == 0)
            {
                break;
            }
        
            foreach (var (coordinate, value) in changes)
            {
                seatsCopy[coordinate] = value;
            }
        }

        return seatsCopy.Count(seat => seat.value == '#');
    }
}
