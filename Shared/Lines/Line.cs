using System.Drawing;

namespace AoC.Shared.Lines;

public class Line
{
    public Point PointA { get; init; }
    public Point PointB { get; init; }

    public int DeltaX => PointB.X - PointA.X;
    public int DeltaY => PointB.Y - PointA.Y;
    public float Slope => DeltaY / DeltaX;
}
