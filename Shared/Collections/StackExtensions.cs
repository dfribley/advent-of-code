namespace AoC.Shared.Collections;

public static class StackExtensions
{
    public static void Add<TValue>(this Stack<TValue> stack, TValue value)
    {
        stack.Push(value);
    }

    public static void Add<TValue>(this Stack<TValue> stack, TValue[] array)
    {
        foreach (var value in array)
        {
            stack.Push(value);
        }
    }
}
