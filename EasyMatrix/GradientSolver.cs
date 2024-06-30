using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMatrix
{
    public class GradientSolver : IterativeSolver
    {
        public GradientSolver(AccurateMatrix A, decimal[] b, decimal tol, int maxIter)
            : base(A, b, tol, maxIter) { }

        //public override decimal[] Solve()
        //{
        //    int n = A.rows;
        //    decimal[] x = new decimal[n];
        //    decimal[] r = (decimal[])b.Clone();
        //    decimal[] Ap = new decimal[n];

        //    for (int k = 0; k < maxIter; k++)
        //    {
        //        decimal alpha = Dot(r, r) / Dot(r, MatrixVectorMultiply(r));

        //        for (int i = 0; i < n; i++)
        //        {
        //            x[i] += alpha * r[i];
        //        }

        //        decimal[] rNew = VectorSubtract(r, MatrixVectorMultiply(alpha, r));

        //        if (Norm(rNew) / Norm(b) < tol)
        //            return x;

        //        r = rNew;
        //    }
        //    throw new Exception("Gradient method did not converge.");
        //}

        public override decimal[] SolverLogic(int i, decimal[] x)
        {
            throw new NotImplementedException();
        }

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

        private decimal Dot(decimal[] a, decimal[] b)
        {
            decimal sum = 0;
            for (int i = 0; i < a.Length; i++)
            {
                sum += a[i] * b[i];
            }
            return sum;
        }
    }
}
