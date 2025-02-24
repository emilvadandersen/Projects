# Compiler and execution settings
CSC = mcs
EXEC = main.exe
SRC = main.cs
NTERMS = 100000000

# Use GNU time (gtime) on macOS
TIME_CMD = gtime -p

# Default target: Compile and run tests
all: $(EXEC) run_tests

# Compile the C# program
$(EXEC): $(SRC)
	$(CSC) -out:$(EXEC) $(SRC)

# Run tests with different thread counts and measure execution time
run_tests: $(EXEC)
	@echo "Running tests..." > results.txt
	@echo "Running harmonic sum calculations..." > out.txt

	@echo "=========================================" >> results.txt
	@echo "Manual Multithreading Tests" >> results.txt
	@echo "=========================================" >> results.txt

	@echo "Manual Multithreading Results:" >> out.txt
	$(TIME_CMD) mono $(EXEC) -terms:$(NTERMS) -threads:1 2>> results.txt >> out.txt
	$(TIME_CMD) mono $(EXEC) -terms:$(NTERMS) -threads:2 2>> results.txt >> out.txt
	$(TIME_CMD) mono $(EXEC) -terms:$(NTERMS) -threads:4 2>> results.txt >> out.txt
	$(TIME_CMD) mono $(EXEC) -terms:$(NTERMS) -threads:8 2>> results.txt >> out.txt

	@echo "=========================================" >> results.txt
	@echo "Parallel.For Test (Incorrect)" >> results.txt
	@echo "=========================================" >> results.txt

	@echo "Parallel.For (Incorrect) Result:" >> out.txt
	$(TIME_CMD) mono $(EXEC) -terms:$(NTERMS) -parallel 2>> results.txt >> out.txt

	@echo "=========================================" >> results.txt
	@echo "Parallel.For with ThreadLocal (Fixed)" >> results.txt
	@echo "=========================================" >> results.txt

	@echo "Parallel.For with ThreadLocal Result:" >> out.txt
	$(TIME_CMD) mono $(EXEC) -terms:$(NTERMS) -threadlocal 2>> results.txt >> out.txt

	@echo "Tests complete. See results.txt for execution times and out.txt for harmonic sums."

# Clean compiled files
clean:
	rm -f $(EXEC) results.txt out.txt
