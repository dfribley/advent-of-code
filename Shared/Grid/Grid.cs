using System.Collections;
using System.Drawing;
using System.Text;
using AoC.Shared.Points;

namespace AoC.Shared.Grid;

public class Grid : IEnumerable<(Point coordinate, char value)>
{
	private readonly Point[] neighbors = new[] { new Point(-1, 0), new Point(-1, -1), new Point(0, -1), new Point(1, -1), new Point(1, 0), new Point(1, 1), new Point(0, 1), new Point(-1, 1) };

	private string[] _grid;

    public Grid(IEnumerable<string> rows)
	{
		_grid = rows.Reverse().ToArray();
	}

	public string[] rows => _grid;

	public int TotalX { get => _grid[0].Length; }

	public int TotalY { get => _grid.Length; }

	public int MaxX { get => _grid[0].Length - 1; }

	public int MaxY { get => _grid.Length - 1; }

	public bool WrapNorth { get; set; }

	public bool WrapEast { get; set; }

	public bool WrapSouth { get; set; }

	public bool WrapWest { get; set; }

	public Point TopLeft { get => new(0, MaxY); }

	public Point BottomLeft { get => new(0, 0); }

	public Point BottomRight { get => new(MaxX, 0); }

    public char this[Point point]
	{
		get => GetCharAt(point.X, point.Y);
        set => SetCharAt(point.X, point.Y, value);
    }

	public char this[int x, int y]
	{
		get => GetCharAt(x, y);
        set => SetCharAt(x, y, value);
    }

	public bool IsValid(Point point)
	{
		return (point.X >= 0 || WrapWest) &&
			   (point.X <= MaxX || WrapEast) &&
			   (point.Y >= 0 || WrapSouth) &&
			   (point.Y <= MaxY || WrapNorth);
    }

	[Obsolete("Use foreach/enumerator instead")]
	public IEnumerable<(Point coordinate, char value)> All()
    {
        for (int y = 0; y <= MaxY; y++)
        {
            for (int x = 0; x < _grid[0].Length; x++)
            {
                yield return (new Point(x, y), this[x, y]);
            }
        }
    }

	public IEnumerable<(Point coordinate, char value)> Neighbors(Point coordinate)
	{
		return neighbors
			.Select(np => coordinate.Add(np))
			.Where(IsValid)
			.Select(np => (np, this[np]));
	}
	
	public IEnumerable<(Point coordinate, char value)> NeighborsInSight(Point coordinate, Func<char, bool> canSeeThrough)
	{
		foreach (var direction in neighbors)
		{
			var np = coordinate.Add(direction);

			while (IsValid(np))
			{
				if (!canSeeThrough(this[np]))
				{
					yield return (np, this[np]);
					break;
				}
				
				np = np.Add(direction);
			}
		}
	}

	public IEnumerable<Point> Where(Func<char, bool> condition)
	{
        for (int y = 0; y <= MaxY; y++)
        {
            for (int x = 0; x < _grid[0].Length; x++)
            {
				if (condition(_grid[y][x]))
				{
					yield return new Point(x, y);
				}
            }
        }
    }

	public Point First(Func<char, bool> condition)
	{
        for (int y = 0; y <= MaxY; y++)
        {
            for (int x = 0; x < _grid[0].Length; x++)
            {
                if (condition(_grid[y][x]))
                {
                    return new Point(x, y);
                }
            }
        }

		throw new("Condition not found in grid");
    }


	public IEnumerable<int> GetXWhere(Func<IEnumerable<char>, bool> condition)
    {
        return System.Linq.Enumerable.Range(0, _grid[0].Length)
            .Where(col => condition(_grid.Select(row => row[col])))
            .ToArray();
    }

	public IEnumerable<int> GetYWhere(Func<IEnumerable<char>, bool> condition)
	{
        return System.Linq.Enumerable.Range(0, _grid.Length)
            .Where(row => condition(_grid[row]))
            .ToArray();
    }

    public Grid Rotate90()
    {
		_grid = System.Linq.Enumerable.Range(0, TotalX)
			.Select(x => new string(_grid.Select(y => y[x]).ToArray()))
			.Reverse()
			.ToArray();

		return this;
    }

    public Grid Rotate90Prime()
    {
        _grid = System.Linq.Enumerable.Range(0, TotalX)
            .Select(x => new string(_grid.Select(y => y[x]).Reverse().ToArray()))
            .ToArray();

		return this;
    }

	public Grid Clone()
	{
		return new Grid(((string[])_grid.Clone()).Reverse());
	}

	private void SetCharAt(int x, int y, char value)
	{
		var sb = new StringBuilder(_grid[y]);
		sb[x] = value;

		_grid[y] = sb.ToString();
    }

	public IEnumerator<(Point coordinate, char value)> GetEnumerator()
	{
		for (var y = MaxY; y >= 0; y--)
		{
			for (var x = 0; x <= MaxX; x++)
			{
				yield return (new Point(x, y), this[x, y]);
			}
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
	
	public void Print()
	{
		var pw = new PixelWriter.PixelWriter(MaxX + 1);
		
		foreach (var (coordinate, value) in this)
		{
			pw.Write(value);
		}
	}

	private char GetCharAt(int x, int y)
	{
		if (x > MaxX && WrapEast)
		{
			x %= _grid[0].Length;
		}

		if (x < 0 && WrapWest)
		{
			x = TotalX + ((x + 1) % TotalX) - 1;
		}

		if (y > MaxY && WrapNorth)
		{
			y %= _grid.Length;
		}

		if (y < 0 && WrapSouth)
		{
			y = TotalY + ((y + 1) % TotalY) - 1;
		}

		return _grid[y][x];
	}
}
