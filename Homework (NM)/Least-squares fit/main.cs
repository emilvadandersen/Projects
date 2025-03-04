using System;
using System.IO;
using System.Globalization;

public class main {
    static void Main() {
        System.Console.WriteLine("QR for 5x3 matrix");

        // Create a random number generator
        Random rand = new Random();

        // Define a new 5x3 matrix A with random numbers between 1 and 10
        matrix A = new matrix(5, 3);
        for (int i = 0; i < 5; i++) {
            for (int j = 0; j < 3; j++) {
                A[i, j] = rand.Next(1, 11);  // Random integer between 1 and 10
            }
        }

        // Print the matrix A
        Console.WriteLine("Matrix A:");
        A.print();

        // Perform QR decomposition
        (matrix Q, matrix R) = QRGS.decomp(A);

        // Print Q and R matrices
        Console.WriteLine("Matrix Q:");
        Q.print();
        Console.WriteLine("Matrix R:");
        R.print();

        // Verify QR decomposition by multiplying Q and R
        matrix A_reconstructed = Q * R;
        Console.WriteLine("Reconstructed matrix A (Q * R):");
        A_reconstructed.print();

        // Least-squares fit part
        int noOfDataPoints = 9;  // Number of data points
        vector x = new vector(noOfDataPoints);  // Independent variable x
        vector y = new vector(noOfDataPoints);  // Dependent variable y
        vector dy = new vector(noOfDataPoints); // Uncertainties in y (dy)
        string filename = "data.txt";  // Data file location

        // Read data from the file
        string[] lines = File.ReadAllLines(filename);

        for (int i = 0; i < noOfDataPoints; i++) {
            try {
                string[] parts = lines[i].Split(new char[] {' ', '\t'}, StringSplitOptions.RemoveEmptyEntries);
                x[i] = double.Parse(parts[0], CultureInfo.InvariantCulture);  // Parse x
                y[i] = double.Parse(parts[1], CultureInfo.InvariantCulture);  // Parse y
                dy[i] = double.Parse(parts[2], CultureInfo.InvariantCulture); // Parse dy (uncertainty)
            } catch (FormatException ex) {
                Console.WriteLine($"Error parsing line {i + 1}: '{lines[i]}'. Exception: {ex.Message}");
                return;  // Stop execution if parsing fails
            }
        }

        // Transform the data (take logarithm of y, and adjust dy)
        for (int i = 0; i < noOfDataPoints; i++) {
            y[i] = Math.Log(y[i]);  // Apply logarithmic transformation to y
            dy[i] /= y[i];  // Adjust uncertainty
        }

        // Define the basis functions for the least-squares fit
        var fs = new Func<double, double>[] { z => 1.0, z => -z };  // Linear model: y = a - b*x

        // Perform least-squares fitting using the defined basis functions
        leastsquares ls = new leastsquares();
        (vector res, matrix cov) = ls.lsfit(fs, x, y, dy);

        // Extract the fit parameters a (intercept) and b (decay constant)
        double a = res[0];
        double b = res[1];

        // Display the fitted parameters
        Console.WriteLine($"\nFit parameters: a ≈ {a:F3}; b ≈ {b:F3}");
        
        // Calculate the half-life from the decay constant b
        double halflife = Math.Log(2) / b;
        Console.WriteLine($"Half-life is: {halflife:F3} days (expected ~3.63 days)");

        // Print the covariance matrix
        cov.print("Covariance matrix:");

        // Extract the uncertainties on the fitted parameters
        double uncer1 = Math.Sqrt(cov[0, 0]);  // Uncertainty in intercept a
        double uncer2 = Math.Sqrt(cov[1, 1]);  // Uncertainty in decay constant b
        Console.WriteLine($"Uncertainties in the fitting parameters: {uncer1:F3}; {uncer2:F3}");

        // Calculate the uncertainty in the half-life
        double halflifeuncer = Math.Log(2) * uncer2 / b;
        Console.WriteLine($"Uncertainty in half-life: {halflifeuncer:F3}");

        // Compare the calculated half-life with the expected value (3.63 days)
        if (halflife - halflifeuncer <= 3.63 && halflife + halflifeuncer >= 3.63) {
            Console.WriteLine("The calculated half-life agrees with the expected value within the estimated uncertainty.");
        } else {
            Console.WriteLine("The calculated half-life does not agree with the expected value within the estimated uncertainty.");
        }
    }
}
