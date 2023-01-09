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
    }
}
