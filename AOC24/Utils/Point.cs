namespace AOC24.Utils;

public interface IPoint
{
    int X { get; }
    int Y { get; }
}

public record struct Point(int X, int Y) : IPoint
{
    public Point((int x, int y) p) : this(p.x, p.y) { }
}

public record struct Heading(int X, int Y) : IPoint
{
    public static readonly Heading Up = new (0, -1);
    public static readonly Heading Right = new (1, 0);
    public static readonly Heading Down = new (0, 1);
    public static readonly Heading Left = new (-1, 0);
}

public record struct Bounds(int MinX, int MaxX, int MinY, int MaxY) { }