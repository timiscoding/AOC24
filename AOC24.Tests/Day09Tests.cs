using AOC24.Solutions;

namespace AOC24.Tests;

[TestFixture]
public class Day09Tests
{
    [Test]
    public void FileBlocks_ReturnsCorrectValues()
    {
        int[] diskmap = [1, 2, 3, 4];
        var spans = Day09.FileBlocks(diskmap);
        var expected = new FileSpan[]
        {
            new(0, 0, 1), 
            new(3, 1, 1),
            new(4, 1, 1),
            new(5, 1, 1)
        };
        
        CollectionAssert.AreEqual(expected, spans);
    }
    
    [Test]
    public void FreeSpaceBlocks_ReturnsCorrectValues()
    {
        int[] diskmap = [1, 2, 3, 4];
        var spans = Day09.FreeSpaceBlocks(diskmap);
        var expected = new FreeSpaceSpan[]
        {
            new(1, 1), 
            new(2, 1),
            new(6, 1),
            new(7, 1),
            new(8, 1),
            new(9, 1),
        };
        
        CollectionAssert.AreEqual(expected, spans);
    }
    
    [Test]
    // Test edge case when defrag should stop moving blocks
    // In this test there is a free space right at the file block count
    // which is used to check when to stop moving blocks
    public void Defrag1_FreeSpaceAtFileBlockCount_ReturnsCorrectValues()
    {
        int[] diskmap = [1, 2, 3, 4, 5]; // block representation 0..111....22222
        var spans = Day09.Defrag1(diskmap);
        
        Assert.That(Day09.FileBlocksString(spans), Is.EqualTo("022111222"));
    }
    
    [Test]
    public void Defrag1_ReturnsCorrectValues()
    {
        int[] diskmap = [2, 3, 3, 3, 1, 3, 3, 1, 2]; // block representation 00...111...2...333.44
        var spans = Day09.Defrag1(diskmap);
        
        Assert.That(Day09.FileBlocksString(spans), Is.EqualTo("00443111332"));
    }
    
    [Test]
    public void Checksum_ReturnsValue()
    {
        var spans = new FileSpan[]
        {
            new(0, 0, 2),
            new(2, 1, 3),
            new(5, 2, 2),
        };
        
        Assert.That(Day09.Checksum(spans), Is.EqualTo(31));
    }
    
    [Test]
    public void Checksum_Defrag1_ReturnsValue()
    {
        // block representation 00...111...2...333.44.5555.6666.777.888899
        int[] diskmap = "2333133121414131402".Select(c => int.Parse(c.ToString())).ToArray(); 
        var spans = Day09.Defrag1(diskmap);
        
        Assert.That(Day09.Checksum(spans), Is.EqualTo(1928));
    }
    
    [Test]
    public void FreeSpaceSpans_ReturnsCorrectValues()
    {
        int[] diskmap = [1, 2, 3, 4];
        var spans = Day09.FreeSpaceSpans(diskmap);
        var expected = new FreeSpaceSpan[] { new(1, 2), new(6, 4) };
        
        CollectionAssert.AreEqual(expected, spans);
    }
    
    [Test]
    public void FileSpans_ReturnsCorrectValues()
    {
        int[] diskmap = [1, 2, 3, 4];
        var spans = Day09.FileSpans(diskmap);
        var expected = new FileSpan[] { new(0, 0, 1), new(3, 1, 3) };
        
        CollectionAssert.AreEqual(expected, spans);
    }
    
    [Test]
    public void Defrag2_MoveWholeFilesToFreeSpaces_ReturnsCorrectValues()
    {
        int[] diskmap = [2, 3, 3, 3, 1, 3, 3, 1, 2]; // block representation 00...111...2...333.44
        var spans = Day09.Defrag2(diskmap);
        var expected = new FileSpan[]
        {
            new(2, 4, 2),
            new(8, 3, 3),
            new(4, 2, 1),
            new(5, 1, 3),
            new(0, 0, 2)
        };
        
        CollectionAssert.AreEquivalent(expected, spans);
    }
    
    [Test]
    public void Checksum_Defrag2_ReturnsValue()
    {
        // block representation 00...111...2...333.44.5555.6666.777.888899
        int[] diskmap = "2333133121414131402".Select(c => int.Parse(c.ToString())).ToArray(); 
        var spans = Day09.Defrag2(diskmap);
        
        Assert.That(Day09.Checksum(spans), Is.EqualTo(2858));
    }
}