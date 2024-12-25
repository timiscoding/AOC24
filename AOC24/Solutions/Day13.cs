using System.Text.RegularExpressions;
using AOC24.Utils;
using LinearSystem = (AOC24.Solutions.Matrix2X2 claws, AOC24.Solutions.Matrix2X1 prize);

namespace AOC24.Solutions;

public record struct Matrix2X2(double M11, double M12, double M21, double M22)
{
    public static Matrix2X2 Inverse(Matrix2X2 m)
    {
        double determinant = m.M11 * m.M22 - m.M12 * m.M21;
        if (determinant == 0) throw new InvalidOperationException("Determinant cannot be 0");
        var invDet = 1.0d / determinant;
        return new Matrix2X2(
            m.M22 * invDet, -m.M12 * invDet, 
            -m.M21 * invDet, m.M11 * invDet);
    }

    public static Matrix2X1 Multiply(Matrix2X2 m, Matrix2X1 v)
    {
        var a = m.M11 * v.M11 + m.M12 * v.M21;
        var b = m.M21 * v.M11 + m.M22 * v.M21;
        // remove rounding errors when dealing with float arithmetic. 
        // The rounding precision was selected by trial and error until I got the right answer :/
        return new Matrix2X1(Math.Round(a, 3), Math.Round(b, 3)); 
    }
}

public record struct Matrix2X1(double M11, double M21) { }

public static class Day13
{
    public const int TokenACost = 3;
    public const int TokenBCost = 1;
    public static void Solve()
    {
        var input = InputReader.GetText("Day13.txt");
        var machines = GetLinearSystem(input);
        Console.WriteLine($"Day 13 Part 1 - cost to win: {machines.Sum(TokensToWin)}");
        machines = GetLinearSystem(input, prizeOffset: 10000000000000); 
        Console.WriteLine($"Day 13 Part 2 - cost to win: {machines.Sum(TokensToWin)}");
    }

    /*
     * Gets the total token cost to win the prize. Output 0 if the solutions to the linear system are non integral.
     *
     * The solution is calculated using linear algebra Mv = P where M are the button coords, P is the prize coords
     *
     * Solving for v gives the scalars:
     * v = inverse(M) * P
     *
     * I could've used a linear system solver like MathNet but I decided to implement it manually.
     */
    public static double TokensToWin(LinearSystem machine)
    {
        var pushes = Matrix2X2.Multiply(Matrix2X2.Inverse(machine.claws), machine.prize);
        var a = pushes.M11;
        var b = pushes.M21;
        if (!(double.IsInteger(a) && double.IsInteger(b))) return 0;
        return TokenACost * a + TokenBCost * b;
    }
    
    /*
     * Converts input into a linear equation system in matrix form
     *
     * Essentially we are solving for xA + yB = P where A, B and P are vectors and x, y are scalars. In matrix form:
     * 
     * Matrix M are the coords of buttons A and B
     * X_a X_b
     * Y_a Y_b
     *
     * Matrix P are the prize coords
     * X_p
     * Y_p
     */
    public static IEnumerable<LinearSystem> GetLinearSystem(string input, long prizeOffset = 0)
    {
        foreach (var machine in input.Split("\n\n"))
        {
            var nums = machine.Split('\n', ',').Select(token => double.Parse(Regex.Match(token, @"\d+").Value)).ToArray();
            yield return (new Matrix2X2(nums[0], nums[2], nums[1], nums[3]), new Matrix2X1(nums[4] + prizeOffset, nums[5] + prizeOffset));
        }
    }
}