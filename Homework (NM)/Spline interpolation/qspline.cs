public class qspline
{
    // Arrays to store the x and y values, as well as coefficients for the spline
    double[] x, y, b, c;
    
    // Property to get the number of points (length of the x array)
    int n => x.Length;

    // Constructor that initializes the spline with the provided x and y data points
    public qspline(double[] x, double[] y)
    {
        // Clone the input arrays to avoid modifying the original data
        this.x = (double[])x.Clone();
        this.y = (double[])y.Clone();

        // Initialize coefficient arrays for b (first derivatives) and c (second derivatives)
        b = new double[n - 1];
        c = new double[n - 1];

        // Temporary arrays to store intermediate calculations
        var p = new double[n - 1]; // p values (slope of the secant lines between adjacent points)
        var h = new double[n - 1]; // h values (differences between adjacent x-values)

        // Calculate the h and p values for the spline (differences between adjacent x and y values)
        for (int i = 0; i < n - 1; i++)
        {
            h[i] = x[i + 1] - x[i]; // h is the difference between adjacent x values
            p[i] = (y[i + 1] - y[i]) / h[i]; // p is the difference in y-values divided by h
        }

        c[0] = 0;  // Start the recursion for calculating the second derivative coefficients

        // Recursion upwards: calculate the second derivative coefficients c[i]
        for (int i = 0; i < n - 2; i++)
            c[i + 1] = (p[i + 1] - p[i] - c[i] * h[i]) / h[i + 1];

        c[n - 2] /= 2;  // Final step to finish the recursion downward

        // Recursion downwards: finalize the second derivative coefficients c[i]
        for (int i = n - 3; i >= 0; i--)
            c[i] = (p[i + 1] - p[i] - c[i + 1] * h[i + 1]) / h[i];

        // Calculate the b values (first derivative coefficients)
        for (int i = 0; i < n - 1; i++)
            b[i] = p[i] - c[i] * h[i]; // b[i] is calculated from p[i] and c[i]
    }

    // Binary search to find the correct interval for a given z value
    int binsearch(double z)
    {
        // Ensure that z is within the bounds of the x values
        if (z < x[0] || z > x[n - 1]) throw new System.ArgumentException("z is out of bounds");

        // Binary search between the first and last indices to find the appropriate interval for z
        int i = 0, j = n - 1;
        while (j - i > 1)
        {
            int m = (i + j) / 2; // Find the midpoint of the interval
            if (z > x[m]) i = m; // Move the left bound if z is greater than the midpoint
            else j = m; // Move the right bound otherwise
        }
        return i; // Return the index of the interval containing z
    }

    // Evaluate the spline at a given point z
    public double eval(double z)
    {
        // Find the correct interval for z using binary search
        int i = binsearch(z);

        // Calculate the difference between z and the x-value at the interval
        double h = z - x[i];

        // Return the spline value at z, using the cubic spline formula
        return y[i] + h * (b[i] + h * c[i]);
    }

    // Evaluate the derivative of the spline at a given point z
    public double deriv(double z)
    {
        // Find the correct interval for z using binary search
        int i = binsearch(z);

        // Calculate the difference between z and the x-value at the interval
        double h = z - x[i];

        // Return the derivative of the spline at z, using the derivative formula
        return b[i] + 2 * h * c[i];
    }

    // Evaluate the integral of the spline from the first point (x[0]) to a given point z
    public double integ(double z)
    {
        // Find the correct interval for z using binary search
        int i = binsearch(z);

        double sum = 0; // Initialize the sum to accumulate the integral value
        double h;

        // Loop over the intervals up to the point i
        for (int k = 0; k < i; k++)
        {
            h = x[k + 1] - x[k]; // Calculate the width of the interval
            sum += h * (y[k] + h * (b[k] / 2 + h * c[k] / 3)); // Integrate the cubic spline over this interval
        }

        // Handle the last interval from x[i] to z
        h = z - x[i];
        return sum + h * (y[i] + h * (b[i] / 2 + h * c[i] / 3)); // Add the contribution from the last interval
    }
}
