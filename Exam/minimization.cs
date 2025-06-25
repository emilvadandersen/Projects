using System;
using static System.Math;

// Extension method for vector type
public static class Extensions {
    // Checks if any element in the vector satisfies the given predicate
    public static bool any(this vector v, Func<double, bool> predicate) {
        for (int i = 0; i < v.size; i++) {
            if (predicate(v[i])) return true;
        }
        return false;
    }
}

public class Minimization {

    // Numerical gradient of a scalar function φ at point x
    public static vector gradient(Func<vector, double> φ, vector x) {
        double fx = φ(x); // evaluate function at current point
        var g = new vector(x.size);

        for (int i = 0; i < x.size; i++) {
            double dx = (1 + Abs(x[i])) * Pow(2, -26); // step size
            x[i] += dx;
            g[i] = (φ(x) - fx) / dx; // forward difference
            x[i] -= dx;
        }
        return g;
    }

    // Numerical Hessian of φ at point x using gradient differences
    public static matrix hessian(Func<vector, double> φ, vector x) {
        int n = x.size;
        var H = new matrix(n, n);
        vector g0 = gradient(φ, x); // base gradient

        for (int j = 0; j < n; j++) {
            double dx = (1 + Abs(x[j])) * Pow(2, -13);
            x[j] += dx;
            vector g1 = gradient(φ, x); // new gradient with perturbed variable
            for (int i = 0; i < n; i++) {
                H[i, j] = (g1[i] - g0[i]) / dx; // central difference on gradients
            }
            x[j] -= dx;
        }
        return H;
    }

    // Newton's method for minimizing scalar functions with line search
    public static (vector, int) newton(
        Func<vector, double> φ,
        vector x,
        double acc = 1e-3,
        int maxSteps = 1000
    ) {
        int steps = 0;

        while (steps < maxSteps) {
            steps++;
            vector g = gradient(φ, x); // compute gradient

            if (g.norm() < acc) break; // stop if gradient is small

            matrix H = hessian(φ, x); // compute Hessian
            vector dx;

            try {
                // Solve H dx = -g using QR decomposition
                var qr = new QRGS(H);
                vector neg_g = -g;
                dx = qr.solve(neg_g);
            } catch {
                Console.Error.WriteLine("Newton: Failed to solve linear system");
                break;
            }

            double λ = 1.0; // initial step size
            double fx = φ(x);
            bool acceptedStep = false;

            // Backtracking line search
            while (λ >= 1.0 / 1024) {
                var x_try = x + λ * dx;
                double fx_try = φ(x_try);
                if (fx_try < fx || steps == 1) {  // always take first step
                    x = x_try;
                    acceptedStep = true;
                    break;
                }
                λ /= 2;
            }

            if (!acceptedStep) break; // terminate if no step is accepted
        }

        return (x, steps);
    }

    // Gauss-Newton method with Levenberg–Marquardt damping
    public static (vector, int) gaussNewton(
        Func<vector, vector> residuals,           // residual function
        Func<vector, matrix> jacobianFunc,        // analytical or numerical Jacobian
        vector x,                                 // initial guess
        double acc = 1e-3,
        int maxSteps = 1000
    ) {
        int steps = 0;
        double lambda = 1e-3; // LM damping parameter

        while (steps < maxSteps) {
            steps++;

            vector r = residuals(x);
            double norm_r = r.norm();
            if (norm_r < acc) break; // residual norm small enough

            matrix J = jacobianFunc(x); // compute Jacobian
            matrix JTJ = J.transpose() * J;
            vector JTr = J.transpose() * r;

            int n = x.size;
            matrix JTJ_damped = JTJ + lambda * matrix.id(n); // apply damping
            vector dx;

            try {
                var qr = new QRGS(JTJ_damped);
                dx = qr.solve(-JTr); // solve the linear system
            } catch {
                Console.Error.WriteLine("Gauss-Newton LM: Failed to solve linear system");
                break;
            }

            double λ = 1.0;
            bool acceptedStep = false;

            // Backtracking with constraints (e.g. gamma >= 0)
            while (λ >= 1.0 / 1024) {
                var x_try = x + λ * dx;

                if (x_try.any(z => double.IsNaN(z) || double.IsInfinity(z))) {
                    λ /= 2; continue;
                }
                if (x_try[1] < 0) { // gamma constraint (damping factor must be non-negative)
                    λ /= 2; continue;
                }

                var r_try = residuals(x_try);
                double norm_r_try = r_try.norm();

                if (norm_r_try < norm_r || steps == 1) {
                    x = x_try;
                    norm_r = norm_r_try;
                    lambda *= 0.8; // reduce damping on success
                    acceptedStep = true;
                    break;
                }

                λ /= 2;
            }

            if (!acceptedStep) {
                lambda *= 4; // increase damping on failure
            }

            if (dx.norm() < acc) break; // step size small enough
        }

        return (x, steps);
    }

    // Numerical Jacobian for a vector-valued function
    public static matrix jacobian(Func<vector, vector> f, vector x) {
        int m = f(x).size;
        int n = x.size;
        matrix J = new matrix(m, n);
        vector fx = f(x); // original evaluation

        for (int j = 0; j < n; j++) {
            double dx = (1 + Abs(x[j])) * Pow(2, -26);
            x[j] += dx;
            vector fx_dx = f(x); // perturb one variable
            x[j] -= dx;

            for (int i = 0; i < m; i++) {
                J[i, j] = (fx_dx[i] - fx[i]) / dx;
            }
        }
        return J;
    }

    // Manually implemented analytical Jacobian for the oscillatory decay model
    public static Func<vector, matrix> AnalyticalJacobian(
        double[] t,
        double[] y,
        double[] dy
    ) {
        return (vector x) => {
            int n = t.Length;
            matrix J = new matrix(n, 4); // 4 parameters: A, gamma, omega, phi
            double A = x[0], gamma = x[1], omega = x[2], phi = x[3];

            for (int i = 0; i < n; i++) {
                double ti = t[i];
                double denom = dy[i];
                double exp_term = Exp(-gamma * ti);
                double arg = omega * ti + phi;
                double cos_term = Cos(arg);
                double sin_term = Sin(arg);

                // Partial derivatives w.r.t. A, gamma, omega, phi
                J[i, 0] = -exp_term * cos_term / denom;                   // ∂r/∂A
                J[i, 1] = A * ti * exp_term * cos_term / denom;           // ∂r/∂gamma
                J[i, 2] = -A * exp_term * ti * sin_term / denom;          // ∂r/∂omega
                J[i, 3] = -A * exp_term * sin_term / denom;               // ∂r/∂phi
            }
            return J;
        };
    }
}
