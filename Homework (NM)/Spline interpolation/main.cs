using System;
using System.IO;
using static System.Console;
using static System.Math;

class main
{
    static void Main()
    {
        int n = 10; // Number of data points
        double[] x = new double[n]; // Array to store x-values
        double[] y = new double[n]; // Array to store y-values

        // Fill x and y arrays with data points for a Cosine function
        for (int i = 0; i < n; i++)
        {
            x[i] = i; // x values from 0 to n-1
            y[i] = Cos(i); // y values as cosine of x[i]
        }

        // Save the cosine data to a file named "cosTable.txt"
        using (StreamWriter writer = new StreamWriter("cosTable.txt"))
        {
            for (int i = 0; i < x.Length; i++)
            {
                writer.WriteLine($"{x[i]}\t{y[i]}"); // Write x and y values separated by a tab
            }
        }

        // Linear Interpolation Output Files
        // Create StreamWriter objects for storing the linear interpolation values and its integral
        var linInterpData = new StreamWriter("linInter.txt");
        var linInterpInteg = new StreamWriter("linInterInteg.txt");
        
        // Iterate over the range of x-values to evaluate the linear spline and its integral
        for (double z = x[0]; z <= x[x.Length - 1]; z += 0.01) // Using a step size of 0.01
        {
            // Evaluate linear interpolation and its integral at z, writing the results to the files
            linInterpData.WriteLine($"{z}\t{linterp.linterpEval(x, y, z)}"); // Interpolation result for z
            linInterpInteg.WriteLine($"{z}\t{linterp.linterpInteg(x, y, z)}"); // Integral result for z
        }

        // Close the files after writing the data
        linInterpData.Close();
        linInterpInteg.Close();

        // Update y-values for Quadratic Spline Data (using Sine function)
        for (int i = 0; i < n; i++)
        {
            y[i] = Sin(i); // Replace the cosine values with sine values
        }

        // Save the sine data to a file named "sinTable.txt"
        using (StreamWriter writer = new StreamWriter("sinTable.txt"))
        {
            for (int i = 0; i < x.Length; i++)
            {
                writer.WriteLine($"{x[i]}\t{y[i]}"); // Write x and sine y-values separated by a tab
            }
        }

        // Create the quadratic spline and write the results to files
        qspline qspline = new qspline(x, y);
        using (StreamWriter qInterpData = new StreamWriter("quaInter.txt"))
        using (StreamWriter qInterpInteg = new StreamWriter("quaInterInteg.txt"))
        {
            // Iterate over the range of x-values for quadratic spline evaluation
            for (double z = x[0]; z <= x[x.Length - 1]; z += 0.01) // Using a step size of 0.01
            {
                // Write both the quadratic spline value and its derivative to quaInter.txt
                qInterpData.WriteLine($"{z}\t{qspline.eval(z)}\t{qspline.deriv(z)}"); // Eval and derivative for z
                qInterpInteg.WriteLine($"{z}\t{qspline.integ(z)}"); // Integral result for z
            }
        }
    }
}
