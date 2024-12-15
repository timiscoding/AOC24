using AOC24.Solutions;

namespace AOC24.Tests;

[TestFixture]
public class Day07Tests
{
    private Day07.Operation[] _ops = [Day07.Add, Day07.Mul, Day07.Concat];
    
    [TestCase(new long[] { 2, 3 }, 100, false)]
    [TestCase(new long[] { 1, 2 }, 3, true)]      // add
    [TestCase(new long[] { 2, 3 }, 6, true)]      // mul
    [TestCase(new long[] { 2, 3 }, 23, true)]     // concat
    public void CanSolve_TwoNums_ReturnsCorrectResult(long[] nums, long target, bool expected)
    {
        var actual = Day07.CanSolve(_ops, nums, target);
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [TestCase(new long[] { 2, 3, 4 }, 100, false)]
    [TestCase(new long[] { 2, 3, 4 }, 9, true)]      // 2+3+4
    [TestCase(new long[] { 2, 3, 4 }, 20, true)]     // 2+3*4
    [TestCase(new long[] { 2, 3, 4 }, 54, true)]     // 2+3||4
    [TestCase(new long[] { 2, 3, 4 }, 24, true)]     // 2*3*4
    [TestCase(new long[] { 2, 3, 4 }, 10, true)]     // 2*3+4
    [TestCase(new long[] { 2, 3, 4 }, 64, true)]     // 2*3||4
    [TestCase(new long[] { 2, 3, 4 }, 234, true)]    // 2||3||4
    [TestCase(new long[] { 2, 3, 4 }, 27, true)]      // 2||3+4
    [TestCase(new long[] { 2, 3, 4 }, 92, true)]      // 2||3*4
    public void CanSolve_ThreeNums_ReturnsCorrectResult(long[] nums, long target, bool expected)
    {
        var actual = Day07.CanSolve(_ops, nums, target);
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(new long[] { 2, 3, 4 }, 100, false)]
    [TestCase(new long[] { 2, 3, 4 }, 9, true)]      // 2+3+4
    [TestCase(new long[] { 2, 3, 4 }, 20, true)]     // 2+3*4
    [TestCase(new long[] { 2, 3, 4 }, 54, true)]     // 2+3||4
    [TestCase(new long[] { 2, 3, 4 }, 24, true)]     // 2*3*4
    [TestCase(new long[] { 2, 3, 4 }, 10, true)]     // 2*3+4
    [TestCase(new long[] { 2, 3, 4 }, 64, true)]     // 2*3||4
    [TestCase(new long[] { 2, 3, 4 }, 234, true)]    // 2||3||4
    [TestCase(new long[] { 2, 3, 4 }, 27, true)]      // 2||3+4
    [TestCase(new long[] { 2, 3, 4 }, 92, true)]      // 2||3*4
    public void CanSolveIterative_ReturnsCorrectResult(long[] nums, long target, bool expected)
    {
        var actual = Day07.CanSolveIterative(_ops, nums, target);
        Assert.That(actual, Is.EqualTo(expected));
    }
}