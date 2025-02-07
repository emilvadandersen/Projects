using System;

class MainProgram
{
    static void Main()
    {
        Console.WriteLine("=== Maximum/Minimum Integers ===");
        EpsilonTasks.FindMaxMinIntegers();

        Console.WriteLine("\n=== Machine Epsilon ===");
        EpsilonTasks.CalculateEpsilon();

        Console.WriteLine("\n=== Floating-Point Comparison ===");
        EpsilonTasks.TestComparison();

        Console.WriteLine("\n=== Testing ApproxEqual Function ===");

        // Example test cases for ApproxEqual
        double x = 0.1 + 0.2;
        double y = 0.3;

        Console.WriteLine($"x: {x}, y: {y}");
        Console.WriteLine($"x == y: {x == y} (Direct comparison fails)");
        Console.WriteLine($"ApproxEqual(x, y): {EpsilonTasks.ApproxEqual(x, y)} (Using correct method)");

        double a = 1e10, b = 1e10 + 1;
        Console.WriteLine($"a: {a}, b: {b}");
        Console.WriteLine($"ApproxEqual(a, b): {EpsilonTasks.ApproxEqual(a, b)} (Checks relative precision)");
    }
}
