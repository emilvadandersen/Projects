# Compiler
CSC = mcs

# Output binary
OUT = main.exe

# Source files
SRC = main.cs QRGS.cs matrix.cs vector.cs

# Default target to run the program and redirect output to out.txt
all: run

# Compile the program
$(OUT): $(SRC)
	$(CSC) -out:$(OUT) $(SRC)

# Run the program and redirect output to out.txt, then replace commas with dots
run: $(OUT)
	mono $(OUT) > out.txt
	@sed -i '' 's/,/\./g' out.txt
