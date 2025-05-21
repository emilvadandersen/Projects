using System;
using System.Collections.Generic;
using static System.Math;

public class ann {
    public int n; // Number of hidden neurons
    public Func<double, double> f = x => x * Exp(-x * x); // Activation function: Gaussian wavelet
    public vector parameters; // Stores all parameters: a_i, b_i, w_i for each neuron

    // Constructor: initializes parameters randomly
    public ann(int n) {
        this.n = n;
        parameters = new vector(3 * n); // Each neuron has 3 parameters
        var rand = new Random();
        for (int i = 0; i < n; i++) {
            parameters[3 * i]     = 2 * rand.NextDouble() - 1;      // a_i ∈ [-1, 1]
            parameters[3 * i + 1] = 0.1 + 0.4 * rand.NextDouble();  // b_i ∈ [0.1, 0.5]
            parameters[3 * i + 2] = 2 * rand.NextDouble() - 1;      // w_i ∈ [-1, 1]
        }
    }

    // Compute network output at point x
    public double response(double x) {
        double sum = 0;
        for (int i = 0; i < n; i++) {
            double a = parameters[3 * i];
            double b = parameters[3 * i + 1];
            double w = parameters[3 * i + 2];
            sum += f((x - a) / b) * w;
        }
        return sum;
    }

    // Compute first derivative of the network output
    public double derivative(double x) {
        double sum = 0;
        for (int i = 0; i < n; i++) {
            double a = parameters[3 * i];
            double b = parameters[3 * i + 1];
            double w = parameters[3 * i + 2];
            double u = (x - a) / b;
            double df = (1 - 2 * u * u) * Exp(-u * u); // Derivative of x*exp(-x^2)
            sum += (w / b) * df;
        }
        return sum;
    }

    // Compute second derivative of the network output
    public double second_derivative(double x) {
        double sum = 0;
        for (int i = 0; i < n; i++) {
            double a = parameters[3 * i];
            double b = parameters[3 * i + 1];
            double w = parameters[3 * i + 2];
            double u = (x - a) / b;
            double d2f = (6 * u - 4 * Pow(u, 3)) * Exp(-u * u); // 2nd derivative of x*exp(-x^2)
            sum += (w / (b * b)) * d2f;
        }
        return sum;
    }

    // Compute an analytical antiderivative of the network output
    public double antiderivative(double x) {
        double sum = 0;
        for (int i = 0; i < n; i++) {
            double a = parameters[3 * i];
            double b = parameters[3 * i + 1];
            double w = parameters[3 * i + 2];
            double u = (x - a) / b;
            sum += w * (-0.5 * b * Exp(-u * u)); // Integral of x*exp(-x^2) is -0.5*exp(-x^2)
        }
        return sum;
    }

    // Train the network to fit the target values using optimization
    public void train(List<double> xs, List<double> ys) {
        // Define cost function: mean squared error between model and target values
        Func<vector, double> cost = (vector p) => {
            double sum = 0;
            for (int k = 0; k < xs.Count; k++) {
                double xk = xs[k];
                double yk = ys[k];
                double net = 0;

                // Evaluate network at xk using parameters p
                for (int i = 0; i < n; i++) {
                    double a = p[3 * i];
                    double b = p[3 * i + 1];
                    double w = p[3 * i + 2];
                    net += f((xk - a) / b) * w;
                }

                // Add squared error
                sum += Pow(net - yk, 2);
            }
            return sum;
        };

        // Optimize parameters to minimize cost function
        var (result, steps) = Minimization.newton(cost, parameters);
        parameters = result; // Update internal parameters
    }
}
