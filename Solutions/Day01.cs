using System.Data;
using AOC24.Utils;

namespace AOC24.Solutions;

public static class Day01
{
    public static dynamic[] rows;
    public static int[] list1;
    public static int[] list2; 
    public static void Solve()
    {
        var columnTypeMap = new Dictionary<string, Type>
        {
            { "Lid1", typeof(int) },
            { "Lid2", typeof(int) }
        };
        rows = InputReader.ReadInputAsDynamic("Day01.txt", columnTypeMap).ToArray();
        
        list1 = new int[rows.Length];
        list2 = new int[rows.Length];
        for (int i = 0; i < rows.Length; i++)
        {
            list1[i] = rows[i].Lid1;
            list2[i] = rows[i].Lid2;
        }
        
        Console.WriteLine("Solving day 1");
        Console.WriteLine("Part 1");
        Part1();

        Console.WriteLine("Part 2");
        Part2();
    }

    public static void Part1()
    {
        int sum = 0;
        foreach ((int id1, int id2) in list1.Order().Zip(list2.Order()))
        {
            sum += Math.Abs(id1 - id2);
        }

        Console.WriteLine($"Sum: {sum}");
    }

    public static void Part2()
    {
        int similarityScore = list1.Select(id1 => list2.Count(id2 => id2 == id1) * id1).Sum();
        Console.WriteLine($"Similarity score: {similarityScore}");
    }
}