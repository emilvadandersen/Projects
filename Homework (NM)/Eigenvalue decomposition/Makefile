# Compilers and flags
RUN = mono
CSC = mcs

# Executable names
EXEC_JACOBI = jacobiEVD.exe
EXEC_HYDROGEN = hydrogenSolver.exe

# Data and plot files
DATA_FILES = dr_convergence.txt rmax_convergence.txt wavefunctions.txt
PLOTS = Convergence_dr.svg Wavefunctions.svg Convergence_rmax.svg

# Default target: Run both programs, process the output, and generate plots
all: $(PLOTS) Output.txt

# Compilation steps:

# Compile the Jacobi Eigenvalue program
$(EXEC_JACOBI): main.cs jacobi.cs matrix.cs vector.cs
	$(CSC) -target:exe -out:$@ $^

# Compile the Hydrogen Solver program
$(EXEC_HYDROGEN): main_hyd.cs hamiltonian.cs jacobi.cs matrix.cs vector.cs
	$(CSC) -target:exe -out:$@ $^

# Run the Jacobi Eigenvalue program and process output
Output.txt: $(EXEC_JACOBI)
	$(RUN) $(EXEC_JACOBI) > Output.txt
	sed -i '' 's/,/\./g' Output.txt

$(DATA_FILES): $(EXEC_HYDROGEN) 
	$(RUN) $(EXEC_HYDROGEN)  
	@sed -i '' 's/,/\./g' $(DATA_FILES) 

# Plotting dr convergence (Ground State Energy vs Δr)
Convergence_dr.svg: dr_convergence.txt Makefile
	echo "set terminal svg background 'white'; \
	set out 'Convergence_dr.svg'; \
	set title 'Convergence of Ground State Energy (ε0)'; \
	set xlabel 'Δr'; \
	set ylabel 'Ground State Energy (ε0)'; \
	set grid; \
	plot 'dr_convergence.txt' using 1:2 with linespoints pointtype 7 linecolor rgb 'blue' title 'Energy';" \
	| gnuplot

# Plotting rmax convergence (Ground State Energy vs rmax)
Convergence_rmax.svg: rmax_convergence.txt Makefile
	echo "set terminal svg background 'white'; \
	set out 'Convergence_rmax.svg'; \
	set title 'Convergence of Ground State Energy (ε0)'; \
	set xlabel 'rmax'; \
	set ylabel 'Ground State Energy (ε0)'; \
	set grid; \
	plot 'rmax_convergence.txt' using 1:2 with linespoints pointtype 7 linecolor rgb 'red' title 'Energy';" \
	| gnuplot


# Plotting wavefunctions
Wavefunctions.svg: wavefunctions.txt Makefile
	echo "set terminal svg background 'white'; \
	set out 'Wavefunctions.svg'; \
	set title 'Wavefunctions of Hydrogen Atom (f₀(r), f₁(r), f₂(r))'; \
	set xlabel 'Radial Distance (r)'; \
	set ylabel 'Wavefunction Value'; \
	set grid; \
	plot 'wavefunctions.txt' using 1:2 with lines lw 2 lc rgb 'blue' title 'f₀(r)', \
		'wavefunctions.txt' using 1:3 with lines lw 2 lc rgb 'red' title 'f₁(r)', \
		'wavefunctions.txt' using 1:4 with lines lw 2 lc rgb 'black' title 'f₂(r)';" \
	| gnuplot

# Clean up intermediate and output files
clean:
	rm -f $(EXEC_JACOBI) $(EXEC_HYDROGEN) $(DATA_FILES) $(PLOTS) log.*.gpi Output.txt
