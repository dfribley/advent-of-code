namespace AoC.Shared.BinarySearch
{
    public class BinarySearchSet(int low, int high)
    {
        public int Low { get; } = low;

        public int High { get; } = high;

        public BinarySearchSet LowerHalf => new(Low, (int) Math.Floor((Low + High) / 2f));

        public BinarySearchSet UpperHalf => new((int)Math.Ceiling((Low + High) / 2f), High);
    }
}

