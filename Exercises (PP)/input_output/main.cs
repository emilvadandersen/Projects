/*
    C# Input/Output Exercise
    -------------------------
    This program reads numbers from different sources, calculates their sine and cosine, 
    and outputs them in a structured format.

    TASK 1: Command-line arguments
    -------------------------------
    - Accepts numbers as a comma-separated list using the `--numbers=` option.
    - Example command:
        mono main.exe --numbers=1,2,3,4,5

    TASK 2: Standard Input Stream
    ------------------------------
    - Reads a sequence of numbers from standard input, separated by spaces, tabs, or newlines.
    - Example command:
        echo "1 2 3 4 5" | mono main.exe

    TASK 3: File Input/Output
    --------------------------
    - Reads numbers from an input file and writes results to an output file.
    - Example command:
        mono main.exe --input=input.txt --output=output.txt
    - Can be automated using Makefile and then calling `make` command in terminal.
*/

using System;
using System.IO;

class Program
{
    static void ProcessNumbers(string[] numbers)
    {
        foreach (string num in numbers)
        {
            if (double.TryParse(num, out double value))
            {
                Console.WriteLine($"{value} sin({value}) = {Math.Sin(value):F6} cos({value}) = {Math.Cos(value):F6}");
            }
            else
            {
                Console.Error.WriteLine($"Error: '{num}' is not a valid number.");
            }
        }
    }

    static void ProcessFile(string inputFile, string outputFile)
    {
        // Check if the input file exists
        if (!File.Exists(inputFile))
        {
            Console.Error.WriteLine("Error: Input file not found.");
            return;
        }

        // Open the input and output files
        using (StreamReader instream = new StreamReader(inputFile))
        using (StreamWriter outstream = new StreamWriter(outputFile, false))
        {
            string line;
            // Read each line from the input file
            while ((line = instream.ReadLine()) != null)
            {
                string[] numbers = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string num in numbers)
                {
                    if (double.TryParse(num, out double value))
                    {
                        outstream.WriteLine($"{value} sin({value}) = {Math.Sin(value):F6} cos({value}) = {Math.Cos(value):F6}");
                    }
                    else
                    {
                        Console.Error.WriteLine($"Error: '{num}' is not a valid number.");
                    }
                }
            }
        }
    }

    static void Main(string[] args)
    {
        string inputFile = null;
        string outputFile = null;
        string[] numbers = null;

        if (args.Length > 0)
        {
            // Loop through the command-line arguments
            for (int i = 0; i < args.Length; i++)
            {
                string[] words = args[i].Split('=');
                if (words.Length == 2)
                {
                    if (words[0] == "--numbers")
                    {
                        // Capture numbers directly from the --numbers argument
                        numbers = words[1].Split(',');
                    }
                    else if (words[0] == "--input")
                    {
                        // Capture the input file path from the --input argument
                        inputFile = words[1];
                    }
                    else if (words[0] == "--output")
                    {
                        // Capture the output file path from the --output argument
                        outputFile = words[1];
                    }
                }
            }
        }

        // Case 1: Numbers are passed directly via --numbers argument
        if (numbers != null)
        {
            ProcessNumbers(numbers);
        }
        // Case 2: Input and Output file arguments are passed
        else if (inputFile != null && outputFile != null)
        {
            ProcessFile(inputFile, outputFile);
        }
        // Case 3: No arguments provided, read from standard input
        else
        {
            string input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input))
            {
                numbers = input.Split(new[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                ProcessNumbers(numbers);
            }
        }
    }
}
