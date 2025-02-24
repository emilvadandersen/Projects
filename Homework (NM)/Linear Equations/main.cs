using System;

class MainClass
{
    static void Main()
    {
        var rnd = new System.Random(1);
        int n = 4;  // Number of rows and columns for square matrix (n x n)
        int m = 3;  // Number of columns for the tall matrix (n > m)

        // --- QR Decomposition on a Tall Matrix A (n > m) ---
        Console.WriteLine("---- QR Decomposition Check (Tall Matrix A) ----");
        Console.WriteLine();

        // Generate a random tall matrix A (n x m)
        matrix A = new matrix(n, m);  // Tall matrix with n rows and m columns
        for (int i = 0; i < n; i++)
            for (int j = 0; j < m; j++)
                A[i, j] = rnd.NextDouble();

        A.print("Matrix A (n x m):");

        // Perform QR Decomposition
        QRGS qr = new QRGS(A);

        // Display Q and R matrices
        qr.Q.print("Matrix Q (Orthogonal):");
        qr.R.print("Matrix R (Upper Triangular):");

        // Check if R is upper triangular
        bool isUpperTriangular = true;
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < i; j++)
            {
                if (Math.Abs(qr.R[i, j]) > 1e-6)  // Should be close to 0 below diagonal
                {
                    isUpperTriangular = false;
                    break;
                }
            }
        }
        Console.WriteLine("R is Upper Triangular: " + (isUpperTriangular ? "Passed" : "Failed"));

        // Verify that Q^T * Q = I (identity matrix)
        matrix QTQ = qr.Q.transpose() * qr.Q;
        QTQ.print("Q^T * Q (Should be Identity):");

        // Check QR = A (Reconstruction check)
        matrix QR = qr.Q * qr.R;
        QR.print("QR (Should be A):");

        // Check if QR approximately equals A
        bool qrCheck = true;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                if (Math.Abs(QR[i, j] - A[i, j]) > 1e-6)  // Allow small floating point errors
                {
                    qrCheck = false;
                    break;
                }
            }
        }
        Console.WriteLine("QR Check (QR = A): " + (qrCheck ? "Passed" : "Failed"));

        Console.WriteLine();
        Console.WriteLine("---- Solve Check (Square Matrix A) ----");
        Console.WriteLine();

        // --- Solve Check (on a Square Matrix) ---
        // Generate a random square matrix A (n x n)
        matrix squareA = new matrix(n, n);  // Square matrix for solve check
        for (int i = 0; i < n; i++)
            for (int j = 0; j < n; j++)
                squareA[i, j] = rnd.NextDouble();

        squareA.print("Square Matrix A (n x n):");

        // Generate a random vector b of size n
        vector b = new vector(n);
        for (int i = 0; i < n; i++)
            b[i] = rnd.NextDouble();

        b.print("Random Vector b:");

        // Perform QR Decomposition on the square matrix
        QRGS squareQR = new QRGS(squareA);

        // Display Q and R matrices for the square matrix
        squareQR.Q.print("Matrix Q (Orthogonal) for Solve:");
        squareQR.R.print("Matrix R (Upper Triangular) for Solve:");

        // Solve QRx = b
        vector x = squareQR.solve(b);
        vector Ax = squareA * x;  // Reconstruct Ax

        x.print("Solution x:");
        Ax.print("Reconstructed Ax (Should be b):");

        // Check if Ax approximately equals b
        bool solveCheck = true;
        for (int i = 0; i < n; i++)
        {
            if (Math.Abs(Ax[i] - b[i]) > 1e-6)  // Allow for small floating point errors
            {
                solveCheck = false;
                break;
            }
        }
        Console.WriteLine("Solve Check (Ax = b): " + (solveCheck ? "Passed" : "Failed"));

        Console.WriteLine();
        Console.WriteLine("---- Inverse Check (Square Matrix A) ----");
        Console.WriteLine();

        // --- Inverse Check (on a Square Matrix) ---
        // Calculate the inverse of squareA using QR decomposition
        matrix A_inv = squareQR.inverse();  // A_inv = R^-1 * Q^T
        A_inv.print("A Inverse:");

        // Verify that AB = I (identity matrix)
        matrix AB = squareA * A_inv;
        AB.print("AB (Should be Identity):");

        // Check if AB is close to the identity matrix
        bool inverseCheck = true;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (Math.Abs(AB[i, j] - (i == j ? 1.0 : 0.0)) > 1e-6)  // Identity matrix check
                {
                    inverseCheck = false;
                    break;
                }
            }
        }
        Console.WriteLine("Inverse Check (AB = I): " + (inverseCheck ? "Passed" : "Failed"));
    }
}
