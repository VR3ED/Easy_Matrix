using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMatrix
{
    /// <summary>
    /// Implements solver with gradient method
    /// </summary>
    public class GradientSolver : IterativeSolver
    {

        /// <summary>
        /// static variable that memorizes r for each iteration
        /// </summary>
        private static decimal[] r;

        /// <summary>
        /// static variable that memorizes Ap for each iteration
        /// </summary>
        private static decimal[] Ar;

        /// <summary>
        /// static variable that memorizes alpha for each iteration
        /// </summary>
        private static decimal alpha;

        /// <summary>
        /// static variable that memorizes rnew for each iteration
        /// </summary>
        private static decimal[] rNew;


        /// <summary>
        /// basic contructor
        /// </summary>
        /// <param name="A">Decimals matrix</param>
        /// <param name="b">vecror of known terms</param>
        /// <param name="tol">tollerance index. Up to 28-29 digits precision</param>
        /// <param name="maxIter">maximun number of iterations required</param>
        public GradientSolver(AccurateMatrix A, decimal[] b, decimal tol, int maxIter): base(A, b, tol, maxIter) 
        {
            r = (decimal[])b.Clone();
            Ar = new decimal[A.rows];
            alpha = 0;
            rNew = new decimal[A.rows];
        }

        /// <summary>
        /// implements Gradient formula
        /// </summary>
        /// <param name="i">current iteration index</param>
        /// <param name="x">x vector</param>
        /// <returns></returns>
        public override decimal[] SolverLogic(int i, decimal[] x)
        {
            //alpha initialization
            if (i == 0)
            {
                var Ax = MatrixVectorMultiply(x);
                r = VectorsSubtraction(b,Ax);

                // Calcolare Ar = A * r
                Ar = MatrixVectorMultiply(r);

                // Calcolare alpha = (r^T * r) / (r^T * A * r)
                decimal rDotr = Dot(r, r);
                decimal rDotAr = Dot(r, Ar);

                // Evitare la divisione per zero
                if (rDotAr == 0)
                {
                    //throw new Exception("Il metodo del gradiente ha incontrato una divisione per zero.");
                    alpha = 0;
                }

                alpha = rDotr / rDotAr;
            }

            x[i] += alpha * r[i];

            //if (i == A.rows - 1)
            //{
            //    // Aggiornare r = r - alpha * Ap
            //    for (int j = 0; j < A.rows; j++)
            //    {
            //        r[j] -= alpha * Ar[j];
            //    }
            //}

            return x;
        }

        /// <summary>
        /// check if current solver arrived at an acceptable solution
        /// </summary>
        /// <param name="x">current solutions vector</param>
        /// <returns></returns>
        public override bool SolverExitCondition(decimal[] x)
        {
            return Norm(r) / Norm(b) < tol;
        }



        #region ELEMENTAL OPERATIONS

        private decimal[] MatrixVectorMultiply(decimal[] vector)
        {
            decimal[] result = new decimal[A.rows];
            for (int i = 0; i < A.rows; i++)
            {
                for (int j = 0; j < A.columns; j++)
                {
                    result[i] += A.matrix[i, j] * vector[j];
                }
            }
            return result;
        }

        private decimal Dot(decimal[] a, decimal[] b)
        {
            decimal sum = 0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += a[i] * b[i];
            }
            return sum;
        }

        #endregion
    }
}
