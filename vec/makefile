# Compiler
CSC = csc

# Build commands
MKLIB = $(CSC) -target:library -out:vec.dll vec.cs
MKEXE = $(CSC) -reference:vec.dll -out:main.exe main.cs

# Output files
LIBRARY = vec.dll
EXECUTABLE = main.exe
OUTPUT = Out.txt

# Default target
all: $(EXECUTABLE)

# Run program and save output
$(OUTPUT): $(EXECUTABLE)
	mono $(EXECUTABLE) > $@

# Compile main.cs and link with vec.dll
$(EXECUTABLE): main.cs $(LIBRARY)
	$(MKEXE)

# Compile vec.cs into vec.dll
$(LIBRARY): vec.cs
	$(MKLIB)

# Run the compiled program
run: $(EXECUTABLE)
	mono $(EXECUTABLE)
