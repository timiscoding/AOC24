using AOC24.Utils;

namespace AOC24.Solutions;

public class Guard
{
    private static readonly Heading[] Dirs = [ Heading.Up, Heading.Right, Heading.Down, Heading.Left ];
    private readonly char[,] _map;
    private int MaxX => _map.GetLength(1) - 1;
    private int MaxY => _map.GetLength(0) - 1;

    public Heading Dir { get; private set; } = Heading.Up;
    public Point Pos { get; private set; }
    public Point Origin { get; }

    public Guard(char[,] map)
    {
        _map = map;
        Pos = FindGuard();
        Origin = Pos;
    }
    
    private Point FindGuard()
    {
        for (var y = 0; y < _map.GetLength(0); y++)
        {
            for (var x = 0; x < _map.GetLength(1); x++)
            {
                if (_map[y, x] == '^')
                {
                    return new Point(x, y);
                }
            }
        }
        throw new InvalidOperationException("No guard found");
    }
    private void ChangeDirection()
    {
        Dir = Dirs[(Array.IndexOf(Dirs, Dir) + 1) % Dirs.Length];
    }

    public HashSet<Point> GetPath()
    {
        var visited = new HashSet<Point> { Pos };
        while (true)
        {
            Point newPos = new Point(Pos.X + Dir.X, Pos.Y + Dir.Y);
            if (!WithinMap(newPos))
            {
                break;
            } 
            if (!IsObstacle(newPos))
            {
                Pos = newPos;
                visited.Add(Pos);
            }
            else
            {
                ChangeDirection();
            }
        }
        ResetOrigin();
        return visited;
    }

    public bool IsStuck(Point newObstacle)
    {
        _map[newObstacle.Y, newObstacle.X] = '#';
        var visited = new HashSet<(Point, Heading)>();
        bool isStuck = false;
       
        while (true)
        {
            Point newPos = new Point(Pos.X + Dir.X, Pos.Y + Dir.Y);
            if (!WithinMap(newPos))
            {
                break;
            } 
            if (!IsObstacle(newPos))
            {
                Pos = newPos;
                if (visited.Add((Pos, Dir))) continue;
                isStuck = true;
                break;
            }
            ChangeDirection();
        }
        
        _map[newObstacle.Y, newObstacle.X] = '.';
        ResetOrigin();
        return isStuck;
    }

    private void ResetOrigin()
    {
        Pos = Origin;
        Dir = Heading.Up;
    }

    private bool IsObstacle(Point p) =>
        _map[p.Y, p.X] == '#';
    
    private bool WithinMap(Point p)
        => p.X >= 0 && p.X <= MaxX && p.Y >= 0 && p.Y <= MaxY;
}

public static class Day06
{
    public static void Solve()
    {
        var map = InputReader.Get2DArray("Day06.txt");
        var guard = new Guard (map);
        var visited = guard.GetPath();
        Console.WriteLine($"Part 1 - distinct visited positions: {visited.Count}");

        var obstacles = new HashSet<Point>();
        visited.Remove(guard.Origin);
        foreach (var newObstacle in visited) 
        {
            if (guard.IsStuck(newObstacle))
            {
                obstacles.Add(newObstacle);
            }
        }
        Console.WriteLine($"Part 2 - new obstacle options: {obstacles.Count}");
    }
}