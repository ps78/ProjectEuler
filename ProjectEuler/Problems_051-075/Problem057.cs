using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.IO;
using System.Numerics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=57
    /// It is possible to show that the square root of two can be expressed as an infinite continued fraction.
    /// 
    /// √ 2 = 1 + 1/(2 + 1/(2 + 1/(2 + ... ))) = 1.414213...
    /// 
    /// By expanding this for the first four iterations, we get:
    /// 
    /// 1 + 1/2 = 3/2 = 1.5
    /// 1 + 1/(2 + 1/2) = 7/5 = 1.4
    /// 1 + 1/(2 + 1/(2 + 1/2)) = 17/12 = 1.41666...
    /// 1 + 1/(2 + 1/(2 + 1/(2 + 1/2))) = 41/29 = 1.41379...
    /// 
    /// The next three expansions are 99/70, 239/169, and 577/408, but the eighth expansion, 1393/985, is 
    /// the first example where the number of digits in the numerator exceeds the number of digits in the denominator.
    /// 
    /// In the first one-thousand expansions, how many fractions contain a numerator with more digits than denominator?
    /// </summary>
    public class Problem057 : EulerProblemBase
    {
        public Problem057() : base(57, "Square root convergents", 0, 153) { }

        public override long Solve(long n)
        {
            // iterate x(i+1) = 1/(2+x(i)), x(0) = 1/2
            // then the i-th approximation of Sqrt(2) is 1 + x(i)

            // define x(i):=a/b, with x(0)=1/2
            BigInteger a = 1;
            BigInteger b = 2;
            
            // then x(i+1) = 1/(2+x(i)) = b/(2b+a)
            int result = 0;
            for (int i = 2; i <= 1000; i++)
            {
                BigInteger t = a;
                a = b;
                b = 2 * b + t;

                // actually, the fraction should be simplified, by dividing by the GCD of a+b, b. But it is not necessary for the given problem size
                //BigInteger gcd = GCD.Compute(a + b, b);
                //BigInteger numerator = (a + b) / gcd;
                //BigInteger denominator = b / gcd;
                //BigInteger numerator = (a + b);
                //BigInteger denominator = b ;

                if ((a + b).ToString().Length > b.ToString().Length)
                    result++;

                //Console.WriteLine("X({0}) = {1}/{2}", i, numerator, denominator);
            }
            
            return (long)result;
        }
    }
}