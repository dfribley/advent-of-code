// Advent of Code challenge: https://adventofcode.com/2024/day/11

using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 11\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var stones = File.ReadAllText(inputFile)
        .Split(" ", StringSplitOptions.RemoveEmptyEntries)
        .ToDictionary(id => id.ToInt64(), id => 1L);
    var part2Stones = stones.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

    Console.WriteLine($"Part 1: {BlinkALot(25, stones)}");
    Console.WriteLine($"Part 2: {BlinkALot(75, part2Stones)}\n");
}

return;

long BlinkALot(int blinks, Dictionary<long, long> stones)
{
    while (blinks-- > 0)
    {
        var newStones = new Dictionary<long, long>();
        
        foreach (var stone in stones.Keys)
        {
            var blinkResult = Blink(stone);
            
            foreach (var newStone in blinkResult)
            {
                if (!newStones.ContainsKey(newStone))
                {
                    newStones[newStone] = stones[stone];
                }
                else
                {
                    newStones[newStone] += stones[stone];
                }
            }
        }

        stones = newStones;
    }

    return stones.Values.Sum();
}

long[] Blink(long stone)
{
    if (stone == 0)
    {
        return [1];
    }

    var stoneString = stone.ToString();

    if (stoneString.Length % 2 != 0)
    {
        return [stone * 2024];
    }
    
    var mid = stoneString.Length / 2;
    
    return [stoneString[..mid].ToInt64(), stoneString[mid..].ToInt64()];
}
