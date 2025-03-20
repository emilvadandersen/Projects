# Compilers and flags
RUN=mono  
CSC=mcs 

# Files used in the project
OUT=main.exe  
OUT_ERF=main-erf.exe  
DATA_ERF=out.erf.data.txt  
PLOTS=Plot.erf.svg  

# The target to generate the output files
all: Out.txt $(PLOTS)  # Ensure Out.txt is generated first

# Compilation steps:
matlib.dll: integrate.cs 
	$(CSC) -target:library -out:$@ $^

$(OUT): main.cs matlib.dll  
	$(CSC) -target:exe -out:$@ -reference:matlib.dll $<  

$(OUT_ERF): main-erf.cs matlib.dll  
	$(CSC) -target:exe -out:$@ -reference:matlib.dll $<  

# Generate output text
Out.txt: $(OUT)  # Ensure $(OUT) is built first
	$(RUN) $< > $@  
	@sed -i '' 's/,/\./g' $@  

# Generate data files
$(DATA_ERF): $(OUT_ERF)  
	$(RUN) $< 1> $@ 2> log-erf  
	@sed -i '' 's/,/\./g' $@  

# Plotting using gnuplot
$(PLOTS): $(DATA_ERF) Makefile
	echo 'set terminal svg background "white"; \
	set out "$@"; \
	set title "erf(x) as a definite integral"; \
	set xlabel "x"; \
	set ylabel "erf(x)"; \
	set key out; \
	set grid; \
	plot "out.erf.data.txt" index 0 using 1:2 with lines lw 2 lc rgb "blue" title "erf(x)", \
	"out.erf.data.txt" index 1 using 1:2 with points pt 7 ps 1 lc rgb "red" title "table";' \
	| tee log.plot.gpi | gnuplot

# Clean up files
clean:  
	rm -f *.dll *.exe *.txt *.svg log*
