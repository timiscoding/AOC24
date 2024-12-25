using System.Dynamic;

namespace AOC24.Utils;

public static class InputReader
{
    public static IEnumerable<List<int>> ReadInputAsIntList(string file)
    {
        var lines = GetLines(file);
        foreach (var row in lines)
        {
            yield return row.Split().Select(int.Parse).ToList();
        }
    }
    
    public static IEnumerable<dynamic> ReadInputAsDynamic(string file, Dictionary<string, Type> columnTypeMap, string? delimit = " ")
    {
        var rows = new List<dynamic>();
        var lines = GetLines(file);
        
        foreach (var line in lines)
        {
            dynamic row = new ExpandoObject();
            var columns = line.Split(delimit, StringSplitOptions.RemoveEmptyEntries);
            var rowDict = row as IDictionary<string, object>;

            var columnNames = columnTypeMap.Keys.ToArray();
            
            for (int i = 0; i < columns.Length; i++)
            {
                var value = columns[i];
                
                if (columnTypeMap.TryGetValue(columnNames[i], out var targetType))
                    rowDict[columnNames[i]] = ConvertToType(value, targetType);
            }

            rows.Add(row);
        }

        return rows;
    }

    public static string GetText(string file)
    {
        return File.ReadAllText(GetPath(file)).Trim();
    }
    public static string[] GetLines(string file)
    {
        return File.ReadAllLines(GetPath(file));
    }

    public static char[,] Get2DArray(string file)
    {
        var lines = GetLines(file);
        var grid = new char[lines.Length, lines[0].Length];
        for (int y = 0; y < grid.GetLength(0); y++)
        {
            for (int x = 0; x < grid.GetLength(1); x++)
            {
                grid[y, x] = lines[y][x];
            }
        }

        return grid;
    }
    
    private static string GetPath(string file) => Path.Combine("../../../", "Inputs", file);


    public static object ConvertToType(string value, Type targetType)
    {
        if (targetType == typeof(int)) return int.Parse(value);
        return value;
    }
}