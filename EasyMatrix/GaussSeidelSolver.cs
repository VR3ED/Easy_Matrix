using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMatrix
{
    /// <summary>
    /// Implements Gauss-Seidel. It immediately uses the new calculated values ​​of the solution vector elements during iteration
    /// </summary>
    public class GaussSeidelSolver : IterativeSolver
    {

        /// <summary>
        /// constructor method
        /// </summary>
        /// <param name="A">Matrix to solve</param>
        /// <param name="b">Solutions vectors</param>
        /// <param name="tol"></param>
        /// <param name="maxIter"></param>
        public GaussSeidelSolver(AccurateMatrix A, decimal[] b, decimal tol, int maxIter)
            : base(A, b, tol, maxIter) { }


        /// <summary>
        /// implements jacobi formula
        /// </summary>
        /// <param name="i">current iteration index</param>
        /// <param name="x">x vector</param>
        /// <returns></returns>
        public override decimal[] SolverLogic(int i, decimal[] x)
        {
            decimal sigma = 0;
            for (int j = 0; j < A.columns; j++)
            {
                if (j != i)
                    sigma += A.matrix[i, j] * x[j];
            }

            //formula calcolo gauss seidel
            x[i] = (b[i] - sigma) / A.matrix[i, i];

            return x;
        }


    }
}
