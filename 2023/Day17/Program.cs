using System.Drawing;
using AoC.Shared.Grid;
using AoC.Shared.Points;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 17\n\n");

var north = new Point(0, 1);
var south = new Point(0, -1);
var west = new Point(-1, 0);
var east = new Point(1, 0);

foreach (var inputFile in new[] { "sample.txt", "input.txt"})
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var map = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToGrid();
    var heatLossLookup = map
        .All()
        .ToDictionary(t => t.coordinate, t => t.value.ToString().ToInt32());
    var start = map.TopLeft;
    var end = map.BottomRight;

    int findMinHeatLoss(int minConsecutive, int maxConsecutive)
    {
        var knownLevels = new Dictionary<(Point pos, Point dir, int consec), int>();
        var queue = new PriorityQueue<(Point pos, Point dir, int consec), int>();

        queue.Enqueue((start, east, 0), 0);

        while (queue.TryDequeue(out var move, out var heatLoss))
        {
            if (knownLevels.ContainsKey(move))
            {
                continue;
            }
            else
            {
                knownLevels.Add(move, heatLoss);
            }

            var nextMoves = new List<(Point pos, Point dir, int consec)>();
            
            if (move.consec < maxConsecutive)
            {
                nextMoves.Add((move.pos.Add(move.dir), move.dir, move.consec + 1));
            }

            if (move.consec >= minConsecutive)
            {
                if (move.dir == north || move.dir == south)
                {
                    nextMoves.Add((move.pos.Add(east), east, 1));
                    nextMoves.Add((move.pos.Add(west), west, 1));
                }
                else
                {
                    nextMoves.Add((move.pos.Add(north), north, 1));
                    nextMoves.Add((move.pos.Add(south), south, 1));
                }
            }

            foreach (var nextMove in nextMoves.Where(m => map.IsValid(m.pos)))
            {
                queue.Enqueue(nextMove, heatLoss + heatLossLookup[nextMove.pos]);
            }
        }

        return knownLevels
            .Where(kvp => kvp.Key.pos == end && kvp.Key.consec >= minConsecutive)
            .Select(kvp => kvp.Value)
            .Min();
    }

    Console.WriteLine($"Part 1: {findMinHeatLoss(0, 3)}");
    Console.WriteLine($"Part 2: {findMinHeatLoss(4, 10)}\n");
}
