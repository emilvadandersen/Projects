using System;

public static class Jacobian {
	// Computes the numerical Jacobian matrix of a vector function f at point x
	public static matrix jacobian(Func<vector, vector> f, vector x, vector fx = null, vector dx = null) {
		// If dx not provided, estimate step size for finite differences
		if (dx == null)
			dx = x.map(xi => Math.Max(Math.Abs(xi), 1.0) * Math.Pow(2, -26)); // Typical finite-difference step

		// If fx not provided, compute f(x) once for reuse
		if (fx == null)
			fx = f(x);

		int n = x.size;
		matrix J = new matrix(n, n); // Jacobian matrix to fill

		for (int j = 0; j < n; j++) {
			x[j] += dx[j];                      // Apply small perturbation to the j-th variable
			vector df = f(x) - fx;              // Compute change in function
			for (int i = 0; i < n; i++)
				J[i, j] = df[i] / dx[j];        // Approximate partial derivative ∂f_i/∂x_j
			x[j] -= dx[j];                      // Restore x[j] after perturbation
		}
		return J;
	}
}
