using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 7\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var path = new Stack<string>();
    var sizes = new Dictionary<string, int>();
    
    File.ReadAllLines(inputFile)
        .ToList()
        .ForEach(l =>
        {
            var parts = l.Split(' ');

            if (parts[1] == "cd")
            {
                if (parts[2] == "..")
                {
                    path.Pop();
                }
                else
                {
                    path.Push(parts[2]);
                }
            }
            else if (parts[1] != "ls" && parts[0] != "dir")
            {
                var size = parts[0].ToInt32();
                var pathFolders = path.Reverse().ToList();

                for (var i = 0; i < pathFolders.Count; i++)
                {
                    var dirKey = pathFolders[0] + string.Join("/", pathFolders.Skip(1).Take(i));

                    if (!sizes.ContainsKey(dirKey))
                    {
                        sizes[dirKey] = size;
                    }
                    else
                    {
                        sizes[dirKey] += size;
                    }
                }
            }
        });

    var part1 = sizes.Values
        .Where(s => s <= 100000)
        .Sum();
    
    Console.WriteLine($"Part 1: {part1}");

    var spaceNeeded = 30000000 - (70000000 - sizes["/"]);
    var part2 = sizes.Values
        .Where(s => s >= spaceNeeded)
        .Order()
        .First();
    
    Console.WriteLine($"Part 2: {part2}\n");
}