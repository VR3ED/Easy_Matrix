using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMatrix
{
    public class GaussSeidelSolver : IterativeSolver
    {
        public GaussSeidelSolver(AccurateMatrix A, decimal[] b, decimal tol, int maxIter)
            : base(A, b, tol, maxIter) { }

        //public override decimal[] Solve()
        //{
        //    int n = A.rows;
        //    decimal[] x = new decimal[n];

        //    for (int k = 0; k < maxIter; k++)
        //    {
        //        for (int i = 0; i < n; i++)
        //        {
        //            decimal sigma = 0;
        //            for (int j = 0; j < n; j++)
        //            {
        //                if (j != i)
        //                    sigma += A.matrix[i, j] * x[j];
        //            }
        //            x[i] = (b[i] - sigma) / A.matrix[i, i];
        //        }

        //        if (NormAxMinusB(x) < tol)
        //            return x;
        //    }
        //    throw new Exception("Gauss-Seidel method did not converge.");
        //}

        public override decimal[] SolverLogic(int i, decimal[] x)
        {
            throw new NotImplementedException();
        }


    }
}
