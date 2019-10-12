using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace NumberTheory
{
    /// <summary>
    /// Represents a linear equation consisting of m equations with n unknowns of the form
    /// 
    /// an xn + an-1*xn-1 + ... + a2 x2 + a1 x1 + a0 = 0
    /// 
    /// in general, n=m to get one solution
    /// All an are BigIntegers
    /// 
    /// Solution not implemented. See Problem 101 for a potential implemtation
    /// </summary>
    public class LinearEquationSystem
    {
        public int N { get; }
        public int M { get; }

        private LinearEquation[] equation;
        public LinearEquation this[int index]
        {
            get => equation[index];
            set
            {
                if (value.N != N)
                    throw new InvalidOperationException("The number of coeffictions of all equations in the system must match");
                equation[index] = value;
            }
        }

        public LinearEquationSystem(int n, int m)
        {
            N = n;
            M = m;
            equation = new LinearEquation[M];
            for (int i = 0; i < m; i++)
                equation[i] = new LinearEquation(new BigInteger[N+1]);
        }
                
        public override string ToString()
        {
            var sb = new StringBuilder();
            for (int i  =0; i < M; i++)
            {
                sb.Append(equation[i].ToString());
                sb.Append("\n");
            }
            return sb.ToString();
        }

    }

    public class LinearEquation
    {        
        /// <summary>
        /// Maximum number of unknowns of the equation. Note that there are N+1 coefficients
        /// </summary>
        public int N { get; }

        /// <summary>
        /// Returns the actual number of unknowns
        /// </summary>
        public int Unknowns { get => a.Count(i => !i.IsZero) - 1; }

        private BigInteger[] a;

        /// <summary>
        /// Coefficients. A[0] is the constant coefficient
        /// </summary>
        public BigInteger this [int index]
        {
            get => a[index];
            set { a[index] = value; }
        }
        

        public LinearEquation(IEnumerable<BigInteger> coefficients)
        {
            a = coefficients.ToArray();
            BigInteger[] c = new BigInteger[coefficients.Count()];
            int j = 0;
            foreach (var i in coefficients)
                c[j++] = i;
            N = a.Length - 1;
        }

        /// <summary>
        /// Multiplies all coefficients by the given factor x
        /// </summary>        
        public static LinearEquation operator *(LinearEquation e, BigInteger x)
        {
            var b = new BigInteger[e.N + 1];
            for (int i = 0; i <= e.N; i++)
                b[i] = e[i] * x;
            return new LinearEquation(b);
        }

        /// <summary>
        /// Creates a new linear equation by subtracting f from e
        /// </summary>
        public static LinearEquation operator -(LinearEquation e, LinearEquation f)
        {
            if (e.N != f.N)
                throw new InvalidOperationException("The equations must have the same number of coefficients");

            var b = new BigInteger[e.N + 1];
            for (int i = 0; i <= e.N; i++)
                b[i] = e[i] - f[i];
            return new LinearEquation(b);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            for (int i = N; i >= 1; i--)
                if (!a[i].IsZero)
                {
                    if ((sb.Length > 0) && (a[i] > 0))
                        sb.Append("+ ");

                    if (a[i] > 1)
                        sb.Append(a[i].ToString() + "*");
                    else if (a[i] < -1)
                        sb.Append("- " + (-a[i]).ToString() + "*");
                    else if (a[i] == -1)
                        sb.Append("- ");

                    sb.Append("x");
                    sb.Append(i);
                    sb.Append(" ");
                }
            if (sb.Length == 0)
                sb.Append("0 ");
            sb.Append("= " + (-a[0]).ToString());

            return sb.ToString();
        }
    }

}
