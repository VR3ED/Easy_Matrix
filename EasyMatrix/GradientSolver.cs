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
        private static decimal[] AxMinusB;

        /// <summary>
        /// static variable that memorizes Ar for each iteration
        /// </summary>
        private static decimal[] Ar;

        /// <summary>
        /// static variable that memorizes alpha for each iteration
        /// </summary>
        private static decimal alpha;


        /// <summary>
        /// basic contructor
        /// </summary>
        /// <param name="A">Decimals matrix</param>
        /// <param name="b">vecror of known terms</param>
        /// <param name="tol">tollerance index. Up to 28-29 digits precision</param>
        /// <param name="maxIter">maximun number of iterations required</param>
        public GradientSolver(AccurateMatrix A, decimal[] b, decimal tol, int maxIter): base(A, b, tol, maxIter) 
        {
            AxMinusB = (decimal[])b.Clone();
            Ar = new decimal[A.rows];
            alpha = 0;
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
                AxMinusB = VectorsSubtraction(b,Ax); //indichiamo b-Ax = r

                // Calcolare Ar = A * (Ax-b)
                Ar = MatrixVectorMultiply(AxMinusB);

                // Calcolare alpha = (r^T * r) / (r^T * A * r)
                decimal rDotr = Dot(AxMinusB, AxMinusB);
                decimal rDotAr = Dot(AxMinusB, Ar);

                // Evitare la divisione per zero
                if (rDotAr == 0)
                {
                    alpha = 0;
                }

                alpha = rDotr / rDotAr;
            }

            x[i] += alpha * AxMinusB[i];

            return x;
        }

        /// <summary>
        /// check if current solver arrived at an acceptable solution
        /// </summary>
        /// <param name="x">current solutions vector</param>
        /// <returns></returns>
        public override bool SolverExitCondition(decimal[] x)
        {
            return NormAxMinusBFracNormB(x) < tol;
        }
        
    }
}
