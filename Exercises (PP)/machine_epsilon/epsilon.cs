using System;

class EpsilonTasks
{
    /// <summary>
    /// Task 1: Find the Maximum and Minimum Representable Integers
    /// 
    /// This function determines:
    /// - The largest integer that can be stored before overflow occurs.
    /// - The smallest integer that can be stored before underflow occurs.
    /// 
    /// It uses a `while` loop to increment and decrement an integer until it no longer changes.
    /// The results are compared with `int.MaxValue` and `int.MinValue`.
    /// </summary>
    public static void FindMaxMinIntegers()
    {
        int max = 1;
        while (max + 1 > max)
        {
            max++;
        }
        Console.WriteLine("Max Integer Found: " + max);
        Console.WriteLine("Expected int.MaxValue: " + int.MaxValue);

        int min = -1;
        while (min - 1 < min)
        {
            min--;
        }
        Console.WriteLine("Min Integer Found: " + min);
        Console.WriteLine("Expected int.MinValue: " + int.MinValue);
    }

    /// <summary>
    /// Task 2: Calculate Machine Epsilon
    /// 
    /// Machine epsilon (𝜀) is the smallest number that, when added to 1.0, produces a result different from 1.0.
    /// This determines the precision limit of floating-point arithmetic.
    /// 
    /// - For `double`, the expected epsilon is `2^-52` (~2.22e-16).
    /// - For `float`, the expected epsilon is `2^-23` (~1.19e-7).
    /// 
    /// The function iteratively divides epsilon by 2 until `1.0 + epsilon == 1.0`, then it takes the last valid epsilon.
    /// </summary>
    public static void CalculateEpsilon()
    {
        double epsDouble = 1.0;
        while (1.0 + epsDouble > 1.0)
        {
            epsDouble /= 2.0;
        }
        epsDouble *= 2.0;  // Restore last valid value

        float epsFloat = 1.0f;
        while (1.0f + epsFloat > 1.0f)
        {
            epsFloat /= 2.0f;
        }
        epsFloat *= 2.0f;  // Restore last valid value

        Console.WriteLine($"Machine Epsilon (double): {epsDouble} (Expected: {Math.Pow(2, -52)})");
        Console.WriteLine($"Machine Epsilon (float): {epsFloat} (Expected: {Math.Pow(2, -23)})");
    }

    /// <summary>
    /// Task 3: Compare Two Doubles Using Machine Epsilon
    /// 
    /// This function demonstrates that floating-point arithmetic is imprecise.
    /// - It creates two numbers: `a = 1.0 + (1 / 2^52)`, which is slightly greater than `1.0`.
    /// - It compares `a` and `b` using `==`, which may lead to unexpected results.
    /// </summary>
    public static void TestComparison()
    {
        double invEpsilon = Math.Pow(2, 52);
        double a = 1.0 + 1.0 / invEpsilon;
        double b = 1.0;

        Console.WriteLine($"a: {a}");
        Console.WriteLine($"b: {b}");

        if (a == b)
            Console.WriteLine("a == b (Unexpected result: floating-point precision error!)");
        else if (a > b)
            Console.WriteLine("a > b (Expected: a is slightly greater than b)");
        else
            Console.WriteLine("a < b (Unexpected result!)");

        // Demonstrating floating-point errors with decimal fractions
        double d1 = 0.1 + 0.1 + 0.1 + 0.1 + 0.1 + 0.1 + 0.1 + 0.1 + 0.1 + 0.1; // Expected: 1.0
        double d2 = 1.0;

        Console.WriteLine($"d1: {d1}");
        Console.WriteLine($"d2: {d2}");
        Console.WriteLine($"d1 == d2: {d1 == d2} (Expected: false due to floating-point errors)");
        Console.WriteLine($"ApproxEqual(d1, d2): {ApproxEqual(d1, d2)} (Expected: true, using proper comparison)");
    }

    /// <summary>
    /// Task 4: Approximate Equality Comparison Function
    /// 
    /// Since floating-point numbers are imprecise, direct comparisons (==) can fail.
    /// This function compares two numbers with:
    /// - Absolute precision (useful when numbers are close to zero).
    /// - Relative precision (useful when numbers have large magnitudes).
    /// 
    /// Returns `true` if numbers are close enough, `false` otherwise.
    /// </summary>
    public static bool ApproxEqual(double a, double b, double acc = 1e-9, double eps = 1e-9)
    {
        if (Math.Abs(a - b) < acc) return true; // Absolute difference check
        return Math.Abs(a - b) < Math.Max(Math.Abs(a), Math.Abs(b)) * eps; // Relative precision check
    }
}
