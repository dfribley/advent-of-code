using AoC.Shared.Strings;
using System.Numerics;

Console.WriteLine("AOC - Day 8\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var trees = File.ReadAllLines(inputFile).ToList();
    var scores = trees
        .SelectMany((row, r) =>
        {
            return row.Select((tree, c) =>
            {
                var visible = false;
                var scenicScore = 1;
                var treeHeight = tree.ToString().ToInt32();

                foreach (var dir in new[] { new Vector2(-1, 0), new Vector2(0, -1), new Vector2(1, 0), new Vector2(0, 1) })
                {
                    var pos = new Vector2(c, r);
                    var distance = 0;

                    while (true)
                    {
                        pos += dir;

                        if (pos.X < 0 || pos.X == row.Length || pos.Y < 0 || pos.Y == trees.Count)
                        {
                            visible = true;
                            break;
                        }

                        distance++;

                        if (trees[(int)pos.Y][(int)pos.X].ToInt32() >= treeHeight)
                        {
                            break;
                        }
                    }

                    scenicScore *= distance;
                }

                return (visible, scenicScore);
            });
        });

    var part1 = scores
        .Where(t => t.visible)
        .Count();
    Console.WriteLine($"Part 1: {part1}");

    var part2 = scores
        .OrderByDescending(t => t.scenicScore)
        .First()
        .scenicScore;
    Console.WriteLine($"Part 2: {part2}\n");
}