using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    // Data class to hold the range and partial sum for each thread
    public class Data
    {
        public int a, b;
        public double sum;
    }

    // Function to compute a part of the harmonic sum
    public static void Harm(object obj)
    {
        var arg = (Data)obj;
        arg.sum = 0;
        for (int i = arg.a; i < arg.b; i++)
        {
            arg.sum += 1.0 / i;
        }
    }

    static void Main(string[] args)
    {
        // Default values
        int nthreads = 1, nterms = (int)1e8;
        bool useParallelFor = false;
        bool useThreadLocal = false;

        // Parse command-line arguments
        foreach (var arg in args)
        {
            var words = arg.Split(':');
            if (words[0] == "-threads") nthreads = int.Parse(words[1]);
            if (words[0] == "-terms") nterms = (int)float.Parse(words[1]);
            if (words[0] == "-parallel") useParallelFor = true;
            if (words[0] == "-threadlocal") useThreadLocal = true;
        }

        double total = 0;

        if (useParallelFor)
        {
            // ***********************
            // Using Parallel.For (Incorrect)
            // ***********************
            Console.WriteLine("Using Parallel.For (Incorrect method)");
            total = 0;
            Parallel.For(1, nterms + 1, (int i) => total += 1.0 / i);
        }
        else if (useThreadLocal)
        {
            // ***********************
            // Using Parallel.For with ThreadLocal (Corrected)
            // ***********************
            Console.WriteLine("Using Parallel.For with ThreadLocal (Correct method)");
            var sum = new ThreadLocal<double>(() => 0, trackAllValues: true);

            Parallel.For(1, nterms + 1, (int i) => sum.Value += 1.0 / i);

            total = sum.Values.Sum(); // Aggregate results
        }
        else
        {
            // ***********************
            // Manual Multithreading
            // ***********************
            Console.WriteLine($"Using manual multithreading with {nthreads} threads");

            // Prepare thread data
            Data[] parameters = new Data[nthreads];
            for (int i = 0; i < nthreads; i++)
            {
                parameters[i] = new Data();
                parameters[i].a = 1 + nterms / nthreads * i;
                parameters[i].b = 1 + nterms / nthreads * (i + 1);
            }
            parameters[parameters.Length - 1].b = nterms + 1; // Adjust last endpoint

            // Create and start threads
            Thread[] threads = new Thread[nthreads];
            for (int i = 0; i < nthreads; i++)
            {
                threads[i] = new Thread(Harm);
                threads[i].Start(parameters[i]);
            }

            // Wait for all threads to complete
            foreach (var thread in threads) thread.Join();

            // Aggregate results
            total = 0;
            foreach (var p in parameters) total += p.sum;
        }

        // Output final result
        Console.WriteLine($"Harmonic sum: {total}");
    }
}
