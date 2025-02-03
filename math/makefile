# Rule to generate Out.txt by running main.exe with Mono
Out.txt : main.exe
	mono $< > $@  # Run main.exe and redirect output to Out.txt
# The use of `$<` and `$@` ensures proper dependency management, with `$<` representing the first prerequisite (`main.exe`) and `$@` representing the target (`Out.txt`).

# Rule to compile main.cs into an executable (main.exe), linking it with sfuns.dll
main.exe : main.cs sfuns.dll
	mcs -target:exe -reference:sfuns.dll -out:main.exe main.cs

# Rule to compile sfuns.cs into a shared library (sfuns.dll)
sfuns.dll : sfuns.cs
	mcs -target:library -out:sfuns.dll sfuns.cs

# Clean rule to remove compiled files
clean:
	rm -f main.exe sfuns.dll Out.txt
