using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMatrix
{
    /// <summary>
    /// Abstract class that defines the structure of a generic array with properties for the size and number of valued values.
    /// </summary>
    public abstract class AMatrix
    {
        internal decimal[,] matrix { get; set; }
        public int rows { get; set; }
        public int columns { get; set; }
        public int number_of_valorized { get; set; }
        public string matrix_name { get; set; }


        public abstract void InitializeMatrix(decimal[,] values);
    }
}
