using System;

public static class linterp {
    
    // Linear interpolation evaluation function
    public static double linterpEval(double[] x, double[] y, double z) {
        // Find the index i such that x[i] <= z < x[i+1]
        int i = 0;
        while (i < x.Length - 1 && z > x[i + 1]) {
            i++; // Increase i until z is within the range of x[i] and x[i+1]
        }

        // Calculate the step size (h) between x[i] and x[i+1]
        double h = x[i + 1] - x[i];

        // Calculate the slope (a) between the two points (x[i], y[i]) and (x[i+1], y[i+1])
        double a = (y[i + 1] - y[i]) / h;

        // Return the interpolated value at z using the linear interpolation formula
        return y[i] + a * (z - x[i]);
    }

    // Linear interpolation integral function
    public static double linterpInteg(double[] x, double[] y, double z) {
        // Initialize the integral to 0
        double integral = 0;

        // Loop through each segment of the piecewise linear interpolation
        for (int i = 0; i < x.Length - 1; i++) {
            // If z is beyond the current segment, add the integral of the segment
            if (z >= x[i + 1]) {
                // Calculate the step size (h) for the current segment
                double h = x[i + 1] - x[i];

                // Calculate the slope (a) between the two points (x[i], y[i]) and (x[i+1], y[i+1])
                double a = (y[i + 1] - y[i]) / h;

                // Add the area under the linear segment to the integral
                integral += y[i] * h + 0.5 * a * Math.Pow(h, 2);
            } else {
                break; // If z is within the current segment, exit the loop
            }
        }
        // Return the total integral value up to z
        return integral;
    }
}
