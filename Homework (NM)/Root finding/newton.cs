using System;

public static class Newton {
	public static vector newton(
		Func<vector, vector> f,
		vector start,
		double acc = 1e-2,
		vector dx = null
	) {
		vector x = start.copy();
		vector fx = f(x), z, fz;

		do {
			if (fx.norm() < acc) break;
			matrix J = Jacobian.jacobian(f, x, fx, dx);
			var QR = new QRGS(J);
			vector Dx = QR.solve(-fx);
			double λ = 1.0, λmin = 1.0 / 1024;
			do {
				z = x + λ * Dx;
				fz = f(z);
				if (fz.norm() < (1 - λ / 2) * fx.norm()) break;
				if (λ < λmin) break;
				λ /= 2;
			} while (true);
			x = z; fx = fz;
		} while (true);
		return x;
	}
}
