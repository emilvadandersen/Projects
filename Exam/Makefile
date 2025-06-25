all: main.exe Fit.svg

main.exe: main.cs vector.cs matrix.cs qr.cs minimization.cs
	mcs -target:exe -out:main.exe main.cs vector.cs matrix.cs qr.cs minimization.cs

oscillatory_decay.data.txt fitNewton.data.txt fitGaussNewton.data.txt Out.txt: main.exe
	mono main.exe
	sed -i '' 's/,/\./g' fitNewton.data.txt
	sed -i '' 's/,/\./g' fitGaussNewton.data.txt
	sed -i '' -e 's/,/\./g' -e 's/;/,/g' Out.txt

Fit.svg: oscillatory_decay.data.txt fitNewton.data.txt fitGaussNewton.data.txt
	echo '\
	set term svg size 800,600 enhanced background "white";\
	set output "Fit.svg";\
	set title "Oscillatory Decay Fit: Newton vs Gauss-Newton";\
	set xlabel "Time t";\
	set ylabel "Signal y";\
	set yrange [-100:120];\
	set key top right;\
	plot \
	 "oscillatory_decay.data.txt" using 1:2:3 with yerrorbars title "Data" pt 0 lc rgb "black",\
	 "fitNewton.data.txt" using 1:2 with lines lw 2 lc rgb "blue" title "Newton Fit",\
	 "fitGaussNewton.data.txt" using 1:2 with lines lw 2 lc rgb "red" title "Gauss-Newton Fit",\
	 "fitGaussNewton.data.txt" using 1:3 with lines lw 2 lc rgb "#FFA500" title "Gauss-Newton Fit (phase - π/2)"\
	' | gnuplot

clean:
	rm -f main.exe *.svg fit*.txt Out.txt
