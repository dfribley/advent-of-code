namespace AoC.Shared.Enumerable;

public class Wrapable<T>
{
	private int _position;
	private readonly T[] _values;

	public Wrapable(IEnumerable<T> values)
	{
		_values = values.ToArray();
		_position = 0;
	}

	public Direction Direction { get; init; } = Direction.Forward;

	public T Current => _values[_position];

	public void SetIndex(int index) => _position = index;

	public T Next()
	{
        _position += Direction == Direction.Forward ? 1 : -1;

        if (_position < 0)
        {
            _position = _values.Length - 1;
        }
        else if (_position >= _values.Length)
        {
            _position = 0;
        }

        return Current;
    }
}

public enum Direction
{
	Forward,
	Backward
}
