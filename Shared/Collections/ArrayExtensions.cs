namespace Shared.Collections
{
    public static class ArrayExtensions
    {
        public static int WrapIndex(this string str, int index)
        {
            return WrapIndex(str.ToCharArray(), index);
        }

        public static int WrapIndex<T>(this T[] array, int index)
        {
            if (index >= array.Length)
            {
                return 0;
            }

            if (index < 0)
            {
                return array.Length - 1;
            }

            return index;
        }

        public static int ToInt32(this bool[] array)
        {
            if (array.Length > 32)
            {
                throw new ArgumentOutOfRangeException("Array more than 32 bits!");
            }

            var number = 0;

            for (var i = 0; i < array.Length; i++)
            {
                if (array[i])
                {
                    number |= (1 << i);
                }
            }

            return number;
        }

        public static int ToInt32(this IEnumerable<bool> enumerable)
        {
            return enumerable.ToArray().ToInt32();
        }

        public static long ToInt64(this bool[] array)
        {
            if (array.Length > 64)
            {
                throw new ArgumentOutOfRangeException("Array more than 64 bits!");
            }

            var number = 0L;

            for (var i = 0; i < array.Length; i++)
            {
                if (array[i])
                {
                    number |= (1L << i);
                }
            }

            return number;
        }

        public static long ToInt64(this IEnumerable<bool> enumerable)
        {
            return enumerable.ToArray().ToInt64();
        }
    }
}
