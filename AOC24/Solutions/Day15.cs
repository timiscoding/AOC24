using System.Numerics;
using AOC24.Utils;
using WarehouseMap = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
using Moves = System.Collections.Generic.List<System.Numerics.Complex>;

namespace AOC24.Solutions;

public record struct Warehouse(WarehouseMap Map, int Width, int Height, bool DoubleWidth) { }

public static class Day15
{
    public static Complex Down = -Complex.ImaginaryOne;
    public static Complex Up = Complex.ImaginaryOne;
    public static Complex Left = -1;
    public static Complex Right = 1;
    public const char WallTile = '#';
    public const char BoxTile = 'O';
    public const char Box2LeftTile = '[';
    public const char Box2RightTile = ']';
    public static readonly char[] Box2Tiles = [Box2LeftTile, Box2RightTile];
    public const char SpaceTile = '.';
    public const char RobotTile = '@';
    
    public static void Solve()
    {
        var input = InputReader.GetText("Day15.txt");
        Console.WriteLine($"Day 15 Part 1 - Box GPS sum {Part1(input)}");
        Console.WriteLine($"Day 15 Part 2 - Box GPS sum {Part2(input)}");
    }

    public static double Part1(string input)
    {
        var (warehouse, moves) = ParseInput(input);
        var pos = FindRobot(warehouse);
        foreach (var move in moves)
            pos = Move(warehouse, pos, move);
        Print(warehouse);
        return warehouse.Map.Sum(tile => tile.Value == BoxTile ? 100 * Math.Abs(tile.Key.Imaginary) + tile.Key.Real : 0);
    }
    
    public static double Part2(string input)
    {
        var (warehouse, moves) = ParseInput(input, doubleWidth: true);
        var pos = FindRobot(warehouse);
        foreach (var move in moves)
            pos = Move(warehouse, pos, move);
        Print(warehouse);
        return warehouse.Map.Sum(tile => tile.Value == Box2LeftTile ? 100 * Math.Abs(tile.Key.Imaginary) + tile.Key.Real : 0);
    }

    public static Complex MoveRobot(Warehouse w, Moves moves, Complex startPos)
    {
        var pos = startPos;
        foreach (var move in moves)
        {
            pos = Move(w, pos, move);
        }
        return pos;
    }

    public static Complex Move(Warehouse w, Complex from, Complex dir) =>
        w.DoubleWidth ? MoveDoubleWidthTile(w.Map, from, dir) : MoveSingleWidthTile(w.Map, from, dir);

    public static Complex MoveSingleWidthTile(WarehouseMap map, Complex from, Complex dir)
    {
        bool seenBox = false;
        var curPos = from + dir;
        while (map[curPos] == BoxTile)
        {
            seenBox = true;
            curPos += dir;
        }
        if (map[curPos] == WallTile) return from;
        map[from] = SpaceTile;
        map[from + dir] = RobotTile;
        if (seenBox) map[curPos] = BoxTile;
        return from + dir;
    }

    public static Complex MoveDoubleWidthTile(WarehouseMap map, Complex from, Complex dir) =>
        dir == Left || dir == Right ? MoveHorizontal(map, from, dir) : MoveVertical(map, from, dir);
    
    /* Moves the robot at coord 'from' to coord 'dir' (Left or Right) and returns the new lcoation.
     * This method applies only for the double width warehouse.
     *
     * Implementation is similar to MoveSingleWidthTile except when there boxes found. If moving is possible then
     * starting with the space tile, adjacent tile pairs are swapped until it reaches the robot. This effectively moves
     * all tiles in the direction desired.
     */
    public static Complex MoveHorizontal(WarehouseMap map, Complex from, Complex dir)
    {
        if (!(dir == Left || dir == Right)) throw new ArgumentOutOfRangeException($"Invalid dir: {dir}");
        var curPos = from + dir;
        while (Box2Tiles.Contains(map[curPos])) curPos += dir;
        if (map[curPos] == WallTile) return from;
        var revDir = ReverseDir(dir);
        while (curPos != from)
        {
            (map[curPos], map[curPos + revDir])  = (map[curPos + revDir], map[curPos]);
            curPos += revDir;
        }
        return from + dir;
    }

    /* Moves a robot located at 'from' vertically given by the coordinate 'dir' (either Up or Down) and returns their new
     * location. This method applies only to the double width warehouse. 
     *
     * When there's a box in front of the robot, it looks at every point in front of it and checks 2 things.
     * 1. If there's a wall, robot can't move
     * 2. If there's another box, add it to the queue
     * These steps are repeated until there are no more boxes found at which point, the boxes are all shifted up along
     * with the robot.
     */ 
    public static Complex MoveVertical(WarehouseMap map, Complex from, Complex dir)
    {
        if (!(dir == Up || dir == Down)) throw new ArgumentOutOfRangeException($"Invalid dir: {dir}");
        var curPos = from + dir;
        var tile = map[curPos];
        if (tile == WallTile) return from;
        if (tile == SpaceTile)
        {
            (map[curPos], map[from]) = (RobotTile, SpaceTile);
            return curPos;
        }

        TryGetBox2(map, curPos, out var box); 
        var q = new Queue<Complex>(box);
        var boxes = new HashSet<Complex>(box);
        while (q.Count > 0)
        {
            curPos = q.Dequeue();
            if (map[curPos + dir] == WallTile) return from;
            if (!TryGetBox2(map, curPos + dir, out box)) continue;
            foreach (var pos in box)
            {
                if (!boxes.Add(pos)) continue;
                q.Enqueue(pos);
            }
        }
        
        /* shifting boxes works a bit like bubble sort in that the space above the boxes bubbles down as every box shifts
          up. The boxes order needs to be reversed since the furthest boxes from the robot need to swapped with empty 
          space first */
        foreach (var pos in boxes.Reverse()) 
        {
            (map[pos], map[pos + dir]) = (map[pos + dir], map[pos]);
        }
        map[from] = SpaceTile;
        map[from + dir] = RobotTile;
        
        return from + dir;
    }

    private static bool TryGetBox2(WarehouseMap map, Complex pos, out Complex[] tiles)
    {
        var tile = map[pos];
        if (!Box2Tiles.Contains(tile)) tiles = [];
        else tiles = [pos, pos + (tile == Box2LeftTile ? Right : Left)];
        return tiles.Length != 0;
    }

    private static Complex ReverseDir(Complex dir) =>
        dir switch
        {
            _ when dir == Left => Right,
            _ when dir == Right => Left,
            _ when dir == Up => Down,
            _ when dir == Down => Up,
            _ => throw new ArgumentOutOfRangeException($"Invalid direction: {dir}")
        };

    private static Complex FindRobot(Warehouse w) =>
        w.Map.Single(p => p.Value == RobotTile).Key;

    private static void Print(Warehouse w, string title = "")
    {
        Console.WriteLine();
        if (title.Length > 0) Console.WriteLine(title);
        Console.WriteLine(" " + string.Concat(Enumerable.Range(0, w.Width).Select(i => i % 10)));
        Console.WriteLine(string.Join("\n", AsString(w).Split("\n").Select((line, y) => $"{y % 10}{line}")));
    }

    public static string AsString(Warehouse w)
    {
        string res = "";
        for (var y = 0; y < w.Height; y++)
        {
            for (var x = 0; x < w.Width; x++)
            {
                res += w.Map[x + y * Down];
            }
            res += "\n";
        }
        return res.TrimEnd();
    }

    public static (Warehouse, Moves) ParseInput(string input, bool doubleWidth = false)
    {
        var parts = input.Split("\n\n");
        var warehouse = GetMap(parts[0], doubleWidth);
        var moves = GetMoves(parts[1]);
        return (warehouse, moves);
    }
    public static Warehouse GetMap(string input, bool doubleWidth = false)
    {
        var lines = input.Split('\n');
        var width = lines[0].Length;
        var height = lines.Length;
        var query = 
            from Y in Enumerable.Range(0, height)
            from X in Enumerable.Range(0, width)
            let Char = lines[Y][X]
            select new { X, Y, Char };

        if (!doubleWidth)
            return new Warehouse(
                Map: query.Select(v => (v.X + v.Y * Down, v.Char)).ToDictionary(),
                Width: width,
                Height: height,
                DoubleWidth: false);
        
        var map = query.SelectMany(v =>
        {
            // Map original x=0 to p=0 & p2=1, then map x=1 -> p=2 & p2=3 etc
            var p = 2 * v.X + v.Y * Down;   
            var p2 = 2 * v.X + 1 + v.Y * Down;
            return v.Char switch
            {
                WallTile => new[] { (p, WallTile), (p2, WallTile) },
                BoxTile => new[] { (p, Box2LeftTile), (p2, Box2RightTile) },
                RobotTile => new[] { (p, RobotTile), (p2, SpaceTile) },
                SpaceTile => new[] { (p, SpaceTile), (p2, SpaceTile) },
                _ => throw new ArgumentOutOfRangeException($"Invalid char: {v.Char}")
            };
        }).ToDictionary();
        return new Warehouse(Map: map, Width: 2 * width, Height: height, DoubleWidth: true);
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