using System.Numerics;
using static AOC24.Solutions.Day12;

namespace AOC24.Tests;

[TestFixture]
public class Day12Tests
{
    [Test]
    public void Fences_SquarePlot_ReturnsFencesAndPlot()
    {
        var garden = GetGarden(
            """
            ....
            .AA.
            .AA.
            ....
            """.Split("\n"));
        var plot = new HashSet<Complex>();
        var fences = Fences(garden, new Complex(1, -1), ref plot);
        var expectedPlot = new HashSet<Complex>
        {
            new(1, -1),
            new(2, -1),
            new(1, -2),
            new(2, -2),
        };
        var expectedFences = new Dictionary<Complex, Border>
        {
            {new(1, -1), Border.Left | Border.Top},
            {new(2, -1), Border.Right | Border.Top},
            {new(1, -2), Border.Left | Border.Bottom},
            {new(2, -2), Border.Right | Border.Bottom},
        };
        CollectionAssert.AreEquivalent(expectedPlot, plot);
        CollectionAssert.AreEquivalent(expectedFences, fences);
    }
    
    [Test]
    public void Fences_ConcavePlot_ReturnsFencesAndPlot()
    {
        var garden = GetGarden(
            """
                YY
                Y.
                Y.
                YY
                """.Split("\n"));
        var plot = new HashSet<Complex>();
        var fences = Fences(garden, new Complex(0, 0), ref plot);
        var expectedPlot = new HashSet<Complex>
        {
            new(0, 0),
            new(0, -1),
            new(0, -2),
            new(0, -3),
            new(1, 0),
            new(1, -3),
        };
        var expectedFences = new Dictionary<Complex, Border>
        {
            {new(0, 0), Border.Left | Border.Top},
            {new(0, -1), Border.Right | Border.Left},
            {new(1, 0), Border.Top | Border.Right | Border.Bottom},
            {new(0, -2), Border.Right | Border.Left},
            {new(0, -3), Border.Left | Border.Bottom},
            {new(1, -3), Border.Top | Border.Right | Border.Bottom},
        };
        CollectionAssert.AreEquivalent(expectedPlot, plot);
        CollectionAssert.AreEquivalent(expectedFences, fences);
    }
    
    [Test]
    public void FenceCosts_ByFenceExampleOne_ReturnsCost()
    {
        var garden = GetGarden(
            """
                AAAA
                BBCD
                BBCC
                EEEC
                """.Split("\n"));
        var actual = FenceCosts(garden);
        Assert.That(actual, Is.EqualTo(140));
    }
    
    [Test]
    public void FenceCosts_ByFenceExampleTwo_ReturnsCost()
    {
        var garden = GetGarden(
            """
                OOOOO
                OXOXO
                OOOOO
                OXOXO
                OOOOO
                """.Split("\n"));
        var actual = FenceCosts(garden);
        Assert.That(actual, Is.EqualTo(772));
    }
    
    [Test]
    public void FenceCosts_ByFenceExampleThree_ReturnsCost()
    {
        var garden = GetGarden(
            """
                RRRRIICCFF
                RRRRIICCCF
                VVRRRCCFFF
                VVRCCCJFFF
                VVVVCJJCFE
                VVIVCCJJEE
                VVIIICJJEE
                MIIIIIJJEE
                MIIISIJEEE
                MMMISSJEEE
                """.Split("\n"));
        var actual = FenceCosts(garden);
        Assert.That(actual, Is.EqualTo(1930));
    }
    
    [Test]
    public void SideCount_SinglePlantPlot_ReturnsCount()
    {
        var fences = new Dictionary<Complex, Border>()
        {
            { new Complex(0, 0), Border.Left | Border.Top | Border.Right | Border.Bottom },
        };
        var sideCount = SideCount(fences);
        Assert.That(sideCount, Is.EqualTo(4));
    }
    
    [Test]
    public void SideCount_SquarePlot_ReturnsCount()
    {
        var fences = new Dictionary<Complex, Border>()
        {
            { new Complex(0, 0), Border.Top | Border.Left },
            { new Complex(1, 0), Border.Top | Border.Right },
            { new Complex(0, -1), Border.Left | Border.Bottom },
            { new Complex(1, -1), Border.Right | Border.Bottom },
        };
        var sideCount = SideCount(fences);
        Assert.That(sideCount, Is.EqualTo(4));
    }
    
    [Test]
    public void SideCount_ZigZagPlot_ReturnsCount()
    {
        /*
         * P
         * PP
         *  P
         */
        var fences = new Dictionary<Complex, Border>()
        {
            { new Complex(0, 0), Border.Top | Border.Left | Border.Right },
            { new Complex(0, -1), Border.Left | Border.Bottom },
            { new Complex(1, -1), Border.Top | Border.Right },
            { new Complex(1, -2), Border.Left | Border.Bottom | Border.Right },
        };
        var sideCount = SideCount(fences);
        Assert.That(sideCount, Is.EqualTo(8));
    }
    
    [Test]
    public void SideCount_CShapePlotFencesInReverseOrder_ReturnsCount()
    {
        /*
         * PPP
         * P
         * P
         * PPP
         */
        var fences = new Dictionary<Complex, Border>()
        {
            { new Complex(0, 0), Border.Top | Border.Left },
            { new Complex(1, 0), Border.Top | Border.Bottom },
            { new Complex(2, 0), Border.Top | Border.Right | Border.Bottom },
            { new Complex(0, -1), Border.Left | Border.Right },
            { new Complex(0, -2), Border.Left | Border.Right },
            { new Complex(0, -3), Border.Left | Border.Bottom },
            { new Complex(1, -3), Border.Top | Border.Bottom },
            { new Complex(2, -3), Border.Top | Border.Right | Border.Bottom },
        };
        var sideCount = SideCount(fences);
        Assert.That(sideCount, Is.EqualTo(8));
    }
    
    [Test]
    public void FenceCosts_BySideExampleOne_ReturnsCost()
    {
        var garden = GetGarden(
            """
                AAAA
                BBCD
                BBCC
                EEEC
                """.Split("\n"));
        var actual = FenceCosts(garden, bulkDiscount: true);
        Assert.That(actual, Is.EqualTo(80));
    }
    
    [Test]
    public void FenceCosts_BySideExampleTwo_ReturnsCost()
    {
        var garden = GetGarden(
            """
                OOOOO
                OXOXO
                OOOOO
                OXOXO
                OOOOO
                """.Split("\n"));
        var actual = FenceCosts(garden, bulkDiscount: true);
        Assert.That(actual, Is.EqualTo(436));
    }
    
    [Test]
    public void FenceCosts_BySideExampleThree_ReturnsCost()
    {
        var garden = GetGarden(
            """
                EEEEE
                EXXXX
                EEEEE
                EXXXX
                EEEEE
                """.Split("\n"));
        var actual = FenceCosts(garden, bulkDiscount: true);
        Assert.That(actual, Is.EqualTo(236));
    }
    
    [Test]
    public void FenceCosts_BySideExampleFour_ReturnsCost()
    {
        var garden = GetGarden(
            """
                AAAAAA
                AAABBA
                AAABBA
                ABBAAA
                ABBAAA
                AAAAAA
                """.Split("\n"));
        var actual = FenceCosts(garden, bulkDiscount: true);
        Assert.That(actual, Is.EqualTo(368));
    }
    
    [Test]
    public void FenceCosts_BySideExampleFive_ReturnsCost()
    {
        var garden = GetGarden(
            """
                RRRRIICCFF
                RRRRIICCCF
                VVRRRCCFFF
                VVRCCCJFFF
                VVVVCJJCFE
                VVIVCCJJEE
                VVIIICJJEE
                MIIIIIJJEE
                MIIISIJEEE
                MMMISSJEEE
                """.Split("\n"));
        var actual = FenceCosts(garden, bulkDiscount: true);
        Assert.That(actual, Is.EqualTo(1206));
    }
}