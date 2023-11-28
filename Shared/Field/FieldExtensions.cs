namespace AoC.Shared.Field;

public static class FieldExtensions
{
    public static Field ToField(this IEnumerable<string> rows)
    {
        return new Field(rows);
    }
}
