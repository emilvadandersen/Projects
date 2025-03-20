using static System.Math;
using System;

public static class main
{
    public static void Main()
    {
        double acc = 1e-3;  // Accuracy
        double eps = 1e-3;  // Relative error tolerance

        double a = 0;
        double b = 1;

        Console.WriteLine("Recursive Adaptive Integrator:\n");

        // Integral of sqrt(x) from 0 to 1
        Func<double, double> func = x => Sqrt(x);
        int i = 0;
        double res = integration.integrate(func, a, b, ref i, acc, eps);
        Console.WriteLine($"∫_0^1 √x dx = {res}; expected {2.0 / 3.0}");
        Console.WriteLine($"Absolute Error: {Abs(res - 2.0 / 3.0)}");
        Console.WriteLine($"Function Evaluations: {i}\n");

        // Integral of 1/sqrt(x) from 0 to 1
        i = 0;
        func = x => 1 / Sqrt(x);
        res = integration.integrate(func, a, b, ref i, acc, eps);
        Console.WriteLine($"∫_0^1 1/√x dx = {res}; expected 2");
        Console.WriteLine($"Absolute Error: {Abs(res - 2)}");
        Console.WriteLine($"Function Evaluations: {i}\n");

        // Integral of 4sqrt(1-x²) from 0 to 1 (Quarter circle)
        i = 0;
        func = x => 4 * Sqrt(1 - x * x);
        res = integration.integrate(func, a, b, ref i, acc, eps);
        Console.WriteLine($"∫_0^1 4√(1-x²) dx = {res}; expected {PI}");
        Console.WriteLine($"Absolute Error: {Abs(res - PI)}");
        Console.WriteLine($"Function Evaluations: {i}\n");

        // Integral of ln(x)/sqrt(x) from 0 to 1
        i = 0;
        func = x => Log(x) / Sqrt(x);
        res = integration.integrate(func, a, b, ref i, acc, eps);
        Console.WriteLine($"∫_0^1 ln(x)/√x dx = {res}; expected -4");
        Console.WriteLine($"Absolute Error: {Abs(res - (-4))}");
        Console.WriteLine($"Function Evaluations: {i}\n");

        Console.WriteLine("Clenshaw–Curtis Adaptive Integrator:\n");

        // Clenshaw–Curtis Integration for 1/sqrt(x)
        i = 0;
        func = x => 1 / Sqrt(x); // Ensure correct function is set
        res = integration.clenshawintegrate(func, a, b, ref i, acc, eps);
        Console.WriteLine($"∫_0^1 1/√x dx using Clenshaw–Curtis = {res}; expected 2");
        Console.WriteLine($"Absolute Error: {Abs(res - 2)}");
        Console.WriteLine($"Function Evaluations: {i}");
        Console.WriteLine($"Scipy Reference Evaluations: 117\n");

        // Clenshaw–Curtis Integration for ln(x)/sqrt(x)
        i = 0;
        func = x => Log(x) / Sqrt(x); // Ensure correct function is set
        res = integration.clenshawintegrate(func, a, b, ref i, acc, eps);
        Console.WriteLine($"∫_0^1 ln(x)/√x dx using Clenshaw–Curtis = {res}; expected -4");
        Console.WriteLine($"Absolute Error: {Abs(res - (-4))}");
        Console.WriteLine($"Function Evaluations: {i}");
        Console.WriteLine($"Scipy Reference Evaluations: 315\n");

        Console.WriteLine("Comparison:\n");
        Console.WriteLine("Normal integration routine is slower than Python. The Clenshaw–Curtis implementation is faster than Python.");
    }
}
