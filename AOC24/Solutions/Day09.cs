using System.Diagnostics;
using AOC24.Utils;

namespace AOC24.Solutions;

public record FileSpan(int BlockIndex, int Id, int Size)
{
    public int BlockIndex = BlockIndex;
}

public record FreeSpaceSpan(int BlockIndex, int Size)
{
    public int BlockIndex = BlockIndex;
    public int Size = Size;
}

public static class Day09
{
    public static void Solve()
    {
        var input = InputReader.GetText("Day09.txt");
        var diskmap = input.Select(c => int.Parse(c.ToString())).ToArray();
        
        var spans = Defrag1(diskmap);
        var checksum = Checksum(spans);
        Console.WriteLine($"checksum: {checksum}");

        spans = Defrag2(diskmap);
        checksum = Checksum(spans);
        Console.WriteLine($"checksum: {checksum}");
    }
    
    public static long Checksum(FileSpan[] spans) =>
        spans.Select(f => Enumerable.Range(f.BlockIndex, f.Size).Sum(blockIndex => f.Id * (long)blockIndex)).Sum();

    public static FileSpan[] Defrag1(int[] diskmap)
    {
        var freeSpaces = FreeSpaceBlocks(diskmap).ToArray();
        var files = FileBlocks(diskmap).Reverse().ToArray();
        var fileBlockCount = diskmap.Where((_, i) => int.IsEvenInteger(i)).Sum();
        foreach (var (free, file) in freeSpaces.Zip(files))
        {
            if (free.BlockIndex >= fileBlockCount) break;
            file.BlockIndex = free.BlockIndex; // no need to update freespace block index as it doesn't get used to solve problem
        }

        return files;
    }

    public static FileSpan[] Defrag2(int[] diskmap)
    {
        var freespaces = FreeSpaceSpans(diskmap).ToArray();
        var files = FileSpans(diskmap).Reverse().ToArray();
        foreach (var file in files)
        {
            var freespace = freespaces.FirstOrDefault(free => free.Size >= file.Size && free.BlockIndex < file.BlockIndex);
            if (freespace == null) continue;
            file.BlockIndex = freespace.BlockIndex;
            freespace.BlockIndex += file.Size;
            freespace.Size -= file.Size;
            // no need to add free space where file used to be as it's not used to solve the problem
        }

        return files;
    }

    public static IEnumerable<FileSpan> FileBlocks(int[] diskmap)
    {
        return diskmap.SelectMany((size, i) =>
        {
            if (int.IsOddInteger(i)) return [];
            var blockIndex = diskmap[..i].Sum();
            return Enumerable.Range(blockIndex, size).Select(index => new FileSpan(index, i / 2, 1));
        });
    }
    
    public static IEnumerable<FreeSpaceSpan> FreeSpaceBlocks(int[] diskmap)
    {
        return diskmap.SelectMany((size, i) =>
        {
            if (int.IsEvenInteger(i)) return [];
            var blockIndex = diskmap[..i].Sum();
            return Enumerable.Range(blockIndex, size).Select(index => new FreeSpaceSpan(index, 1));
        });
    }

    public static IEnumerable<FileSpan> FileSpans(int[] diskmap)
    {
        return diskmap
            .Select((size, i) => int.IsEvenInteger(i) ? new FileSpan(diskmap[..i].Sum(), i / 2, size) : null)
            .OfType<FileSpan>();
    }

    public static IEnumerable<FreeSpaceSpan> FreeSpaceSpans(int[] diskmap)
    {
        return diskmap
            .Select((size, i) => int.IsOddInteger(i) ? new FreeSpaceSpan(diskmap[..i].Sum(), size) : null)
            .OfType<FreeSpaceSpan>();
    }

    public static string FileBlocksString(FileSpan[] files)
    {
        string[] res = new string[files.Length];
        foreach (var file in files)
        {
            res[file.BlockIndex] = file.Id.ToString();
        }

        Console.WriteLine(string.Join("", res));
        return string.Join("", res);
    }
}