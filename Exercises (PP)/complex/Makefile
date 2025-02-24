# Define filenames
TARGET = program.exe
SRC = program.cs
OUTPUT_FILE = output.txt

# Default target: build and run
all: build run

# Build target: compile the C# program
build:
	@echo "Building the project..."
	mcs -reference:System.Numerics.dll -out:$(TARGET) $(SRC)

# Run target: execute the program and save output to output.txt
run:
	@echo "Running the program and saving output to $(OUTPUT_FILE)..."
	mono $(TARGET) > $(OUTPUT_FILE)
	@echo "Replacing commas with dots in $(OUTPUT_FILE)..."
	sed 's/,/\./g' $(OUTPUT_FILE) > temp.txt && mv temp.txt $(OUTPUT_FILE)

# Clean target: remove output file and temporary files
clean:
	@echo "Cleaning up..."
	rm -f $(OUTPUT_FILE) temp.txt
