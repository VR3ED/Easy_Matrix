using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMatrix
{
    /// <summary>
    /// Implements Conjugate Gradient.
    /// </summary>
    public class ConjugateGradientSolver : IterativeSolver
    {
        
        private decimal[] r;


        private decimal[] p;


        private decimal rsOld;


        private decimal alpha;


        private decimal[] Ap;


        public ConjugateGradientSolver(AccurateMatrix A, decimal[] b, decimal tol, int maxIter): base(A, b, tol, maxIter) 
        {
            r = (decimal[])b.Clone();
            p = (decimal[])b.Clone();
            rsOld = Dot(r, r);
            Ap = MatrixVectorMultiply(p);
            alpha = rsOld / Dot(p, Ap);
        }

        
        public override decimal[] SolverLogic(int i, decimal[] x)
        {
            if (i == 0)
            {
                Ap = MatrixVectorMultiply(p);
                alpha = rsOld / Dot(p, Ap);
            }

            x[i] += alpha * p[i];

            r[i] -= alpha * Ap[i];

            return x;
        }

        public override bool SolverExitCondition(decimal[] x)
        {
            decimal rsNew = Dot(r, r);
            if ((decimal)Math.Sqrt((double)rsNew) < tol)
                return true;

            for (int i = 0; i < A.rows; i++)
            {
                p[i] = r[i] + (rsNew / rsOld) * p[i];
            }

            rsOld = rsNew;

            return false;
        }

    }
}
