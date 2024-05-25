using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace EasyMatrix
{
    /// <summary>
    /// Create a matrix using maximum precision
    /// </summary>
    public class AccurateMatrix
    {
        decimal[,] matrix {  get; set; }
        int rows { get; set; }
        int columns { get; set; }
        int number_of_valorized { get; set; }


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
            rows = int.Parse( firstRow.Split(' ')[0] );
            columns = int.Parse(firstRow.Split(' ')[2]);
            number_of_valorized = int.Parse(firstRow.Split(' ')[4]); ;

            // Create a two-dimensional array of decimals
            matrix = new decimal[rows, columns];

            // Parse the lines and populate the matrix
            for (int i = 2; i < number_of_valorized+2; i++)
            {
                string[] values = lines[i].Split(' ');
                matrix[int.Parse(values[0])-1, int.Parse(values[2])-1] = decimal.Parse(values[4]);
            }
        }


        /// <summary>
        /// Display the matrix
        /// </summary>
        /// <returns>string matrix</returns>
        public override string ToString()
        {
            string output = "Matrix:\n";

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    output += (matrix[i, j] + " ");
                }
                output += "\n";
            }
            return output;
        }

    }
}
