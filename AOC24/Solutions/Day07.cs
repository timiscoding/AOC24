using System.Collections;
using AOC24.Utils;

namespace AOC24.Solutions;

public static class Day07
{
    public delegate ulong Operation(ulong a, ulong b);
    
    public static void Solve()
    {
        var lines = InputReader.GetLines("Day07.txt");
        var equations = lines.Select(line =>
        {
            var nums = line.Split([':', ' '], StringSplitOptions.RemoveEmptyEntries).Select(ulong.Parse).ToArray();
            return new { Target = nums[0], Nums = nums[1..] };
        }).ToArray();

        List<Operation> ops = [Add, Mul];
        var sum = equations.Where(e => CanSolve(ops, e.Nums, e.Target)).Aggregate(0UL, (sum, v) => sum + v.Target);
        Console.WriteLine($"Day 7 Part 1: {sum} {sum:N0}");
        
        sum = equations.Where(e => CanSolve(ops.Concat([ Concat ]), e.Nums, e.Target)).Aggregate(0UL, (sum, v) => sum + v.Target);
        Console.WriteLine($"Day 7 Part 2: {sum} {sum:N0}");
    }
    
        public static bool CanSolve(IEnumerable<Operation> ops, ulong[] nums, ulong target, ulong total = 0, int index = 1)
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
        
    public static ulong Add(ulong num1, ulong num2) => num1 + num2;
    public static ulong Mul(ulong num1, ulong num2) => num1 * num2;
    public static ulong Concat(ulong num1, ulong num2) => ulong.Parse($"{num1}{num2}");

}