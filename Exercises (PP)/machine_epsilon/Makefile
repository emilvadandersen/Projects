# Compiler
CSC = mcs

# Output executable
OUTPUT = epsilon.exe

# Source files
SRC = main.cs epsilon.cs

# Default rule: Compile and run
all: compile run

# Compilation
compile:
	$(CSC) -out:$(OUTPUT) $(SRC)

# Run the compiled program and save output to out.txt
run:
	mono $(OUTPUT) | tee out.txt

# Clean compiled files
clean:
	rm -f $(OUTPUT) out.txt
