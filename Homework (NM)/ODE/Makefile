# Compilers and flags
RUN=mono  
CSC=mcs 

# Files used in the project
OUT=main.exe  
DATA_FILES=theta.txt omega.txt circular.txt elliptical.txt relativistic.txt  
PLOTS=pendulum.svg planetary.svg  

# The target to generate the output files
all: $(PLOTS)  

# Compilation steps:
$(OUT): main.cs ODE.dll  
	$(CSC) -target:exe -out:$@ -reference:ODE.dll $<  

ODE.dll: ODE.cs vector.cs  
	$(CSC) -target:library -out:$@ $^ 

# Generate data files
$(DATA_FILES): $(OUT) 
	$(RUN) $(OUT)  
	@sed -i '' 's/,/\./g' $(DATA_FILES)  

# Plotting pendulum motion
pendulum.svg: theta.txt omega.txt Makefile
	echo 'set terminal svg background "white"; \
	set out "$@"; \
	set title "Damped Pendulum Motion"; \
	set xlabel "Time (t)"; \
	set ylabel "Angle and Velocity"; \
	set grid; \
	plot "theta.txt" using 1:2 with lines lw 2 lc rgb "blue" title "Theta (t)", \
	"omega.txt" using 1:2 with lines lw 2 lc rgb "red" title "Omega (t)";' \
	| tee log.pendulum.gpi | gnuplot

# Plotting planetary orbits
planetary.svg: circular.txt elliptical.txt relativistic.txt Makefile
	echo 'set terminal svg background "white"; \
	set output "planetary.svg"; \
	set title "Planetary Orbits"; \
	set xlabel "Angle (phi)"; \
	set ylabel "Radial Distance (r)"; \
	set grid; \
	set yrange [0.4:1.8]; \
	plot "circular.txt" using 1:2 with lines lw 2 lc rgb "red" title "Circular Orbit", \
	"elliptical.txt" using 1:2 with lines lw 2 lc rgb "blue" title "Elliptical Orbit", \
	"relativistic.txt" using 1:2 with lines lw 2 lc rgb "#FFA500" title "Relativistic Precession";' \
	| tee log.planetary.gpi | gnuplot

# Clean up files
clean:
	rm -f *.dll *.exe *.txt *.svg log* *.gpi
