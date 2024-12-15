using AOC24.Solutions;
using AOC24.Utils;

namespace AOC24.Tests;

[TestFixture]
public class Day08Tests
{
    private Bounds _bounds = new Bounds { MinX = -5, MinY = -5, MaxX = 5, MaxY = 5 };
    
    [Test]
    public void GetAntinodes_HorizontalGradient_ReturnsCorrectValues()
    {
        var actual = Day08.Antinodes(new Point(0, 0), new Point(1, 0), _bounds);
        var expected = new List<Point>
        {
            new(2, 0),
            new(-1, 0)
        };
        CollectionAssert.AreEqual(expected, actual);
    }
    
    [Test]
    public void GetAntinodes_VerticalGradient_ReturnsCorrectValues()
    {
        var actual = Day08.Antinodes(new Point(0, 0), new Point(0, 1), _bounds);
        var expected = new List<Point>
        {
            new(0, 2),
            new(0, -1)
        };
        CollectionAssert.AreEqual(expected, actual);
    }
    
    [Test]
    public void GetAntinodes_PositiveGradient_ReturnsCorrectValues()
    {
        var actual = Day08.Antinodes(new Point(0, 0), new Point(1, 1), _bounds);
        var expected = new List<Point>
        {
            new(2, 2),
            new(-1, -1)
        };
        CollectionAssert.AreEqual(expected, actual);
    }
    
    [Test]
    public void GetAntinodes_NegativeGradient_ReturnsCorrectValues()
    {
        var actual = Day08.Antinodes(new Point(0, 0), new Point(-1, 1), _bounds);
        var expected = new List<Point>
        {
            new(-2, 2),
            new(1, -1)
        };
        CollectionAssert.AreEqual(expected, actual);
    }
    
    [Test]
    public void GetAntinodes_NegativeGradientOrderReversed_ReturnsCorrectValues()
    {
        var actual = Day08.Antinodes(new Point(-1, 1), new Point(0, 0), _bounds);
        var expected = new List<Point>
        {
            new(-2, 2),
            new(1, -1)
        };
        CollectionAssert.AreEquivalent(expected, actual);
    }
    
    [Test]
    public void GetAntinodes_AntinodeOutsideBounds_ReturnsAntinodeInsideBounds()
    {
        var actual = Day08.Antinodes(new Point(-5, -5), new Point(0, 0), _bounds);
        var expected = new List<Point>
        {
            new(5, 5)
        };
        CollectionAssert.AreEquivalent(expected, actual);
    }
    
    [Test]
    public void GetAntinodes_AntinodesAllDistances_ReturnsCorrectValues()
    {
        var actual = Day08.Antinodes(new Point(0, 0), new Point(2, 2), _bounds, allDistances: true);
        var expected = new List<Point>
        {
            new(0, 0),
            new(2, 2),
            new(4, 4),
            new(-2, -2),
            new(-4, -4)
        };
        CollectionAssert.AreEquivalent(expected, actual);
    }
}