using System.Drawing;

namespace AoC.Shared.Lines;

public class Line
{
    public Line()
    {
    }

    public Line(Point pointA, Point pointB)
    {
        PointA = pointA;
        PointB = pointB;
    }

    public Point PointA { get; set; }
    public Point PointB { get; set; }

    public int DeltaX => PointB.X - PointA.X;
    public int DeltaY => PointB.Y - PointA.Y;

    public Point ABVector => new(PointB.X - PointA.X, PointB.Y - PointA.Y);
    public Point BAVector => new(PointA.X - PointB.X, PointA.Y - PointB.Y);

    public float Slope => DeltaY / DeltaX;

    public IEnumerable<Point> Intersects(Line line)
    {
        return line.GetPoints().Intersect(this.GetPoints()).ToList();
    }

    public bool Hides(Point c)
    {
        var acVector = new Point(c.X - PointA.X, c.Y - PointA.Y);
        
        // Collinearity check (cross product)
        var cross = acVector.X * ABVector.Y - acVector.Y * ABVector.X;
        if (cross != 0)
        {
            return false;
        }
        
        // Same direction check
        var dot = acVector.X * ABVector.X + acVector.Y * ABVector.Y;
        if (dot <= 0)
        {
            return false;
        }
        
        // C is farther from A than B
        var distAc = acVector.X * ABVector.X + acVector.Y * ABVector.Y;
        var distAb = ABVector.X * ABVector.X + ABVector.Y * ABVector.Y;
        
        return distAc > distAb;
    }
    
    public List<Point> GetIntersectionPoints(Line other)
    {
        var points = new List<Point>();

        // Vertical lines
        if (DeltaX == 0 && other.DeltaX == 0 && PointA.X == other.PointA.X)
        {
            var y1 = Math.Min(PointA.Y, PointB.Y);
            var y2 = Math.Max(PointA.Y, PointB.Y);
            var y3 = Math.Min(other.PointA.Y, other.PointB.Y);
            var y4 = Math.Max(other.PointA.Y, other.PointB.Y);

            var overlapStart = Math.Max(y1, y3);
            var overlapEnd = Math.Min(y2, y4);

            for (var y = overlapStart; y <= overlapEnd; y++)
                points.Add(new Point(PointA.X, y));
        }
        // Horizontal lines
        else if (DeltaY == 0 && other.DeltaY == 0 && PointA.Y == other.PointA.Y)
        {
            var x1 = Math.Min(PointA.X, PointB.X);
            var x2 = Math.Max(PointA.X, PointB.X);
            var x3 = Math.Min(other.PointA.X, other.PointB.X);
            var x4 = Math.Max(other.PointA.X, other.PointB.X);

            var overlapStart = Math.Max(x1, x3);
            var overlapEnd = Math.Min(x2, x4);

            for (var x = overlapStart; x <= overlapEnd; x++)
                points.Add(new Point(x, PointA.Y));
        }
        else
        {
            // Check for intersection (for axis-aligned lines)
            if (DeltaX == 0 && other.DeltaY == 0)
            {
                // l1 vertical, l2 horizontal
                if (IsBetween(PointA.X, other.PointA.X, other.PointB.X) &&
                    IsBetween(other.PointA.Y, PointA.Y, PointB.Y))
                {
                    points.Add(new Point(PointA.X, other.PointA.Y));
                }
            }
            else if (DeltaY == 0 && other.DeltaX == 0)
            {
                // l1 horizontal, l2 vertical
                if (IsBetween(other.PointA.X, PointA.X, PointB.X) &&
                    IsBetween(PointA.Y, other.PointA.Y, other.PointB.Y))
                {
                    points.Add(new Point(other.PointA.X, PointA.Y));
                }
            }
        }

        return points;
    }

    private static bool IsBetween(int val, int start, int end)
    {
        return (val >= Math.Min(start, end) && val <= Math.Max(start, end));
    }
}
