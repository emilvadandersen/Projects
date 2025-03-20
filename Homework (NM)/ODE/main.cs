using System;
using System.Collections.Generic;
using static System.Math;

public static class main
{
    public static void Main()
    {
        // Pendulum parameters
        double b = 0.25; // Damping coefficient
        double c = 5.0;  // Stiffness coefficient

        // Interval for the solution
        (double start, double end) timeInterval = (0, 10);
        double stepSize = 0.1, tolerance = 1e-6;

        // Damped oscillator equation
        System.Func<double, vector, vector> func_osc = (double time, vector y) => {
            vector dydt = new vector(2);
            dydt[0] = y[1]; // theta'(t) = omega(t)
            dydt[1] = -b * y[1] - c * Sin(y[0]); // omega'(t) = -b * omega(t) - c * sin(theta(t))
            return dydt;
        };

        // Initial conditions for the damped oscillator
        vector y_osc = new vector(2);
        y_osc[0] = PI - 0.1;
        y_osc[1] = 0.0;

        // Solve the damped oscillator system
        var xlist = new List<double>();
        var ylist = new List<vector>();
        ODE.driver(func_osc, timeInterval.start, y_osc, timeInterval.end, tolerance, tolerance, stepSize, xlist, ylist);

        // Write pendulum results to files
        using (var outfile1 = new System.IO.StreamWriter("theta.txt"))
        using (var outfile2 = new System.IO.StreamWriter("omega.txt"))
        {
            for (int i = 0; i < xlist.Count; i++) {
                outfile1.WriteLine($"{xlist[i]} {ylist[i][0]}"); // Time and theta(t)
                outfile2.WriteLine($"{xlist[i]} {ylist[i][1]}"); // Time and omega(t)
            }
        }

        // ========== PLANETARY MOTION ==========
        System.Func<double, vector, vector> planetaryODE = (double phi, vector y) => {
            double epsilon = 0.015; // Relativistic correction (set to 0 for Newtonian cases)
            vector dydphi = new vector(2);
            dydphi[0] = y[1]; // u' = du/dphi
            dydphi[1] = 1 - y[0] + epsilon * y[0] * y[0]; // u'' = 1 - u + ε u^2
            return dydphi;
        };

        // Initial conditions for planetary motion
        vector y_circular = new vector(1.0, 0.0);   // Newtonian circular orbit
        vector y_elliptical = new vector(1.0, -0.5); // Newtonian elliptical orbit
        vector y_relativistic = new vector(1.0, -0.5); // Relativistic precession

        // Solve for multiple orbits
        double phiStart = 0, phiEnd = 20 * PI; // Simulate 10 full orbits

        SolveAndWrite(phiStart, phiEnd, y_circular, "circular.txt", tolerance, stepSize, 0.0);
        SolveAndWrite(phiStart, phiEnd, y_elliptical, "elliptical.txt", tolerance, stepSize, 0.0);
        SolveAndWrite(phiStart, phiEnd, y_relativistic, "relativistic.txt", tolerance, stepSize, 0.015);
    }

    public static void SolveAndWrite(
        double start, double end, vector y0, string filename,
        double tolerance, double stepSize, double epsilon)
    {
        var xlist = new List<double>();
        var ylist = new List<vector>();

        // Define the ODE system inside SolveAndWrite, using the given epsilon
        System.Func<double, vector, vector> planetaryODE = (double phi, vector y) => {
            vector dydphi = new vector(2);
            dydphi[0] = y[1]; // u' = du/dphi
            dydphi[1] = 1 - y[0] + epsilon * y[0] * y[0]; // u'' = 1 - u + ε u^2
            return dydphi;
        };

        // Solve ODE
        ODE.driver(planetaryODE, start, y0, end, tolerance, tolerance, stepSize, xlist, ylist);

        // Write output
        using (var outfile = new System.IO.StreamWriter(filename))
        {
            for (int i = 0; i < xlist.Count; i++)
            {
                outfile.WriteLine($"{xlist[i]} {ylist[i][0]}");
            }
        }
    }
}