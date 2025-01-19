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
        
        /// <summary>
        /// resido: Ax-b
        /// </summary>
        private decimal[] r; 

        /// <summary>
        /// passo 
        /// </summary>
        private decimal[] p;


        private decimal rDOTr;


        private decimal alpha;


        private decimal[] Ap;


        /// <summary>
        /// basic contructor
        /// </summary>
        /// <param name="A">Decimals matrix</param>
        /// <param name="b">vecror of known terms</param>
        /// <param name="tol">tollerance index. Up to 28-29 digits precision</param>
        /// <param name="maxIter">maximun number of iterations required</param>
        /// <param name="x">vectror of unknown terms</param>
        public ConjugateGradientSolver(AccurateMatrix A, decimal[] b, decimal tol, int maxIter, decimal[]? x = null) : base(A, b, tol, maxIter) 
        {
            if(x == null)
            {
                //vettore delle incognite --> inizializzato a zero
                x = new decimal[A.rows];
            }

            //calcola prodotto matrice A per vettore x
            decimal[] Ax = MatrixVectorMultiply(x);

            //calcola il residio 0 = b - Ax
            r = VectorsSubtraction(b, Ax);

            //calcola passo 0
            p = (decimal[])r.Clone();

            //calcola r*r all'iterazione 0
            rDOTr = Dot(r, r);

            #region inizializzazione per non avere variabili = null
            Ap = new decimal[A.rows];
            #endregion
        }


        /// <summary>
        /// implements Conjugate Gradient formula
        /// </summary>
        /// <param name="i">current iteration index</param>
        /// <param name="x">x vector</param>
        /// <returns></returns>
        public override decimal[] SolverLogic(int i, decimal[] x)
        {
            if (i == 0)
            {
                Ap = MatrixVectorMultiply(p);
                alpha = rDOTr / Dot(p, Ap);
            }

            x[i] += alpha * p[i];

            r[i] -= alpha * Ap[i];

            return x;
        }


        /// <summary>
        /// check if current solver arrived at an acceptable solution
        /// </summary>
        /// <param name="x">current solutions vector</param>
        /// <returns></returns>
        public override bool SolverExitCondition(decimal[] x)
        {
            if (NormAxMinusBFracNormB(x) < tol)
            {
                return true;
            }
            else
            {
                #region setup for next iteration
                decimal rDOTr_new = Dot(r, r);
                decimal beta = rDOTr_new / rDOTr;
                for (int i = 0; i < A.rows; i++)
                {
                    p[i] = r[i] + beta * p[i];
                }

                rDOTr = rDOTr_new;
                #endregion

                return false;
            }    
        }

    }
}
