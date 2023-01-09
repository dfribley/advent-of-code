namespace Shared.Enumerable
{
    public class SplitGroup<T>
    {
        public int Id { get; set; }
        public IEnumerable<T> Values { get; set; }
    }
}
