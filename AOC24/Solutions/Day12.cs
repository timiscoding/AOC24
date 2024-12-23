using System.Numerics;
using AOC24.Utils;

namespace AOC24.Solutions;

public static class Day12
{
    public static Complex Down = -Complex.ImaginaryOne;
    public static Complex Up = Complex.ImaginaryOne;
    public static Complex Left = -1;
    public static Complex Right = 1;
    
    [Flags]
    public enum Border
    {
        None = 0,
        Top = 1,
        Right = 2,
        Bottom = 4,
        Left = 8,
    }
    public static void Solve()
    {
        var input = InputReader.GetLines("Day12.txt");
        var garden = GetGarden(input);
        
        Console.WriteLine($"Day 12 Part 1 - fence cost: {FenceCosts(garden)}");
        Console.WriteLine($"Day 12 Part 2 - fence cost: {FenceCosts(garden, bulkDiscount: true)}");
    }

    public static int FenceCosts(Dictionary<Complex, char> garden, bool bulkDiscount = false)
    {
        var visited = new HashSet<Complex>();
        var cost = 0;
        foreach (var point in garden.Keys)
        {
            if (visited.Contains(point)) continue;
            
            var plot = new HashSet<Complex>();
            var fences = Fences(garden, point, ref plot);
            if (bulkDiscount)
            {
                var sides = SideCount(fences);
                cost += sides * plot.Count;
            }
            else
            {
                var fenceCount = fences.Sum(p => int.PopCount((int)p.Value));
                cost += fenceCount * plot.Count;
            }
            visited.UnionWith(plot);
        }

        return cost;
    }

    /* given `fences` of a plot, it returns the number of sides for the plot by visiting every fence and walking along
     * it's border in a certain direction. Every time it visits a fence with the same border, it marks it as visited
     * and then counts the total walk as having 1 side. 
     */
    public static int SideCount(Dictionary<Complex, Border> fences)
    {
        // order the points from top to bottom, left to right. By doing so, when we walk along fences in a line
        // we only need to search in one direction because it'll start from either the left most or top most point
        var unvisited = fences
            .OrderByDescending(k => k.Key.Imaginary)
            .ThenBy(v => v.Key.Real)
            .ToDictionary(k => k.Key, v => v.Value);

        var sides = 0;
        while (unvisited.Any(p => p.Value > 0))
        {
            var fence = unvisited.First(p => p.Value > 0).Key;
            foreach (var border in new[] { Border.Top, Border.Right, Border.Bottom, Border.Left })
            {
                if (!unvisited[fence].HasFlag(border)) continue;
                unvisited[fence] &= ~border;
                if (TryVisitDir(Down, fence, border)) sides++;
                else if (TryVisitDir(Right, fence, border)) sides++;
                else sides++;
            }
        }
        return sides;

        bool TryVisitDir(Complex dir, Complex fence, Border border)
        {
            var hasDir = false;
            var pt = fence;
            while (unvisited.TryGetValue(pt + dir, out var b) && b.HasFlag(border))
            {
                hasDir = true;
                unvisited[pt + dir] &= ~border; // remove border from fence to mark it as visited
                pt += dir;
            }
            return hasDir;
        }
    }

    /* given a garden and the starting 'point', it finds all interconnected plants that are the same as the start point
     * and stores it in `plot` and also returns the contour points of the plot
     */
    public static Dictionary<Complex, Border> Fences(Dictionary<Complex, char> garden, Complex point, ref HashSet<Complex> plot)
    {
        var queue = new Queue<Complex>();
        var fences = new Dictionary<Complex, Border>();
        plot.Add(point);
        queue.Enqueue(point);
        while (queue.Count > 0)
        {
            var pt = queue.Dequeue();
            var plant = garden[pt];
            foreach (var dir in new List<Complex> {Up, Right, Down, Left})
            {
                if (plot.Contains(pt + dir)) continue;
                if (garden.ContainsKey(pt + dir) && garden[pt + dir] == plant)
                {
                    queue.Enqueue(pt + dir);
                    plot.Add(pt + dir);
                }
                else
                {
                    fences[pt] = fences.GetValueOrDefault(pt, Border.None) | GetBorder(dir);
                }
                    
            }
        }
        return fences;
    }

    public static Dictionary<Complex, char> GetGarden(string[] input)
    {
        var garden = new Dictionary<Complex, char>();
        foreach (var y in Enumerable.Range(0, input.Length))
            foreach (var x in Enumerable.Range(0, input[0].Length))
                garden[x + y * Down] = input[y][x];
        return garden;
    }
    
    private static Border GetBorder(Complex dir) =>
        dir switch
        {
            _ when dir == -Complex.ImaginaryOne => Border.Bottom,
            _ when dir == Complex.ImaginaryOne => Border.Top,
            _ when dir == -1 => Border.Left,
            _ when dir == 1 => Border.Right,
            _ => throw new ArgumentException($"Invalid direction: {dir}")
        };
}