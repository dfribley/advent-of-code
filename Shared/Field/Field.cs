using System.Drawing;

namespace AoC.Shared.Field;

public class Field
{
	private string[] field;

	public Field(IEnumerable<string> rows)
	{
		field = rows.Reverse().ToArray();
	}

	public int MaxX { get => field[0].Length - 1; }

	public int MaxY { get => field.Length - 1; }

	public bool WrapNorth { get; set; }

	public bool WrapEast { get; set; }

	public bool WrapSouth { get; set; }

	public bool WrapWest { get; set; }

	public Point TopLeft { get => new(0, MaxY); }

	public char? this[Point point]
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

	private char GetCharAt(int x, int y)
	{
		if (x > MaxX && WrapEast)
		{
			x %= field[0].Length;
		}

		if (x < 0 && WrapWest)
		{
			y += x % field[0].Length;
		}

		if (y > MaxY && WrapNorth)
		{
			y %= field.Length;
		}

		if (y < 0 && WrapSouth)
		{
			y += y % field.Length;
		}

		return field[y][x];
	}
}
