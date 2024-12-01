using System.Collections;
using System.Text;

namespace AoC.Shared.Collections;

public static class BitArrayExtensions
{
    public static string ToBinaryString(this BitArray bitArray)
    {
        var sb = new StringBuilder();

        foreach (var bit in bitArray)
        {
            sb.Append((bool)bit ? "1" : "0");
        }

        return new string(sb.ToString().Reverse().ToArray());
    }
}
