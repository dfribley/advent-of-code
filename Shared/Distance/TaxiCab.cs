using System.Drawing;

namespace Shared.Distance
{
    public static class TaxiCab
    {
        public static int GetDistance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        public static IEnumerable<Point> GetPerimeterPoints(Point a, int x, int distance)
        {
            var deltaY = distance - Math.Abs(a.X - x);

            yield return new Point(x, a.Y + deltaY);
            yield return new Point(x, a.Y - deltaY);
        }
    }
}
