using System.Drawing;
using AoC.Shared.Points;

namespace AoC.Shared.Grid;

public class Grid
{
	private readonly string[] grid;
	private readonly Point[] neighbors = new[] { new Point(-1, 0), new Point(-1, -1), new Point(0, -1), new Point(1, -1), new Point(1, 0), new Point(1, 1), new Point(0, 1), new Point(-1, 1) };

    public Grid(IEnumerable<string> rows)
	{
		grid = rows.Reverse().ToArray();
	}

	public int MaxX { get => grid[0].Length - 1; }

	public int MaxY { get => grid.Length - 1; }

	public bool WrapNorth { get; set; }

	public bool WrapEast { get; set; }

	public bool WrapSouth { get; set; }

	public bool WrapWest { get; set; }

	public Point TopLeft { get => new(0, MaxY); }

	public char this[Point point]
	{
		get => GetCharAt(point.X, point.Y);
	}

	public bool IsValid(Point point)
	{
		return (point.X >= 0 || WrapWest) &&
			   (point.X <= MaxX || WrapEast) &&
			   (point.Y >= 0 || WrapSouth) &&
			   (point.Y <= MaxY || WrapNorth);
    }

	public IEnumerable<(Point coordinate, char value)> Neighbors(Point coordinate)
	{
		return neighbors
			.Select(np => coordinate.Add(np))
			.Where(IsValid)
			.Select(np => (np, this[np]));
	}

	private char GetCharAt(int x, int y)
	{
		if (x > MaxX && WrapEast)
		{
			x %= grid[0].Length;
		}

		if (x < 0 && WrapWest)
		{
			y += x % grid[0].Length;
		}

		if (y > MaxY && WrapNorth)
		{
			y %= grid.Length;
		}

		if (y < 0 && WrapSouth)
		{
			y += y % grid.Length;
		}

		return grid[y][x];
	}
}
