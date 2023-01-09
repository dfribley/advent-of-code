namespace AoC.Day13
{
    internal static class PacketParser
    {
        internal static IList<object> ToPacket(this string source)
        {
            return ParseList(source, out int _);
        }

        private static IList<object> ParseList(string source, out int chars)
        {
            var list = new List<object>();
            var i = 1;

            while (source[i] != ']')
            {
                if (source[i] == '[')
                {
                    list.Add(ParseList(source[i..], out int processed));
                    i += processed;
                }
                else if (source[i] == ',')
                {
                    i++;
                }
                else
                {
                    var val = source[i..source.IndexOfAny(new[] { ',', ']' }, i)];
                    list.Add(Convert.ToInt32(val));
                    i += val.Length;
                }
            }

            chars = i + 1;
            return list;
        }
    }
}
