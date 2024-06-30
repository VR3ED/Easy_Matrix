// See https://aka.ms/new-console-template for more information

using EasyMatrix;
using Library_Metodi_del_Calcolo_Scientifico;


WrapMatrix wrapMatrix = new WrapMatrix(@"C:\Users\Cava\Documents\REPOS\C#\Library Metodi del Calcolo Scientifico\EasyMatrix\Matrixes\spa1.mtx");
Console.WriteLine(wrapMatrix);
Console.ReadKey();
Console.Clear();

//var x = wrapMatrix.matrix.Cholesky();
//Console.WriteLine(x);
//Console.ReadKey();
//Console.Clear();


decimal[] tol = [0.0001m, 0.000001m, 0.00000001m, 0.0000000001m];
int maxIter = 50000;

AccurateMatrix A = new AccurateMatrix(@"C:\Users\Cava\Documents\REPOS\C#\Library Metodi del Calcolo Scientifico\EasyMatrix\Matrixes\spa1.mtx");
Console.WriteLine(A.IsSymmetricPositiveDefinite());
Console.ReadKey();
Console.Clear();

decimal[] b = new decimal[A.columns]; // inizializzazione vettore dei termini noti
b = Enumerable.Repeat(1.0m, A.columns).ToArray(); // inserimento di soli 1 all'interno del vettore di termini noti


decimal[] result = [];
IterativeSolver solver = new JacobiSolver(A, b, tol[0], maxIter);
result = solver.Solve();

Console.WriteLine("Solution:");
foreach (var item in result)
{
    Console.WriteLine(item);
}

Console.ReadKey();

