namespace AoC.Shared.BinaryString;

public static class BinaryStringExtensions
{
    public static BinaryString ToBinaryString(this long value, int length)
    {
        var binary = Convert.ToString(value, 2);
        return new BinaryString(binary.PadLeft(length, '0'));
    }
    
    public static BinaryString ToBinaryString(this int value, int length)
    {
        return ((long)value).ToBinaryString(length);
    }
    
    public static long ToInt64(this BinaryString binaryString)
    {
        return Convert.ToInt64(binaryString.ToString(), 2);
    }
    
    public static int ToInt32(this BinaryString binaryString)
    {
        return Convert.ToInt32(binaryString.ToString(), 2);
    }
}