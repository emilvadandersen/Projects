# Compilers and flags
RUN=mono  
CSC=mcs 

# Files used in the project
OUT=main.exe  
DATA_FILES=linInter.txt sinTable.txt quaInter.txt cosTable.txt linInterInteg.txt quaInterInteg.txt  
PLOTS=Lspline.svg Qspline.svg 

# The target to generate the output files
all: $(PLOTS)  

# Compilation steps:
$(OUT): main.cs linterp.dll qspline.dll  
	$(CSC) -target:exe -out:$@ -reference:linterp.dll -reference:qspline.dll $<  

linterp.dll: linterp.cs  
	$(CSC) -target:library -out:$@ $<

qspline.dll: qspline.cs  
	$(CSC) -target:library -out:$@ $<

# Generate the .txt data files by running the compiled executable (main.exe)
$(DATA_FILES): $(OUT) 
	$(RUN) $(OUT)  
	@sed -i '' 's/,/\./g' $(DATA_FILES)  

# Plotting using gnuplot for Linear Spline (Lspline.svg)
Lspline.svg: linInter.txt linInterInteg.txt cosTable.txt Makefile
	echo "set terminal svg background 'white'; \
	set out '$@'; \
	set title 'Linear Spline and its Integral'; \
	set xlabel 'x'; \
	set ylabel 'y'; \
	set key out; \
	set grid; \
	plot 'linInter.txt' using 1:2 with lines lw 2 lc rgb 'blue' title 'Linear Spline', \
	'linInterInteg.txt' using 1:2 with lines lw 2 lc rgb 'black' title 'Integral', \
	'cosTable.txt' using 1:2 with points pt 7 ps 1 lc rgb 'red' title 'Cos Data Points';" \
	| tee log.lin.gpi | gnuplot

# Plotting using gnuplot for Qspline.svg (Quadratic Spline plot)
Qspline.svg: quaInter.txt quaInterInteg.txt sinTable.txt Makefile
	echo 'set terminal svg background "white"; \
	set out "$@"; \
	set title "Quadratic Spline, Derivative, and Integral"; \
	set xlabel "x"; \
	set ylabel "y"; \
	set key out; \
	set grid; \
	plot "quaInter.txt" using 1:2 with lines lw 2 lc rgb "blue" title "Quadratic Spline", \
	"quaInter.txt" using 1:3 with lines lw 2 lc rgb "#FFA500" title "Derivative", \
	"quaInterInteg.txt" using 1:2 with lines lw 2 lc rgb "black" title "Integral", \
	"sinTable.txt" using 1:2 with points pt 7 ps 1 lc rgb "red" title "Sin Data Points";' \
	| tee log.qua.gpi | gnuplot

# Clean up intermediate and output files
clean:
	rm -f *.dll *.exe *.txt *.svg log.*.gpi