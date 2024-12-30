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
    var output = RunProgram(program, registerA);
    
    Console.WriteLine($"Part 1: {string.Join(",", output)}");

    var exponent = program.Length - 1;
    var part2 = 0L;

    do
    {
        part2 += (long)Math.Pow(8, exponent);
        output = RunProgram(program, part2);
        
        if (output.Length > program.Length)
        {
            throw new Exception("Output is larger than the program");
        }
        
        var compare = program.Length - 1 - exponent;
        if (output.TakeLast(compare).SequenceEqual(program.TakeLast(compare)))
        {
            exponent = Math.Max(0, exponent - 1);
        }
    } while (!output.SequenceEqual(program));
    
    Console.WriteLine($"Part 2: {part2}\n");
}

return;

int[] RunProgram(int[] program, long registerA)
{
    var regA = registerA;
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
                var val = GetComboOperand(operand) % 8;
                output.Add((int)val);
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

    return output.ToArray();

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