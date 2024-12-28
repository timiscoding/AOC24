using System.Numerics;
using static AOC24.Solutions.Day15;

namespace AOC24.Tests;

[TestFixture]
public class Day15Tests
{
    [Test]
    public void Move_LeftToSpace()
    {
        var input = ".@";
        var warehouse = GetMap(input);
        var pos = Move(warehouse, new Complex(1, 0), Left);

        Assert.That(pos, Is.EqualTo(new Complex(0, 0)));
        Assert.That(warehouse.Map[new Complex(0, 0)], Is.EqualTo(RobotTile));
        Assert.That(warehouse.Map[new Complex(1, 0)], Is.EqualTo(SpaceTile));
    }
    
    [Test]
    public void Move_LeftToWall()
    {
        var input = "#@";
        var warehouse = GetMap(input);
        var pos = Move(warehouse, new Complex(1, 0), Left);
        Assert.That(pos, Is.EqualTo(new Complex(1, 0)));
        Assert.That(warehouse.Map[new Complex(0, 0)], Is.EqualTo(WallTile));
        Assert.That(warehouse.Map[new Complex(1, 0)], Is.EqualTo(RobotTile));
    }
    
    [Test]
    public void Move_LeftBoxNextToSpace()
    {
        var input = ".O@";
        var warehouse = GetMap(input);
        Move(warehouse, new Complex(2, 0), Left);
        Assert.That(AsString(warehouse), Is.EqualTo("O@."));
    }
    
    [Test]
    public void Move_LeftBoxNextToWall()
    {
        var input = "#O@";
        var warehouse = GetMap(input);
        var pos = Move(warehouse, new Complex(2, 0), Left);
        Assert.That(AsString(warehouse), Is.EqualTo("#O@"));
        Assert.That(pos, Is.EqualTo(new Complex(2, 0)));
    }
    
    [Test]
    public void Move_LeftBoxes()
    {
        var input = ".OO@";
        var warehouse = GetMap(input);
        var pos = Move(warehouse, new Complex(3, 0), Left);
        Assert.That(AsString(warehouse), Is.EqualTo("OO@."));
        Assert.That(pos, Is.EqualTo(new Complex(2, 0)));
    }
    
    [Test]
    public void Move_LeftBoxesNextToWall()
    {
        var input = "#OO@";
        var warehouse = GetMap(input);
        var pos = Move(warehouse, new Complex(3, 0), Left);
        Assert.That(AsString(warehouse), Is.EqualTo("#OO@"));
        Assert.That(pos, Is.EqualTo(new Complex(3, 0)));
    }


    [Test]
    public void Move_Up()
    {
        var input = """
                    .
                    O
                    O
                    @
                    """;
        var warehouse = GetMap(input);
        var pos = Move(warehouse, new Complex(0, -3), Up);
        var expected = """
                       O
                       O
                       @
                       .
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
        Assert.That(pos, Is.EqualTo(new Complex(0, -2)));
    }
    
    [Test]
    public void Move_Down()
    {
        var input = """
                    @
                    O
                    O
                    .
                    """;
        var warehouse = GetMap(input);
        var pos = Move(warehouse, new Complex(0, 0), Down);
        var expected = """
                       .
                       @
                       O
                       O
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
        Assert.That(pos, Is.EqualTo(new Complex(0, -1)));
    }
    
    [Test]
    public void Move_Right()
    {
        var input = "@OO.";
        var warehouse = GetMap(input);
        var pos = Move(warehouse, new Complex(0, 0), Right);
        Assert.That(AsString(warehouse), Is.EqualTo(".@OO"));
        Assert.That(pos, Is.EqualTo(new Complex(1, 0)));
    }
    
    [Test]
    public void Move_LeftBoxNextToSpaceInDoubleWidthMap()
    {
        var input = "..[]@."; 
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(4, 0), Left);
        Assert.That(AsString(warehouse), Is.EqualTo(".[]@.."));
    }
    
    [Test]
    public void Move_LeftBoxNextToWallInDoubleWidthMap()
    {
        var input = "##[]@.";
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(4, 0), Left);
        Assert.That(AsString(warehouse), Is.EqualTo("##[]@."));
    }
    
    [Test]
    public void Move_UpBoxInDoubleWidthMap()
    {
        var input = """
                    ..
                    []
                    @.
                    """; 
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(0, -2), Up);
        var expected = """
                       []
                       @.
                       ..
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
    }
    
    [Test]
    public void Move_UpBoxWithWallInDoubleWidthMap()
    {
        var input = """
                    ##
                    []
                    .@
                    """; 
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(1, -2), Up);
        var expected = """
                       ##
                       []
                       .@
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
    }
    
    [Test]
    public void Move_UpBoxWithOneWallInDoubleWidthMap()
    {
        var input = """
                    #.
                    []
                    .@
                    """; 
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(1, -2), Up);
        var expected = """
                       #.
                       []
                       .@
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
    }
    
    [Test]
    public void Move_UpBoxWithOneWallInDoubleWidthMap2()
    {
        var input = """
                    .#
                    []
                    .@
                    """; 
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(1, -2), Up);
        var expected = """
                       .#
                       []
                       .@
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
    }
    
    [Test]
    public void Move_UpBoxesInDoubleWidthMap()
    {
        var input = """
                    ..
                    []
                    []
                    @.
                    """; 
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(0, -3), Up);
        var expected = """
                       []
                       []
                       @.
                       ..
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
    }
    
    [Test]
    public void Move_UpBoxesInDoubleWidthMap2()
    {
        var input = """
                    ...
                    [].
                    .[]
                    ..@
                    """; 
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(2, -3), Up);
        var expected = """
                       [].
                       .[]
                       ..@
                       ...
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
    }
    
    [Test]
    public void Move_UpBoxesInDoubleWidthMap3()
    {
        var input = """
                    ....
                    [][]
                    .[].
                    ..@.
                    """; 
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(2, -3), Up);
        var expected = """
                       [][]
                       .[].
                       ..@.
                       ....
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
    }
    
    [Test]
    public void Move_UpBoxesWithWallInDoubleWidthMap()
    {
        var input = """
                    ...#
                    [][]
                    .[].
                    ..@.
                    """; 
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(2, -3), Up);
        var expected = """
                       ...#
                       [][]
                       .[].
                       ..@.
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
    }
    
    [Test]
    public void Move_UpOffsetBoxesInDoubleWidthMap()
    {
        var input = """
                    ...
                    .[]
                    [].
                    .[]
                    ..@
                    """; 
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(2, -4), Up);
        var expected = """
                       .[]
                       [].
                       .[]
                       ..@
                       ...
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
    }
    
    [Test]
    public void Move_UpOffsetBoxesWithWallInDoubleWidthMap()
    {
        var input = """
                    ...
                    .[]
                    []#
                    .[]
                    ..@
                    """; 
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(2, -4), Up);
        var expected = """
                       ...
                       .[]
                       []#
                       .[]
                       ..@
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
    }
    
    [Test]
    public void Move_UpOffsetBoxesWithWallInDoubleWidthMap2()
    {
        var input = """
                    ...
                    #[]
                    [].
                    .[]
                    ..@
                    """; 
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(2, -4), Up);
        var expected = """
                       ...
                       #[]
                       [].
                       .[]
                       ..@
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
    }
    
    [Test]
    public void Move_UpBoxesInDoubleWidthMap_ShouldOnlyMoveCertainBoxes()
    {
        var input = """
                    ..[]..
                    []..[]
                    .[][].
                    [][][]
                    ...@..
                    """; 
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(3, -4), Up);
        var expected = """
                       [][][]
                       .[][].
                       ..[]..
                       [].@[]
                       ......
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
    }
    
    [Test]
    public void Move_DownBoxesInDoubleWidthMap_ShouldOnlyMoveCertainBoxes()
    {
        var input = """
                    ...@..
                    [][][]
                    .[][].
                    []..[]
                    ..[]..
                    """; 
        var warehouse = GetMap(input);
        warehouse.DoubleWidth = true;
        Move(warehouse, new Complex(3, 0), Down);
        var expected = """
                       ......
                       [].@[]
                       ..[]..
                       .[][].
                       [][][]
                       """;
        Assert.That(AsString(warehouse), Is.EqualTo(expected));
    }
}