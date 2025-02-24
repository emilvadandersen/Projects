using System;
using static System.Math;
using static sfuns; // Import functions from sfuns.cs

static class main
{
    static void Main()
    {
        // Task 1: Compute and print mathematical values
        double sqrt2 = Sqrt(2.0);
        double pow2_1_5 = Pow(2, 1.0 / 5);
        double exp_pi = Pow(E, PI);
        double pi_exp = Pow(PI, E);

        Console.WriteLine($"sqrt2^2 = {sqrt2 * sqrt2} (should equal 2)");
        Console.WriteLine($"2^(1/5) = {pow2_1_5}");
        Console.WriteLine($"e^pi = {exp_pi}");
        Console.WriteLine($"pi^e = {pi_exp}");

        // Task 2: Compute and print Gamma(1) to Gamma(10)
        Console.WriteLine("\nGamma function values:");
        for (int i = 1; i <= 10; i++)
        {
            Console.WriteLine($"Gamma({i}) = {fgamma(i)}");
        }

        // Task 3: Compute and print Log-Gamma(1) to Log-Gamma(10)
        Console.WriteLine("\nLog-Gamma function values:");
        for (int i = 1; i <= 10; i++)
        {
            Console.WriteLine($"Log-Gamma({i}) = {lngamma(i)}");
        }
    }
}