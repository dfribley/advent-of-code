using System.Numerics;

namespace AoC.Shared.PixelWriter;

public static class PixelWriterHelper
{
    public static void WriteCollection(IEnumerable<Vector2> values)
    {
        if (!values.Any())
        {
            return;
        }

        var xmin = (int)values.First().X;
        var ymin = (int)values.First().Y;
        var xmax = xmin;
        var ymax = ymin;

        foreach (var v in values)
        {
            xmin = Math.Min(xmin, (int)v.X);
            xmax = Math.Max(xmax, (int)v.X);
            ymin = Math.Min(ymin, (int)v.Y);
            ymax = Math.Max(ymax, (int)v.Y);
        }
        
        var pixelWriter = new PixelWriter((xmax + 1) - xmin);

        for (var y = ymin; y <= ymax; y++)
        {
            for (var x = xmin; x <= xmax; x++)
            {
                var point = new Vector2(x, y);

                if (values.Contains(point))
                {
                    pixelWriter.Write('#');
                }
                else
                {
                    pixelWriter.Write(' ');
                }
            }
        }
    }
}
