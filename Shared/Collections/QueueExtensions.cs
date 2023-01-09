namespace Shared.Collections
{
    public static class QueueExtensions
    {
        public static void Add<TValue>(this Queue<TValue> queue, TValue value)
        {
            queue.Enqueue(value);
        }

        public static void Add<TValue>(this Queue<TValue> queue, IEnumerable<TValue> values)
        {
            foreach (var value in values)
            {
                queue.Enqueue(value);
            }
        }
    }
}
