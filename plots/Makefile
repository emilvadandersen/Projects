# Main target: Generates plot.gamma.svg, plot.erf.svg, and plot.ln_gamma.svg
all: plot.gamma.svg plot.erf.svg plot.ln_gamma.svg

# Rule to generate Gamma function plot from data.txt
plot.gamma.svg : data.txt
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key bottom ;\
		set xlabel "x" ;\
		set ylabel "Î“(x)" ;\
		plot [-4:][-20:20] \
			"$<" index 1 with lines title "gamma(x)" \
			,"$<" index 3 with points title "factorials" \
	' | tee log.gamma.gpi | gnuplot

# Rule to generate Error function plot from data.txt and erf.data
plot.erf.svg : data.txt erf.data
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key left ;\
		plot  \
			"$<" index 0 with lines title "erf(x)" \
			,"erf.data" with points pointtype 4 title "tabulated erf(x)" \
	' | tee log.erf.gpi | gnuplot

# Rule to generate ln(Gamma) function plot from data.txt
plot.ln_gamma.svg : data.txt
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key bottom ;\
		set xlabel "x" ;\
		set ylabel "ln(Î“(x))" ;\
		plot [-4:][-20:20] \
			"$<" index 2 with lines title "ln(gamma(x))" \
	' | tee log.ln_gamma.gpi | gnuplot

# Rule to generate data.txt by running the compiled C# program and replacing commas with dots
data.txt: main.exe
	@echo "Generating data.txt and replacing commas with dots"
	mono $< > data.txt.tmp
	@sed -i '' 's/,/\./g' data.txt.tmp && mv data.txt.tmp data.txt

# Rule to compile the C# source into a DLL
sfuns.dll : sfuns.cs
	mcs -target:library -out:$@ sfuns.cs

# Rule to compile the main C# program
main.exe : sfuns.dll main.cs
	mcs -target:exe -out:$@ -reference:sfuns.dll main.cs

# Clean up generated files
clean:
	$(RM) *.exe *.dll [Oo]ut* [Ll]og* *.svg data.txt
