// Advent of Code challenge: https://adventofcode.com/2024/day/9

using AoC.Shared.Strings;

Console.WriteLine("AoC - Day 9\n\n");

foreach (var inputFile in new[] { "sample.txt", "input.txt" })
{
    if (!File.Exists(inputFile))
    {
        continue;
    }

    Console.WriteLine($"[{inputFile}]\n");

    var diskMap = File.ReadAllLines(inputFile)[0];
    
    var fileIndexes = new SortedDictionary<int, (int fid, int size)>();
    var spaceIndexes = new SortedDictionary<int, int>();
    
    var fileId = 0;
    var ins = 0;
    
    for (var i = 0; i < diskMap.Length; i++)
    {
        var val = diskMap[i].ToInt32();

        if (val == 0)
        {
            continue;
        }
        
        if (i % 2 == 0)
        {
            fileIndexes.Add(ins, (fileId++, val));
        }
        else
        {
            spaceIndexes.Add(ins, val);
        }

        ins += val;
    }

    // Copy file positions now for part 2
    var p2FileIndexes = fileIndexes
        .Select(kvp => (kvp.Value.fid, kvp.Key, kvp.Value.size))
        .OrderByDescending(t => t.fid)
        .ToArray();
    
    // Block based defrag
    foreach (var space in spaceIndexes)
    {
        var lastFile = fileIndexes.Last();

        // Quick exit once all that's left is space
        if (space.Key > lastFile.Key)
        {
            break;
        }
        
        for (var i = 0; i < space.Value; i++)
        {
            var (fid, size) = lastFile.Value;
            var block = space.Key + i;
            var spaceLeft = space.Value - i;

            if (size >= spaceLeft)
            {
                // Need to fill space and possibly split file
                fileIndexes.Add(block, (fid, spaceLeft));

                if (size == spaceLeft)
                {
                    fileIndexes.Remove(lastFile.Key);
                }
                else
                {
                    fileIndexes[lastFile.Key] = (fid, size - spaceLeft);
                }
                
                break;
            }

            // File is smaller than the space we're working with
            fileIndexes.Add(block, (fid, size));
            fileIndexes.Remove(lastFile.Key);
            
            // Get next file
            lastFile = fileIndexes.Last();

            // If we're the last file, no need for further processing
            if (lastFile.Key == block)
            {
                break;
            }
            
            // Advance our block pointer the size we just consumed
            i += size - 1;
        }
    }
    
    var part1 = 0L;
    foreach (var kvp in fileIndexes)
    {
        var (fid, size) = kvp.Value;

        for (var i = 0; i < size; i++)
        {
            part1 += (kvp.Key + i) * fid;
        }
    }
   
    Console.WriteLine($"Part 1: {part1}");
    
    // File based defrag

    for (var i = 0; i < p2FileIndexes.Length; i++)
    {
        var (fid, idx, size) = p2FileIndexes[i];

        foreach (var space in spaceIndexes)
        {
            // space is past the file
            if (space.Key > idx)
            {
                break;
            }
            
            // Is there room for the file?
            if (space.Value >= size)
            {
                // Relocate the file
                p2FileIndexes[i] = (fid, space.Key, size);

                // Remove old space
                spaceIndexes.Remove(space.Key);
                
                // Add new if necessary
                if (size < space.Value)
                {
                    spaceIndexes[space.Key + size] = space.Value - size;
                }

                break;
            }
        }
    }
    
    var part2 = 0L;
    foreach (var (fid, idx, size) in p2FileIndexes)
    {
        for (var i = 0; i < size; i++)
        {
            part2 += (idx + i) * fid;
        }
    }
    
    Console.WriteLine($"Part 2: {part2}\n");
}
