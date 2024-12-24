using System.Drawing;

namespace AoC.Shared.Points;

public static class PointExtensions
{
    public static Point Add(this Point point1, Point point2)
    {
        return new Point(point1.X + point2.X, point1.Y + point2.Y);
    }
    
    public static Point Subtract(this Point point1, Point point2)
    {
        return new Point(point1.X - point2.X, point1.Y - point2.Y);
    }

    public static Point AddX(this Point point, int x)
    {
        return new Point(point.X + x, point.Y);
    }

    public static Point AddY(this Point point, int y)
    {
        return new Point(point.X, point.Y + y);
    }
    
    public static Point Multiply(this Point point, int value)
    {
        return new Point(point.X * value, point.Y * value);
    }
    
    public static Point RotateRight(this Point point, int degrees)
    {
        var radians = degrees % 360 * Math.PI / 180;
        var x = (int)Math.Round(point.X * Math.Cos(radians) + point.Y * Math.Sin(radians));
        var y = (int)Math.Round(-point.X * Math.Sin(radians) + point.Y * Math.Cos(radians));
        return new Point(x, y);
    }
    
    public static Point RotateRight(this Point point, Point around, int degrees)
    {
        var radians = degrees % 360 * (Math.PI / 180);
        var translatedX = point.X - around.X;
        var translatedY = point.Y - around.Y;
        var x = (int)Math.Round(translatedX * Math.Cos(radians) + translatedY * Math.Sin(radians)) + around.X;
        var y = (int)Math.Round(-translatedX * Math.Sin(radians) + translatedY * Math.Cos(radians)) + around.Y;
        return new Point(x, y);
    }
    
    public static Point RotateLeft(this Point point, int degrees)
    {
        return RotateRight(point, 360 - degrees % 360);
    }
    
    public static Point RotateLeft(this Point point, Point around, int degrees)
    {
        return RotateRight(point, around, 360 - degrees % 360);
    }
}
