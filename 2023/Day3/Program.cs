using System.Drawing;
using AoC.Shared.Enumerable;
using AoC.Shared.Grid;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 3\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var grid = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToGrid();

    var partNumbers = new List<int>();
    var gearParts = new Dictionary<Point, IList<int>>();

    var number = string.Empty;
    var isPart = false;
    var gears = new HashSet<Point>();

    void processPartNumber()
    {
        if (isPart)
        {
            var partNumber = number.ToInt32();

            partNumbers.Add(partNumber);

            foreach (var gear in gears)
            {
                if (!gearParts.ContainsKey(gear))
                {
                    gearParts.Add(gear, new List<int>());
                }

                gearParts[gear].Add(partNumber);
            }
        }

        number = string.Empty;
        isPart = false;
        gears.Clear();
    }

    for (var y = 0; y <= grid.MaxY; y++)
    {
        for (var x = 0; x <= grid.MaxX; x++)
        {
            var coord = new Point(x, y);

            if (grid[coord].IsDigit())
            {
                number += grid[coord];

                foreach (var (coordinate, value) in grid.Neighbors(coord))
                {
                    if (value == '*')
                    {
                        gears.Add(coordinate);
                    }

                    if (value != '.' && !value.IsDigit())
                    {
                        isPart = true;
                    }
                }
            }
            else
            {
                processPartNumber();
            }
        }

        processPartNumber();
    }

    Console.WriteLine($"Part 1: {partNumbers.Sum()}");
    Console.WriteLine($"Part 2: {gearParts.Where(kvp => kvp.Value.Count == 2).Select(kvp => kvp.Value.Product()).Sum()}\n");
}
