using AoC.Shared.Enumerable;
using AoC.Shared.Grid;
using AoC.Shared.Looping;

Console.WriteLine("AOC - Day 13\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var notes = File.ReadAllLines(inputFile)
        .Split(string.IsNullOrEmpty)
        .Select(g => g.Values.ToGrid())
        .ToList();

    int summarizeNote(Grid note, int smudgesAllowed = 0)
    {
        var total = 0;

        2.Loop((i) =>
        {
            for (int x = 0; x < note.TotalX - 1; x++)
            {
                var left = x;
                var right = x + 1;
                var smudges = 0;

                while (smudges <= smudgesAllowed && left >= 0 && right < note.TotalX)
                {
                    for (int y = 0; y < note.TotalY; y++)
                    {
                        if (note[left, y] != note[right, y])
                        {
                            smudges++;
                        }
                    }

                    left--;
                    right++;
                }

                if (smudges == smudgesAllowed)
                {
                    total += i switch
                    {
                        0 => x + 1, // columns
                        _ => (x + 1) * 100 // rows
                    };

                    break;
                }
            }

            note.Rotate90Prime();
        });
        
        return total;
    }

    Console.WriteLine($"Part 1: {notes.Select(note => summarizeNote(note.Clone())).Sum()}");
    Console.WriteLine($"Part 2: {notes.Select(note => summarizeNote(note, 1)).Sum()}\n");
}
