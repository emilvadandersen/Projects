using System;
using System.Collections.Generic;

public static class Hydrogen {
    // Parameters for the radial integration range and tolerances
    public static double rmin = 1e-5;
    public static double rmax = 8;
    public static double acc = 1e-6;
    public static double eps = 1e-6;

    // Returns the right-hand side of the Schrödinger equation as a first-order system
    public static Func<double, vector, vector> schrodinger(double E) => (r, f) => {
        double dfdr = f[1];                           // First derivative: f'
        double d2fdr2 = -2 * (E + 1 / r) * f[0];      // Second derivative: f'' from the Schrödinger equation
        return new vector(dfdr, d2fdr2);              // Return the system as a vector [f', f'']
    };

    // Integrates the Schrödinger equation from rmin to rmax for a given energy E
    public static vector integrate(double E) {
        vector f0 = new vector(rmin - rmin * rmin, 1 - 2 * rmin); // Boundary conditions near r = 0
        var result = ODE.driver(schrodinger(E), rmin, f0, rmax, acc: acc, eps: eps); // Integrate using ODE solver
        List<double> xlist = result.Item1;     // List of r values
        List<vector> ylist = result.Item2;     // List of corresponding [f, f'] values
        return ylist[ylist.Count - 1];         // Return the final value f(rmax)
    }

    // Computes the full radial wavefunction solution f(r) over the range for a given E
    public static (List<double>, List<double>) radial_solution(double E) {
        vector f0 = new vector(rmin - rmin * rmin, 1 - 2 * rmin); // Boundary conditions
        var result = ODE.driver(schrodinger(E), rmin, f0, rmax, acc: acc, eps: eps); // Solve ODE
        List<double> xlist = result.Item1;     // r values
        List<vector> ylist = result.Item2;     // f(r), f'(r) values

        List<double> rvals = new List<double>(), fvals = new List<double>();
        for (int i = 0; i < xlist.Count; i++) {
            rvals.Add(xlist[i]);               // Store r
            fvals.Add(ylist[i][0]);            // Store f(r)
        }
        return (rvals, fvals);                 // Return lists of r and f(r)
    }

    // Function M(E): returns f(rmax) for a given E, used in root finding
    public static double M(double E) => integrate(E)[0];

    // Exact analytic solution for the hydrogen ground state wavefunction (up to normalization)
    public static double exact(double r) => r * Math.Exp(-r);
}
