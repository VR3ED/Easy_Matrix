using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyMatrix
{
    public  abstract class AMatrix
    {
        internal decimal[,] matrix { get; set; }
        public int rows { get; set; }
        public int columns { get; set; }
        public int number_of_valorized { get; set; }
    }
}
