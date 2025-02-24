# Compiler
CSC = csc

# Build commands
MKLIB = $(CSC) -target:library -out:vec.dll vec.cs
MKEXE = $(CSC) -reference:vec.dll -out:main.exe main.cs

# Output files
LIBRARY = vec.dll
EXECUTABLE = main.exe
OUTPUT = out.txt

# Default target: Compile, run, and save output
all: $(EXECUTABLE)
	mono $(EXECUTABLE) | tee $(OUTPUT)

# Compile main.cs and link with vec.dll
$(EXECUTABLE): main.cs $(LIBRARY)
	$(MKEXE)

# Compile vec.cs into vec.dll
$(LIBRARY): vec.cs
	$(MKLIB)

# Clean compiled files
clean:
	rm -f $(EXECUTABLE) $(LIBRARY) $(OUTPUT)
