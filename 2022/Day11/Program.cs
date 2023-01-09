using AoC.Puzzle11;
using AoC.Shared.Strings;
using Shared.Collections;
using Shared.Enumerable;

Console.WriteLine("AOC - Day 11\n");

static long getMonkeyBusiness(IDictionary<int, Monkey> monkeys, int rounds, Func<long, long> adjustWorry)
{
    var inspections = new Dictionary<int, int>();

    while (rounds-- > 0)
    {
        foreach (var monkey in monkeys.Values)
        {
            while (monkey.Items.Any())
            {
                var worry = monkey.Operation(monkey.Items.Dequeue());
                worry = adjustWorry(worry);

                if (worry % monkey.Test == 0)
                {
                    monkeys[monkey.OnTrue].Items.Enqueue(worry);
                }
                else
                {
                    monkeys[monkey.OnFalse].Items.Enqueue(worry);
                }

                if (!inspections.ContainsKey(monkey.Id))
                {
                    inspections.Add(monkey.Id, 0);
                }

                inspections[monkey.Id]++;
            }
        }
    }

    var monkeyBusiness = 1L;
    inspections
        .Values
        .OrderDescending()
        .Take(2)
        .ToList()
        .ForEach(inspectionCount => monkeyBusiness *= inspectionCount);

    return monkeyBusiness;
};

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    Console.WriteLine($"[{inputFile}]\n");

    var monkeys = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .Select(sg =>
        {
            var monkeyDef = sg.Values.ToList();
            var opDef = monkeyDef[2].Split(' ');

            long operation(long worry)
            {
                var right = opDef[^1] == "old" ? worry : opDef[^1].ToInt32();
                return opDef[^2] == "+" ? worry + right : worry * right;
            }

            return new Monkey
            {
                Id = monkeyDef[0].Split(' ')[1][..^1].ToInt32(),
                Items = new Queue<long> { monkeyDef[1][(monkeyDef[1].IndexOf(':') + 1)..].Split(',').Select(s => (long)s.ToInt32()) },
                Operation = operation,
                Test = monkeyDef[3].Split(' ').Last().ToInt32(),
                OnTrue = monkeyDef[4].Split(' ').Last().ToInt32(),
                OnFalse = monkeyDef[5].Split(' ').Last().ToInt32()
            };
        })
        .ToDictionary(m => m.Id, m => m);

    var part1 = getMonkeyBusiness(monkeys.Clone(), 20, worry => (int)Math.Floor((decimal)worry / 3));
    Console.WriteLine($"Part 1: {part1}");

    var lcm = 1;
    monkeys
        .Values
        .Select(monkey => monkey.Test)
        .ToList()
        .ForEach(divisable =>
        {
            lcm *= divisable;
        });
    var part2 = getMonkeyBusiness(monkeys, 10000, worry => worry % lcm);
    Console.WriteLine($"Part 2: {part2}\n");
}