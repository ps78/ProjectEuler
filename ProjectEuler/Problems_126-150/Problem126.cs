using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Diagnostics;

namespace ProjectEuler
{    
    /// <summary>
    /// https://projecteuler.net/problem=126
    /// 
    /// The minimum number of cubes to cover every visible face on a cuboid measuring 3 x 2 x 1 is twenty-two.
    /// If we then add a second layer to this solid it would require forty-six cubes to cover every visible face, the third layer would 
    /// require seventy-eight cubes, and the fourth layer would require one-hundred and eighteen cubes to cover every visible face.
    /// However, the first layer on a cuboid measuring 5 x 1 x 1 also requires twenty-two cubes; similarly the first layer on cuboids 
    /// measuring 5 x 3 x 1, 7 x 2 x 1, and 11 x 1 x 1 all contain forty-six cubes.
    /// We shall define C(n) to represent the number of cuboids that contain n cubes in one of its layers. 
    /// So C(22) = 2, C(46) = 4, C(78) = 5, and C(118) = 8.
    /// It turns out that 154 is the least value of n for which C(n) = 10.
    /// Find the least value of n for which C(n) = 1000.
    /// </summary>
    public class Problem126 : EulerProblemBase
    {
        public Problem126() : base(126, "Cuboid layers", 1000, 18522) { }

        public override bool Test() => Solve(10) == 154 && F(1, 5, 1, 1) == 22 && F(4, 3, 2, 1) == 118;

        public override long Solve(long n)
        {
            // Solution from here:
            // https://tio.run/##pVZLc9s2EL7nV2x94ksyATKSKkWTg9Kc1F6cS0aj6YAkZDEBQQ9J2dZ0kr/uLh4USVeJ1HYkgnh8@8DHxS6@sEc2Kh@4/JJ9fXlJBatr@J3l8q83AA@HROQp1A1r8PVY5hkUuOTcNVUu7zdbVt3XrgICiFLeK2DVfMoLDku4O9YNL8aSyVLNOO7ijQba@fLQjB9QSyOkkzokDEP3J4ibFRPpQaAbaKXi9UE0c7jxnX8YGZ1ccG8Jn/g3UNQ3qBjgGz74t3tBtZA6qs3tBlS/WNK3f6IrwWqzXUr@pCY3xTZgQRKkgQjkMlxo8K6sHLYki53DAvUL3XfLYsF839XLBpAsmQYk@DOApAMYSLpMLCS1kLQPMSCBVluQ0CAxBAGsNh1gRLa@37m5kO@KhewE8p2z2sjtcpl3KireHCo5@i6NmB2SHm23tx/zqm5AsCOvwFkdEhUM5Q4YJEdIVJO6cwCHeQn4wLwU28RLXfCAanm0POZjsJLkmT5Hc3CIRxFIvAhb6kUajsHjUD/yJ66n@oToF7Vq7nhaysw4Mj9vD7uOE3vslrpabaJbs7aAsoI6Lx5Evst59hMNzE98PYp/7D9tbZHWFh1uxACoH7e70ROEdn0aYy@eWBt3J89aE1YFUVB8tOa4FZ548Vk9n/Z5dQVHPyKpRbx//x/ommnBC5yht62JdgPxrB1Ow7b3r1iZDdRNvNm16n97fuBpg8rxvBgDkNcwnQVQlwpY78uDyCDhMDMSH8tD1ez/H7/ROX5R5iK/hF5NcDRgYHri@9e4r@I6dgm9Stl5Lgk5Qya1B8ue60NRsOqoc0pSPvK5mR5BL/GgZ2ui0gOS5CNFviLIoy1ymBrW6oitScedF7fAwfGAddQC1@b8zk6mB995HfdwOmFZhX@UT/DEQXLcdlPiPgu@x6m0LJJccmj26lEWmVS8dDprLDCYQWusaoIrxgqscYFOpr16N9Ny@EFzidWOmbSbZWopb2DPqiwtMzybfTY/8Cp/NF9B6UT7rIGnsvpa6w/DhNCOgCPU5wlHucz4M89cqwS6cjJ/zXafUE@okRgR3beyJiZ3DgloEAUQuibP@5jlfZUajQriY45XYuo0OqFREQ5yv85ven1E7CIxBSPUjw3FbnDeAXLBAR0ixDhAzjig1kO7Zu3TuO/AMIW/sk4vWDcFwlinZ6yb@hh3pdCmstkgsanRdHbeg@iCByqUnch4EJ3xwNRms2Y9mJ62TYdDPOitE6uyqjAVDOLtF/jMjmOYf@jHqo40FYbqoJhUtgDRTqQHVCPbm8cpUs14eKHb6QsdC1Sb6DbVrbBXPHOveRXOfi@Y/e8jod7m4vPt5eVv

            long nLimit = n == 10 ? 500 : 20000;
            var C = new long[nLimit];
            for (int a = 1; F(1, a, a, a) < nLimit; a++)
                for (int b = a; F(1, a, b, b) < nLimit; b++)
                    for (int c = b; F(1, a, b, c) < nLimit; c++)
                        for (int l = 1; F(l, a, b, c) < nLimit; l++)
                            C[F(l, a, b, c)]++;

            return C.Contains(n) ? Array.IndexOf(C, n) : 0;
        }

        /// <summary>
        /// number of cubes in the n-th layer of a a x b x c box
        /// </summary>
        private long F(long n, long a, long b, long c)
        {
            return 2 * (a * b + a * c + b * c) + 4 * (n - 1) * (a + b + c) + 4 * (n - 1) * (n - 2);
        }

        #region First attempt, too slow

        public long Solve_v1(long n)
        {
            int N = 50;
            var partitions = new List<long>[N + 1];
            for (long m = 3; m <= N; m++)
            {
                partitions[m] = new List<long>();
                long F1;
                long a = 0, b = 0, c = 0;
                long part = GetNextPartition(m, a, b, c, out F1);
                while (part != 0)
                {
                    partitions[m].Add(part);
                    FromLong(part, out a, out b, out c);
                    part = GetNextPartition(m, a, b, c, out F1);
                }
            }

            for (long m = 3; m <= N; n++)
            {
                if (partitions[m].Count != PartitionCount3(m))
                {
                    Console.WriteLine("{0}: expected {1} partitions, found {2}", m, PartitionCount3(m), partitions[m].Count);
                }
            }

            return 0;

            /*
            ulong limit = 50;
            
            var buffer = new Dictionary<ulong, LayerLimit>();
            var solutions = new Dictionary<ulong, ulong>(); // n, number of boxes with an Fi(a,b,c)=n
            
            // create all triple-partitions of s(limit), such that a+b+c <= s(limit) and a <= b <= c
            // this guarantees to catch all F1(a,b,c) that are smaller or equal to limit
            // s(limit) = limit/4 + 3/2
            ulong lowerLimit = 1;
            ulong upperLimit = (ulong)(limit / 4.0 + 1.5);
            while (true)
            {
                var partitions = CreatePartions((int)lowerLimit, (int)upperLimit);

                ExtendBuffer(ref buffer, ref solutions, partitions, limit);
                
                var sols = solutions.Where(kv => kv.Value == ProblemSize).ToList();
                if (sols.Count > 0)
                    break;
                 
                Console.WriteLine("current max: {0:N0}, buffer-size: {1:N0}, distinct n: {2:N0}, last part count: {3:N0}, limit: {4:N0}", 
                    solutions.Values.Max(), buffer.Count, solutions.Count, partitions.Count(), limit);

                lowerLimit = upperLimit + 1;
                upperLimit = lowerLimit + 1;
                limit = 2 + 4 * (upperLimit - 2);
            }            

            return (ulong)solutions.Where(kv => kv.Value == ProblemSize).ToList().Select(kv => kv.Key).Min();
            */
        }

        public class LayerLimit
        {
            public long LayerIndex { get; set; }
            public long FValue { get; set; }
            public LayerLimit()
            {
                LayerIndex = 0;
                FValue = 0;
            }
            public void Set(long layerIdx, long fval)
            {
                LayerIndex = layerIdx;
                FValue = fval;
            }
        }

        private long FTime = 0;
        private long LookupTime = 0;
        private Stopwatch sw = new Stopwatch();

        protected static long ToLong(long a, long b, long c)
        {
            return (a << 42) | (b << 21) | c;
        }

        protected static void FromLong(long n, out long a, out long b, out long c)
        {
            a = (n >> 42) % 0x200000;
            b = (n >> 21) % 0x200000;
            c = n % 0x200000;
        }

        /// <summary>
        /// create all a,b,c such that 
        ///     max >= a+b+c  
        ///     a+b+c >= min 
        ///     c >= b>= a
        /// </summary>
        private IEnumerable<long> CreatePartions(long min, long max)
        {
            min = Math.Max(min, 3);
            long a = 1;
            long b = 1;
            long c = min - a - b;
            
            while (a + b + c <= max)
            {
                if (a + b + c < min)
                    c = min - a - b;

                yield return ToLong(a, b, c);

                c++;
                
                if (a + b + c > max)
                {
                    b++;
                    c = b;
                }
                if (a + b + c > max)
                {
                    a++;
                    b = a;
                    c = b;
                }
            }
        }

        public class ABC
        {
            public long A { get; private set; }
            public long B { get; private set; }
            public long C { get; private set; }
            public long F1 { get; private set; }
            public long AsLong() { return ToLong(A, B, C);  }
            public ABC(long a, long b, long c)
            {
                A = a; B = b; C = c;
                F1 = 2 * (A * B + B * C + A * C); 
            }
            public override string ToString()
            {
                return "<" + A.ToString() + "," + B.ToString() + "," + C.ToString() + "> F=" + F1.ToString();
            }
        }

        /// <summary>
        /// number of partitions of n with p parts at max
        /// </summary>
        /// <returns></returns>
        public long PartitionCount(long n, long p)
        {
            if (n <= 1)
                return 1;
            if ((n == 2) && (p >= 2))
                return 2;            
            if (p == 1)
                return 1;
                        
            return PartitionCount(n - p, p) + PartitionCount(n, p - 1);
        }

        public long PartitionCount3(long n)
        {
            return PartitionCount(n, 3) - PartitionCount(n, 2);
        }
        
        /// <summary>
        /// creates all partitions a,b,c, of n with F1(a,b,c)=2*(ab+bc+ac) between min and max and 
        /// ordered such that F1 is ascending
        /// </summary>
        private long GetNextPartition(long n, long a, long b, long c, out long F1Value)
        {
            // first partition, with minimal F1
            if (a == 0)
            {
                F1Value = 2 * (2*(n - 2) + 1);
                return ToLong(1, 1, n - 2);
            }

            // no more partitions:
            if ((b <= a + 1) && (c <= a + 1))
            {
                F1Value = 0;
                return 0;
            }
            
            long f = 2*(a * b + b * c + a * c);
            

            // there are various possiblities for the next partition:
            // 1. a,   b+1  c-1
            // 2. a+1  b-1  c
            // 3. a+1  b-2  c+1
            // 4. a-1  b+2  c-1            
            // 5. a+1  b    c-1
            // 6. a-2  b+4  c-2
            // 7. a-2  b+3  c-1
            ABC t;
            var lst = new List<ABC>();
            if (b + 1 <= c - 1)
            {
                t = new ABC(a, b + 1, c - 1);
                if (t.F1 >= f) lst.Add(t);
            }
            if (a + 1 <= b - 1)
            {
                t = new ABC(a + 1, b - 1, c);
                if (t.F1 >= f) lst.Add(t);
            }
            if (a + 1 <= b - 2)
            {
                t = new ABC(a + 1, b - 2, c + 1);
                if (t.F1 >= f) lst.Add(t);
            }
            if ((a - 1 >= 1) && (b + 2 <= c - 1))
            {
                t = new ABC(a - 1, b + 2, c - 1);
                if (t.F1 >= f) lst.Add(t);
            }
            if ((a + 1 <= b) && (b <= c - 1))
            {
                t = new ABC(a + 1, b, c - 1);
                if (t.F1 >= f) lst.Add(t);
            }
            if ((a - 2 >= 1) && (b + 4 <= c - 2))
            {
                t = new ABC(a - 2, b + 4, c - 2);
                if (t.F1 >= f) lst.Add(t);
            }
            if ((a - 2 >= 1) && (b + 3 <= c - 1))
            {
                t = new ABC(a - 2, b + 3, c - 1);
                if (t.F1 >= f) lst.Add(t);
            }

            lst.Sort((x, y) => x.F1.CompareTo(y.F1));

            // we might have the situation, that the current f value and the smallest
            // new one are equal. In this situation, we must only thake the new one if 
            // its a-value is not smaller than the current a value            
            if ((lst[0].F1 == f) && (lst[0].A == a - 1))
                lst.RemoveAt(0);

            F1Value = lst[0].F1;
            return lst[0].AsLong();
        }        

        private void ExtendBuffer(ref Dictionary<long, LayerLimit> buffer, ref Dictionary<long, long> solutions, IEnumerable<long> partitions, long currentLimit)
        {
            sw.Restart();
            // add new partitions to buffer. these must NOT already exist in the buffer
            foreach (var part in partitions)
                buffer.Add(part, new LayerLimit());
            FTime = sw.ElapsedTicks;

            sw.Restart();
            // extend buffer
            foreach (var pair in buffer)
            {                
                long n = pair.Value.LayerIndex + 1;
                long box = pair.Key;                
                while (true)
                {
                    long a, b, c;
                    FromLong(box, out a, out b, out c);
                    
                    long f = F(n, a, b, c);
            
                    if (f <= currentLimit)
                    {
                        pair.Value.Set(n, f);
                    
                        if (solutions.ContainsKey(f))
                            solutions[f]++;
                        else
                            solutions.Add(f, 1);                        
                    }
                    else
                        break;
                    n++;
                }
            }
            LookupTime = sw.ElapsedTicks;
        }
        
        /// <summary>
        /// Returns F(n+1, a,b,c) - F(n, a,b,c)
        /// </summary>
        private long Fdelta(long n, long a, long b, long c)
        {
            return 4 * (a + b + c) + 8 * (n - 1);
        }

        #endregion    
    }

}