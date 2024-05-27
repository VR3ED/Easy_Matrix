using MathNet.Numerics.Data.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using System;
using System.Collections.Generic;
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

        private int chunk_load = 500;

        /// <summary>
        /// get a matrix of decimals
        /// </summary>
        /// <param name="filePath">Specify the path to the.mtx file</param>
        public AccurateMatrix (string filePath)
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            string firstRow = lines[1];

            // Determine the dimensions of the matrix
            var firstRowParts = firstRow.Split( ' ');
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
                        decimal value = decimal.Parse(values[4]);
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
            int threadCount = Math.Max(1, rows*columns / chunk_load);

            // Split the work among the threads
            int chunkSize = rows*columns / threadCount;
            var tasks = new Task<string>[threadCount];
            var stringBuilders = new StringBuilder[threadCount];

            for (int t = 0; t < threadCount; t++)
            {
                int start = t * chunkSize;
                //int end = (t == threadCount - 1) ? rows : start + chunkSize;
                stringBuilders[t] = new StringBuilder();

                tasks[t] = Task.Run(() =>
                {
                    var sb = stringBuilders[t-1];
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

    }
}
