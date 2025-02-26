using System;

class MainClass
{
    static void Main()
    {
        int n = 4; // Size of the matrix
        Random rand = new Random();
        matrix A = new matrix(n, n);

        // Generate a random symmetric matrix
        for (int i = 0; i < n; i++)
        {
            for (int j = i; j < n; j++)
            {
                double value = rand.NextDouble(); // Random value
                A[i, j] = value;
                A[j, i] = value; // Ensure symmetry
            }
        }

        Console.WriteLine("Original Matrix A:");
        A.print();

        // Compute eigenvalues and eigenvectors using Jacobi's method
        (vector w, matrix V) = jacobi.cyclic(A);

        Console.WriteLine("\nEigenvalues:");
        w.print();

        Console.WriteLine("\nEigenvectors Matrix V:");
        V.print();

        // Construct diagonal matrix D from eigenvalues
        matrix D = new matrix(n, n);
        for (int i = 0; i < n; i++)
            D[i, i] = w[i];

        // Compute various checks: VT A V == D, VDVT == A, VTV == I, VVT == I
        matrix VT = V.transpose();
        matrix VTAV = VT * A * V;
        matrix VDVt = V * D * VT;
        matrix VTV = VT * V;
        matrix VVT = V * VT;

        Console.WriteLine("\nCheck V^T A V == D: (D being the diagonal matrix with the corresponding eigenvalues)");
        VTAV.print();

        Console.WriteLine("\nCheck V D V^T == A:");
        VDVt.print();

        Console.WriteLine("\nCheck V^T V == I (Identity):");
        VTV.print();

        Console.WriteLine("\nCheck V V^T == I (Identity):");
        VVT.print();
    }
}
