using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory
{
    public class Fibonacci
    {
        public static ulong Get(uint number)
        {
            if (number == 0)
                return 0;
            else if (number == 1)
                return 1;
            else
                return Get(number - 1) + Get(number - 2);
        }
    }
}
