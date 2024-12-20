using System.Numerics;
using AOC24.Solutions;

namespace AOC24.Tests;

[TestFixture]
public class Day11Tests
{
    [Test]
    public void Split_1000_ShouldReturnCorrectResult()
    {
        (BigInteger, BigInteger) expected = (10, 0);
        Assert.That(Day11.Split(new BigInteger(1000)), Is.EqualTo(expected));
    }
    
    [Test]
    public void Split_LargeNumber_ShouldReturnCorrectResult()
    {
        (BigInteger, BigInteger) expected = (123456789012345, 678901234567890);
        Assert.That(Day11.Split(BigInteger.Parse("123456789012345678901234567890")), Is.EqualTo(expected));
    }


    [Test]
    public void Blink_OneTime_ReturnsCorrectResult()
    {
        var stones = Day11.GetStones("0 1 10 99 999");
        var actual = Day11.Blink(stones, 1);
        BigInteger[] expected = [1, 2024, 1, 0, 9, 9, 2021976]; 
        CollectionAssert.AreEqual(expected, actual);
    }
    
    [Test]
    public void Blink_SixTimes_ReturnsCorrectResult()
    {
        var stones = Day11.GetStones("125 17");
        var actual = Day11.Blink(stones, 6);
        BigInteger[] expected = [2097446912, 14168, 4048, 2, 0, 2, 4, 40, 48, 2024, 40, 48, 80, 96, 2, 8, 6, 7, 6, 0, 3, 2]; 
        CollectionAssert.AreEqual(expected, actual);
    }
}