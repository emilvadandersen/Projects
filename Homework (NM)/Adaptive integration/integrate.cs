using static System.Math;
using System;

public static class integration
{
    // Adaptive recursive integration using higher and lower order quadrature rules
    public static double integrate(Func<double, double> f, double a, double b, ref int iterations, double acc, double eps, double f2 = double.NaN, double f3 = double.NaN)
    {
        iterations++; // Increment the iteration count
        double h = b - a; // Interval width

        // If first call, compute f2 and f3
        if (double.IsNaN(f2))
        {
            f2 = f(a + 2 * h / 6);
            f3 = f(a + 4 * h / 6);
        }

        double f1 = f(a + h / 6);
        double f4 = f(a + 5 * h / 6);

        double Q = (2 * f1 + f2 + f3 + 2 * f4) / 6 * h; // Higher order rule
        double q = (f1 + f2 + f3 + f4) / 4 * h; // Lower order rule
        double err = Abs(Q - q); // Error estimate

        // If error is within tolerance, return result
        if (err <= acc + eps * Abs(Q))
        {
            return Q;
        }
        else
        {
            // Otherwise, split the interval and integrate recursively
            double result = integrate(f, a, (a + b) / 2, ref iterations, acc / Sqrt(2), eps, f1, f2) +
                            integrate(f, (a + b) / 2, b, ref iterations, acc / Sqrt(2), eps, f3, f4);
            return result;
        }
    }

    // Adaptive integration using Clenshaw-Curtis variable transformation
    public static double clenshawintegrate(Func<double, double> f, double a, double b, ref int iterations, double acc = 0.001, double eps = 0.001, double f2 = double.NaN, double f3 = double.NaN)
    {
        iterations = 0; // Initialize iteration count

        // Transform function using Clenshaw-Curtis substitution
        Func<double, double> clenshawf = theta => f((a + b) / 2 + (b - a) / 2 * Cos(theta)) * Sin(theta) * (b - a) / 2;

        // Integrate transformed function over [0, π]
        double result = integrate(clenshawf, 0, PI, ref iterations, acc, eps, f2, f3);
        return result;
    }
}
