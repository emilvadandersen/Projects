using System;

public static class Newton {
	public static vector newton(
		Func<vector, vector> f,     // Function whose root we want to find
		vector start,               // Initial guess
		double acc = 1e-2,          // Desired accuracy
		vector dx = null            // Step size for numerical Jacobian (optional)
	) {
		vector x = start.copy();    // Current guess
		vector fx = f(x), z, fz;    // fx: f(x), z: trial step, fz: f(z)

		do {
			// Check for convergence
			if (fx.norm() < acc) break;

			// Compute Jacobian J ≈ df/dx
			matrix J = Jacobian.jacobian(f, x, fx, dx);

			// Solve J·Dx = -f(x) using QR decomposition
			var QR = new QRGS(J);
			vector Dx = QR.solve(-fx);

			// Backtracking line search
			double λ = 1.0, λmin = 1.0 / 1024;
			do {
				z = x + λ * Dx;         // Trial step
				fz = f(z);              // Evaluate function at new point

				// Accept step if sufficient decrease
				if (fz.norm() < (1 - λ / 2) * fx.norm()) break;

				// If step is too small, give up
				if (λ < λmin) break;

				λ /= 2;                 // Reduce step size
			} while (true);

			// Update guess
			x = z;
			fx = fz;

		} while (true);

		return x; // Return converged solution
	}
}
