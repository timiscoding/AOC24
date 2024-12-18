using AOC24.Solutions;
using AOC24.Utils;

namespace AOC24.Tests;

[TestFixture]
public class Day10Tests
{
    [Test]
    public void TrailRating_OneTrail_Returns1()
    {
        var input = """
                       6501
                       7432
                       89..
                       """.Split();
        var map = new Map(input);

        var actual = Day10.TrailRating(map, new Point(2, 0), new Point(1, 2));
        Assert.That(actual, Is.EqualTo(1));
    }
    
    [Test]
    public void TrailRating_NoTrail_Returns0()
    {
        var input = """
                    6501
                    7432
                    80..
                    """.Split();
        var map = new Map(input);

        var actual = Day10.TrailRating(map, new Point(2, 0), new Point(1, 2));
        Assert.That(actual, Is.EqualTo(0));
    }
    
    [Test]
    public void GetScore_TwoTrailsToDifferentPeaks_Returns2()
    {
        var input = """
                    ...0...
                    ...1...
                    ...2...
                    6543456
                    7.....7
                    8.....8
                    9.....9
                    """.Split();
        var map = new Map(input);

        var actual = Day10.GetScore(map);
        Assert.That(actual, Is.EqualTo(2));
    }
    
    [Test]
    public void TrailRating_OneTrailHeadThreeTrails_Returns3()
    {
        var input = """
                    .....0.
                    ..4321.
                    ..5..2.
                    ..6543.
                    ..7..4.
                    ..8765.
                    ..9....
                    """.Split();
        var map = new Map(input);

        var actual = Day10.TrailRating(map, new Point(5, 0), new Point(2, 6));
        Assert.That(actual, Is.EqualTo(3));
    }
}