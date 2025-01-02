using System.Drawing;

namespace AoC.Shared.Grid;

public static class GridDirections
{
    public static Point North = new(0, 1);
    public static Point NorthEast = new(1, 1);
    public static Point East = new(1, 0);
    public static Point SouthEast = new(1, -1);
    public static Point South = new(0, -1);
    public static Point SouthWest = new(-1, -1);
    public static Point West = new(-1, 0);
    public static Point NorthWest = new(-1, 1);

    public static IEnumerable<Point> Neighbors = new[] { North, NorthEast, East, SouthEast, South, SouthWest, West, NorthWest };
    public static IEnumerable<Point> SideNeighbors = new[] { North, East, South, West };
    public static IEnumerable<Point> DiagonalNeighbors = new[] { NorthEast, SouthEast, SouthWest, NorthWest };

    public static IEnumerable<Point> WestNeighbors = new[] { NorthWest, West, SouthWest };
    public static IEnumerable<Point> NorthNeighbors = new[] { NorthWest, North, NorthEast };
    public static IEnumerable<Point> EastNeighbors = new[] { NorthEast, East, SouthEast };
    public static IEnumerable<Point> SouthNeighbors = new[] { SouthWest, South, SouthEast };
    
    public static IEnumerable<Point> Vertical = new[] { North, South };
    public static IEnumerable<Point> Horizontal = new[] { East, West };
}
