public readonly struct Part
{
    public int X { get; init; }

    public int M { get; init; }

    public int A { get; init; }

    public int S { get; init; }

    public Part(int x, int m, int a, int s)
    {
        X = x;
        M = m;
        A = a;
        S = s;
    }

    public int GetValue(char category)
    {
        return category switch
        {
            'x' => X,
            'm' => M,
            'a' => A,
            _ => S
        };
    }
}
