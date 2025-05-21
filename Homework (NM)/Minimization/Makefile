all: main.exe Fit.svg

# Build
main.exe: main.cs vector.cs matrix.cs qr.cs minimization.cs
	mcs -target:exe -out:main.exe main.cs vector.cs matrix.cs qr.cs minimization.cs

# Run and process data
fit.data.txt Out.txt: main.exe higgs.data.txt
	mono main.exe 
	sed -i '' 's/,/\./g' fit.data.txt
	sed -i '' -e 's/,/\./g' -e 's/;/,/g' Out.txt

Fit.svg: higgs.data.txt fit.data.txt
	echo '\
	set term svg background "white";\
	set output "Fit.svg";\
	set title "Higgs Boson Fit (Breit-Wigner)";\
	set xlabel "Energy [GeV]";\
	set ylabel "Signal";\
	set key top right;\
	plot \
	 "higgs.data.txt" using 1:2:3 with yerrorbars title "Data" pt 7 ps 1 lc rgb "black",\
	 "fit.data.txt" using 1:2 with lines title "Breit-Wigner Fit" lw 2 lc rgb "blue"\
	' | gnuplot

clean:
	rm -f main.exe Out.txt fit.data.txt Fit.svg
