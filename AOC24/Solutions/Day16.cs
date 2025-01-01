using System.Numerics;
using AOC24.Utils;
using Maze = System.Collections.Generic.Dictionary<System.Numerics.Complex, char>;
namespace AOC24.Solutions;

/*  Pos: x,y coord in maze
    Dir: direction node is facing which is relative to previous node it came from
    Distance: tentative shortest distance to this node from the start node
    */
public record struct Node(Complex Pos, Complex Dir, int Distance) { }

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
        
        ShortestPath(maze);
    }

    public static void ShortestPath(Maze maze)
    {
        var q = new PriorityQueue<Node, int>(); // the priority value is the shortest distance so far for that node
        // stores the shortest distance from 'start' to a node. During path finding, it stores tentative shortest paths
        // and as the algorithm runs, final shortest paths are progressively updated
        var dist = new Dictionary<Node, int>();       
        var prev = new Dictionary<Node, Node>(); // stores the 'previous' node v for a given node u that forms the chain of the shortest path
        var visited = new HashSet<Complex>();
        
        var start = maze.Keys.Single(p => maze[p] == 'S');
        var end = maze.Keys.Single(p => maze[p] == 'E');
        var startNode = new Node(start, Right, 0);
        
        q.Enqueue(startNode, 0);
        dist[startNode] = 0;
        
        while (q.Count > 0)
        {
            var node = q.Dequeue();

            /* in dijkstra, distances are updated only if they are shorter than what is currently stored. But dotnet doesn't
             * have a method to update a node priority so instead we add all of them, which means there will be stale/older nodes
             * with longer distances which we don't want to process
             */
            if (node.Distance > dist[node]) continue; 
            
            visited.Add(node.Pos);

            if (node.Pos == end)
            {
                Console.WriteLine($"Day 16 Part 1 - shortest path: {dist[node]}");
                var cur = prev[node];
                var path = new List<Node>();
                while (cur != startNode)
                {
                    path.Add(cur);
                    cur = prev[cur];
                }
            
                Print(maze, path);
                return;
            }
            foreach (var dir in new[] { Up, Right, Left, Down })
            {
                if (!maze.ContainsKey(node.Pos + dir) || visited.Contains(node.Pos + dir)) continue;
                var edgeWeight = dir == node.Dir ? 1 : 1001;
                var newDistance = dist[node] + edgeWeight;
                var newNode = new Node(node.Pos + dir, dir, newDistance);
                dist[newNode] = newDistance;
                prev[newNode] = node;
                q.Enqueue(newNode, newDistance);
            }
        }
    }
    
    public static Dictionary<Complex, char> PathArrows(IEnumerable<Node> path)
    {
        return path.Select(node =>
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

    public static void Print(Maze maze, IEnumerable<Node> path)
    {
        Console.WriteLine(AsString(maze, path));
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

        return res;
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