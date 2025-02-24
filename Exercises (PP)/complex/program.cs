using System;
using System.Numerics;

class Program
{
    // Approximation function to compare two complex numbers
    static bool Approx(Complex a, Complex b, double tolerance = 1e-6)
    {
        return Math.Abs(a.Real - b.Real) < tolerance && Math.Abs(a.Imaginary - b.Imaginary) < tolerance;
    }

    static void Main()
    {
        // Open the output file
        string outputFile = "output.txt";
        using (System.IO.StreamWriter file = new System.IO.StreamWriter(outputFile))
        {
            // Square root of -1 (should be ±i)
            Complex sqrt_neg1 = Complex.Sqrt(-1);
            file.WriteLine("√-1 = " + FormatComplex(sqrt_neg1));  // Expected: ±i
            file.WriteLine("// The square root of -1 is i (0 + 1i). In C#, it returns the principal root, which is i.");
            file.WriteLine("// Approximation check: " + (Approx(sqrt_neg1, new Complex(0, 1)) ? "Pass" : "Fail"));
            file.WriteLine();

            // Square root of i (should be e^(iπ/4))
            Complex sqrt_i = Complex.Sqrt(Complex.ImaginaryOne);
            file.WriteLine("√i = " + FormatComplex(sqrt_i));  // Expected: 1/√2 + i/√2
            file.WriteLine("// The square root of i is e^(iπ/4), which is 1/√2 + i/√2. It is one of the two possible roots.");
            file.WriteLine("// Approximation check: " + (Approx(sqrt_i, new Complex(Math.Cos(Math.PI / 4), Math.Sin(Math.PI / 4))) ? "Pass" : "Fail"));
            file.WriteLine();

            // Logarithm of i (should be iπ/2)
            Complex ln_i = Complex.Log(Complex.ImaginaryOne);
            file.WriteLine("ln(i) = " + FormatComplex(ln_i));  // Expected: iπ/2
            file.WriteLine("// The natural logarithm of i is iπ/2. This is the principal branch of the logarithm.");
            file.WriteLine("// Approximation check: " + (Approx(ln_i, new Complex(0, Math.PI / 2)) ? "Pass" : "Fail"));
            file.WriteLine();

            // Exponentiation of i^i (should be e^(-π/2))
            Complex i_i = Complex.Pow(Complex.ImaginaryOne, Complex.ImaginaryOne);
            file.WriteLine("i^i = " + i_i.Real);  // Expected: e^(-π/2) ≈ 0.208
            file.WriteLine("// i^i is a real number, and it evaluates to e^(-π/2), which is approximately 0.208.");
            file.WriteLine("// Approximation check: " + (Math.Abs(i_i.Real - Math.Exp(-Math.PI / 2)) < 1e-6 ? "Pass" : "Fail"));
            file.WriteLine();

            // Exponentiation of e^(iπ) (should be -1)
            Complex e_ipi = Complex.Exp(Complex.ImaginaryOne * Math.PI);
            file.WriteLine("e^(iπ) = " + FormatComplex(e_ipi));  // Expected: -1
            file.WriteLine("// e^(iπ) is Euler's formula, which simplifies to -1.");
            file.WriteLine("// Approximation check: " + (Approx(e_ipi, new Complex(-1, 0)) ? "Pass" : "Fail"));
            file.WriteLine();

            // Sin of iπ (should be sinh(π))
            Complex sin_ipi = Complex.Sin(Complex.ImaginaryOne * Math.PI);
            double sinh_pi = Math.Sinh(Math.PI);  // Expected: sinh(π)
            file.WriteLine("sin(iπ) = " + FormatComplex(sin_ipi));  // Expected: sinh(π)
            file.WriteLine("// sin(iπ) is equivalent to sinh(π), since the sine of an imaginary number is the hyperbolic sine.");
            file.WriteLine("// Approximation check (imaginary part only): " + 
                (Math.Abs(sin_ipi.Imaginary - sinh_pi) < 1e-6 ? "Pass" : "Fail"));
            file.WriteLine();
        }
    }

    // Method to format complex number as Real;Imaginary
    static string FormatComplex(Complex c)
    {
        return $"Re: {c.Real} ; Im: {c.Imaginary}";
    }
}
