using System.Numerics;

namespace AoC.Shared.Distance;

public static class Euclidean
{
    public static double GetDistance(Vector3 a, Vector3 b)
    {
        return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
    }
}