using System.Data;
using System.Security;
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
        public Problem233() : base(233, "Lattice Points on a Circle", 11, 271204031455541309) { }

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
            ulong keyPrimeLimit = (ulong)(N / (Math.Pow(5, 3) * Math.Pow(13, 2))); // =upper bound for biggest possible prime factor
            ulong[] keyPrimes = Sieve.GetPrimes(from: 5, to: keyPrimeLimit).Where(p => (p - 1) % 4 == 0).ToArray();
            
            // get all primes of the form p = 4k-1 up to the limit 10^11 / (5^3*13^2*17)
            ulong otherPrimeLimit = (ulong)(N / (Math.Pow(5, 3) * Math.Pow(13, 2) * 17)) ;
            ulong[] otherPrimes = Sieve.GetPrimes(from: 2, to: otherPrimeLimit).Where(p => (p==2) || ((p + 1) % 4 == 0)).ToArray();

            List<ulong[]> expSets = GetExponentSets(L);
            foreach (var expSet in expSets)
            {
                int[] selectedPrimeIdx = Enumerable.Repeat(-1, expSet.Length).ToArray();
                FindKeyNumbers(expSet, keyPrimes, N, ref selectedPrimeIdx, 0, ref keyNumbers);
            }
            
            // now we have all numbers built only from prime factors of the form 4k+1
            // we need to add for each of these all possible factors of the form 4k-1 and 2
            // there can be a maximum of 6 additional factors:
            //    359125 * 2 * 3 * 7 * 11 * 19 * 23 = 72'505'182'750
            ulong sum = keyNumbers.Sum();
            var addNumbers = new List<ulong>();
            for (int i = 0; i < keyNumbers.Count; i++)
            {                
                var factors = new Stack<ulong>();
                ExpandFactors(keyNumbers[i], otherPrimes, N, 0, ref addNumbers, ref factors);
            }
            sum += addNumbers.Sum();

            return (long)sum;
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

        /// <summary>
        /// finds all keyNumbers x < limit, which can be represented as the product of the given 
        /// keyPrimes with exponents from the given expSet. 
        /// Recursive method. KeyPrimes are taken in ascending order and expSet is not shuffled
        /// </summary>
        /// <param name="expSet"></param>
        /// <param name="keyPrimes"></param>
        /// <param name="limit"></param>
        /// <param name="keyNumbers"></param>
        private void FindKeyNumbers(ulong[] expSet, ulong[] keyPrimes, ulong limit, 
                                    ref int[] selectedPrimeIdx, int currentPosition, 
                                    ref List<ulong> keyNumbers,
                                    ulong currentProduct = 1)
        {
            if (currentPosition >= expSet.Length)
            {
                if (currentProduct <= limit)
                    keyNumbers.Add(currentProduct);
            }
            else
            {
                int start = currentPosition == 0 ? 0 : selectedPrimeIdx[currentPosition - 1] + 1;
                for (int i = start; i < keyPrimes.Length; i++)
                {
                    double nextFactor = Math.Pow(keyPrimes[i], expSet[currentPosition]);
                    if (nextFactor > limit || nextFactor*currentProduct > limit)
                        return;

                    selectedPrimeIdx[currentPosition] = i;
                    ulong previousProduct = currentProduct;
                    currentProduct *= keyPrimes[i].Power(expSet[currentPosition]);
                    FindKeyNumbers(expSet, keyPrimes, limit, ref selectedPrimeIdx, currentPosition + 1, ref keyNumbers, currentProduct);
                    currentProduct = previousProduct;
                }
            }
        }

        /// <summary>
        /// Generates all ordered sets [e0, e1, .. ek] of exponents such that 
        /// 
        /// with l = 4 * f0 * f1 *..fk where fi divide l
        /// and fi = 2*ei+1
        /// 
        /// i.e. l = 420 => 
        ///     420 = 4 * 3 * 5 * 7 => f = {5}  => e = {1,2,3}
        ///     420 = 4 * 3 * 35    => f = {3,35}   => e = {1,17}
        ///     420 = 4 * 5 * 21    => f = {5,21}   => e = {2,10}
        ///     420 = 4 * 7 * 15    => f = {7,15}   => e = {3,7}
        ///     420 = 4 * 105       => f = {105}    => e = {52}
        /// 
        /// returns all permutations of the corresponding sets e={..}
        /// </summary>
        /// <param name="l"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private List<ulong[]> GetExponentSets(ulong l)
        {
            if (l % 4 != 0)
                throw new ArgumentException("l must be divisible by 4");

            var factorSets = GetMultiplicativePartitions(l / 4);

            // coreSets contains the factors f, the actual exponents f = 2e+1
            var expSets = factorSets.Select(fset => fset.Select(f => (f - 1) / 2).ToArray()).ToArray();

            var permutedSets = new List<ulong[]>();
            foreach (var set in expSets)
                foreach (var s in Permutation.Create<ulong, ulong[]>(set))
                    permutedSets.Add(s);

            return permutedSets;
        }

        /// <summary>
        /// This function should return all products that form n
        /// A general implementation would be nice but is fairly complicated
        /// </summary>
        private static List<ulong[]> GetMultiplicativePartitions(ulong n)
        {
            switch (n)
            {
                case 105: 
                    return [[3, 5, 7], [3, 35], [5, 21], [7, 15], [105]];
                case 9:
                    return [[3, 3], [9]];
                default: 
                    throw new NotImplementedException("Partitioning of {n} not implemented");
            }
        }

        private ulong CountLatticePoints(ulong N)
        {
            // we actually need to do this for M = (2N)^2/2
            // this in fact doubles all exponents, hence we cannot have any odd exponents
            var factors = Sieve.GetPrimeFactors(N);
            ulong product = 4;
            foreach(var factor in factors.Where(f => (f.Factor - 1) % 4 == 0))
                product *= (2*factor.Exponent + 1);
            
            return product;
        }

        /// <summary>
        /// Solves the equation (x-N/2)^2 + (y-N/2)^2 = N^2/2
        /// for y, for all x in 0..N-1
        /// 
        /// y^2 - Ny + x^2 - Nx + N^2/4 - N^2/2 + N^2/4 = 0 
        /// y^2 - Ny - x(N-x) = 0
        /// 
        /// y = (N - Sqrt(N^2 + 4x(N-x))) / 2
        /// </summary>
        /// <param name="N"></param>
        /// <returns></returns>
        private static ulong CountLatticePointsBruteForce(ulong N)
        {
            ulong count = 0;
            for (ulong x = 0; x < N; x++)
            {
                ulong t = N * N + 4 * x * (N - x);
                if (double.IsInteger(Math.Sqrt(t)))
                    count++;
            }
            return 4*count;
        }
    }
}
