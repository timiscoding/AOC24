using AOC24.Utils;

namespace AOC24.Solutions;

public class Map
{
    private readonly int[,] _grid;
    public int GridHeight { get; }
    public int GridWidth { get; }
    
    public Map(string[] input)
    {
        GridHeight = input.Length;
        GridWidth = input[0].Length;
        _grid = new int[GridHeight, GridWidth];
        InitGrid(input);
    }
    
    public int GetHeight(Point p) => _grid[p.Y, p.X];

    public IEnumerable<Point> GetLocations(int height)
    {
        return
            from y in Enumerable.Range(0, GridHeight)
            from x in Enumerable.Range(0, GridWidth)
            let h = _grid[y, x]
            where h == height
            select new Point(x, y);
    }

    public IEnumerable<Point> GetNeighbours(Point pos)
    {
        Point[] neighbours =
        [
            pos with { Y = pos.Y - 1 },
            pos with { X = pos.X + 1 },
            pos with { Y = pos.Y + 1 },
            pos with { X = pos.X - 1 }
        ];
        return neighbours.Where(p => p.X >= 0 && p.X < GridWidth && p.Y >= 0 && p.Y < GridHeight);
    }

    private void InitGrid(string[] input)
    {
        for (int y = 0; y < input.Length; y++)
        {
            for (int x = 0; x < input[y].Length; x++)
            {
                var v = input[y][x];
                _grid[y, x] = v == '.' ? -1 : int.Parse(v.ToString());   
            }
        }
    }

    public void Print()
    {
        for (int y = 0; y < GridHeight; y++)
        {
            for (int x = 0; x < GridWidth; x++)
            {
                Console.Write($"{_grid[y, x]}");
            }
            Console.WriteLine();
        }
    }
}
public static class Day10
{
    
    public static void Solve()
    {
        var map = new Map(InputReader.GetLines("Day10.txt"));
        Console.WriteLine($"Day 10 Part 1 score {GetScore(map)}");
        Console.WriteLine($"Day 10 Part 2 rating {GetRating(map)}");
    }

    public static int GetScore(Map map)
    {
        var trailheads = map.GetLocations(0).ToList();
        var peaks = map.GetLocations(9).ToList();
        return trailheads.SelectMany(head => peaks.Where(peak => TrailRating(map, head, peak) > 0)).Count();
    }

    public static int GetRating(Map map)
    {
        var trailheads = map.GetLocations(0).ToList();
        var peaks = map.GetLocations(9).ToList();
        return trailheads.SelectMany(head => peaks.Select(peak => TrailRating(map, head, peak))).Sum();
    }

    public static int TrailRating(Map map, Point trailhead, Point target)
    {
        Queue<Point> queue = new();
        queue.Enqueue(trailhead);
        var rating = 0;
        
        while (queue.Count > 0)
        {
            var p = queue.Dequeue();
            while (true)
            {
                int height = map.GetHeight(p);
                if (height == 9 && p == target)
                {
                    rating++;
                }
                var moves = map.GetNeighbours(p).Where(n => map.GetHeight(n) == height + 1).ToList();
                if (!moves.Any())
                {
                    break;
                }
                foreach (var point in moves.Skip(1))
                {
                    queue.Enqueue(point);
                }
                p = moves.First();
            }
            
        }

        return rating;
    }
}