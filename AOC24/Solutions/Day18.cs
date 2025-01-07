using System.Numerics;
using AOC24.Utils;
namespace AOC24.Solutions;

public static class Day18
{
    public static int Width = 71;
    public static int Height = 71;
    public static int Corrupted = 1024;
    public static Complex Left = -1;
    public static Complex Right = 1;
    public static Complex Up = Complex.ImaginaryOne;
    public static Complex Down = -Complex.ImaginaryOne;
    
    public static void Solve()
    {
        var bytes = GetBytes(InputReader.GetLines("Day18.txt"));
        Console.WriteLine($"Day 18 - Part 1 Shortest path: {ShortestPath(bytes.Take(Corrupted).ToHashSet())}");
        Console.WriteLine($"Day 18 - Part 2 First byte with unreachable path: {FindSplitGraph(bytes.ToList(), Corrupted)}");
    }

    public static Complex FindSplitGraph(List<Complex> bytes, int corrupted)
    {
        while (ShortestPath(bytes.Take(corrupted).ToHashSet()) != -1) corrupted++;
        return bytes[corrupted - 1];
    }

    public static int ShortestPath(HashSet<Complex> bytes)
    {
        var start = Complex.Zero;
        var end = (Width - 1) + (Height - 1) * Down;
        var dist = new Dictionary<Complex, int>();

        var q = new Queue<Complex>();
        var visited = new HashSet<Complex>();
        q.Enqueue(start);
        dist[start] = 0;
        while (q.Count > 0)
        {
            var pos = q.Dequeue();
            if (pos == end)
            {
                return dist[end];
            }
            visited.Add(pos);
            foreach (var dir in new[] { Up, Down, Left, Right })
            {
                if (visited.Contains(pos + dir) || !InsideGrid(pos + dir) || bytes.Contains(pos + dir)) continue;
                if (dist.GetValueOrDefault(pos + dir, int.MaxValue) > dist[pos] + 1)
                {
                    dist[pos + dir] = dist[pos] + 1;
                    q.Enqueue(pos + dir);
                }
            }
        }
        
        return -1;
        
        bool InsideGrid(Complex pos) => pos.Real >= 0 && pos.Imaginary <= 0 && pos.Real < Width && pos.Imaginary > -Height;
    }

    public static void PrintGrid(HashSet<Complex> bytes)
    {
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                Console.Write(bytes.Contains(x + y * Down) ? "#" : ".");
            }
            Console.WriteLine();
        }
    }

    public static IEnumerable<Complex> GetBytes(string[] lines) =>
        lines.Select(l =>
        {
            var nums = l.Split(',');
            return new Complex(int.Parse(nums[0]), -int.Parse(nums[1]));
        });
}