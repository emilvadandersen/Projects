# Compiler settings
CC = mcs            # C# Compiler command
EXEC = main.exe      # Output executable
SRC = main.cs        # Source file

# Input and output files
INPUT_FILE = input.txt
OUTPUT_FILE = output.txt

# Default target
all: build run

# Build target
build:
	$(CC) $(SRC) -out:$(EXEC)   # Compile the C# file into the executable

# Run target
run:
	# Replace commas with dots in the input file, then run the program
	sed 's/,/\./g' $(INPUT_FILE) | mono $(EXEC) --input=/dev/stdin --output=$(OUTPUT_FILE)

	# After processing, replace commas with dots in the output file (if needed)
	sed 's/,/\./g' $(OUTPUT_FILE) > temp.txt && mv temp.txt $(OUTPUT_FILE)

# Clean up target
clean:
	rm -f $(EXEC) $(OUTPUT_FILE)  # Remove executable and output file
	rm -f temp.txt                # Clean up temp file
