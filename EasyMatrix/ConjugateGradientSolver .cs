using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMatrix
{
    public class ConjugateGradientSolver : IterativeSolver
    {
        public ConjugateGradientSolver(AccurateMatrix A, decimal[] b, decimal tol, int maxIter)
            : base(A, b, tol, maxIter) { }

        //public override decimal[] Solve()
        //{
        //    int n = A.rows;
        //    decimal[] x = new decimal[n];
        //    decimal[] r = (decimal[])b.Clone();
        //    decimal[] p = (decimal[])b.Clone();
        //    decimal rsOld = Dot(r, r);

        //    for (int k = 0; k < maxIter; k++)
        //    {
        //        decimal[] Ap = MatrixVectorMultiply(p);
        //        decimal alpha = rsOld / Dot(p, Ap);

        //        for (int i = 0; i < n; i++)
        //        {
        //            x[i] += alpha * p[i];
        //        }

        //        for (int i = 0; i < n; i++)
        //        {
        //            r[i] -= alpha * Ap[i];
        //        }

        //        decimal rsNew = Dot(r, r);
        //        if ((decimal)Math.Sqrt((double)rsNew) < tol)
        //            return x;

        //        for (int i = 0; i < n; i++)
        //        {
        //            p[i] = r[i] + (rsNew / rsOld) * p[i];
        //        }

        //        rsOld = rsNew;
        //    }
        //    throw new Exception("Conjugate Gradient method did not converge.");
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
