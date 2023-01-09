using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Lines
{
    public static class LineExtensions
    {
        public static bool IsHorizontal(this Line line)
        {
            return line.PointA.Y == line.PointB.Y;
        }

        public static bool IsVertical(this Line line)
        {
            return line.PointA.X == line.PointB.X;
        }

        public static bool IsDiaganol(this Line line)
        {
            return !line.IsVertical() && Math.Abs(line.DeltaY / line.DeltaX) == 1;
        }

        public static IEnumerable<Point> GetPoints(this Line line)
        {
            if (line.IsVertical() || line.IsHorizontal() || line.IsDiaganol())
            {
                var points = new List<Point>();
                var xChange = line.DeltaX == 0 ? 0 : line.DeltaX / Math.Abs(line.DeltaX);
                var yChange = line.DeltaY == 0 ? 0 : line.DeltaY / Math.Abs(line.DeltaY);
                var point = line.PointA;

                do
                {
                    points.Add(point);
                    point = new Point(point.X + xChange, point.Y + yChange);
                } while (point != line.PointB);

                points.Add(line.PointB);

                return points;
            }

            throw new NotImplementedException();
        }
    }
}
