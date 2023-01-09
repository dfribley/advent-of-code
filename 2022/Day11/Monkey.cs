using System.Numerics;

namespace AoC.Puzzle11
{
    internal class Monkey : ICloneable
    {
        public int Id { get; set; }
        public Queue<long> Items { get; set; }
        public Func<long, long> Operation { get; set; }
        public int Test { get; set; }
        public int OnTrue { get; set; }
        public int OnFalse { get; set; }

        public object Clone()
        {
            return new Monkey
            {
                Id = Id,
                Items = new Queue<long>(Items),
                Operation = Operation,
                Test = Test,
                OnTrue = OnTrue,
                OnFalse = OnFalse
            };
        }
    }
}
