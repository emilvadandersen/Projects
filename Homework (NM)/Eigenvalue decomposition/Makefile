# Compiler and flags
CSC = csc
FLAGS = -out:JacobiEVD.exe

# Source files
SOURCES = main.cs jacobi.cs matrix.cs vector.cs

# Executable name
EXEC = JacobiEVD.exe

# Default target: Compile and run the program
all: $(EXEC)
	@echo "Running program..."
	@mono $(EXEC) > output.txt
	@sed -i '' 's/,/\./g' output.txt
	@echo "Output saved to output.txt (commas replaced with dots)"

# Compile the project
$(EXEC): $(SOURCES)
	@echo "Compiling..."
	$(CSC) $(FLAGS) $(SOURCES)

# Clean up generated files
clean:
	@echo "Cleaning up..."
	rm -f $(EXEC) output.txt