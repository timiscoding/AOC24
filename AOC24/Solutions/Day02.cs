using AOC24.Utils;

namespace AOC24.Solutions;

public static class Day02
{
    private static IEnumerable<List<int>> lines = InputReader.ReadInputAsIntList("Day02.txt");
    
    public static void Solve()
    {
        Console.WriteLine("--- Day 2");
        Part1();
        Part2();
    }

    private static void Part1()
    {
        var safeCount = lines.Count(report => IsReportSafe(report));
        Console.WriteLine($"Part 1 - Safe reports: {safeCount}");
    }

    private static void Part2()
    {
        int safeCount = 0;
        foreach (var report in lines)
        {
            if (!IsReportSafe(report))
            {
                for (int i = 0; i < report.Count; i++)
                {
                    int val = report[i];
                    report.RemoveAt(i);
                    if (IsReportSafe(report))
                    {
                        safeCount++;
                        break;
                    }
                    report.Insert(i, val);
                }
            }
            else
            {
                safeCount++;
            }
        }

        Console.WriteLine($"Part 2 - Safe reports: {safeCount}");
    }

    private static bool IsReportSafe(List<int> levels)
    {
        var diffs = AdjDiff(levels).ToArray();
        return diffs.All(d => Math.Abs(d) < 4) && (diffs.All(d => d > 0) || diffs.All(d => d < 0));
    }
    
    private static IEnumerable<int> AdjDiff(List<int> levels)
    {
        for (int i = 1; i < levels.Count(); i++)
        {
            yield return levels[i] - levels[i - 1];
        }
    }
}