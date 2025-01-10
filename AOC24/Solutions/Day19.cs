using AOC24.Utils;
using Cache = System.Collections.Generic.Dictionary<string, long>;

namespace AOC24.Solutions;

public class TrieNode
{
    public readonly Dictionary<char, TrieNode> Children = new();
    public bool IsTerminal;
    public string Word;
}

public class Trie
{

    public TrieNode Root = new();

    public Trie() { }

    public Trie(IEnumerable<string> words)
    {
        foreach (var w in words) Insert(w);
    }
    
    public void Insert(string word)
    {
        var node = Root;
        foreach (var c in word)
        {
            if (!node.Children.ContainsKey(c))
            {
                node.Children.Add(c, new TrieNode());
            }
            node = node.Children[c];
        }
        node.IsTerminal = true;
        node.Word = word;
    }
    
    public bool Contains(string word)
    {
        var node = Root;
        foreach (var c in word)
        {
            if (!node.Children.TryGetValue(c, out var child)) return false;
            node = child;
        }
        return node.IsTerminal;
    }

    public bool IsPrefix(string word)
    {
        var node = Root;
        foreach (var c in word)
        {
            if (!node.Children.TryGetValue(c, out var child)) return false;
            node = child;
        }
        return true;
    }

    public IEnumerable<string> AllWords(string str)
    {
        var node = Root;
        foreach (var c in str)
        {
            if (!node.Children.TryGetValue(c, out var child)) yield break;
            if (child.IsTerminal) yield return child.Word;
            node = child;
        }
    }
}

public static class Day19
{
    public static void Solve()
    {
        var (patterns, designs) = Parse(InputReader.GetText("Day19.txt"));
        var trie = new Trie(patterns);
        var makeable = designs.Count(d => CanMake(trie, d));
        Console.WriteLine($"Day 19 - Part 1 Possible designs: {makeable}");
        var set = new HashSet<string>(patterns);
        Console.WriteLine($"Day 19 - Part 2 Design yield count: {designs.Sum(d => CountMakes(set, d))}");
    }

    public static bool CanMake(Trie patterns, string design, int start = 0)
    {
        if (start >= design.Length) return true;
        var res = false;
        foreach (var word in patterns.AllWords(design[start..]))
        {
            var nextStart = start + word.Length;
            res = CanMake(patterns, design, nextStart);
            if (res) break;
        }
        return res;
    }

    /* counts the combinations a design can be made by patterns.
     Struggled to solve this one on my own and found some pseudocode/working example on Reddit. 
     Since iterating thru each combination would take too long, it builds the count by looking at smaller sub designs 
     and stores the intermediate results in a cache to avoid recomputing them later on. */
    public static long CountMakes(HashSet<string> patterns, string design)
    {
        Cache cache = new Cache();
        return Run(design);

        long Run(string design2)
        {
            if (design2.Length == 0) return 1;

            foreach (var p in patterns.Where(design2.StartsWith))
            {
                var nextDesign = design2.Substring(p.Length);
                if (!cache.TryGetValue(nextDesign, out var res)) res = Run(nextDesign);
                cache[design2] = cache.GetValueOrDefault(design2, 0) + res;
            }
            return cache.GetValueOrDefault(design2, 0);
        }
    }

    private static (string[] patterns, string[] designs) Parse(string input)
    {
        var parts = input.Split("\n\n");
        return (parts[0].Split(", "), parts[1].Split());
    }
}