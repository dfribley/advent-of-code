namespace AoC.Shared.Looping;

public static class IntExtensions
{
    public static void Loop(this int count, Action<int> action)
    {
        for (var i = 0; i < count; i++)
        {
            action(i);
        }
    }
}
