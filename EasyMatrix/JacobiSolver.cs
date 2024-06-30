using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMatrix
{
    /// <summary>
    /// Implement Jacobi's method. Uses an iteration vector and a prior vector to compute the approximate solution iteratively.
    /// </summary>
    public class JacobiSolver : IterativeSolver
    {

        private static decimal[] xOld;

        public JacobiSolver(AccurateMatrix A, decimal[] b, decimal tol, int maxIter): base(A, b, tol, maxIter) 
        {
            xOld = new decimal[A.columns];
        }


        //public override decimal[] Solve()
        //{
        //    int n = A.rows;
        //    decimal[] x = new decimal[n]; //vettore delle soluzioni --> inizializzato a zero
        //    decimal[] xOld = new decimal[n];

        //    for (int k = 0; k < maxIter; k++)
        //    {
        //        for (int i = 0; i < n; i++)
        //        {
        //            decimal sigma = 0;
        //            for (int j = 0; j < n; j++)
        //            {
        //                if (j != i)
        //                    sigma += A.matrix[i, j] * xOld[j];
        //            }

        //            //formula per calcolo di jacobi
        //            x[i] = (b[i] - sigma) / A.matrix[i, i];
        //        }

        //        if (NormAxMinusB(x) < tol)
        //            return x;

        //        Array.Copy(x, xOld, n);
        //    }
        //    throw new Exception("Jacobi method did not converge.");
        //}
        

        public override decimal[] SolverLogic(int i, decimal[] x)
        {
            #region init vector x and xOld
            if (x == null)
            {
                x = new decimal[base.A.rows];
            }

            if (xOld == null)
            {
                xOld = new decimal[base.A.rows];
            }
            #endregion

            decimal sigma = 0;
            for (int j = 0; j < base.A.rows; j++)
            {
                if (j != i)
                    sigma += A.matrix[i, j] * xOld[j];
            }

            //formula per calcolo di jacobi
            x[i] = (b[i] - sigma) / A.matrix[i, i];

            //se sono arrivato alla fine mi salvo il nuvo xOld
            if (i == A.rows-1)
            {
                Array.Copy(x, xOld, A.rows);
            }

            return x;
        }
    }
}
