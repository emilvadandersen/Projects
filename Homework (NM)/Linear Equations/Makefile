# Compiler
CSC = mcs

# Output binary
OUT = main.exe

# Source files
SRC = main.cs QRGS.cs matrix.cs vector.cs

# Default target to run the program and redirect output to out.txt
all: run qr_decomp_timing_fit.svg

# Compile the program
$(OUT): $(SRC)
	$(CSC) -out:$(OUT) $(SRC)

# Run the program and redirect output to out.txt, then replace commas with dots
run: $(OUT)
	mono $(OUT) > out.txt
	@sed -i '' 's/,/\./g' out.txt
	@sed -i '' 's/,/\./g' out.times.data

qr_decomp_timing_fit.svg: out.times.data Makefile
	echo '\
	set term svg background "white";\
	set out "$@";\
	set title "QR Decomposition Timing (Fit with N³)";\
	set yrange [0:0.06];\
	set xlabel "Matrix Size (N)";\
	set ylabel "Time (seconds)";\
	f(N) = a * N**3;\
	fit f(x) "$<" using 1:2 via a;\
	plot \
	 "$<" using 1:2 with points pt 7 ps 1 lc rgb "blue" title "QR Decomposition Time",\
	 f(x) with lines lw 2 lc rgb "red" title "Fit: N³"\
	' | gnuplot

# Clean up generated files
clean:
	rm -f $(OUT) out.txt *.data *.svg *.log
