using System.Numerics;
using static AOC24.Solutions.Day15;

namespace AOC24.Tests;

[TestFixture]
public class Day15Tests
{
    [Test]
    public void Move_LeftToSpace()
    {
        var input = " @";
        var map = GetMap(input);
        var pos = Move(map, new Complex(1, 0), Left);

        Assert.That(pos, Is.EqualTo(new Complex(0, 0)));
        Assert.That(map[new Complex(0, 0)], Is.EqualTo(RobotTile));
        Assert.That(map[new Complex(1, 0)], Is.EqualTo(SpaceTile));
    }
    
    [Test]
    public void Move_LeftToWall()
    {
        var input = "#@";
        var map = GetMap(input);
        var pos = Move(map, new Complex(1, 0), Left);
        Assert.That(pos, Is.EqualTo(new Complex(1, 0)));
        Assert.That(map[new Complex(0, 0)], Is.EqualTo(WallTile));
        Assert.That(map[new Complex(1, 0)], Is.EqualTo(RobotTile));
    }
    
    [Test]
    public void Move_LeftBoxNextToSpace()
    {
        var input = " O@";
        var map = GetMap(input);
        var pos = Move(map, new Complex(2, 0), Left);
        var expected = new Dictionary<Complex, char>
        {
            { new Complex(1, 0), RobotTile },
            { new Complex(0, 0), BoxTile },
            { new Complex(2, 0), SpaceTile },
        };
        CollectionAssert.AreEquivalent(expected, map);
    }
    
    [Test]
    public void Move_LeftBoxNextToWall()
    {
        var input = "#O@";
        var map = GetMap(input);
        var pos = Move(map, new Complex(2, 0), Left);
        var expected = new Dictionary<Complex, char>
        {
            { new Complex(0, 0), WallTile },
            { new Complex(1, 0), BoxTile },
            { new Complex(2, 0), RobotTile },
        };
        CollectionAssert.AreEquivalent(expected, map);
        Assert.That(pos, Is.EqualTo(new Complex(2, 0)));
    }
    
    [Test]
    public void Move_LeftBoxes()
    {
        var input = " OO@";
        var map = GetMap(input);
        var pos = Move(map, new Complex(3, 0), Left);
        var expected = new Dictionary<Complex, char>
        {
            { new Complex(0, 0), BoxTile },
            { new Complex(1, 0), BoxTile },
            { new Complex(2, 0), RobotTile },
            { new Complex(3, 0), SpaceTile },
        };
        CollectionAssert.AreEquivalent(expected, map);
        Assert.That(pos, Is.EqualTo(new Complex(2, 0)));
    }
    
    [Test]
    public void Move_LeftBoxesNextToWall()
    {
        var input = "#OO@";
        var map = GetMap(input);
        var pos = Move(map, new Complex(3, 0), Left);
        var expected = new Dictionary<Complex, char>
        {
            { new Complex(0, 0), WallTile },
            { new Complex(1, 0), BoxTile },
            { new Complex(2, 0), BoxTile },
            { new Complex(3, 0), RobotTile },
        };
        CollectionAssert.AreEquivalent(expected, map);
        Assert.That(pos, Is.EqualTo(new Complex(3, 0)));
    }


    [Test]
    public void Move_Up()
    {
        var input = """
                     
                    O
                    O
                    @
                    """;
        var map = GetMap(input);
        var pos = Move(map, new Complex(0, -3), Up);
        var expected = new Dictionary<Complex, char>
        {
            { new Complex(0, 0), BoxTile },
            { new Complex(0, -1), BoxTile },
            { new Complex(0, -2), RobotTile },
            { new Complex(0, -3), '.' },
        };
        CollectionAssert.AreEquivalent(expected, map);
        Assert.That(pos, Is.EqualTo(new Complex(0, -2)));
    }
    
    [Test]
    public void Move_Down()
    {
        var input = """
                    @
                    O
                    O
                     
                    """;
        var map = GetMap(input);
        var pos = Move(map, new Complex(0, 0), Down);
        var expected = new Dictionary<Complex, char>
        {
            { new Complex(0, 0), SpaceTile },
            { new Complex(0, -1), RobotTile },
            { new Complex(0, -2), BoxTile },
            { new Complex(0, -3), BoxTile },
        };
        CollectionAssert.AreEquivalent(expected, map);
        Assert.That(pos, Is.EqualTo(new Complex(0, -1)));
    }
    
    [Test]
    public void Move_Right()
    {
        var input = "@OO ";
        var map = GetMap(input);
        var pos = Move(map, new Complex(0, 0), Right);
        var expected = new Dictionary<Complex, char>
        {
            { new Complex(0, 0), SpaceTile },
            { new Complex(1, 0), RobotTile },
            { new Complex(2, 0), BoxTile },
            { new Complex(3, 0), BoxTile },
        };
        CollectionAssert.AreEquivalent(expected, map);
        Assert.That(pos, Is.EqualTo(new Complex(1, 0)));
    }
}