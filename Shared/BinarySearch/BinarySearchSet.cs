namespace AoC.Shared.BinarySearch
{
    public class BinarySearchSet
	{
        private readonly int low;
        private readonly int high;

        public BinarySearchSet(int low, int high)
		{
            this.low = low;
            this.high = high;
        }

        public int Low { get => low; }

        public int High { get => high; }

        public BinarySearchSet LowerHalf
        {
            get
            {
                return new BinarySearchSet(low, (int) Math.Floor((low + high) / 2f));
            }
        }

        public BinarySearchSet UpperHalf
        {
            get
            {
                return new BinarySearchSet((int)Math.Ceiling((low + high) / 2f), high);
            }
        }
	}
}

