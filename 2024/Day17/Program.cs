// Advent of Code challenge: https://adventofcode.com/2024/day/17

using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 17\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt"})
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var input = File.ReadAllLines(inputFile)
        .Where(line => !string.IsNullOrEmpty(line))
        .ToList();

    var registerA = input[0].Split(": ")[1].ToInt32();
    var program = input[3].Split(": ")[1].Split(",").Select(int.Parse).ToArray();

    Console.WriteLine($"Part 1: {RunProgram(program, registerA)}");
    Console.WriteLine($"Part 2:\n");
}

return;

string RunProgram(int[] program, int registerA)
{
    var regA = (long) registerA;
    var regB = 0L;
    var regC = 0L;
    var insPointer = 0;
    var output = new List<int>();

    do
    {
        var ins = (Instructions)program[insPointer];
        var operand = program[insPointer + 1];

        switch (ins)
        {
            case Instructions.Adv:
                regA = (long) Math.Floor(regA / Math.Pow(2, GetComboOperand(operand)));
                break;
            case Instructions.Bxl:
                regB ^= operand;
                break;
            case Instructions.Bst:
                regB = GetComboOperand(operand) % 8;
                break;
            case Instructions.Jnz:
                if (regA != 0)
                {
                    insPointer = operand;
                    continue;
                }
                break;
            case Instructions.Bxc:
                regB ^= regC;
                break;
            case Instructions.Out:
                var val = (int) GetComboOperand(operand) % 8;
                output.Add(val);
                break;
            case Instructions.Bdv:
                regB = (long) Math.Floor(regA / Math.Pow(2, GetComboOperand(operand)));
                break;
            case Instructions.Cdv:
                regC = (long) Math.Floor(regA / Math.Pow(2, GetComboOperand(operand)));
                break;
        }

        insPointer += 2;
    } 
    while (insPointer < program.Length);

    return string.Join(",", output);

    long GetComboOperand(int operand) => operand switch
    {
        4 => regA,
        5 => regB,
        6 => regC,
        _ => operand
    };
}

internal enum Instructions
{
    Adv,
    Bxl,
    Bst,
    Jnz,
    Bxc,
    Out,
    Bdv,
    Cdv,
}