using System;
using System.Collections.Generic;
using static System.Math;

public static class ODE {  

    // Runge-Kutta 2nd/3rd order step function
    public static (vector, vector) rkstep23(
        Func<double, vector, vector> F, // Function representing the system: dy/dx = F(x, y)
        double x,  // Current x-value (independent variable)
        vector y,  // Current y-value (state of the system)
        double h   // Step size
    ) { 
        // Compute the Runge-Kutta increments (intermediate steps)
        vector k0 = F(x, y);                       // k0: Evaluate function at (x, y)
        vector k1 = F(x + h / 2, y + k0 * (h / 2)); // k1: Midpoint evaluation
        vector k2 = F(x + 3 * h / 4, y + k1 * (3 * h / 4)); // k2: 3/4 step evaluation

        // Compute the next value approximation using weighted sum
        vector ka = k0 * (2.0 / 9) + k1 * (3.0 / 9) + k2 * (4.0 / 9); 
        vector kb = k1; // 2nd-order estimate
        
        // Compute the next state estimate
        vector yh = y + ka * h; 
        
        // Estimate error using difference between two different approximations
        vector er = (ka - kb) * h; 

        return (yh, er); // Return estimated next value and error
    }

    // ODE Solver using adaptive step-size control
    public static (List<double>, List<vector>) driver(
        Func<double, vector, vector> F, // Function defining the ODE system
        double a,   // Start value of x (initial time)
        vector ya,  // Initial state y(a)
        double b,   // End value of x (final time)
        double acc = 1e-2, // Absolute accuracy tolerance
        double eps = 1e-2, // Relative accuracy tolerance
        double h = 0.01,   // Initial step size
        List<double> xlist = null, // Optional list to store x-values
        List<vector> ylist = null  // Optional list to store y-values
    ) { 
        // Ensure the interval is valid
        if (a > b) throw new Exception("driver: a > b");

        double x = a;   // Initialize x at the start of the interval
        vector y = ya;  // Initialize y at the starting condition

        // Initialize lists if they are null
        if (xlist == null) xlist = new List<double>();
        if (ylist == null) ylist = new List<vector>();

        // Add initial conditions to lists
        xlist.Add(x);
        ylist.Add(y);

        // Adaptive step-size loop
        do {
            if (x >= b) return (xlist, ylist); // Stop when reaching b
            if (x + h > b) h = b - x; // Ensure the last step doesn’t overshoot b

            // Perform one Runge-Kutta step
            var (yh, erv) = rkstep23(F, x, y, h);

            // Compute error tolerance
            double tol = Max(acc, yh.norm() * eps) * Sqrt(h / (b - a)); 
            double err = erv.norm(); // Compute actual error

            // Accept step if error is within tolerance
            if (err < tol) {
                x += h;  // Advance x
                y = yh;  // Accept the new y value

                // Store step in list if provided
                xlist.Add(x);
                ylist.Add(y);
            } 

            // Adjust step size based on error (adaptive step size control)
            h *= Min(Pow(tol / err, 0.25) * 0.95, 2); // Scale step size
        } while (true);
    }
}
