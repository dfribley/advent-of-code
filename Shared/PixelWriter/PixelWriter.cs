namespace AoC.Shared.PixelWriter;

public class PixelWriter
{
    private readonly int _width;

    public PixelWriter(int width)
    {
        _width = width;
    }

    public int LineNumber { get; private set; } = 0;
    public int ColumnNumber { get; private set; } = 0;

    public void Write(char pixel)
    {
        Console.Write(pixel);

        if (++ColumnNumber == _width)
        {
            Console.WriteLine();
            ColumnNumber = 0;
        }
    }

    public void Write(Grid.Grid grid)
    {
        for (int y = grid.MaxY; y >= 0; y--)
        {
            for (int x = 0; x <= grid.MaxX; x++)
            {
                Write(grid[x, y]);
            }
        }
    }
}
