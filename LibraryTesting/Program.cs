// See https://aka.ms/new-console-template for more information

using EasyMatrix;
using System.Runtime.InteropServices;

decimal[] tol = [0.0001m, 0.000001m, 0.00000001m, 0.0000000001m, 0.00000000000001m];
int maxIter = 50000;

string file_path = "";

bool test_run = true;

if (test_run)
{
    file_path = "/home/Cava/Documents/Repos/C#/Easy_Matrix/EasyMatrix/Matrixes/vem1.mtx";
    
    AccurateMatrix A = new AccurateMatrix(file_path);

    decimal[] b = new decimal[A.columns]; // inizializzazione vettore dei termini noti
    b = Enumerable.Repeat(1.0m, A.columns).ToArray(); // inserimento di soli 1 all'interno del vettore di termini noti
    
    IterativeSolver solver = new JacobiSolver(A, b, tol[0], maxIter);
    
    solver.Solve();
    
    Console.WriteLine("fatto");
    Console.ReadKey();
}
else
{
    for (int matrixIndex = 0; matrixIndex < 4; matrixIndex++)
    {
        
        string matrixName = "";
        
        //define wich matrix to use
        switch (matrixIndex)
        {
            case 0:
            {
                matrixName = "spa1.mtx";
                break;
            }
            case 1:
            {
                matrixName = "spa2.mtx";
                break;
            }
            case 2:
            {
                matrixName = "vem1.mtx";
                break;
            }
            case 3:
            {
                matrixName = "vem2.mtx";
                break;
            }
            default:
            {
                matrixName = "spa3.mtx";
                break;
            }
        }
        
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            file_path = "/home/Cava/Documents/Repos/C#/Easy_Matrix/EasyMatrix/Matrixes/"+matrixName;
        }
        else
        {
            file_path = @"C:\Users\Cava\Documents\REPOS\C#\Library Metodi del Calcolo Scientifico\EasyMatrix\Matrixes\"+matrixName;
        }

        AccurateMatrix A = new AccurateMatrix(file_path);

        decimal[] b = new decimal[A.columns]; // inizializzazione vettore dei termini noti
        b = Enumerable.Repeat(1.0m, A.columns).ToArray(); // inserimento di soli 1 all'interno del vettore di termini noti


        //foreach tollerance required
        for(int i=0; i<tol.Length-1; i++)
        {
            //foreach solver implemented
            for(int j=0; j<4; j++)
            {
                decimal[] result = [];
                IterativeSolver solver;
                //define wich solver to use
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
            
                Console.WriteLine("Finished "+ A.matrix_name +"(simmetrica e definita positiva: "+ A.IsSymmetricPositiveDefinite() + ")" + " " + solver.GetType().Name);
                
                Console.WriteLine();

                //foreach (var item in result) { Console.WriteLine(item); } Console.ReadKey();
            }
        }
    }
}



