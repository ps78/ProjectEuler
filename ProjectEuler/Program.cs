using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            var pm = new ProblemManager(
                // all solved problems
                Enumerable.Range(1, 100)
                .Union(new[] { 101, 121, 144, 146, 148, 169, 200, 243, 307 })
                // unsolved problems
                //.Append(543) 
            );
            
            // problems to solve for awards:
            // - trinary triumph: solve 243
            // - fibonacci fever: solve 233

            pm.Run();
        }
    }
}
