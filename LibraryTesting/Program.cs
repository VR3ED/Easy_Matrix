// See https://aka.ms/new-console-template for more information

using EasyMatrix;


decimal[] tol = [0.0001m, 0.000001m, 0.00000001m, 0.0000000001m, 0.00000000000001m];
int maxIter = 50000;

AccurateMatrix A = new AccurateMatrix(@"C:\Users\Cava\Documents\REPOS\C#\Library Metodi del Calcolo Scientifico\EasyMatrix\Matrixes\spa1.mtx");
//Console.WriteLine(A.IsSymmetricPositiveDefinite());
//Console.ReadKey();
//Console.Clear();

decimal[] b = new decimal[A.columns]; // inizializzazione vettore dei termini noti
b = Enumerable.Repeat(1.0m, A.columns).ToArray(); // inserimento di soli 1 all'interno del vettore di termini noti


for(int i=0; i<tol.Length-1; i++)
{
    decimal[] result = [];
    IterativeSolver solver;
    for(int j=0; j<4; j++)
    {
        switch (j)
        {
            case 0:
            {
                solver = new JacobiSolver(A, b, tol[i], maxIter);
                break;
            }
            case 1:
            {
                solver = new GaussSeidelSolver(A, b, tol[i], maxIter);
                break;
            }
            case 2:
            {
                solver = new GradientSolver(A, b, tol[i], maxIter);
                break;
            }
            case 3:
            {
                solver = new ConjugateGradientSolver(A, b, tol[i], maxIter);
                break;
            }
            default:
            {
                solver = new ConjugateGradientSolver(A, b, tol[4], maxIter);
                break;
            }
        }
        result = solver.Solve();

        Console.WriteLine("Solution:");
        foreach (var item in result)
        {
            Console.WriteLine(item);
        }
    }
} 

Console.ReadKey();

