using System;
using static System.Console;
using static System.Math;

public class main
{
    static int nx = 0; // Initialize the point counter
    static double acc = 1e-10; // Accuracy for the integration
    static double eps = 0; // Relative error tolerance (not used in this case)

    // Error function (erf) implementation using integral representation
    public static double erf(double z)
    {
        if (z < 0) return -erf(-z); // Handle the negative case

        // Define the function to integrate: exp(-x^2)
        Func<double, double> f = x => { nx++; return Exp(-x * x); }; 

        double q; 
        int iterations = 0; // Declare a variable to count the iterations

        // Case when z is less than 1.25
        if (z < 1.25) 
        {
            q = 2 / Sqrt(PI) * integration.integrate(f, 0, z, ref iterations, acc, eps); // Call adaptive integrator
        }
        else
        {
            // Case when z is greater than or equal to 1.25
            Func<double, double> F = t => f(z + (1 - t) / t) / t / t; 
            q = 1 - 2 / Sqrt(PI) * integration.integrate(F, 0, 1, ref iterations, acc, eps); // Call adaptive integrator for the transformed function
        }

        // Output the result and the number of points used in the integration
        Error.WriteLine($"z={z:f6} npoints={nx}, iterations={iterations}");

        nx = 0; // Reset the point counter for the next iteration
        return q; // Return the computed erf value
    }

    public static void Main()
    {
        // Test the erf function with a range of z values
        for (double z = -3; z <= 3; z += 1.0 / 8)
            Console.WriteLine($"{z} {erf(z)}");

        // Compare the calculated erf values with known values
        WriteLine("\n");
        WriteLine("-2   -0.995322265");
        WriteLine("-1   -0.842700793");
        WriteLine("-0.5 -0.520499878");
        WriteLine("-0.2 -0.222702589");
        WriteLine(" 0.2  0.222702589");
        WriteLine(" 0.5  0.520499878");
        WriteLine(" 1    0.842700793");
        WriteLine(" 2    0.995322265");
    }
}
