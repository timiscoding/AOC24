using System.Numerics;
using System.Text.RegularExpressions;
using AOC24.Utils;

using RobotConfig = (System.Numerics.Vector2 pos, System.Numerics.Vector2 vel);

namespace AOC24.Solutions;

public record struct Robot(Vector2 Pos, Vector2 Vel) { }

public static class Day14
{
    public static readonly int RoomWidth = 101;
    public static readonly int RoomHeight = 103;
    private const int MaxTime = 10_403; // at 101 x 103 seconds, the robots will be at same position as time 0   
    public static Vector2[] Neighbours =
    [
        new(0, -1),
        new(1, -1),
        new(1, 0),
        new(1, 1),
        new(0, 1),
        new(-1, 1),
        new(-1, 0),
        new(-1, -1),
    ];
    public static void Solve()
    {
        var robots = GetRobots(InputReader.GetLines("Day14.txt"));
        MoveRobots(robots, 100);
        var factor = SafetyFactor(robots);
        Console.WriteLine($"Day 14 Part 1 - safety factor: {factor}");
        var map2 = MoveRobots(robots, -100); // reset robots back to original positions
        var timeSeconds = FindXmasTreePattern(robots);
        Console.WriteLine($"Day 14 Part 2 - robots at: {timeSeconds}s form a christmas tree pattern");
    }

    public static int FindXmasTreePattern(List<Robot> robots)
    {
        int timeSeconds = 1;
        var map = MoveRobots(robots, 0);
        int candidates = 0;
        int tree = 0;
        while (timeSeconds < MaxTime)
        {
            map = MoveRobots(robots, 1);
            // started with a `count` of 5 and kept increasing it until there were single digit candidates 
            if (HasNeighbours(map, 50)) 
            {
                candidates++;
                tree = timeSeconds;
                PrintMap(map);
            }
            timeSeconds++;
        }
        Console.WriteLine($"candidates: {candidates}");
        return tree;
    }
    
    /* Looks for `count` robots in a line in the room and returns true if any were found.
     * 
     * In the problem statement, we were only told the robots at a certain time formed a picture of a xmas tree.
     * I assumed there would be a straight line somewhere in the room so the idea was to search for one while
     * increasing the line length to narrow down the possibilities from MaxTime to a few candidates.
     */
    public static bool HasNeighbours(HashSet<Vector2> map, int count)
    {
        foreach (var pos in map)
        {
            var neighbours = 0;
            var seen = new HashSet<Vector2>();
            var stack = new Stack<Vector2>();
            stack.Push(pos);
            while (stack.Count > 0)
            {
                var cur = stack.Pop();
                seen.Add(cur);
                foreach (var dir in Neighbours)
                {
                    if (!map.Contains(cur + dir) || seen.Contains(cur + dir)) continue;
                    stack.Push(cur + dir);
                }
                neighbours++;
                if (neighbours == count) return true;
            }
        }
        return false;
    }
    
    public static int SafetyFactor(List<Robot> robots) =>
        GroupByQuadrants(robots).Aggregate(1, (product, quadrant) => product * quadrant.Value.Length);

    public static Dictionary<(int, int), Robot[]> GroupByQuadrants(List<Robot> robots)
    {
        var midWidth = RoomWidth / 2f;
        var midHeight = RoomHeight / 2f;
        return (from robot in robots
                let x = (int)robot.Pos.X
                let y = (int)robot.Pos.Y
                where x != (int)Math.Floor(midWidth) && y != (int)Math.Floor(midHeight)
                let quadrant = ((int)Math.Floor(x / midWidth), (int)Math.Floor(y / midHeight))
                group robot by quadrant into g
                select KeyValuePair.Create(g.Key, g.ToArray())).ToDictionary();
    }

    public static HashSet<Vector2> MoveRobots(List<Robot> robots, int timeSeconds)
    {
        var robotMap = new HashSet<Vector2>();
        for (var i = 0; i < robots.Count; i++)
        {
            var robot = robots[i];
            var newPos = robot.Pos + robot.Vel * timeSeconds;
            var x = (float)Mod(newPos.X, RoomWidth);
            var y = (float)Mod(newPos.Y, RoomHeight);
            robots[i] = robot with { Pos = new Vector2(x, y) };
            robotMap.Add(robots[i].Pos);
        }
        return robotMap;
    }

    public static void Print(List<Robot> robots)
    {
        for (var y = 0; y < RoomHeight; y++)
        {
            for (var x = 0; x < RoomWidth; x++)
            {
                var onTile = robots.Count(r => r.Pos.X == x && r.Pos.Y == y);
                Console.Write(onTile > 0 ? onTile : ".");
            }

            Console.WriteLine();
        }
    }

    public static void PrintMap(HashSet<Vector2> map)
    {
        Console.WriteLine(" " + string.Concat(Enumerable.Range(0, RoomWidth).Select(i => i % 10)));
        for (var y = 0; y < RoomHeight; y++)
        {
            Console.Write(y % 10);
            for (var x = 0; x < RoomWidth; x++)
            {
                Console.Write(map.Contains(new Vector2(x, y)) ? "1" : ".");
            }
            Console.WriteLine();
        }
    }

    public static List<Robot> GetRobots(string[] lines)
    {
        var a = lines.SelectMany(line => Regex.Matches(line, @"-?\d+").Select(m => int.Parse(m.Value)).Chunk(4).Select((p, i) => new Robot(new Vector2(p[0], p[1]), new Vector2(p[2], p[3]))));
        return a.ToList();
    }
    
    /* in C#, modulo of the type -dividend % divisor doesn't give the value we want. eg. -5 % 7 = -5
     * What is required is for negative dividends to wrap around so they are always in the range [0,6]
     * eg. -5 % 7 = 2 which is what this custom implementation does.
     */
    public static double Mod(double dividend, double divisor)
    {
        return (dividend > 0, divisor > 0) switch
        {
            (true, true) => dividend % divisor,
            (false, true) => dividend - divisor * Math.Floor(dividend / divisor), // C# modulo does Math.Truncate instead
            _ => throw new InvalidOperationException("Invalid sign for divisor")
        };
    }
}