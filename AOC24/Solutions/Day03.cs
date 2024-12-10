using System.Text.RegularExpressions;
using AOC24.Utils;

namespace AOC24.Solutions;

public static class Day03
{
    public static void Solve()
    {
        var instructions = InputReader.GetText("Day03.txt");
        Console.WriteLine("--- Day 3");
        Console.WriteLine($"Part 1: sum {Part1(instructions)}");
        Console.WriteLine($"Part 2: sum {Part2(instructions)}");
    }

    static int Part1(string instructions)
    {
        var matches = Regex.Matches(instructions, @"mul\((\d+),(\d+)\)");
        var sum = matches.Select(m => Mult(m.Groups[1].Value, m.Groups[2].Value)).Sum();
        return sum;
    }
    
    static int Part2(string instructions)
    {
        var noNewLines = instructions.Replace("\n", "");
        var pattern = @"don't\(\).*?do\(\)|don't\(\).*?$";
        var noDisabledMuls = Regex.Replace(noNewLines, pattern, "");
        return Part1(noDisabledMuls);
    }

    static int Mult(string a, string b) => int.Parse(a) * int.Parse(b);
}

