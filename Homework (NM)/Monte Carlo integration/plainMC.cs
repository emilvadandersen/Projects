using System; 
using static System.Math;

public static class PlainMC
{
    // Function to perform Monte Carlo integration
    public static (double, double) plainmc(Func<vector, double> f, vector a, vector b, int N, Random RND = null)
    {
        int dim = a.size;  // Get the dimensionality of the integration region
        double V = 1;
        
        // Compute the volume of the integration region by multiplying the differences between the bounds for each dimension
        for (int i = 0; i < dim; i++) V *= b[i] - a[i];
        
        double sum = 0, sum2 = 0;  // Initialize sums for mean and variance calculations
        var x = new vector(dim);   // Create a vector to store sample points
        if (RND == null) RND = new Random();  // If no random generator is passed, create a new one
        
        // Perform the Monte Carlo simulation by generating N random points
        for (int i = 0; i < N; i++)
        {
            // Generate random sample points inside the integration region [a, b]
            for (int k = 0; k < dim; k++) x[k] = a[k] + RND.NextDouble() * (b[k] - a[k]);
            
            double fx = f(x);  // Evaluate the function at the random point
            sum += fx;         // Accumulate the sum of function values
            sum2 += fx * fx;   // Accumulate the sum of squared function values
        }

        // Calculate the mean and the standard deviation (sigma) of the function values
        double mean = sum / N;
        double sigma = Sqrt(sum2 / N - mean * mean);  // Standard deviation formula

        // Return the result: mean multiplied by the volume of the region, and sigma multiplied by volume divided by sqrt(N)
        return (mean * V, sigma * V / Sqrt(N));
    }
}
