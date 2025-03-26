using System;
using System.Collections.Generic;
using static System.Math;

public static class HaltonMC
{
    // Prime numbers used as bases for the Halton sequence
    private static readonly int[] primes = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29 };

    // Generates the nth value of the Halton sequence for a given base
    private static double Halton(int index, int baseValue)
    {
        double result = 0;
        double f = 1.0 / baseValue;
        
        // Compute the fractional part of the sequence by iterating over digits in the base
        while (index > 0)
        {
            result += f * (index % baseValue);  // Add the fractional digit to the result
            index /= baseValue;  // Move to the next digit
            f /= baseValue;  // Scale for the next fractional digit
        }
        
        return result;  // Return the Halton value for the given index and base
    }

    // Generates a multidimensional Halton sequence
    public static IEnumerable<double[]> GenerateHaltonSequence(int numSamples, int dimensions)
    {
        // For each sample, generate a vector of Halton values based on the number of dimensions
        for (int i = 0; i < numSamples; i++)
        {
            double[] sample = new double[dimensions];
            
            // Generate the Halton value for each dimension using the primes as the base
            for (int j = 0; j < dimensions; j++)
            {
                sample[j] = Halton(i, primes[j]);  // Use the appropriate prime number as the base for each dimension
            }
            
            yield return sample;  // Yield the generated sample
        }
    }

    // Monte Carlo integration using Halton sequence
    public static (double, double) Run(Func<vector, double> f, vector a, vector b, int N)
    {
        int dim = a.size;  // Get the number of dimensions from the vector 'a'
        double V = 1;
        
        // Compute the volume of the integration region (bounding box of the integration domain)
        for (int i = 0; i < dim; i++) V *= b[i] - a[i];
        
        double sum = 0, sum2 = 0;  // Initialize sums for mean and variance calculations
        var haltonSeq = GenerateHaltonSequence(N, dim);  // Generate the Halton sequence for the specified number of samples and dimensions
        
        // Iterate over the generated Halton sequence and evaluate the function at each point
        foreach (var x in haltonSeq)
        {
            double fx = f(new vector(x));  // Evaluate the function 'f' at the current point (converted to a vector)
            sum += fx;  // Accumulate the sum of function values
            sum2 += fx * fx;  // Accumulate the sum of squared function values
        }

        // Compute the mean and standard deviation (sigma) of the function values
        double mean = sum / N;
        double sigma = Sqrt(sum2 / N - mean * mean);  // Standard deviation formula

        // Return the result: mean of function values times volume, and uncertainty (sigma) times volume divided by sqrt(N)
        return (mean * V, sigma * V / Sqrt(N));
    }
}
