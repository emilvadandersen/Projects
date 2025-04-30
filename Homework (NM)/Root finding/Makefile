# Files
EXEC = main.exe
SRC = main.cs vector.cs matrix.cs qr.cs newton.cs jacobian.cs ode.cs hydrogen.cs
OUT_TXT = Output.txt
WAVE_TXT = wavefunction.dat
PLOT = wavefunction.svg

# Default rule
all: $(PLOT) $(OUT_TXT)

# Compile
$(EXEC): $(SRC)
	mcs -target:exe -out:$@ $(SRC)

# Run the program and fix commas
$(OUT_TXT) $(WAVE_TXT): $(EXEC)
	mono $(EXEC)
	@sed -i '' 's/,/\./g' $(OUT_TXT)
	@sed -i '' 's/,/\./g' $(WAVE_TXT)

# Plot with gnuplot
wavefunction.svg: wavefunction.dat Makefile
	echo '\
	set term svg background "white";\
	set out "$@";\
	set title "Hydrogen Ground State Wavefunction";\
	set xlabel "r";\
	set ylabel "f(r)";\
	plot \
	 "wavefunction.dat" using 1:2 with lines lw 3 lc rgb "blue" title "Numerical",\
	 "wavefunction.dat" using 1:3 with lines lw 2 lc rgb "red" title "Exact (re^{-r})"\
	' | gnuplot

# Clean up
clean:
	rm -f $(EXEC) $(OUT_TXT) $(WAVE_TXT) $(PLOT)
