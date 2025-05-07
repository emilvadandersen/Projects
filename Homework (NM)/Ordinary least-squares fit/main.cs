using System;
using System.IO;
using System.Globalization;

public class main {
    static void Main() {
        // --- QR Decomposition Part ---
        Console.WriteLine("QR for 5x3 matrix");
        Console.WriteLine("Matrix A:");
        
        Random rand = new Random();
        matrix A = new matrix(5, 3);
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 3; j++) {
                A[i, j] = rand.Next(1, 11);
            }
        }
        A.print();

        (matrix Q, matrix R) = QRGS.decomp(A);

        Console.WriteLine("Matrix Q:");
        Q.print();
        Console.WriteLine("Matrix R:");
        R.print();

        matrix A_reconstructed = Q * R;
        Console.WriteLine("Reconstructed matrix A (Q * R):");
        A_reconstructed.print();

        // --- Least-Squares Fit Part ---
        Console.WriteLine();
        Console.WriteLine("Least-squares fit to radioactive decay:");

        int noOfDataPoints = 9;
        vector x = new vector(noOfDataPoints);
        vector y = new vector(noOfDataPoints);
        vector dy = new vector(noOfDataPoints);
        string filename = "data.txt";

        string[] lines = File.ReadAllLines(filename);
        for (int i = 0; i < noOfDataPoints; i++) {
            try {
                string[] parts = lines[i].Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                x[i] = double.Parse(parts[0], CultureInfo.InvariantCulture);
                y[i] = double.Parse(parts[1], CultureInfo.InvariantCulture);
                dy[i] = double.Parse(parts[2], CultureInfo.InvariantCulture);
            } catch (FormatException ex) {
                Console.WriteLine($"Error parsing line {i + 1}: '{lines[i]}'. Exception: {ex.Message}");
                return;
            }
        }

        // Transform the data
        for (int i = 0; i < noOfDataPoints; i++) {
            y[i] = Math.Log(y[i]);
            dy[i] /= y[i];
        }

        var fs = new Func<double, double>[] { z => 1.0, z => -z };

        leastsquares ls = new leastsquares();
        (vector res, matrix cov) = ls.lsfit(fs, x, y, dy);

        double a = res[0], b = res[1];

        Console.WriteLine();
        Console.WriteLine($"Fit parameters (from ln(Activity) = a - b*t):");
        Console.WriteLine($"a ≈ {a:F3} (ln(A₀)) and b ≈ {b:F3} (decay constant)");

        double halflife = Math.Log(2) / b;
        Console.WriteLine($"Half-life = ln(2)/b ≈ {halflife:F3} days (expected ~3.63 days)");

        Console.WriteLine();
        Console.WriteLine("Covariance matrix (in ln-domain):");
        cov.print();

        double uncer1 = Math.Sqrt(cov[0, 0]);
        double uncer2 = Math.Sqrt(cov[1, 1]);

        Console.WriteLine($"Uncertainties in the fitting parameters:");
        Console.WriteLine($"  σ_a ≈ {uncer1:F3}");
        Console.WriteLine($"  σ_b ≈ {uncer2:F3}");

        double halflifeuncer = Math.Log(2) * uncer2 / b;
        Console.WriteLine($"Uncertainty in half-life: {halflifeuncer:F3} days");

        Console.WriteLine();
        if (halflife - halflifeuncer <= 3.63 && halflife + halflifeuncer >= 3.63) {
            Console.WriteLine("The calculated half-life agrees with the expected value within the estimated uncertainty.");
        } else {
            Console.WriteLine("The calculated half-life does not agree with the expected value within the estimated uncertainty.");
        }

        // Write fit bands to file
        using (StreamWriter sw = new StreamWriter("fitbands.txt")) {
            for (double z = 0; z <= 15; z += 0.1) {
                double best = a - b * z;
                double upper = (a + uncer1) - (b - uncer2) * z;
                double lower = (a - uncer1) - (b + uncer2) * z;
                sw.WriteLine($"{z} {best} {upper} {lower}");
            }
        }
    }
}
