using AoC.Shared.Enumerable;
using AoC.Shared.Strings;

Console.WriteLine("AOC - Day 1\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var expenses = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .Select(line => line.ToInt32())
        .Order()
        .ToArray();

    string findNumbersThatSum(int parts, int target)
    {
        var queue = new Queue<int[]>();
        queue.Enqueue(new int[parts]);

        while(queue.Count > 0)
        {
            var indexes = queue.Dequeue();
            var sum = indexes
                .Select(i => expenses[i])
                .Sum();

            if (sum == target)
            {
                return indexes
                    .Select(i => expenses[i])
                    .Product()
                    .ToString();
            }

            if (sum > target)
            {
                continue;
            }

            for (var i = 0; i < indexes.Length; i++)
            {
                var newIndexes = (int[])indexes.Clone();
 
                if (++newIndexes[i] >= expenses.Length)
                {
                    continue;
                }

                queue.Enqueue(newIndexes);
            }
        }

        return "Unable to find a solution";
    }

    Console.WriteLine($"Part 1: {findNumbersThatSum(2, 2020)}");
    Console.WriteLine($"Part 2: {findNumbersThatSum(3, 2020)}\n");
}