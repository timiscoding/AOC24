using System.Numerics;
using AOC24.Utils;
using Maze = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
namespace AOC24.Solutions;

/*  Pos: x,y coord in maze
    Dir: direction node is facing which is relative to previous node it came from
    Distance: tentative shortest distance to this node from the start node
    */
public struct Node(Complex pos, Complex dir, int distance) : IEquatable<Node>
{
    public Complex Pos = pos;
    public Complex Dir = dir;
    public readonly int Distance = distance;

    public Node(Complex pos) : this(pos, 0, -1) { }

    /* value equality based on Pos only. This is helpful when using a Dictionary with Node keys as
       any new nodes added with the same Pos will update the same key.
     */
    public bool Equals(Node other) => Pos == other.Pos;
    public override bool Equals(object? obj) => obj is Node node && Equals(node);
    public static bool operator ==(Node left, Node right) => left.Equals(right);
    public static bool operator !=(Node left, Node right) => !left.Equals(right);
    public override int GetHashCode() => Pos.GetHashCode();
    public override string ToString()
    {
        return $"<Pos:{Pos}, Dir:{Dir}, Distance:{Distance}>";
    }
}

public static class Day16
{
    public static int Width;
    public static int Height;
    public static Complex Down = -Complex.ImaginaryOne;
    public static Complex Up = Complex.ImaginaryOne;
    public static Complex Left = -1;
    public static Complex Right = 1;
    
    public static void Solve()
    {
        var maze = Parse(InputReader.GetLines("Day16.txt"));
        var start = maze.Keys.Single(p => maze[p] == 'S');
        var end = maze.Keys.Single(p => maze[p] == 'E');
        var path = ShortestPath(maze, start, end, Right);
        Console.WriteLine($"Day 16 Part 1 - shortest path: {path.Last().Distance}");
        Print(maze, path);
        Console.WriteLine($"Day 16 Part 2 - alternative paths node count: {AltPaths(maze, path)}");
    }

    /* Gets the alternative paths to the given path and returns the number of nodes for all the paths.
     * For all nodes on the path, look for neighbours that aren't on the path and find the shortest path
     * from the neighbour to the end node. Since the cost to the node is already known, the alternative path cost is
     * the sum of those parts plus the edge cost between them.
     */
    public static int AltPaths(Maze maze, List<Node> path)
    {
        var end = path.Last().Pos;
        var distance = path.Last().Distance;
        var altPathNodes = new HashSet<Node>(path);
        foreach (var node in path)
        {
            foreach (var dir in new[] { Up, Down, Left, Right })
            {
                var neighbour = node.Pos + dir;
                if (!maze.ContainsKey(neighbour) || path.Contains(new Node(neighbour))) continue;
                var altPath = ShortestPath(maze, neighbour, end, dir);
                var altDistance = node.Distance + GetEdgeWeight(node.Dir, dir) + altPath.Last().Distance;
                if (altDistance == distance)
                {
                    // Print(maze, path.TakeWhile(n => n != node).Concat([node, ..b]));
                    altPathNodes.UnionWith(altPath);
                } 
            }
        }
        return altPathNodes.Count();
    }
    
    /* Find shortest path given the start position, end position and start direction and returns the list of nodes forming
     * the path. Implementation uses Dijkstra's algo */
    public static List<Node> ShortestPath(Maze maze, Complex startPos, Complex endPos, Complex startDir)
    {
        var q = new PriorityQueue<Node, int>(); // the priority value is the shortest distance so far for that node
        // stores the shortest distance from 'start' to a node. During path finding, it stores tentative shortest paths
        // and as the algorithm runs, final shortest paths are progressively updated
        var dist = new Dictionary<Node, int>();       
        var prev = new Dictionary<Node, Node>(); // stores the 'previous' node v for a given node u that forms the chain of the shortest path
        var visited = new HashSet<Complex>();

        var end = new Node(endPos);
        var start = new Node(startPos, startDir, 0);
        
        q.Enqueue(start, 0);
        dist[start] = 0;
        
        while (q.Count > 0)
        {
            var node = q.Dequeue();
            /* in dijkstra, distances are updated only if they are shorter than what is currently stored. But dotnet doesn't
             * have a method to update a node priority so instead we add all of them to the queue, which means there will be stale/older nodes
             * with longer distances which we don't want to process
             */
            if (node.Distance > dist[node]) continue; 
            visited.Add(node.Pos);
            if (node == end) return GetPath(prev, node);
            
            foreach (var dir in new[] { Up, Right, Left, Down })
            {
                if (!maze.ContainsKey(node.Pos + dir) || visited.Contains(node.Pos + dir)) continue;
                var edgeWeight = GetEdgeWeight(node.Dir, dir); 
                var newDistance = dist[node] + edgeWeight;
                var newNode = new Node(node.Pos + dir, dir, newDistance);
                dist[newNode] = newDistance;
                prev[newNode] = node;
                q.Enqueue(newNode, newDistance);
            }
        }
        throw new Exception("Could not find path");
    }

    /* gets the cost of moving between adjacent cells by comparing their directions.
     * If the directions are the same, make 1 step. cost is 1
     * If the directions are at 90 degrees, make 1 turn and 1 step. cost is 1000 + 1
     * If the directions are opposite to one another, make 2 turns and 1 step. cost is 2000 + 1
     */
    public static int GetEdgeWeight(Complex fromDir, Complex toDir)
    {
        var magnitude = (int)(fromDir - toDir).Magnitude;
        return magnitude switch
        {
            2 => 2001,
            1 => 1001,
            _ => 1
        };
    }
    
    public static Dictionary<Complex, char> PathArrows(IEnumerable<Node> path)
    {
        return path.Skip(1).SkipLast(1).Select(node =>
        {
            var dir = node.Dir;
            var arrow = dir switch
            {
                _ when dir == Right => '>',
                _ when dir == Left => '<',
                _ when dir == Down => 'v',
                _ when dir == Up => '^',
                _ => throw new Exception("invalid direction")
            };
            return (node.Pos, arrow);
        }).ToDictionary();
    }
    
    public static List<Node> GetPath(Dictionary<Node, Node> prev, Node last)
    {
        var cur = last;
        var path = new List<Node>();
        while (true)
        {
            path.Add(cur);
            if (!prev.TryGetValue(cur, out cur)) break; 
        }

        path.Reverse();
        
        return path;
    }

    public static void Print(Maze maze, IEnumerable<Node> path)
    {
        Console.WriteLine(" " + string.Concat(Enumerable.Range(0, Width).Select(x => x % 10)));
        Console.WriteLine(string.Join("\n", AsString(maze, path).Split().Select((line, y) => $"{y % 10}{line}")));
    }

    public static void Print(Maze maze)
    {
        Console.WriteLine(AsString(maze, []));
    }

    public static string AsString(Maze maze, IEnumerable<Node> path)
    {
        var pathArrows = PathArrows(path);
        var res = "";
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (pathArrows.TryGetValue(x + y * Down, out var p)) res += p;
                else res += maze.GetValueOrDefault(x + y * Down, '#');
            }
            res += "\n";
        }

        return res.Trim();
    }

    public static Maze Parse(string[] lines)
    {
        Width = lines[0].Length;
        Height = lines.Length;
        return (from y in Enumerable.Range(0, Height)
            from x in Enumerable.Range(0, Width)
            let c = lines[y][x]
            where c != '#'
            select (new Complex(x, -y), c)).ToDictionary();
    }
}