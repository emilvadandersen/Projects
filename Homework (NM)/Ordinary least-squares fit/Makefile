# The target to generate the output files
all: plot.decay.svg out.txt

# Build output text file
out.txt: main.exe
	mono main.exe > out.txt
	@sed -i '' 's/,/\./g' out.txt  # Replace commas with periods in out.txt

# Compile library
library.dll: matrix.cs vector.cs qr.cs lsfit.cs
	mcs -target:library -out:library.dll matrix.cs vector.cs qr.cs lsfit.cs

# Compile main executable
main.exe: main.cs library.dll
	mcs -target:exe -out:main.exe -reference:library.dll main.cs

# Plotting radioactive decay
plot.decay.svg: data.txt Makefile
	echo 'set terminal svg background "white"; \
	set output "$@"; \
	set xlabel "Time (days)"; \
	set ylabel "ln(Activity)"; \
	set grid; \
	set title "Radioactive Decay Fit for 224Ra"; \
	plot "data.txt" using 1:2:3 with yerrorbars title "Experimental Data" lc rgb "red" pt 7 ps 0.8 lw 1.5, \
	exp(4.963 - 0.172*x) with lines lw 2 lc rgb "blue" dashtype "--" title "Best Fit";' \
	| tee log.decay.gpi | gnuplot

# Clean up generated files
clean:
	rm -f *.dll *.exe *.svg out.txt log*
