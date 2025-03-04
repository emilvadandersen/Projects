public static class QRGS {
    
    // QR Decomposition: Decomposes matrix A into Q and R such that A = Q * R
    public static (matrix, matrix) decomp(matrix A) {
        
        // Create a copy of matrix A to avoid modifying the original matrix
        matrix Q = A.copy();  // Q will store the orthogonal matrix
        matrix R = new matrix(A.size2, A.size2);  // R will store the upper triangular matrix

        // Iterate over each column of A to calculate Q and R
        for (int i = 0; i < A.size2; i++) {
            
            // Compute the norm (magnitude) of column i of Q to fill the diagonal of R
            R[i, i] = Q[i].norm();  
            
            // Normalize the column i of Q to make it a unit vector
            Q[i] /= R[i, i];  

            // Iterate through the remaining columns to compute the off-diagonal elements of R
            for (int j = i + 1; j < A.size2; j++) {
                
                // Calculate the dot product between the i-th and j-th columns of Q to fill R[i, j]
                R[i, j] = Q[i].dot(Q[j]);  
                
                // Subtract the projection of column i from column j to make Q[j] orthogonal to Q[i]
                Q[j] -= Q[i] * R[i, j];  
            }
        }

        // Return the orthogonal matrix Q and the upper triangular matrix R
        return (Q, R);
    }// decomp

    
    // Solves the system Rx = Q^T * b for x using back substitution
    public static vector solve(matrix Q, matrix R, vector b) {
        
        int n = Q.size2;  // Get the size of the matrix (number of rows/columns in Q and R)
        
        // Compute Q^T * b, which is the projection of b onto the columns of Q
        vector QTb = Q.transpose() * b;
        
        vector x = new vector(n);  // Initialize the solution vector x

        // Perform backward substitution to solve the system R * x = Q^T * b
        for (int i = n - 1; i >= 0; i--) {  
            double sum = 0;
            
            // Sum the contributions from the already computed components of x
            for (int j = i + 1; j < n; j++) {
                sum += R[i, j] * x[j];
            }
            
            // Solve for the i-th component of x
            x[i] = (QTb[i] - sum) / R[i, i];
        }

        // Return the solution vector x
        return x;
    }// solve


    // Computes the determinant of the upper triangular matrix R
    public static double det(matrix R) {
        
        double determinant = 1.0;
        
        // The determinant of an upper triangular matrix is the product of its diagonal elements
        for (int i = 0; i < R.size1; i++) {
            determinant *= R[i, i];  // Multiply each diagonal element
        }

        // Return the determinant
        return determinant;
    }// det


    // Computes the inverse of matrix A using QR decomposition
    public static matrix inverse(matrix A) {
        
        int n = A.size1;  // Get the size of the matrix (number of rows/columns in A)
        
        matrix inverseA = new matrix(n, n);  // Initialize the inverse matrix
        
        // For each column i, solve the system A * x = e_i (where e_i is the unit vector)
        for (int i = 0; i < n; i++) {
            
            // Create a unit vector e where only the i-th component is 1, the rest are 0
            vector e = new vector(n);
            for (int j = 0; j < n; j++) {
                e[j] = (i == j) ? 1 : 0;  // Set the i-th component to 1, others to 0
            }

            // Perform QR decomposition of A
            (matrix Q, matrix R) = decomp(A);
            
            // Solve for the i-th column of the inverse of A
            inverseA[i] = solve(Q, R, e);
        }

        // Return the inverse matrix
        return inverseA;
    }// inverse
}
