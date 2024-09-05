using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyMatrix
{
    /// <summary>
    /// Abstract class that defines an iterative solver with an abstract Solve method and helper methods such as Norm
    /// </summary>
    public abstract class IterativeSolver
    {
        /// <summary>
        /// Decimals matrix
        /// </summary>
        protected AccurateMatrix A;

        /// <summary>
        /// vecror of known terms
        /// </summary>
        protected decimal[] b;

        /// <summary>
        /// tollerance index. Up to 28-29 digits precision
        /// </summary>
        protected decimal tol;

        /// <summary>
        /// maximun number of iterations required
        /// </summary>
        protected int maxIter;


        /// <summary>
        /// Constructor common for all Iterative Solvers
        /// </summary>
        /// <param name="A">Decimals matrix</param>
        /// <param name="b">vecror of known terms</param>
        /// <param name="tol">tollerance index. Up to 28-29 digits precision</param>
        /// <param name="maxIter">maximun number of iterations required</param>
        public IterativeSolver(AccurateMatrix A, decimal[] b, decimal tol, int maxIter)
        {
            this.A = A;
            this.b = b;
            this.tol = tol;
            this.maxIter = maxIter;
        }

        
        /// <summary>
        /// Solve method that implents the basics of each solver we will implement from this class
        /// Each Solver will have a differnt implementation of SolverLogic
        /// This means that all Solvers will run this code below to scroll through the matrix,
        /// But only the relative solver logic to the actual solver will get applied
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public decimal[] Solve()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int n = A.rows;
            decimal[] x = new decimal[n]; //vettore delle soluzioni --> inizializzato a zero

            for (int k = 0; k < maxIter; k++)
            {
                for (int i = 0; i < n; i++)
                {
                    x = SolverLogic(i, x);
                }

                if (SolverExitCondition(x))
                {
                    stopwatch.Stop();
                    LogResults(true, tol, k, stopwatch.Elapsed);
                    return x;
                }
            }

            LogResults(false, tol, maxIter, stopwatch.Elapsed);
            throw new Exception("Iterative method did not converge.");
        }

        /// <summary>
        /// abstrac method, each solver implements a differnt so
        /// </summary>
        /// <param name="i">current iteration index</param>
        /// <param name="x">previuously calculated solution from previous iteration</param>
        /// <returns></returns>
        public abstract decimal[] SolverLogic(int i, decimal[] x);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i">current iteration</param>
        /// <param name="x">solu</param>
        /// <returns></returns>
        public abstract bool SolverExitCondition(decimal[] x);


        /// <summary>
        /// Logs the result of the iterator
        /// </summary>
        /// <param name="converge"></param>
        /// <param name="tolerance"></param>
        /// <param name="iterations"></param>
        /// <param name="timeSpent"></param>
        protected void LogResults(bool converge, decimal tolerance, int iterations, TimeSpan timeSpent)
        {
            string filePath = "results.csv";
            bool fileExists = File.Exists(filePath);

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                // If file does not exist, write the header
                if (!fileExists)
                {
                    writer.WriteLine("Matrix,Convergence,SolverType,PrecisionRequired,Iterations,TimeSpent(ms)");
                }

                string solverType = this.GetType().Name;

                // Write the data
                writer.WriteLine($"{A.matrix_name},{converge},{solverType},{tolerance},{iterations},{timeSpent.TotalMilliseconds}");
            }
        }


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
            decimal[] AxMinusB = VectorsSubtraction(Ax,b);

            return Norm(AxMinusB) / Norm(b);
        }


        /// <summary>
        /// executes subraction between two vectors 
        /// (vector1 - vector2)
        /// </summary>
        /// <param name="a">vector1</param>
        /// <param name="b">vector2</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        protected decimal[] VectorsSubtraction(decimal[] a, decimal[] b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException("Vectors are not the same lenght");

            decimal[] result = new decimal[a.Length];
            for (int i = 0; i < a.Length; i++)
            {
                result[i] = a[i] - b[i];
            }
            return result;
        }

        #endregion
    }
}
