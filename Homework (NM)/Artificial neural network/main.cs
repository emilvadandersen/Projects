using System;
using System.Collections.Generic;
using System.IO;

class Program {
    static void Main() {
        var xs = new List<double>(); // List of x-values for training
        var ys = new List<double>(); // Corresponding target y-values (function outputs)

        // Define the target function: f(x) = cos(5x - 1) * exp(-x^2)
        Func<double, double> g = x => Math.Cos(5 * x - 1) * Math.Exp(-x * x);

        int N = 200; // Number of sample points
        for (int i = 0; i < N; i++) {
            double x = -1 + 2.0 * i / (N - 1); // Evenly spaced x in [-1, 1]
            xs.Add(x);
            ys.Add(g(x)); // Sample the target function at x
        }

        // Initialize neural network with 8 hidden neurons
        var network = new ann(8);

        // Train the network on sampled data
        network.train(xs, ys);

        // Write detailed results to "Out.txt"
        using (var writer = new StreamWriter("Out.txt")) {
            writer.WriteLine("   x     f(x)  f'(x)  f''(x)  ∫f(x)dx");

            // Evaluate and print network output, derivatives, and integral over range
            for (double x = -1; x <= 1.05; x += 0.05) {
                double fx = network.response(x);             // Neural net output
                double dfx = network.derivative(x);          // First derivative
                double d2fx = network.second_derivative(x);  // Second derivative
                double intfx = network.antiderivative(x);    // Approximate integral
                writer.WriteLine($"{x:F3}  {fx:F3}  {dfx:F3}  {d2fx:F3}  {intfx:F3}");
            }

            // Documentation and qualitative analysis
            writer.WriteLine("");
            writer.WriteLine("--- Target Function ---");
            writer.WriteLine("f(x) = cos(5x - 1) * exp(-x^2)");

            writer.WriteLine("");
            writer.WriteLine("--- Conclusion ---");
            writer.WriteLine("The approximation varies with every run. Sometimes it is very good; sometimes it is not.");
            writer.WriteLine("Most often; the neural network with 8 hidden neurons approximates the target function and its derivatives;");
            writer.WriteLine("but the values; particularly near some or all peaks; are noticeably underestimated compared to the expected magnitude.");
            writer.WriteLine("This suggests the model captures the general shape but struggles with accurately fitting the higher-amplitude features.\n");

            writer.WriteLine("Improving the approximation may require careful tuning of training parameters such as the learning rate and initialization strategy.");
            writer.WriteLine("Increasing the number of hidden neurons might enhance the network’s capacity but can also introduce instability;");
            writer.WriteLine("potentially causing NaN values or divergence without proper regularization or training adjustments.");
            writer.WriteLine("Adding stricter parameter constraints and refining the cost function can help maintain stable optimization.\n");

            writer.WriteLine("Overall; while the current network mostly captures the qualitative behavior; further work is needed to improve precision");
            writer.WriteLine("at critical points and ensure stable convergence.");
        }

        // Write data for plotting: x, actual f(x), neural network approximation
        using (var plotWriter = new StreamWriter("ann_fit.data.txt")) {
            for (double x = -1; x <= 1; x += 0.05) {
                double actual = g(x);
                double predicted = network.response(x);
                plotWriter.WriteLine($"{x:F5}\t{actual:F5}\t{predicted:F5}");
            }
        }
    }
}
