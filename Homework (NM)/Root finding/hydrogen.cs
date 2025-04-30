using System;
using System.Collections.Generic;

public static class Hydrogen {
    public static double rmin = 1e-5;
    public static double rmax = 8;
    public static double acc = 1e-6;
    public static double eps = 1e-6;

    // Schrodinger equation
    public static Func<double, vector, vector> schrodinger(double E) => (r, f) => {
        double dfdr = f[1];
        double d2fdr2 = -2 * (E + 1 / r) * f[0];
        return new vector(dfdr, d2fdr2);
    };

    // Integrate the Schrodinger equation
    public static vector integrate(double E) {
        vector f0 = new vector(rmin - rmin * rmin, 1 - 2 * rmin); // from boundary condition
        var result = ODE.driver(schrodinger(E), rmin, f0, rmax, acc: acc, eps: eps);
        List<double> xlist = result.Item1; // r values
        List<vector> ylist = result.Item2; // f(r) values
        return ylist[ylist.Count - 1]; // return f(rmax)
    }

    // Return full radial solution
    public static (List<double>, List<double>) radial_solution(double E) {
        vector f0 = new vector(rmin - rmin * rmin, 1 - 2 * rmin); // from BC
        var result = ODE.driver(schrodinger(E), rmin, f0, rmax, acc: acc, eps: eps);
        List<double> xlist = result.Item1; // r values
        List<vector> ylist = result.Item2; // f(r) values

        List<double> rvals = new List<double>(), fvals = new List<double>();
        for (int i = 0; i < xlist.Count; i++) {
            rvals.Add(xlist[i]);
            fvals.Add(ylist[i][0]); // Assuming f[0] is the radial wavefunction f(r)
        }
        return (rvals, fvals);
    }

    // M function: return f(rmax) for given E
    public static double M(double E) => integrate(E)[0];

    // Exact solution for the hydrogen atom wavefunction
    public static double exact(double r) => r * Math.Exp(-r);
}
