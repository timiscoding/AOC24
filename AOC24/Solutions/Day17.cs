using System.Runtime.Intrinsics.Arm;
using AOC24.Utils;

namespace AOC24.Solutions;

public static class Day17
{
    private static long[] _registers = new long[3];
    public static void Solve()
    {
        var input = InputReader.GetText("Day17.txt");
        var program = Parse(input);
        Console.WriteLine($"Day 17 - Part 1 output: {string.Join(",", Execute(program))}");
        Console.WriteLine($"Day 17 - Part 2 Register A: {FindOutputCopy(program)}");
    }

    /* returns the value of register A where the program output matches the program itself.
     * For Part 2, I was stuck for days so I looked for clues on reddit.
     * The key insight is the program outputs the last base 8 digit in A, then throws it away by dividing by 8. This means
     * the first digit in the output is determined by the last digit in A. Conversely, the first digit in A determines
     * the last digit output.
     * The A value for the last digit in the output is found by brute force. Next, find the A value for the last 2 digits
     * of the output by building on the previous A value, shift it 1 place (by multiplying by 8) then find the A value
     * where the output matches the last 2 outputs. Repeat until all outputs are processed. 
     */
    public static long FindOutputCopy(long[] program)
    {
        var res = new List<long>() { 0 };
        foreach (var i in Enumerable.Range(1, program.Length))
        {
            res = res.SelectMany(seed => FindA(program.TakeLast(i), seed * 8)).ToList();
        }

        IEnumerable<long> FindA(IEnumerable<long> desiredOutput, long seed)
        {
            for (var n = 0; n < 8; n++)
            {
                _registers = [seed + n, 0, 0];
                var output = Execute(program);
                if (output.SequenceEqual(desiredOutput)) yield return seed + n;
            }
        }
        return res.Min();
    }

    public static long[] Execute(long[] program)
    {
        var isp = 0L;
        var output = new List<long>();
        while (isp < program.Length)
        {
            var opcode = program[isp];
            var operand = program[isp + 1];
            if (opcode == 0)  _registers[0] = (long)(_registers[0] / Math.Pow(2, Combo(operand)));
            if (opcode == 1) _registers[1] ^= operand;
            if (opcode == 2) _registers[1] = Combo(operand) % 8;
            if (opcode == 3) isp = _registers[0] == 0 ? isp + 2 : operand;
            if (opcode == 4) _registers[1] ^= _registers[2];
            if (opcode == 5) output.Add(Combo(operand) % 8);
            if (opcode == 6) _registers[1] = (long)(_registers[0] / Math.Pow(2, Combo(operand)));
            if (opcode == 7) _registers[2] = (long)(_registers[0] / Math.Pow(2, Combo(operand)));
            if (opcode != 3) isp += 2;
        }
        return output.ToArray();
        
        long Combo(long operand) =>
            operand switch
            {
                <= 3 => operand,
                <= 6 => _registers[operand - 4],
                _ => throw new ArgumentException($"Invalid combo operand {operand}")
            };
    }
    
    public static long[] Parse(string input)
    {
        var parts = input.Split("\n\n");
        _registers = parts[0].Split("\n").Select(line => long.Parse(line.Split().Last())).ToArray();
        var program = parts[1].Split().Last().Split(',').Select(long.Parse).ToArray();
        return program;
    }
}