all: Out.txt Ann_approx.svg

main.exe: main.cs ann.cs vector.cs matrix.cs qr.cs minimization.cs
	mcs -target:exe -out:main.exe main.cs ann.cs vector.cs matrix.cs qr.cs minimization.cs

Out.txt: main.exe
	mono main.exe
	sed -i '' -e 's/,/\./g' -e 's/;/,/g' Out.txt
	sed -i '' 's/,/\./g' ann_fit.data.txt

Ann_approx.svg: ann_fit.data.txt
	echo '\
	set term svg background "white";\
	set output "Ann_approx.svg";\
	set title "Artificial Neural Network Approximation of f(x) = cos(5x - 1) * exp(-x^2)";\
	set xlabel "x";\
	set ylabel "f(x)";\
	set key top right;\
	plot \
	 "ann_fit.data.txt" using 1:2 with lines title "Actual f(x)" lw 2 lc rgb "red",\
	 "ann_fit.data.txt" using 1:3 with lines title "Approximation" lw 2 lc rgb "blue"\
	' | gnuplot


clean:
	rm -f *.exe *.dll *.txt *.svg 