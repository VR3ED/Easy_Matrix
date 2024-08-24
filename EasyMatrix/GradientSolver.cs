using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
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
        private static decimal[] Ap;

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
            Ap = new decimal[A.rows];
            alpha = 0;
            rNew = new decimal[A.rows];
        }


        public decimal[] Solve()
        {
            int n = A.rows;
            decimal[] x = new decimal[n];
            decimal[] r = (decimal[])b.Clone();
            decimal[] Ap = new decimal[n];

            for (int k = 0; k < maxIter; k++)
            {
                decimal alpha = VectorsMultiplication(r, r) / VectorsMultiplication(r, MatrixVectorMultiply(r));

                for (int i = 0; i < n; i++)
                {
                    x[i] += alpha * r[i];
                }

                decimal[] rNew = VectorSubtract(r, MatrixVectorMultiply(alpha, r));

                if (Norm(rNew) / Norm(b) < tol)
                    return x;

                r = rNew;
            }
            throw new Exception("Gradient method did not converge.");
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
                alpha = VectorsMultiplication(r, r) / VectorsMultiplication(r, MatrixVectorMultiply(r));
            }

            x[i] += alpha * r[i];

            if (i == A.rows - 1)
            {
                rNew = VectorSubtract(r, MatrixVectorMultiply(alpha, r));                                   
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
            if (Norm(rNew) / Norm(b) < tol)
            {
                return true;
            }
            else
            {
                r = rNew;
                return false;   
            }
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

        private decimal[] MatrixVectorMultiply(decimal scalar, decimal[] vector)
        {
            decimal[] result = new decimal[vector.Length];
            for (int i = 0; i < vector.Length; i++)
            {
                result[i] = scalar * vector[i];
            }
            return result;
        }

        private decimal[] VectorSubtract(decimal[] a, decimal[] b)
        {
            decimal[] result = new decimal[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = a[i] - b[i];
            }
            return result;
        }

        private decimal VectorsMultiplication(decimal[] a, decimal[] b)
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