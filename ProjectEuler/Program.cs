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
                //Enumerable.Range(1, 102).Union(new[] { 106, 121, 126, 144, 146, 148, 169, 200, 243, 307 })
                new[] {105}
                // unsolved problems
                //.Append(543) 
            );
            
            // problems to solve for awards:
            // - fibonacci fever: solve 233

            pm.Run();
        }
    }
}
