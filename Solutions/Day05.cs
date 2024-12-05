using AOC24.Utils;

namespace AOC24.Solutions;

public static class Day05
{
    private static Dictionary<string, List<string>> _orderMap = new (); 
    public static void Solve()
    {
        Console.WriteLine("--- Day 5");
        var lines = InputReader.GetLines("Day05.txt");
        var rules = lines.TakeWhile(line => line != string.Empty).ToArray();
        var updates = lines.Skip(rules.Length + 1).Select(update => update.Split(',')).ToArray();
        
        ParseRules(rules);
        Part1(updates);
        Part2(updates);
    }

    private static void ParseRules(string[] rules)
    {
        foreach (var rule in rules)
        {
            var pages = rule.Split('|');
            var p = pages[0];
            var p2 = pages[1];
            if (!_orderMap.ContainsKey(p))
                _orderMap[p] = [p2];
            else
            {
                _orderMap[p].Add(p2);
            }
        }
    }

    private static void Part1(string[][] updates)
    {
        var correct = FilterUpdates(updates);
        Console.WriteLine($"Part 1 sum: {SumMiddle(correct)}");
    }

    private static void Part2(string[][] updates)
    {
        var incorrect = FilterUpdates(updates, correct: false);
        var corrected = incorrect.Select(update => update.Order(new PageComparer(_orderMap)).ToArray());
        Console.WriteLine($"Part 2 sum: {SumMiddle(corrected)}");
    }
    
    private static int SumMiddle(IEnumerable<string[]> updates)
        => updates.Select(update => int.Parse(update[update.Length / 2])).Sum();

    private static IEnumerable<string[]> FilterUpdates(string[][] updates, bool correct = true)
    {
        return updates.Where(update =>
        {
            var res = update.SequenceEqual(update.Order(new PageComparer(_orderMap)));
            return correct ? res : !res;
        });
    }
}

public class PageComparer(Dictionary<string, List<string>> orderMap) : IComparer<string>
{
    public int Compare(string? p1, string? p2)
    {
        if (p1 == p2) return 0;
        if (p1 == null) return 1;
        if (p2 == null) return -1;
        
        if (orderMap.TryGetValue(p1, out var pagesAfter) && pagesAfter.Contains(p2)) return -1;
        if (orderMap.TryGetValue(p2, out var pagesAfter2) && pagesAfter2.Contains(p1)) return 1;
        
        throw new ArgumentOutOfRangeException($"Could not find page order for {p1} {p2}");
    }
}