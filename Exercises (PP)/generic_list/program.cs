using System;
using System.IO;

public class genlist<T>
{
    public T[] data;
    public int size => data.Length;  // Property to get the current size
    public T this[int i] => data[i]; // Indexer for accessing elements

    public genlist()
    {
        data = new T[0]; // Initialize an empty list
    }

    public void add(T item)
    {
        T[] newdata = new T[size + 1]; // Create a new array with an extra space
        Array.Copy(data, newdata, size); // Copy existing data to the new array
        newdata[size] = item; // Add the new item at the end
        data = newdata; // Reassign the data to the new array
    }
}

public class Program
{
    public static void Main()
    {
        var list = new genlist<double[]>(); // Create a new generic list to hold double arrays
        char[] delimiters = { ' ', '\t' }; // Define delimiters for splitting input
        var options = StringSplitOptions.RemoveEmptyEntries;

        // Specify the path to the input file (you can change "input.txt" to your actual file path)
        string filePath = "input.txt";

        // Check if the file exists before proceeding
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"The file {filePath} does not exist.");
            return;
        }

        // Open and read the file using StreamReader
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var words = line.Split(delimiters, options); // Split the line into words
                int n = words.Length; // Get the number of words
                var numbers = new double[n]; // Create a new array for the numbers

                // Parse each word to a double and store it in the numbers array
                for (int i = 0; i < n; i++)
                {
                    numbers[i] = double.Parse(words[i]);
                }

                list.add(numbers); // Add the numbers array to the list
            }
        }

        // Print the numbers in exponential format
        for (int i = 0; i < list.size; i++)
        {
            var numbers = list[i]; // Get the numbers array
            foreach (var number in numbers)
            {
                Console.Write($"{number:0.00e+00;-0.00e+00} "); // Format each number in exponential format
            }
            Console.WriteLine(); // Move to the next line after printing a row
        }
    }
}
