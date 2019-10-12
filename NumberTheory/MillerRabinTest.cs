using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory
{
    public class MillerRabinTest : IPrimeTest
    {
        private const ulong two = 2;

        public MillerRabinTest() { }

        /// <summary>
        /// Probabilistic primality test for n
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool IsProbablePrime(ulong n)
        {
            // handle trivial cases
            if (n == 1)
                return false;
            if ((n == 2) || (n == 3))
                return true;
            if (n % 2 == 0)
                return false;

            // write n-1 as d*2^s
            ulong d, s;
            getds(n, out d, out s);

            var listof_a = geta(n);            
            foreach (var a in listof_a)
            {                
                // if a^d != 1 (mod n)
                if (a.ModPower(d, n) != 1)
                {
                    bool isComposite = true;
                    // and for all r: a^(d*2^r) != -1 (mod n)                    
                    for (ulong r = 0; r < s; r++)
                        if ((a.ModPower(d * two.Power(r), n) == n - 1))
                        {
                            isComposite = false;                            
                            break;
                        }
                    if (isComposite)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Deterministic primality test for n
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public bool IsPrime(ulong n)
        {
            // handle trivial cases
            if (n == 1)
                return false;
            if ((n == 2) || (n == 3))
                return true;
            if (n % 2 == 0)
                return false;

            // write n-1 as d*2^s
            ulong d, s;
            getds(n, out d, out s);

            var listof_a = geta(n);
            foreach (var a in listof_a)
            {
                // if a^d != 1 (mod n)
                if (a.ModPower(d, n) != 1)
                {
                    bool isComposite = true;
                    // and for all r: a^(d*2^r) != -1 (mod n)                    
                    for (ulong r = 0; r < s; r++)
                        if ((a.ModPower(d * two.Power(r), n) == n - 1))
                        {
                            isComposite = false;
                            break;
                        }
                    if (isComposite)
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// returns d and s such that n-1 == d*2^s
        /// </summary>
        /// <param name="n"></param>
        /// <param name="d"></param>
        /// <param name="s"></param>
        private void getds(ulong n, out ulong d, out ulong s)
        {
            ulong n1 = n - 1;
            ulong maxS = (ulong)(Math.Log(n1) / Math.Log(2));
            ulong twoPowerS = two.Power(maxS);
            for (ulong currentS = maxS; currentS >= 1; currentS--)
            {
                if (n1 % twoPowerS == 0)
                {
                    s = currentS;
                    d = n1 / twoPowerS;
                    return;
                }
                twoPowerS /= 2;
            }
            d = 1;
            s = 0;
        }

        /// <summary>
        /// returns the list of a's to use in test
        /// for 64-bit numbers there is a predetermined small list of a's to use according to 
        /// https://en.wikipedia.org/wiki/Miller%E2%80%93Rabin_primality_test
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private IEnumerable<ulong> geta(ulong n)
        {
            ulong[] result;

            if (n < 2047)
                result = new ulong[] { 2 };
            else if (n < 1373653)
                result = new ulong[] { 2, 3 };
            else if (n < 9080191)
                result = new ulong[] { 31, 73 };
            else if (n < 25326001)
                result = new ulong[] { 2, 3, 5 };
            else if (n < 4759123141)
                result = new ulong[] { 2, 7, 61 };
            else if (n < 122004669633)
                result = new ulong[] { 2, 13, 23, 1662803 };
            else if (n < 2152302898747)
                result = new ulong[] { 2, 3, 5, 7, 11 };
            else if (n < 3474749660383)
                result = new ulong[] { 2, 3, 5, 7, 11, 13 };
            else if (n < 341550071728321)
                result = new ulong[] { 2, 3, 5, 7, 11, 13, 17 };
            else if (n < 3825123056546413051)
                result = new ulong[] { 2, 3, 5, 7, 11, 13, 17, 19, 23 };
            else // for n < 2^64
                result = new ulong[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37 };

            foreach (var i in result)
                yield return i;
        }

    }
}
