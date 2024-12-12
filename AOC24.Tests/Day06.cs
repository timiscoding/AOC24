using AOC24.Utils;
using AOC24.Solutions;

namespace AOC24.Tests;

public class Day06Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Guard_CreateInstance_ShouldSetGuardOrigin()
    {
        var map = new [,]
        {
            {'.', '.', '.'},
            {'.', '.', '^'},
            {'.', '.', '.'}
        };

        Guard g = new Guard( map );
        Assert.That(g.Origin, Is.EqualTo(new Point(2, 1)));
    }
    
    [Test]
    public void GuardGetPath_ShouldReturnDistinctPositionsVisited()
    {
        var map = new [,]
        {
            {'#', '.'},
            {'.', '#'},
            {'^', '.'},
        };
        Guard g = new Guard(map);
        
        HashSet<Point> visited = g.GetPath();
        
        Assert.That(visited.Count, Is.EqualTo(2));
    }
    
    [Test]
    public void GuardGetPath_ShouldResetGuardOrigin()
    {
        var map = new [,]
        {
            {'#', '.'},
            {'.', '#'},
            {'^', '.'},
        };
        Guard g = new Guard(map);
        
        g.GetPath();
        
        Assert.That(g.Pos, Is.EqualTo(g.Origin));
        Assert.That(g.Dir, Is.EqualTo(Heading.Up));
    }
    
    [Test]
    public void GuardIsStuck_PathHasLoop_ShouldReturnTrue()
    {
        var map = new [,]
        {
            {'.', '#', '.', '.'},
            {'.', '.', '.', '#'},
            {'.', '.', '.', '.'},
            {'.', '^', '#', '.'},
        };
        Guard g = new Guard(map);
        var stuck = g.IsStuck(new Point(0, 2));
        Assert.That(stuck, Is.True);
    }
    
    [Test]
    public void GuardIsStuck_PathHasNoLoop_ShouldReturnFalse()
    {
        var map = new [,]
        {
            {'.', '#', '.', '.'},
            {'.', '.', '.', '#'},
            {'.', '.', '.', '.'},
            {'.', '^', '#', '.'},
        };
        Guard g = new Guard(map);
        var stuck = g.IsStuck(new Point(0, 0));
        Assert.That(stuck, Is.False);
    }
    
    [Test]
    public void GuardIsStuck_ShouldResetGuardToOrigin()
    {
        var map = new [,]
        {
            {'.', '#', '.', '.'},
            {'.', '.', '.', '#'},
            {'.', '.', '.', '.'},
            {'.', '^', '#', '.'},
        };
        Guard g = new Guard(map);
        
        g.IsStuck(new Point(0, 2));
        
        Assert.That(g.Pos, Is.EqualTo(g.Origin));
        Assert.That(g.Dir, Is.EqualTo(Heading.Up));

    }
}