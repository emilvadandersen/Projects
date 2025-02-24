using static System.Math; // Allows direct use of Math functions

public static class sfuns
{
    // Computes the logarithm of the gamma function (lngamma)
    public static double lngamma(double x)
    {
        if (x <= 0) return double.NaN; // Gamma function is not defined for non-positive integers
        if (x < 9) return lngamma(x + 1) - Log(x); // Recurrence relation
        
        // Stirling's approximation for ln(Gamma(x))
        double lnfgamma = x * Log(x + 1 / (12 * x - 1 / x / 10)) - x + Log(2 * PI / x) / 2;
        return lnfgamma;
    }

    // Computes the gamma function using exp(lngamma(x))
    public static double fgamma(double x)
    {
        if (x < 0) return PI / Sin(PI * x) / fgamma(1 - x); // Euler's reflection formula
        return Exp(lngamma(x)); // Compute Gamma(x) using the log-gamma function
    }
} // class sfuns