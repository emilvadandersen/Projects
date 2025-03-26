# Default target
all: Error.svg Integral.svg

# Compile the matrix library (vector.cs)
matlib.dll: vector.cs
	mcs -target:library -out:$@ $<

# Compile PlainMC.dll (PlainMC.cs) with reference to matlib.dll
plainMC.dll: plainMC.cs matlib.dll
	mcs -target:library -out:$@ -reference:matlib.dll $<

# Compile MultiMC.dll (MultiMC.cs) with reference to matlib.dll
multiMC.dll: multiMC.cs matlib.dll
	mcs -target:library -out:$@ -reference:matlib.dll $<

# Compile the main program for pseudo-random Monte Carlo (Main_PseudoRandom.cs)
main_PseudoRandom.exe: main_PseudoRandom.cs matlib.dll plainMC.dll 
	mcs -target:exe -reference:matlib.dll -reference:plainMC.dll $<

# Generate the output file for pseudo-random Monte Carlo simulation
Out_pseudo_random.txt: main_PseudoRandom.exe
	>$@
	for N in $$(seq 200 200 10000); do \
		echo $$N; \
		mono $< $$N >> $@; \
	done
	@sed -i '' 's/,/\./g' Out_pseudo_random.txt Out.txt

# Compile the main program for Halton Monte Carlo (Main_Halton.cs)
main_Halton.exe: main_Halton.cs matlib.dll multiMC.dll
	mcs -target:exe -reference:matlib.dll -reference:multiMC.dll $<

# Generate the output file for Halton Monte Carlo simulation
Out_halton.txt: main_Halton.exe
	>$@
	for N in $$(seq 200 200 10000); do \
		echo $$N; \
		mono $< $$N >> $@; \
	done
	@sed -i '' 's/,/\./g' Out_halton.txt

# Plot the error comparison between pseudo-random and Halton Monte Carlo
Error.svg: Out_pseudo_random.txt Out_halton.txt Makefile
	echo '\
	set term svg background "white";\
	set out "$@";\
	set title "Error Comparison: Pseudo-random vs. Halton";\
	set xlabel "Number of sampling points (N)";\
	set ylabel "Error";\
	a=1; f(x) = a/sqrt(x);\
	fit f(x) "$<" using 1:4 via a;\
	plot \
	 "Out_pseudo_random.txt" using 1:3 with lines lw 2 lc rgb "blue" title "Pseudo-random statistical error"\
	,"Out_pseudo_random.txt" using 1:4 with linespoints lw 2 pt 7 ps 1 lc rgb "red" title "Pseudo-random actual error"\
	,"Out_halton.txt" using 1:3 with lines lw 2 lc rgb "#228B22" title "Halton (Quasi-random) statistical error"\
	,"Out_halton.txt" using 1:4 with linespoints lw 2 pt 7 ps 1 lc rgb "orange" title "Halton (Quasi-random) actual error"\
	,f(x) with lines lw 2 lc rgb "black" title "1/sqrt(N) fit to actual error"\
	' | gnuplot

# Plot the integration results of the unit circle area for both methods
Integral.svg: Out_pseudo_random.txt Out_halton.txt Makefile
	echo '\
	set term svg background "white";\
	set out "$@";\
	set title "Monte Carlo Integration of Unit Circle Area";\
	set xlabel "Number of sampling points (N)";\
	set ylabel "Estimated integral value";\
	plot \
	 "Out_pseudo_random.txt" using 1:2 with linespoints lw 2 pt 7 ps 1 lc rgb "red" title "Pseudo-random MC"\
	,"Out_halton.txt" using 1:2 with linespoints lw 2 pt 7 ps 1 lc rgb "blue" title "Halton (Quasi-random) MC"\
	,pi with lines lw 2 lc rgb "black" title "Exact value (π)"\
	' | gnuplot

# Clean up the generated files
clean:
	rm -f *.dll *.exe *.txt *.svg *.log
