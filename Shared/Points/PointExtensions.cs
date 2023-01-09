using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Shared.Points
{
    public static class PointExtensions
    {
        public static Point Add(this Point point1, Point point2)
        {
            return new Point(point1.X + point2.X, point1.Y + point2.Y);
        }

        public static Point AddX(this Point point, int x)
        {
            return new Point(point.X + x, point.Y);
        }

        public static Point AddY(this Point point, int y)
        {
            return new Point(point.X, point.Y + y);
        }
    }
}
