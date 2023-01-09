namespace AoC.Day13
{
    internal class PacketComparer : IComparer<object>
    {
        public int Compare(object? a, object? b)
        {
            if (a is int intA && b is int intB)
            {
                return intA.CompareTo(intB);
            }

            if(a is IList<object> listA && b is IList<object> listB)
            {
                var i = 0;
                while (i < listA.Count && i < listB.Count)
                {
                    var comp = Compare(listA[i], listB[i]);
                    switch (comp)
                    {
                        case 1:
                        case -1:
                            return comp;
                    }

                    i++;
                }

                if (i == listA.Count && i < listB.Count)
                {
                    return -1;
                }

                if (i == listB.Count && i < listA.Count)
                {
                    return 1;
                }

                return 0;
            }

            if (a is IList<object> && b is int)
            {
                return Compare(a, new[] { b });
            }

            return Compare(new[] { a }, b);
        }
    }
}
