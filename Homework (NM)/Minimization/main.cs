using System;
using System.Collections.Generic;
using System.IO;
using static System.Math;

public class main {
    // Breit-Wigner resonance function WITHOUT amplitude scaling (A = 1)
    // Used in the Higgs boson signal modeling
    static double BreitWigner(double E, double m, double Γ) {
        return 1.0 / (Pow(E - m, 2) + Pow(Γ, 2) / 4);
    }

    public static void Main(string[] args) {
        using (var writer = new StreamWriter("Out.txt")) {
            
            // === PART 1: Minimization tests using known functions ===
            writer.WriteLine("=== PART 1: Minimization tests using known functions ===");
            
            writer.WriteLine("Rosenbrock Minimization:");
            // Initial guess for Rosenbrock function
            vector x_rosen = new vector(2); 
            x_rosen[0] = -3.0; 
            x_rosen[1] = -4.0;

            // Define Rosenbrock function
            var rosen = new Func<vector, double>(v => Pow(1 - v[0], 2) + 100 * Pow(v[1] - v[0]*v[0], 2));

            // Perform Newton minimization
            var (x_min_rosen, steps_rosen) = Minimization.newton(rosen, x_rosen);

            // Output results
            writer.WriteLine($"Minimum at: {x_min_rosen}");
            writer.WriteLine("Expected minimum: (1; 1)");
            writer.WriteLine($"Steps taken: {steps_rosen}");
            writer.WriteLine("");

            writer.WriteLine("Himmelblau Minimization:");

            // Initial guess for Himmelblau's function
            vector x_himmel = new vector(2); 
            x_himmel[0] = -5.0; 
            x_himmel[1] = -3.0;

            // Define Himmelblau's function
            var himmel = new Func<vector, double>(v => Pow(v[0]*v[0] + v[1] - 11, 2) + Pow(v[0] + v[1]*v[1] - 7, 2));

            // Perform Newton minimization
            var (x_min_himmel, steps_himmel) = Minimization.newton(himmel, x_himmel);

            // Output results
            writer.WriteLine($"Minimum at: {x_min_himmel}");
            writer.WriteLine("Expected one of the minima: (3; 2) or (-2.805; 3.131) or (-3.779; -3.283) or (3.584; -1.848)");
            writer.WriteLine($"Steps taken: {steps_himmel}");

            // === PART 2: Fit to Higgs boson data ===

            var energy = new List<double>();  // Measured energies
            var signal = new List<double>();  // Measured signal at those energies
            var error  = new List<double>();  // Measurement uncertainties

            // Read and parse data from file
            foreach (string line in File.ReadLines("higgs.data.txt")) {
                if (line.StartsWith("#") || line.Trim() == "") continue;  // Skip comments and empty lines

                var tokens = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                energy.Add(double.Parse(tokens[0]));
                signal.Add(double.Parse(tokens[1]));
                error.Add(double.Parse(tokens[2]));
            }

            // Define chi-squared function for fitting
            Func<vector, double> chi2 = (v) => {
                double m = v[0], Γ = v[1], A = v[2];  // Parameters: mass, width, amplitude
                double sum = 0;

                for (int i = 0; i < energy.Count; i++) {
                    double E = energy[i], σ = signal[i], Δσ = error[i];
                    double F = A * BreitWigner(E, m, Γ);  // Predicted signal
                    sum += Pow((F - σ) / Δσ, 2);  // Chi-squared contribution
                }
                return sum;
            };

            // Initial guess for fitting parameters
            vector x_init = new vector(3);
            x_init[0] = 125;   // Mass (m)
            x_init[1] = 2.5;   // Width (Γ)
            x_init[2] = 15;    // Amplitude (A)

            // Perform the fit using Newton's method
            var result_fit = Minimization.newton(chi2, x_init);
            vector x_fit = result_fit.Item1;
            double m_fit = x_fit[0], Γ_fit = x_fit[1], A_fit = x_fit[2];

            // Estimate uncertainties using the Hessian
            var H = Minimization.hessian(chi2, x_fit);  // Hessian matrix of chi-squared
            var qrH = new QRGS(H);                      // QR decomposition
            var cov = 2 * qrH.inverse();                // Covariance matrix: inverse of Hessian scaled by 2

            double dm = Sqrt(cov[0, 0]);  // Uncertainty in mass
            double dΓ = Sqrt(cov[1, 1]);  // Uncertainty in width
            double dA = Sqrt(cov[2, 2]);  // Uncertainty in amplitude

            // Output fit results
            writer.WriteLine("");
            writer.WriteLine("=== PART 2: Fit to Higgs boson data ===");
            writer.WriteLine($"Mass     m  = {m_fit:F4} ± {dm:F4}");
            writer.WriteLine($"Width    Γ  = {Γ_fit:F4} ± {dΓ:F4}");
            writer.WriteLine($"Amplitude A = {A_fit:F4} ± {dA:F4}");
            writer.WriteLine($"sqrt(χ²/N)  = {Sqrt(chi2(x_fit)/energy.Count):F4}");

            // === Output data for plotting the fitted curve ===
            using (var dataOut = new StreamWriter("fit.data.txt")) {
                double E_start = energy[0];
                double E_end = energy[energy.Count - 1];
                double step = 1.0 / 16;

                for (double E = E_start; E <= E_end; E += step) {
                    double F = A_fit * BreitWigner(E, m_fit, Γ_fit);  // Predicted value at energy E
                    dataOut.WriteLine($"{E} {F}");  // Save to file for plotting
                }
            }
        }
    }
}
