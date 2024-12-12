using AOC24.Solutions;

namespace AOC24.Tests;

[TestFixture]
public class Day07Tests
{
    private Day07.Operation[] _ops = [Day07.Add, Day07.Mul, Day07.Concat];
    
    [TestCase(new ulong[] { 2, 3 }, 100U, false)]
    [TestCase(new ulong[] { 1, 2 }, 3U, true)]      // add
    [TestCase(new ulong[] { 2, 3 }, 6U, true)]      // mul
    [TestCase(new ulong[] { 2, 3 }, 23U, true)]     // concat
    public void CanSolve_TwoNums_ReturnsCorrectResult(ulong[] nums, ulong target, bool expected)
    {
        var actual = Day07.CanSolve(_ops, nums, target);
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    [TestCase(new ulong[] { 2, 3, 4 }, 100U, false)]
    [TestCase(new ulong[] { 2, 3, 4 }, 9U, true)]      // 2+3+4
    [TestCase(new ulong[] { 2, 3, 4 }, 20U, true)]     // 2+3*4
    [TestCase(new ulong[] { 2, 3, 4 }, 54U, true)]     // 2+3||4
    [TestCase(new ulong[] { 2, 3, 4 }, 24U, true)]     // 2*3*4
    [TestCase(new ulong[] { 2, 3, 4 }, 10U, true)]     // 2*3+4
    [TestCase(new ulong[] { 2, 3, 4 }, 64U, true)]     // 2*3||4
    [TestCase(new ulong[] { 2, 3, 4 }, 234U, true)]    // 2||3||4
    [TestCase(new ulong[] { 2, 3, 4 }, 27U, true)]      // 2||3+4
    [TestCase(new ulong[] { 2, 3, 4 }, 92U, true)]      // 2||3*4
    public void CanSolve_ThreeNums_ReturnsCorrectResult(ulong[] nums, ulong target, bool expected)
    {
        var actual = Day07.CanSolve(_ops, nums, target);
        Assert.That(actual, Is.EqualTo(expected));
    }
    
    
}