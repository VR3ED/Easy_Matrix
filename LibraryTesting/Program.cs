// See https://aka.ms/new-console-template for more information

using EasyMatrix;

//WrapMatrix wrapMatrix = new WrapMatrix(@"C:\Users\Cava\Documents\REPOS\C#\Library Metodi del Calcolo Scientifico\EasyMatrix\Matrixes\spa1.mtx");
//Console.WriteLine(wrapMatrix);
//Console.ReadKey();
//Console.Clear();

//var x = wrapMatrix.matrix.Cholesky();
//Console.WriteLine(x);
//Console.ReadKey();
//Console.Clear();


decimal[] tol = [0.0001m, 0.000001m, 0.00000001m, 0.0000000001m, 1*10^(-14)];
int maxIter = 200000000;

AccurateMatrix A = new AccurateMatrix(@"C:\Users\Cava\Documents\REPOS\C#\Library Metodi del Calcolo Scientifico\EasyMatrix\Matrixes\spa1.mtx");
Console.WriteLine(A.IsSymmetricPositiveDefinite());
Console.ReadKey();
Console.Clear();

decimal[] b = new decimal[A.columns]; // inizializzazione vettore dei termini noti
b = Enumerable.Repeat(1.0m, A.columns).ToArray(); // inserimento di soli 1 all'interno del vettore di termini noti


decimal[] result = [];
IterativeSolver solver = new GaussSeidelSolver(A, b, tol[4], maxIter);
result = solver.Solve();

Console.WriteLine("Solution:");
foreach (var item in result)
{
    Console.WriteLine(item);
}

Console.ReadKey();

