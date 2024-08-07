using System.Data;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=233
    /// 
    /// Let f(N) be the number of points with integer coordinates that are on a circle 
    /// passing through (0,0), (0,N), (N,0) and (N,N).
    /// 
    /// It can be shown that f(10'000) = 36
    /// 
    /// What is the sum of all positive integers N <= 10^11 such that f(N) = 420?
    /// </summary>
    public class Problem233 : EulerProblemBase
    {
        public Problem233() : base(233, "Lattice Points on a Circle", 11, 0) { }

        private SieveOfEratosthenes Sieve = new SieveOfEratosthenes(5_000_000); // limit >= 10^11 / (5^3*13^2)

        private const long L = 420;

        /// <summary>
        /// Problem is related to counting the lattice points on a circle, which can be used
        /// to calculate Pi, see https://www.youtube.com/watch?v=NaL_Cb42WyY
        /// 
        /// The problem in the video however relates to a circles with radius Sqrt(M) where M is an integer.
        /// For our problem, we have a circle with radius of Sqrt(N^2/2). And the center
        /// can be in the center of a lattice unit square.
        /// For N^2/2 to be an integer, N^2 needs to be even => N needs to be even (then the circle center
        /// will also be on a lattice point)
        /// This is not the case, hence we scale the problem: Z = N^2/2 and solve it for M = 2*Z = N^2 (which is an integer).
        /// while 
        ///     N = 1..10^11
        ///     M = 1..10^22
        /// 
        /// Recipe: Given the task is to find the number of lattice points on a circle with radius Sqrt(M), 
        /// M integer, and 0,0 at the center, factor M into prime factors
        /// 
        /// 1) for any prime factor of the form 4k-1 (3, 7, 11, 19,..): 
        ///         if the exponent is even, take 1 as factor
        ///         if the exponent is odd, there are no lattice points on the circle
        /// 2) for any prime factor of the form 4k+1 (5, 13, 17, 29, ..): 
        ///         take it's exponent+1 as factor
        /// 3) for the prime factor of 2, take a factor of 1 regardless of the exponent
        /// 
        /// The product of these factors is the number of lattice points on the circle
        /// 
        /// For L=420, removing factors 2^k, the remaining 105 can be formed as:
        ///   105 = 3 * 5 * 7
        ///   105 = 3 * 35 (not possible, result would by > 10^11)
        ///   105 = 5 * 21
        ///   105 = 7 * 15
        ///   105 = 105 (not possible, result would by > 10^11)
        ///   
        /// We need to build all N <= 10^11 such that it consists of prime factors:
        /// - 2^i, for any i
        /// - p^j, for p prime and p = 4m-1, for any j
        /// - p^k, for p prime and p = 4m+1, PRODUCT(2k+1) = 105 over all k (i.e. p=5, 13, 17, 29, 37, ..)
        /// 
        /// since 105 can only be factored as 3x5x7 or 3x35 or 5x21 or 7x15 or 1x105, the corresponding 
        /// exponents k of p^k must be 1, 2, 3, 7, 10, 17, 52. 
        /// 17 and 52 are not applicable, as 5^17 > 10^11, hence k in {1,2,3,7,10}
        /// Allowed sets for k are:
        ///     {1,2,3}, {1,3,2}, {2,1,3}, {2,3,1}, {3,1,2}, {3,2,1} 
        ///     {10,2} ({2,10} is not an option, result would be >10^11)
        ///     {3,7}, {7,3}
        /// for p we have {5, 13, 17, 29, 37, ..}     
        /// this yields the smallest N = 5^3*13^2*17 = 359125
        /// 
        /// </summary>
        public override long Solve(long n)
        {            
            ulong N = (ulong)Math.Pow(10, n);
            var keyNumbers = new List<ulong>();

            // get all primes of the form p = 4k+1 up to the limit 10^11 / (5^3*13^2)
            ulong keyPrimeLimit = (ulong)(N / (Math.Pow(5, 3) * Math.Pow(13, 2))) ;
            ulong[] keyPrimes = Sieve.GetPrimes(from: 5, to: keyPrimeLimit).Where(p => (p - 1) % 4 == 0).ToArray();
            // get all primes of the form p = 4k-1 up to the limit 10^11 / (5^3*13^2*17)
            ulong otherPrimeLimit = (ulong)(N / (Math.Pow(5, 3) * Math.Pow(13, 2) * 17)) ;
            ulong[] otherPrimes = Sieve.GetPrimes(from: 2, to: otherPrimeLimit).Where(p => (p==2) || ((p + 1) % 4 == 0)).ToArray();

            // these (ordered) sets of exponents are allowed for the prime factors of form 4k+1:
            List<ulong[]> expSets2 = [[10, 2], [3, 7], [7, 3]];
            List<ulong[]> expSets3 = [[1, 2, 3], [1, 3, 2], [2, 1, 3], [2, 3, 1], [3, 1, 2], [3, 2, 1]];

            // for each exponent set we choose all possible prime factors 4k+1 such that the product is < 10^11
            foreach (var expSet in expSets2)
            {
                for (int pos1 = 0; pos1 < keyPrimes.Length; pos1++)
                {
                    bool quit = false;
                    ulong power1 = keyPrimes[pos1].Power(expSet[0]);
                    for (int pos2 = pos1 + 1; pos2 < keyPrimes.Length; pos2++)
                    {                        
                        ulong power2 = keyPrimes[pos2].Power(expSet[1]);
                        if (power1 * power2 > N)
                        {
                            if (pos2 == pos1 + 1)
                                quit = true;
                            break;
                        }
                        else
                            keyNumbers.Add(power1 * power2);
                    }
                    if (quit) break;
                }
            }

            foreach (var expSet in expSets3)
            {
                for (int pos1 = 0; pos1 < keyPrimes.Length; pos1++)
                {
                    bool quit = false;
                    ulong power1 = keyPrimes[pos1].Power(expSet[0]);
                    for (int pos2 = pos1 + 1; pos2 < keyPrimes.Length; pos2++)
                    {
                        ulong power2 = keyPrimes[pos2].Power(expSet[1]);
                        for (int pos3 = pos2 + 1; pos3 < keyPrimes.Length; pos3++)
                        {
                            ulong power3 = keyPrimes[pos3].Power(expSet[2]);
                            if (power1 * power2 * power3 > N)
                            {
                                if (pos3 == pos2 + 1)
                                    quit = true;
                                break;
                            }
                            else
                                keyNumbers.Add(power1 * power2 * power3);
                        }
                        if (quit) break;
                    }
                    if (quit) break;
                }
            }

            // now we have all numbers built only from prime factors of the form 4k+1
            // we need to add for each of these all possible factors of the form 4k-1 and 2
            // there can be a maximum of 6 additional factors:
            //  359125 * 2 * 3 * 7 * 11 * 19 * 23 = 72'505'182'750

            ulong sum = keyNumbers.Sum();
            var addNumbers = new List<ulong>();
            for (int i = 0; i < keyNumbers.Count; i++)
            {                
                var factors = new Stack<ulong>();
                ExpandFactors(keyNumbers[i], otherPrimes, N, 0, ref addNumbers, ref factors);
            }
            sum += addNumbers.Sum();


            foreach (var num in addNumbers.OrderBy(x => new Guid()).Take(10000).ToList())
                if (CountLatticePoints(num) != L)
                    throw new Exception();


            return (long)sum;

            // wrong solutions:
            // 259544953893862880
            // 259567548676753505
        }

        private void ExpandFactors(ulong keyNumber, ulong[] potentialFactors, ulong N, int startIndexAt, ref List<ulong> numbers, ref Stack<ulong> factors)
        {
            ulong currentProduct = keyNumber * factors.Product();

            if (currentProduct <= N && currentProduct > keyNumber)
            {
                numbers.Add(currentProduct);
            }

            for (int i = startIndexAt; i < potentialFactors.Length; i++)
            {
                int numsCountBefore = numbers.Count;

                ulong factor = potentialFactors[i];
                ulong maxFactor = N / currentProduct;
                while (factor <= maxFactor)
                {
                    factors.Push(factor);
                    ExpandFactors(keyNumber, potentialFactors, N, i + 1, ref numbers, ref factors);
                    factors.Pop();
                    factor *= potentialFactors[i];
                }

                // if we couldn't find any additional factor, no need to continue
                if (numbers.Count == numsCountBefore)
                    break;
            }
        }

        private ulong CountLatticePoints(ulong N)
        {
            // we actually need to do this for M = (2N)^2/2
            // this in fact doubles all exponents, hence we cannot have any odd exponents                                                         
            var factors = Sieve.GetPrimeFactors(N);
            ulong product = 4;
            foreach(var factor in factors.Where(f => (f.Item1 - 1) % 4 == 0))
                product *= (2*factor.Item2 + 1);
            
            return product;
        }
    }
}
