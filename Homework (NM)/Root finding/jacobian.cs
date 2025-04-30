using System;

public static class Jacobian {
	public static matrix jacobian(Func<vector, vector> f, vector x, vector fx = null, vector dx = null) {
		if (dx == null) dx = x.map(xi => Math.Max(Math.Abs(xi), 1.0) * Math.Pow(2, -26));
		if (fx == null) fx = f(x);
		int n = x.size;
		matrix J = new matrix(n, n);
		for (int j = 0; j < n; j++) {
			x[j] += dx[j];
			vector df = f(x) - fx;
			for (int i = 0; i < n; i++) J[i, j] = df[i] / dx[j];
			x[j] -= dx[j];
		}
		return J;
	}
}
