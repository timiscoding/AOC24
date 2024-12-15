using AOC24.Utils;

namespace AOC24.Solutions;

public static class Day08
{
    public static void Solve()
    {
        var map = InputReader.Get2DArray("Day08.txt");
        var antinodes = AllAntinodes(map, allDistances: false).ToList();
        Console.WriteLine($"Day 8 Part 1 antinodes: {antinodes.Count}");
        
        antinodes = AllAntinodes(map, allDistances: true).ToList();
        Console.WriteLine($"Day 8 Part 2 antinodes: {antinodes.Count}");
    }

    public static IEnumerable<Point> AllAntinodes(char[,] map, bool allDistances)
    {
        var bounds = new Bounds { MinX = 0, MinY = 0, MaxX = map.GetLength(1) - 1, MaxY = map.GetLength(0) - 1 };
        var allAntinodes =
            from freqGroup in PointsByFrequency(map)
            from (Point f1, Point f2) p in FrequencyPairs(freqGroup.ToList())
            let antinodes = Antinodes(p.f1, p.f2, bounds, allDistances)
            from antinode in antinodes
            select antinode;

        return allAntinodes.Distinct();
    }

    public static IEnumerable<(Point, Point)> FrequencyPairs(List<Point> points)
    {
        for (var i = 0; i < points.Count() - 1; i++)
        {
            for (var j = i + 1; j < points.Count(); j++)
            {
                yield return (points.ElementAt(i), points.ElementAt(j));
            }
        }
    }

    public static IEnumerable<IGrouping<char, Point>> PointsByFrequency(char[,] map) =>
        from y in Enumerable.Range(0, map.GetLength(0))
        from x in Enumerable.Range(0, map.GetLength(1))
        let freq = map[y, x]
        let point = new Point(x, y)
        where freq != '.'
        group point by freq;

    public static IEnumerable<Point> Antinodes(Point a, Point b, Bounds bounds, bool allDistances = false)
    {
        if (allDistances)
        {
            yield return a;
            yield return b;
        }

        int distance = 2;
        int dx = a.X - b.X;
        int dy = a.Y - b.Y;
        while (WithinBounds(bounds, a.X - distance * dx, a.Y - distance * dy, out Point antinode))
        {
            yield return antinode;
            if (allDistances) distance++;
            else break;
        }

        distance = 2;
        dx = b.X - a.X;
        dy = b.Y - a.Y;
        while (WithinBounds(bounds, b.X - distance * dx, b.Y - distance * dy, out Point antinode2))
        {
            yield return antinode2;
            if (allDistances) distance++;
            else break;
        }
    }

    public static void PrintAntinodes(char[,] map, IEnumerable<Point> antinodes)
    {
        Console.WriteLine(" 0123456789012");
        for (var y = 0; y < map.GetLength(0); y++)
        {
            Console.Write(y % 10);
            for (var x = 0; x < map.GetLength(1); x++)
            {
                char val = map[y, x];
                Console.Write(antinodes.Contains(new Point(x, y)) ? '#' : val);
            }
            Console.WriteLine();
        }
    }

    private static bool WithinBounds(Bounds bound, int x, int y, out Point p)
    {
        p = new Point(x, y);
        return x >= bound.MinX && x <= bound.MaxX && y >= bound.MinY && y <= bound.MaxY;
    }
}