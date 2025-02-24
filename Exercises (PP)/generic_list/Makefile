# Define the name of the output executable
TARGET = program.exe

# Define the C# source file
SRC = program.cs

# Define the input and output files
INPUT_FILE = input.txt
OUTPUT_FILE = output.txt

# The default target: build and run the program
all: build run

# Target to build the program
build:
	@echo "Building the project..."
	mcs -out:$(TARGET) $(SRC)

# Target to run the program and process the output file
run:
	@echo "Running the program and saving output to $(OUTPUT_FILE)..."
	mono $(TARGET) < $(INPUT_FILE) > $(OUTPUT_FILE)
	@echo "Replacing commas with dots in $(OUTPUT_FILE)..."
	sed 's/,/\./g' $(OUTPUT_FILE) > temp.txt && mv temp.txt $(OUTPUT_FILE)

# Clean build artifacts and temporary files
clean:
	@echo "Cleaning up unnecessary files..."
	rm -f temp.txt $(OUTPUT_FILE)
