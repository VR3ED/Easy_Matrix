using MathNet.Numerics.Data.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EasyMatrix
{
    /// <summary>
    /// Create a matrix using maximum precision
    /// </summary>
    public class AccurateMatrix : AMatrix
    {
        /// <summary>
        /// number of chunks to be read at a time when reading a file
        /// </summary>
        private int chunk_load = 500;

        /// <summary>
        /// initialize the matrix 
        /// </summary>
        /// <param name="values"></param>
        /// <exception cref="ArgumentException"></exception>
        public override void InitializeMatrix(decimal[,] values)
        {
            if (values.GetLength(0) != rows || values.GetLength(1) != columns)
                throw new ArgumentException("Matrix dimensions do not match.");

            // Check for symmetric and positive definiteness
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (values[i, j] != values[j, i])
                        throw new ArgumentException("Matrix is not symmetric.");
                }
            }

            matrix = values;
            number_of_valorized = rows * columns;
        }


        /// <summary>
        /// initialize the matrix as an all 0 matrix
        /// </summary>
        /// <param name="size"></param>
        public AccurateMatrix(int size)
        {
            rows = size;
            columns = size;
            matrix = new decimal[size, size];
        }


        /// <summary>
        /// check if the matrix is both positive and symmetric
        /// </summary>
        /// <returns></returns>
        public bool IsSymmetricPositiveDefinite()
        {
            // Verify squared natrix
            if (rows != columns)
                return false;

            // Check for symmetry
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (matrix[i, j] != matrix[j, i])
                        return false;
                }
            }

            #region create a copy of the matrix
            decimal[,] tempMatrix = new decimal[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    tempMatrix[i, j] = matrix[i, j];
                }
            }
            #endregion

            // Cholesky decomposition on copied matrix
            for (int i = 0; i < rows; i++)
            {
                decimal sum = 0;
                for (int k = 0; k < i; k++)
                {
                    sum += tempMatrix[i, k] * tempMatrix[i, k];
                }

                decimal diagValue = tempMatrix[i, i] - sum;
                if (diagValue <= 0)
                    return false;

                tempMatrix[i, i] = Sqrt(diagValue);
                for (int j = i + 1; j < rows; j++)
                {
                    sum = 0;
                    for (int k = 0; k < i; k++)
                    {
                        sum += tempMatrix[j, k] * tempMatrix[i, k];
                    }
                    tempMatrix[j, i] = (tempMatrix[j, i] - sum) / tempMatrix[i, i];
                }
            }

            return true;
        }


        /// <summary>
        /// get a matrix of decimals
        /// </summary>
        /// <param name="filePath">Specify the path to the.mtx file</param>
        public AccurateMatrix (string filePath)
        {
            //setup matrix name
            if (filePath.Contains('\\'))
            {
                this.matrix_name = filePath.Split('\\').Last();
            }
            else
            {
                this.matrix_name = filePath.Split('/').Last();
            }

            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            string firstRow = lines[1];

            // Determine the dimensions of the matrix
            var firstRowParts = firstRow.Split(' ');
            rows = int.Parse(firstRowParts[0]);
            columns = int.Parse(firstRowParts[2]);
            number_of_valorized = int.Parse(firstRowParts[4]);

            // Create a two-dimensional array of decimals
            base.matrix = new decimal[rows, columns];

            #region creating multiple trheds to read matrix
            // Determine number of threads to use
            int threadCount = Math.Max(1, number_of_valorized / chunk_load);

            // Split the work among the threads
            int chunkSize = number_of_valorized / threadCount;
            var tasks = new Task[threadCount];

            for (int t = 0; t < threadCount; t++)
            {
                int start = 2 + t * chunkSize;
                int end = (t == threadCount - 1) ? number_of_valorized + 2 : start + chunkSize;

                tasks[t] = Task.Run(() =>
                {
                    for (int i = start; i < end; i++)
                    {
                        string[] values = lines[i].Split(' ');
                        int row = int.Parse(values[0]) - 1;
                        int column = int.Parse(values[2]) - 1;
                        var numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "." };
                        var strining_value = values[4];
                        var exponent = 0;
                        if (strining_value.Contains('e'))
                        {
                            var tmp = strining_value.Split('e')[0];
                            exponent = int.Parse(strining_value.Split('e')[1]);
                            strining_value = tmp;
                        }
                        decimal value = decimal.Parse(strining_value, numberFormatInfo);
                        value = value * (decimal)Math.Pow(10, exponent) ;
                        base.matrix[row, column] = value;
                    }
                    //Console.WriteLine("Chunk finished: "+start+"-"+end);
                });
            }

            // Wait for all tasks to complete
            Task.WaitAll(tasks);
            //this.matrix = Matrix<decimal>.Build.DenseOfArray(base.matrix);
            #endregion
        }


        /// <summary>
        /// Display the matrix
        /// </summary>
        /// <returns>string matrix</returns>
        public override string ToString()
        {
            // Determine number of threads to use
            int threadCount = Math.Max(1, rows * columns / chunk_load);

            // Split the work among the threads
            int chunkSize = rows * columns / threadCount;
            var tasks = new Task<string>[threadCount];
            var stringBuilders = new StringBuilder[threadCount];

            for (int t = 0; t < threadCount; t++)
            {
                int start = t * chunkSize;
                //int end = (t == threadCount - 1) ? rows : start + chunkSize;
                stringBuilders[t] = new StringBuilder();

                tasks[t] = Task.Run(() =>
                {
                    var sb = stringBuilders[t - 1];
                    for (int i = start; i < rows; i++)
                    {
                        for (int j = 0; j < columns; j++)
                        {
                            sb.Append(base.matrix[i, j].ToString("G29")).Append(' ');
                        }
                        sb.AppendLine();
                    }
                    return sb.ToString();
                });
            }

            // Wait for all tasks to complete and concatenate the results
            Task.WaitAll(tasks);
            var result = new StringBuilder();
            foreach (var task in tasks)
            {
                result.Append(task.Result);
            }

            return result.ToString();
        }

        /// <summary>
        /// compute the square root of a number
        /// </summary>
        /// <param name="x">value to calculate the quale root of</param>
        /// <param name="epsilon">an accuracy of calculation of the root from our number.</param>
        /// <returns></returns>
        /// <exception cref="OverflowException"></exception>
        private decimal Sqrt(decimal x, decimal epsilon = 0.0M)
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

    }
}
