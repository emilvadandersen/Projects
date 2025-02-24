class main{

public static int Main(){
	for(double x=-3;x<=3;x+=1.0/8){
		System.Console.WriteLine($"{x} {sfuns.erf(x)}");
		}
	System.Console.WriteLine();
	System.Console.WriteLine();
	double dx=1.0/64;
	for(double x=-5+dx/2;x<=5;x+=dx){
		System.Console.WriteLine($"{x} {sfuns.sgamma(x)}");
		}
	System.Console.WriteLine();
	System.Console.WriteLine();
	for(double x=dx;x<=10;x+=dx){
		System.Console.WriteLine($"{x} {sfuns.lngamma(x)}");
		}
	System.Console.WriteLine();
	System.Console.WriteLine();
	double f=1;
	for(int i=1;i<=10;i++){
		f*=i;
		System.Console.WriteLine($"{i+1} {f}");
	}
return 0;
}

}