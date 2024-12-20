using System.Numerics;
using AOC24.Utils;

namespace AOC24.Solutions;

public static class Day11
{
    public static void Solve()
    {
        var stones2 = GetStones2(InputReader.GetText("Day11.txt"));

        var count = Blink2(stones2, 25);
        Console.WriteLine($"Day 11 Part 1 - After blink 25 there are {count} stones.");
        count = Blink2(stones2, 75);
        Console.WriteLine($"Day 11 Part 2 - After blink 75 there are {count} stones.");
    }

    public static LinkedList<BigInteger> GetStones(string input) => new(input.Split().Select(BigInteger.Parse));
    
    public static Dictionary<long, long> GetStones2(string input) => input.Split().Select(long.Parse).ToDictionary(x => x, x => 1L);

    /*
     * After reading the AOC subreddit, someone used a dictionary which tipped me off that order was unimportant.
     * We simply needed to know what numbers were in a blink and how many times they appeared in the sequence.
     * Then it was a matter of computing the new value once for each distinct stone and multiplying it by some factor.
     * THe original implementation was slow because a stone that appeared in the sequence 1000s of times would be
     * converted the same amount of times.
     */
    public static long Blink2(Dictionary<long, long> stones, int times)
    {
        for (var i = 1; i <= times; i++)
        {
            var newStones = new Dictionary<long, long>();
            foreach (var num in stones.Keys)
            {
                if (num == 0)
                { 
                    newStones[1] = newStones.GetValueOrDefault(1) + stones.GetValueOrDefault(num, 1);
                }
                else if (long.IsEvenInteger(Digits<long>(num)))
                {
                    var (left, right) = Split(num);
                    newStones[left] = newStones.GetValueOrDefault(left) + stones.GetValueOrDefault(num, 1);
                    newStones[right] = newStones.GetValueOrDefault(right) + stones.GetValueOrDefault(num, 1);
                }
                else
                {
                    var newNum = num * 2024;
                    newStones[newNum] = newStones.GetValueOrDefault(newNum) + stones.GetValueOrDefault(num, 1);
                }
            }
            stones = newStones;
        }
        
        return stones.Values.Sum();
    }
    
    /*
     * Originally decided to use a linked list since the problem statement specifically mentioned that order was
     * preserved which I thought was important. However, this was problematic when part 2 asked us to compute a bigger
     * blink value since the number of stones increases exponentially
     */
    public static LinkedList<BigInteger> Blink(LinkedList<BigInteger> stones, int times)
    {
        for (var i = 1; i <= times; i++)
        {
            var stone = stones.First;
            while (stone != null)
            {
                if (stone.Value == 0) stone.Value = 1;
                else if (BigInteger.IsEvenInteger(Digits(stone.Value)))
                {
                    var (left, right) = Split(stone.Value);
                    var rightStone = stones.AddAfter(stone, right);
                    stones.AddAfter(stone, left);
                    stones.Remove(stone);
                    stone = rightStone;
                }
                else stone.Value *= 2024;
                stone = stone.Next;
            }
        }

        return stones;
    }

    public static (BigInteger left, BigInteger right) Split(BigInteger num)
    {
        var halfDigits = Digits(num) / 2;
        var divisor = BigInteger.Pow(10, halfDigits);
        var quotient = BigInteger.DivRem(num, divisor, out var remainder);
        return (quotient, remainder);
    }
    
    public static (long left, long right) Split(long num)
    {
        var halfDigits = Digits(num) / 2;
        var divisor = (long)Math.Pow(10, halfDigits);
        return long.DivRem(num, divisor);
    }

    public static int Digits<T>(T num) => num.ToString().Length;
}