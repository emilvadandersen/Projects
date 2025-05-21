using System;
using System.IO;
using static System.Math;

public class Minimization {

    // === Computes the numerical gradient of a scalar function φ at point x ===
    public static vector gradient(Func<vector, double> φ, vector x) {
        double fx = φ(x);                  // Function value at original x
        var g = new vector(x.size);        // Gradient vector

        for (int i = 0; i < x.size; i++) {
            // Relative perturbation based on x[i]
            double dx = (1 + Abs(x[i])) * Pow(2, -26);

            x[i] += dx;                    // Apply small change to x[i]
            g[i] = (φ(x) - fx) / dx;       // Finite difference approximation
            x[i] -= dx;                    // Restore x[i]
        }

        return g;  // Return gradient vector
    }

    // === Approximates the Hessian matrix of second derivatives ===
    public static matrix hessian(Func<vector, double> φ, vector x) {
        int n = x.size;
        var H = new matrix(n, n);          // Initialize Hessian matrix
        vector g0 = gradient(φ, x);        // Gradient at original point

        for (int j = 0; j < n; j++) {
            double dx = (1 + Abs(x[j])) * Pow(2, -13);  // Larger perturbation than in gradient
            x[j] += dx;                                 // Perturb x[j]
            vector g1 = gradient(φ, x);                 // New gradient
            for (int i = 0; i < n; i++) {
                H[i, j] = (g1[i] - g0[i]) / dx;         // Approximate second partial derivative
            }
            x[j] -= dx;  // Restore x[j]
        }

        return H;  // Return Hessian matrix
    }

    // === Newton's Method for multivariable minimization with backtracking line search ===
    public static (vector, int) newton(
        Func<vector, double> φ,  // Function to minimize
        vector x,                // Initial guess
        double acc = 1e-2,       // Accuracy threshold for gradient norm
        int maxSteps = 1000      // Maximum iterations
    ) {
        int steps = 0;

        while (steps < maxSteps) {
            steps++;
            vector g = gradient(φ, x);  // Compute gradient at current point

            if (g.norm() < acc) break; // Converged if gradient is small

            matrix H = hessian(φ, x);  // Approximate Hessian matrix
            vector dx;

            try {
                var qr = new QRGS(H);  // QR decomposition to solve system
                var neg_g = new vector(g.size);
                for (int i = 0; i < g.size; i++) 
                    neg_g[i] = -g[i];

                dx = qr.solve(neg_g);  // Solve H·dx = -g
            } catch {
                // If system cannot be solved (e.g., singular matrix), stop
                Console.Error.WriteLine("Failed to solve linear system");
                break;
            }

            // Backtracking line search to ensure decrease in φ
            double λ = 1.0;
            while (λ >= 1.0 / 1024) {
                if (φ(x + λ * dx) < φ(x)) break;  // Acceptable step
                λ /= 2;                            // Reduce step size
            }

            x += λ * dx;  // Update position
        }

        return (x, steps);  // Return final position and number of steps
    }
}
