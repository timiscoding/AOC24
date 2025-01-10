using AOC24.Solutions;

namespace AOC24.Tests;

using static AOC24.Solutions.Day19;

[TestFixture]
public class Day19Tests
{

    [Test]
    public void Trie_InsertWord_TrieContainsWord()
    {
        var trie = new Trie();
        trie.Insert("ab");
        Assert.That(trie.Root.Children, Contains.Key('a'));
        Assert.That(trie.Root.Children['a'].Children, Contains.Key('b'));
        Assert.That(trie.Root.Children['a'].Children['b'].IsTerminal, Is.True);
    }
    
    [Test]
    public void Trie_ContainsWord_ReturnsTrue()
    {
        var trie = new Trie();
        trie.Insert("ab");
        Assert.That(trie.Contains("ab"), Is.True);
    }
    
    [Test]
    public void Trie_IsPrefix_ReturnsTrue()
    {
        var trie = new Trie();
        trie.Insert("abc");
        Assert.That(trie.IsPrefix("ab"), Is.True);
    }

    

    [Test]
    public void Trie_InitialiseWithWords_TrieContainsWords()
    {
        var trie = new Trie(["ab", "abc"]);
        Assert.That(trie.Contains("ab"), Is.True);
        Assert.That(trie.Contains("abc"), Is.True);
    }
    
    [Test]
    public void CanMake_NoSolution_ReturnsFalse()
    {
        var patterns = new Trie(["g"]);
        var actual = CanMake(patterns, "gbbr");
        Assert.That(actual, Is.False);
    }

    [Test]
    public void CanMake_SingleSolution_ReturnsTrue()
    {
        var patterns = new Trie(["g", "gb", "bb", "r"]);
        var actual = CanMake(patterns, "gbbr");
        Assert.That(actual, Is.True);
    }
    
    [Test]
    public void CanMake_SingleSolution2_ReturnsTrue()
    {
        var patterns = new Trie(["g", "gb", "br"]);
        var actual = CanMake(patterns, "gbbr");
        Assert.That(actual, Is.True);
    }
    
    [Test]
    public void CanMake_MultipleSolutions_ReturnsTrue()
    {
        var patterns = new Trie(["g", "bb", "r", "gb", "br"]);
        var actual = CanMake(patterns, "gbbr");
        Assert.That(actual, Is.True);
    }

    [Test]
    public void Trie_AllWords_ReturnsWords()
    {
        var patterns = new Trie(["can", "cant"]);
        var actual = patterns.AllWords("canton");
        CollectionAssert.AreEqual(new[] { "can", "cant" }, actual);
    }
}