using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMatrix
{
    /// <summary>
    /// Implement Jacobi's method. Uses an iteration vector and a prior iteration vector to compute the approximate solution iteratively.
    /// </summary>
    public class JacobiSolver : IterativeSolver
    {

        /// <summary>
        /// static variable that memorizes xOld for each iteration
        /// </summary>
        private static decimal[] xOld;

        /// <summary>
        /// constructor method, setups xOld
        /// </summary>
        /// <param name="A">Matrix to solve</param>
        /// <param name="b">Solutions vectors</param>
        /// <param name="tol"></param>
        /// <param name="maxIter"></param>
        public JacobiSolver(AccurateMatrix A, decimal[] b, decimal tol, int maxIter): base(A, b, tol, maxIter) 
        {
            xOld = new decimal[A.columns];
        }

        /// <summary>
        /// implements jacobi formula
        /// </summary>
        /// <param name="i">current iteration index</param>
        /// <param name="x">x vector</param>
        /// <returns></returns>
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
                    //sommatoria di tutte le A[i,j] * x_j^k
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


        /// <summary>
        /// check if current solver arrived at an acceptable solution
        /// </summary>
        /// <param name="x">current solutions vector</param>
        /// <returns></returns>
        public override bool SolverExitCondition(decimal[] x)
        {
            return NormAxMinusB(x) < tol;
        }
    }
}
