﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMatrix
{
    /// <summary>
    /// Abstract class that defines an iterative solver with an abstract Solve method and helper methods such as Norm
    /// </summary>
    public abstract class IterativeSolver
    {
        protected AccurateMatrix A;
        protected decimal[] b;
        protected decimal tol;
        protected int maxIter;

        public IterativeSolver(AccurateMatrix A, decimal[] b, decimal tol, int maxIter)
        {
            this.A = A;
            this.b = b;
            this.tol = tol;
            this.maxIter = maxIter;
        }

        //public abstract decimal[] Solve();

        public decimal[] Solve()
        {
            int n = A.rows;
            decimal[] x = new decimal[n]; //vettore delle soluzioni --> inizializzato a zero

            for (int k = 0; k < maxIter; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    x = SolverLogic(i, x);
                }

                if (NormAxMinusB(x) < tol)
                    return x;
            }
            throw new Exception("Iterative method did not converge.");
        }

        public abstract decimal[] SolverLogic(int i, decimal[] x);

        #region ELEMENTAL OPERATIONS

        /// <summary>
        /// compute the square root of a number
        /// </summary>
        /// <param name="x">value to calculate the quale root of</param>
        /// <param name="epsilon">an accuracy of calculation of the root from our number.</param>
        /// <returns></returns>
        /// <exception cref="OverflowException"></exception>
        protected decimal Sqrt(decimal x, decimal epsilon = 0.0M)
        {
            if (x < 0) throw new OverflowException("Cannot calculate square root from a negative number");

            decimal current = (decimal)Math.Sqrt((double)x), previous;
            do
            {
                previous = current;
                if (previous == 0.0M) return 0;
                current = (previous + x / previous) / 2;
            }
            while (Math.Abs(previous - current) > epsilon);
            return current;
        }


        /// <summary>
        /// compute the normalization value of an array 
        /// </summary>
        /// <param name="vec">vector to compute the normalization</param>
        /// <returns></returns>
        protected decimal Norm(decimal[] vec)
        {
            decimal sum = 0;
            foreach (var v in vec)
                sum += v * v;
            return Sqrt(sum);
        }

        /// <summary>
        /// compute actual tollerance
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        protected decimal NormAxMinusB(decimal[] x)
        {
            //calcola prodotto matrice A per vettore x
            decimal[] Ax = new decimal[b.Length];
            for (int i = 0; i < A.rows; i++)
            {
                for (int j = 0; j < A.columns; j++)
                {
                    Ax[i] += A.matrix[i, j] * x[j];
                }
            }

            //calcola il residio b - Ax
            decimal[] AxMinusB = new decimal[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                AxMinusB[i] = Ax[i] - b[i];
            }

            return Norm(AxMinusB) / Norm(b);
        }

        #endregion
    }
}
