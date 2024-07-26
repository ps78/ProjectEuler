using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory;

public readonly struct BigDecimal
{
    public readonly bool Sign;
    public readonly double Mantissa;
    public readonly int Exponent;

    public double SignedMantissa => Sign ? Mantissa : -Mantissa;

    #region Constructors

    public BigDecimal(double d) : this(true, d, 0) { }
    
    public BigDecimal(double mantissa, int exponent) : this(true, mantissa, exponent) { }
    
    public BigDecimal(bool sign, double mantissa, int exponent)
    {
        (bool s, double m, int e) = Normalize(mantissa, exponent);
        
        Sign = sign ^ !s;
        Mantissa = m;
        Exponent = e;
    }

    #endregion
    #region private Methods

    private static (bool sign, double mantissa, int exponent) Normalize(double m, int e)
    {
        bool sign = m >= 0;
        double mantissa = Math.Abs(m);
        int exponent = e;
        if (mantissa < 1 || mantissa >= 10)
        {
            int log10 = (int)Math.Floor(Math.Log10(mantissa));
            double exp10 = Math.Pow(10, log10);

            mantissa /= exp10;
            exponent += log10;
        }
        return (sign, mantissa, exponent);
    }

    #endregion
    #region Public Methods and operators

    public string ToString(string? format)
    {
        string s = Sign ? "" : "-";
        s += format == null ? Mantissa.ToString() : Mantissa.ToString(format);
        s += $"E{Exponent}";
        return s;
    }

    public override string ToString() => ToString(null);

    public static BigDecimal operator+ (BigDecimal a, BigDecimal b)
    {
        throw new NotImplementedException();
    }

    public static BigDecimal operator^ (BigDecimal a, int n)
    {
        double m = a.Mantissa;
        int exp = a.Exponent;

        // square m as long as possible
        int k = 0;
        while (2 * k <= n)
        {
            m *= m;
            (_, m, int exp2) = Normalize(m, exp);
            exp = 2*exp + exp2;
            k *= 2;
        }

        // now calculated a^(2^i) with 2^i <= n
        // what remains is the exponent n-2^i
        for (int _ = exp; _ < n; _++)
        {
            m *= a.Mantissa;
            (bool s, m, int exp2) = Normalize(m, exp);
            exp = exp + a.Exponent + exp2;
        }

        return new BigDecimal(m, exp);
    }

    // Conversions to/from double
    public static explicit operator BigDecimal(double d) => new BigDecimal(d);
    public static explicit operator double(BigDecimal d) => (d.Sign ? 1 : -1) * d.Mantissa * Math.Pow(10, d.Exponent);

    #endregion
}
