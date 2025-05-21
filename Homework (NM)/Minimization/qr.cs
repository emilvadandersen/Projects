using System;
using static System.Math;

public class QRGS
{
    public matrix Q, R;

    // Constructor: Performs Modified Gram-Schmidt QR Decomposition
    public QRGS(matrix A)
    {
        int n = A.size2;
        Q = A.copy();
        R = new matrix(n, n); // R should be square

        for (int i = 0; i < n; i++)
        {
            R[i, i] = Q[i] % Q[i];  // R[i, i] = ||Q[i]||
            R[i, i] = Sqrt(R[i, i]); // Normalize
            Q[i] = Q[i] / R[i, i];  // Q[i] = Q[i] / ||Q[i]||

            for (int j = i + 1; j < n; j++)
            {
                R[i, j] = Q[i] % Q[j]; // R[i, j] = Q[i] * Q[j]
                Q[j] = Q[j] - Q[i] * R[i, j]; // Orthogonalize
            }
        }
    }

    // Solve QRx = b
    public vector solve(vector b)
    {
        // Ensure the size of b matches the number of rows in Q
        if (b.size != Q.size1)
        {
            throw new ArgumentException($"Vector b must have the same number of rows as matrix Q (expected {Q.size1}, got {b.size}).");
        }

        // Compute Q^T * b (i.e., c = Q^T * b)
        vector c = new vector(Q.size2); // c should have the same size as the number of columns in Q

        for (int i = 0; i < Q.size2; i++) // Loop over columns of Q (size2)
        {
            c[i] = 0;
            for (int j = 0; j < Q.size1; j++) // Loop over rows of Q (size1)
            {
                c[i] += Q[j, i] * b[j]; // Dot product of Q's i-th column with b
            }
        }

        // Perform back-substitution to solve Rx = c
        vector x = backSubstitution(R, c); // Solve Rx = c
        return x;
    }

    // Compute Matrix Inverse
    public matrix inverse()
    {
        int n = R.size1;
        matrix B = new matrix(n, n);
        matrix I = matrix.id(n);

        for (int i = 0; i < n; i++)
        {
            vector e = I[i];
            vector x = solve(e);
            for (int j = 0; j < n; j++)
                B[j, i] = x[j];  // Store column-wise
        }
        return B;
    }

    // Back-substitution for upper triangular system Rx = c
    private vector backSubstitution(matrix R, vector c)
    {
        int n = R.size1;
        vector x = new vector(n);

        for (int i = n - 1; i >= 0; i--)
        {
            double sum = 0;
            for (int j = i + 1; j < n; j++) // Loop over the remaining columns
                sum += R[i, j] * x[j];

            x[i] = (c[i] - sum) / R[i, i];
        }
        return x;
    }
}
