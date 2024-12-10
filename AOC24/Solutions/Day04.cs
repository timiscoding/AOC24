using AOC24.Utils;

namespace AOC24.Solutions;

public static class Day04
{
    public static void Solve()
    {
        Console.WriteLine("--- Day 4");
        var board = InputReader.GetLines("Day04.txt");
        Part1(board);

        Part2(board);
    }

    private static void Part1(string[] board)
    {
        var xs = FindAll(board, 'X');
        var xmases = xs.Select(pos => Xmases(board, pos)).Sum();
        Console.WriteLine($"xmas count: {xmases}");
    }

    private static void Part2(string[] board)
    {
        var ays = FindAll(board, 'A');
        var crossmases = ays.Count(pos => IsCrossmas(board, pos));
        Console.WriteLine($"x-mas count: {crossmases}");
    }

    private static bool IsCrossmas(string[] board, Point p)
    {
        var xs = Enumerable.Range(p.X - 1, 3).ToArray();
        var ys = Enumerable.Range(p.Y - 1, 3).ToArray();
        var diag1 = xs.Zip(ys, (x, y) => new Point(x, y)).ToArray();
        var diag2 = xs.Zip(ys.Reverse(), (x, y) => new Point(x, y)).ToArray();
        Point[][] lines = [diag1, diag2];

        return lines.All(line =>
        {
            var text = GetString(board, line);
            return text == "MAS" || text == "SAM";
        });
    }

    private static IEnumerable<Point> FindAll(string[] board, char target) =>
        board
            .SelectMany((row, y) => row.Select<char, Point?>((c, x) => c == target ? new Point(x, y) : null))
            .OfType<Point>();
    
    private static int Xmases(string[] board, Point p)
    {
        var xs = Enumerable.Range(p.X - 3, 7).ToArray();
        var ys = Enumerable.Range(p.Y - 3, 7).ToArray();
        var hor = xs.Select(x => new Point(x, p.Y)).ToArray();
        var ver = ys.Select(y => new Point(p.X, y)).ToArray();
        var diag1 = xs.Zip(ys).Select(t => new Point(t)).ToArray();
        var diag2 = xs.Zip(ys.Reverse()).Select(t => new Point(t)).ToArray();
        Point[][] lines = [hor, ver, diag1, diag2];

        return lines.Select(line =>
        {
                string text = GetString(board, line);
                var res = text switch
                {
                    "SAMXMAS" => 2,
                    _ when text.StartsWith("SAMX") || text.EndsWith("XMAS") => 1,
                    _ => 0
                };
                return res;
        }).Sum();
    }

    private static string GetString(string[] board, Point[] line)
    {
        string res = "";
        foreach (Point p in line)
        {
            if (p.Y < 0 || p.Y >= board.Length || p.X < 0 || p.X >= board[0].Length) continue;
            res += board[p.Y][p.X];
        }

        return res;
    }
}