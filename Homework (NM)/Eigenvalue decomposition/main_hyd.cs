using System;
using System.IO;

class HydrogenSolver
{
    static void Main()
    {
        // Step 1: Perform hydrogen calculations and save results
        string outputFile = "dr_convergence.txt";
        using (StreamWriter file = new StreamWriter(outputFile))
        {
            file.WriteLine("  Δr       ε0");

            double rmax = 10.0;

            // Step sizes for dr to test
            double[] dr_values = { 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1 };

            foreach (double dr in dr_values)
            {
                int npoints = (int)(rmax / dr) - 1;
                vector r = new vector(npoints);

                // Fill the r vector with appropriate values for the current dr
                for (int i = 0; i < npoints; i++)
                    r[i] = dr * (i + 1);

                // Build the Hamiltonian matrix
                matrix H = Hamiltonian.BuildMatrix(npoints, r, dr);

                // Run Jacobi method to get eigenvalues and eigenvectors
                (vector eigenvalues, _) = jacobi.cyclic(H);

                // Write each dr and corresponding ground state energy to the file
                file.WriteLine($"{dr:F3}   {eigenvalues[0]:F6}");
            }
        }

        // Step 2: Run Convergence Test for rmax
        RunConvergenceTestRmax();

        // Step 3: Save wavefunctions after all calculations and tests
        SaveWavefunctions();
    }

    // Convergence test for rmax
    static void RunConvergenceTestRmax()
    {
        // Open a file to write the results of the convergence test for rmax
        using (var writer = new StreamWriter("rmax_convergence.txt"))
        {
            writer.WriteLine("  rmax      ε0"); // Header for the output file

            // Loop over different values of rmax from 1 to 10
            for (double rmax_test = 1; rmax_test <= 10; rmax_test += 1)
            {
                double dr_fixed = 0.2;  // Fixed step size for dr
                int np = (int)(rmax_test / dr_fixed) - 1;  // Calculate number of grid points based on rmax and dr
                vector r_conv = new vector(np);  // Create a vector to hold radial distances

                // Fill the r_conv vector with the radial distances based on dr_fixed
                for (int i = 0; i < np; i++) r_conv[i] = dr_fixed * (i + 1);

                // Build the Hamiltonian matrix for the current rmax_test
                matrix H_conv = new matrix(np, np);

                // Set up the Hamiltonian matrix elements based on the finite difference method
                // Diagonal and off-diagonal elements for the Laplacian operator (kinetic energy part)
                for (int i = 0; i < np - 1; i++)
                {
                    H_conv[i, i] = -2 * (-0.5 / dr_fixed / dr_fixed); // Diagonal elements
                    H_conv[i, i + 1] = 1 * (-0.5 / dr_fixed / dr_fixed); // Off-diagonal elements
                    H_conv[i + 1, i] = 1 * (-0.5 / dr_fixed / dr_fixed); // Off-diagonal elements
                }

                // Set the last diagonal element
                H_conv[np - 1, np - 1] = -2 * (-0.5 / dr_fixed / dr_fixed);

                // Include the potential energy part (1/r term) in the Hamiltonian
                for (int i = 0; i < np; i++) H_conv[i, i] += -1 / r_conv[i];

                // Solve the eigenvalue problem using the Jacobi method
                (vector e_conv, matrix V_conv) = jacobi.cyclic(H_conv);

                // Write the result of the current rmax_test and its ground state energy to the file
                writer.WriteLine($"{rmax_test:f5} {e_conv[0]:f8}");
            }
        }
    }

    // Save wavefunctions to a file
    static void SaveWavefunctions()
    {
        // Set parameters for hydrogen atom problem
        double rmax = 10.0;  // Maximum radial distance
        double dr = 0.2;  // Step size for r

        int npoints = (int)(rmax / dr) - 1;

        // Create the radial vector r
        vector r = new vector(npoints);
        for (int i = 0; i < npoints; i++)
        {
            r[i] = dr * (i + 1);  // Fill with radial positions
        }

        // Build the Hamiltonian matrix for the hydrogen problem
        matrix H = Hamiltonian.BuildMatrix(npoints, r, dr);

        // Solve the eigenvalue problem using Jacobi's method
        (vector eigenvalues, matrix wavefunctions) = jacobi.cyclic(H);

        // Normalize wavefunctions
        double norm = 1.0 / Math.Sqrt(dr);

        // Writing the wavefunctions to the file
        using (var writer = new StreamWriter("wavefunctions.txt"))
        {
            writer.WriteLine("    r      f0(r)      f1(r)      f2(r)");

            // Loop over each radial point and save the wavefunction values
            for (int i = 0; i < npoints; i++)
            {
                // Radial distance
                double rval = dr * (i + 1);

                // Eigenfunctions (wavefunctions) for each state
                double f0 = norm * wavefunctions[i, 0];  // Ground state wavefunction
                double f1 = norm * wavefunctions[i, 1];  // First excited state
                double f2 = norm * wavefunctions[i, 2];  // Second excited state

                // Write to the file
                writer.WriteLine($"{rval:f5} {f0:f8} {f1:f8} {f2:f8}");
            }
        }
    }
}
