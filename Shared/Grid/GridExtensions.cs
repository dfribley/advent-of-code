namespace AoC.Shared.Grid;

public static class GridExtensions
{
    public static Grid ToGrid(this IEnumerable<string> rows)
    {
        return new Grid(rows);
    }
}
