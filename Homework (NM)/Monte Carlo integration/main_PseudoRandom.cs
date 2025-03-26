using System;
using static System.Console;
using static System.Math;
using System.IO; 

public class main {

    public static void Main(string[] args) {
        int N = 1000;  // Default number of samples (N)
        
        // If a command-line argument is provided, use that as the number of samples
        if (args.Length > 0) N = (int)double.Parse(args[0]);

        Random RND = new Random();  // Initialize a random number generator

        // Define the function for estimating the area of a unit circle
        double R = 1;  // Radius of the unit circle
        Func<vector, double> unitCircle = x => {
            if (x.norm() <= R) return 1; // If the point is inside the unit circle, return 1
            else return 0;               // If the point is outside, return 0
        };

        // Define the bounds for the integration region (square [-1,1] x [-1,1])
        vector aCircle = new vector(-1.0, -1.0); // Lower bound of the square
        vector bCircle = new vector(1.0, 1.0);   // Upper bound of the square

        // Compute the Monte Carlo estimation for the unit circle area using PlainMC
        (double qCircle, double eCircle) = PlainMC.plainmc(unitCircle, aCircle, bCircle, N);

        // Exact value for the unit circle area is π
        double exactCircle = PI;

        // Write the results for the unit circle area estimation to "Out_pseudo_random.txt"
        using (var writer = new StreamWriter("Out_pseudo_random.txt", append: true)) {
            writer.WriteLine($"{N} {qCircle} {eCircle} {Abs(qCircle - exactCircle)}"); // Log N, estimated value, uncertainty, and absolute error
        }

        // Define the function for the hard integral
        Func<vector, double> hardIntegral = x => (1 - Cos(x[0]) * Cos(x[1]) * Cos(x[2])) / (PI * PI * PI);

        // Define the bounds for the integration region for the hard integral (cube [0,π] x [0,π] x [0,π])
        vector a3 = new vector(0.0, 0.0, 0.0); // Lower bound of the cube
        vector b3 = new vector(PI, PI, PI);    // Upper bound of the cube

        // Compute the Monte Carlo estimation for the hard integral using PlainMC
        (double qHard, double eHard) = PlainMC.plainmc(hardIntegral, a3, b3, N);

        // Exact value for the hard integral
        double exactHard = 1.3932039296856768591842462603255;

        // Check if the output file already exists; if not, write the header
        string outFile = "Out.txt";
        bool fileExists = File.Exists(outFile);
        
        // Write the results to "Out.txt"
        using (var writer = new StreamWriter(outFile, append: true)) {
            if (!fileExists) {
                writer.WriteLine("Hard Integral Calculation");
                writer.WriteLine("Integral: ∫(1 - cos(x)cos(y)cos(z)) / π³ dx dy dz; over [0;π]³");
                writer.WriteLine($"Exact Value: {exactHard}");
                writer.WriteLine("N (Sample Points); Estimate; Uncertainty; Absolute Error");
                writer.WriteLine("====================================================================");
            }
            writer.WriteLine($"{N} {qHard} {eHard} {Abs(qHard - exactHard)}"); // Log N, estimated value, uncertainty, and absolute error
        }
    }
}
