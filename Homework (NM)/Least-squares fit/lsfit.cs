using System;

public class leastsquares {
    // Method to perform least squares fitting
    public (vector, matrix) lsfit(Func<double, double>[] fs, vector x, vector y, vector dy) {
        
        // Get the number of data points (n) and the number of functions (m)
        int n = x.size;  // Number of data points
        int m = fs.Length;  // Number of functions
        
        // Initialize matrix A (n x m) and vector b (n)
        matrix A = new matrix(n, m);
        vector b = new vector(n);

        // Construct matrix A and vector b
        // Loop over all data points
        for (int i = 0; i < n; i++) {
            b[i] = y[i] / dy[i];  // Adjust the y-values by dividing by the uncertainties (dy)
            
            // Loop over each function in the model (fs)
            for (int k = 0; k < m; k++) {
                A[i, k] = fs[k](x[i]) / dy[i];  // Populate matrix A with the model function evaluated at x[i], divided by dy[i]
            }
        }
        
        // Perform QR decomposition on matrix A using QRGS
        // QRGS.decomp(A) decomposes A into Q and R matrices
        (matrix Q, matrix R) = QRGS.decomp(A);
        
        // Solve the least-squares problem: c = (R^(-1)) * (Q^T) * b
        vector c = QRGS.solve(Q, R, b);  // This will solve for the coefficients of the model

        // Now calculate the covariance matrix
        // The covariance matrix is given by the inverse of (A^T * A)
        matrix A_sqr = A.transpose() * A;  // Compute A^T * A
        matrix cov = QRGS.inverse(A_sqr);  // Compute the inverse of A^T * A to get the covariance matrix

        // Return the coefficients (c) and covariance matrix (cov)
        return (c, cov);
    }
}
