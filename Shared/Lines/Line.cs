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
}
