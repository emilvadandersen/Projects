using System;

class Program
{
    static void Main()
    {
        // Create two Vec objects
        Vec v1 = new Vec(1, 2, 3);
        Vec v2 = new Vec(1, 2.001, 3);

        // Print vectors
        Console.WriteLine("Vector 1: " + v1);
        Console.WriteLine("Vector 2: " + v2);

        // Perform operations and print results
        Console.WriteLine("Addition: " + (v1 + v2));
        Console.WriteLine("Subtraction: " + (v1 - v2));
        Console.WriteLine("Multiplication by 2: " + (v1 * 2));
        Console.WriteLine("Division by 2: " + (v1 / 2));
        Console.WriteLine("Dot Product: " + v1.Dot(v2));

        // Approximation comparison
        bool approximationResult = v1.Approx(v2);
        Console.WriteLine("Approximation using instance method: " + approximationResult);

        // Static Approximation comparison
        bool staticApproximationResult = Vec.Approx(v1, v2);
        Console.WriteLine("Approximation using static method: " + staticApproximationResult);
    }
}
