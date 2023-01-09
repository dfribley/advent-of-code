using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Lines
{
    public class Line
    {
        public Point PointA { get; init; }
        public Point PointB { get; init; }

        public int DeltaX => PointB.X - PointA.X;
        public int DeltaY => PointB.Y - PointA.Y;
        public float Slope => DeltaY / DeltaX;
    }
}
