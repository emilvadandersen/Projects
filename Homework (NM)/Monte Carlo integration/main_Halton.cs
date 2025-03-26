using System;
using System.IO;
using static System.Math;
using static HaltonMC;

public class Main_Halton {
    
   public static void Main(string[] args) {
        int N = 1000;  // Default number of samples
        if (args.Length > 0) N = (int)double.Parse(args[0]);

        // Define the function for the unit circle area estimation
        double R = 1;
        Func<vector, double> unitCircle = x => {
            if (x.norm() <= R) return 1; // Inside the unit circle
            else return 0;               // Outside the unit circle
        };

        // Bounds for the integration region (square [-1,1] x [-1,1])
        vector aCircle = new vector(-1.0, -1.0); // Lower bound of integration region
        vector bCircle = new vector(1.0, 1.0);   // Upper bound of integration region

        // Use the Halton sequence for Monte Carlo estimation for the unit circle area
        (double qCircle, double eCircle) = Run(unitCircle, aCircle, bCircle, N);

        // Exact value of the unit circle area is pi
        double exactCircle = PI;

        // Write results for the unit circle area to Out_halton.txt
        using (var writer = new StreamWriter("Out_halton.txt", append: true)) {
            writer.WriteLine($"{N} {qCircle} {eCircle} {Abs(qCircle - exactCircle)}");
        }
    }
}