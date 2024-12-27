using System.Numerics;
using AOC24.Utils;

using Warehouse = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
using Moves = System.Collections.Generic.List<System.Numerics.Complex>;

namespace AOC24.Solutions;

public static class Day15
{
    private static int _width;
    private static int _height;
    public static Complex Down = -Complex.ImaginaryOne;
    public static Complex Up = Complex.ImaginaryOne;
    public static Complex Left = -1;
    public static Complex Right = 1;
    public const char WallTile = '#';
    public const char BoxTile = 'O';
    public const char SpaceTile = '.';
    public const char RobotTile = '@';
    public static void Solve()
    {
        var input = InputReader.GetText("Day15.txt");
        var (map, moves) = ParseInput(input);
        var pos = FindRobot(map);
        Print(map, "Initial state");
        MoveRobot(map, moves, pos);
        Print(map, "After moves");
        Console.WriteLine($"Day 15 Part 1 - Box GPS sum {Part1(map)}");
    }

    public static double Part1(Warehouse map) =>
        map.Sum(tile => tile.Value == BoxTile ? 100 * Math.Abs(tile.Key.Imaginary) + tile.Key.Real : 0);

    public static Complex MoveRobot(Warehouse map, Moves moves, Complex startPos)
    {
        var pos = startPos;
        foreach (var move in moves)
        {
            pos = Move(map, pos, move);
        }
        return pos;
    }

    public static Complex Move(Warehouse map, Complex from, Complex dir)
    {
        bool seenBox = false;
        var curPos = from + dir;
        while (map.GetValueOrDefault(curPos) == BoxTile)
        {
            seenBox = true;
            curPos += dir;
        }

        if (map.GetValueOrDefault(curPos) == WallTile) return from;
        map[from] = SpaceTile;
        map[from + dir] = RobotTile;
        if (seenBox) map[curPos] = BoxTile;
        return from + dir;
    }

    private static Complex FindRobot(Warehouse map)
    {
        return map.Single(p => p.Value == RobotTile).Key;
    }

    private static void Print(Warehouse map, string title = "")
    {
        Console.WriteLine();
        if (title.Length > 0) Console.WriteLine(title);
        Console.WriteLine(" " + string.Concat(Enumerable.Range(0, _width).Select(i => i % 10)));
        for (var y = 0; y < _height; y++)
        {
            Console.Write(y % 10);
            for (var x = 0; x < _width; x++)
            {
                Console.Write(map.GetValueOrDefault(x + y * Down, '.'));
            }
            Console.WriteLine();
        }
    }

    public static (Warehouse, Moves) ParseInput(string input)
    {
        var parts = input.Split("\n\n");
        var map = GetMap(parts[0]);
        var moves = GetMoves(parts[1]);
        return (map, moves);
    }
    public static Warehouse GetMap(string input)
    {
        var lines = input.Split('\n');
        _width = lines[0].Length;
        _height = lines.Length;
        return (from y in Enumerable.Range(0, _height)
            from x in Enumerable.Range(0, _width)
            let c = lines[y][x]
            where c != '.'
            select KeyValuePair.Create(x + y * Down, c)).ToDictionary();
    }

    public static List<Complex> GetMoves(string input)
    {
        return input.Replace("\n", "").Select(c => c switch
        {
            '<' => Left,
            '>' => Right,
            '^' => Up,
            'v' => Down,
            _ => throw new ArgumentOutOfRangeException($"Invalid move found: {c}")
        }).ToList();
    }
}