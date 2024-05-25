using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Data.Text;
using MathNet.Numerics.Distributions;
using MathNet.Numerics.LinearAlgebra.Complex;
using System.IO;

namespace Library_Metodi_del_Calcolo_Scientifico
{
    /// <summary>
    /// Wrapper for matrix class
    /// this class always uses maximum precision possible using decimals
    /// </summary>
    public class WrapMatrix
    {
        /// <summary>
        /// matrix to store the data
        /// </summary>
        public Matrix<double> matrix { get; set; }


        #region Constructors

        /// <summary>
        /// create a matrix from a local .mtx file
        /// </summary>
        /// <param name="path">location of the .mtx file</param>
        public WrapMatrix(string path)
        {
            try
            {
                matrix = MatrixMarketReader.ReadMatrix<double>(path);
            }
            catch (Exception E)
            {
                Console.WriteLine($"An error occurred while reading the matrix file: {E.Message}");
                throw new Exception(
                "Could not read the matrix at specified path", E);
            }

        }


        /// <summary>
        /// creates a matrix usind a MathNet.Numerics.LinearAlgebra Matrix
        /// </summary>
        /// <param name="matrix"></param>
        /// <exception cref="Exception"></exception>
        public WrapMatrix(Matrix<double> matrix)
        {
            this.matrix = matrix;
        }

        #endregion

        #region GET

        /// <summary>
        /// overwrites current matrix with a matrix from a .mtx file
        /// </summary>
        /// <param name="path"></param>
        public void ImportMatrix(string path)
        {
            matrix = MatrixMarketReader.ReadMatrix<double>(path);
        }


        /// <summary>
        /// returns a pre-compiled matrix from the library
        /// </summary>
        /// <param name="number">insert a number between 1 and 4</param>
        /// <returns>returns a pre-compiled matrix from the library</returns>
        static public WrapMatrix GetDefaultMatrix(int number)
        {
            switch(number)
            {
                case 1:
                {
                    return new WrapMatrix( MatrixMarketReader.ReadMatrix<double>(Directory.GetCurrentDirectory() + @"..\..\" + @"\Matrixes\spa1.mtx") );
                }
                case 2:
                {
                    return new WrapMatrix(MatrixMarketReader.ReadMatrix<double>(Directory.GetCurrentDirectory() + @"\Matrixes\spa2.mtx") );
                }
                case 3:
                {
                    return new WrapMatrix(MatrixMarketReader.ReadMatrix<double>(Directory.GetCurrentDirectory() + @"\Matrixes\vem1.mtx") );
                }
                case 4:
                {
                    return new WrapMatrix(MatrixMarketReader.ReadMatrix<double>(Directory.GetCurrentDirectory() + @"\Matrixes\vem2.mtx") );
                }
                default: {
                    throw new Exception("Could not read the matrix at specified index, please use a number between 1 and 4");
                }
            }
                
            
        }

        #endregion

        /// <summary>
        /// Prints the matrix
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return matrix.ToString();
        }
    }
}

