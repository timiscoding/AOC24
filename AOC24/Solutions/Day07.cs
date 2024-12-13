using System.Collections;
using AOC24.Utils;

namespace AOC24.Solutions;

public static class Day07
{
    public delegate long Operation(long a, long b);
    
    public static void Solve()
    {
        var lines = InputReader.GetLines("Day07.txt");
        var equations = lines.Select(line =>
        {
            var nums = line.Split([':', ' '], StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
            return new { Target = nums[0], Nums = nums[1..] };
        }).ToArray();

        List<Operation> ops = [Add, Mul];
        var sum = equations.Where(e => CanSolve(ops, e.Nums, e.Target)).Sum(e => e.Target);
        Console.WriteLine($"Day 7 Part 1: {sum} {sum:N0}");
        
        sum = equations.Where(e => CanSolve(ops.Concat([ Concat ]), e.Nums, e.Target)).Sum(e => e.Target);
        Console.WriteLine($"Day 7 Part 2: {sum} {sum:N0}");
    }
    
    public static bool CanSolve(IEnumerable<Operation> ops, long[] nums, long target, long total = 0, int index = 1)
    {
        if (nums.Length < 2) throw new ArgumentException("nums must have at least 2 elements");
        if (index == 1) total = nums[0];
        if (total > target) return false;
        if (index >= nums.Length)
        {
            return total == target;
        }

        foreach (var op in ops)
        {
            if (CanSolve(ops, nums, target, op(total, nums[index]), index + 1))
                return true;
        }
        return false;
    }

    public static bool CanSolveIterative(IEnumerable<Operation> ops, long[] nums, long target)
    {
        Queue<long> q = new();
        q.Enqueue(nums[0]);
        foreach (var n in nums.Skip(1))
        {
            var curQueueLen = q.Count;
            for (var i = 0; i < curQueueLen; i++)
            {
                var subTotal = q.Dequeue();
                foreach (var op in ops)
                {
                    q.Enqueue(op(subTotal, n));
                }
            }
        }
        return q.Any(total => total == target);
    }
        
    public static long Add(long num1, long num2) => num1 + num2;
    public static long Mul(long num1, long num2) => num1 * num2;
    public static long Concat(long num1, long num2) => long.Parse($"{num1}{num2}");

}