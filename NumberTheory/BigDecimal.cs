using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory;

public readonly struct BigDecimal : IEquatable<BigDecimal>, IComparable<BigDecimal>
{
    public readonly bool Sign;
    public readonly decimal Mantissa;
    public readonly int Exponent;

    public decimal SignedMantissa => Sign ? Mantissa : -Mantissa;

    public static BigDecimal Zero => new(true, 0, 0);

    public static BigDecimal One => new(true, 1, 0);

    #region Constructors

    public BigDecimal(decimal d) : this(true, d, 0) { }
    
    public BigDecimal(decimal mantissa, int exponent) : this(true, mantissa, exponent) { }
    
    public BigDecimal(bool sign, decimal mantissa, int exponent)
    {
        (bool s, decimal m, int e) = Normalize(mantissa, exponent);
        
        Sign = sign ^ !s;
        Mantissa = m;
        Exponent = e;
    }

    #endregion
    #region private Methods

    private static (bool sign, decimal mantissa, int exponent) Normalize(decimal m, int e)
    {
        if (m == 0)
            return (true, 0, 0);
        else
        {
            bool sign = m >= 0;
            decimal mantissa = Math.Abs(m);
            int exponent = e;
            if (mantissa < 1 || mantissa >= 10)
            {
                int log10 = (int)Math.Floor(Math.Log10((double)mantissa));
                decimal exp10 = (decimal)Math.Pow(10, log10);

                mantissa /= exp10;
                exponent += log10;
            }
            return (sign, mantissa, exponent);
        }
    }

    #endregion
    #region Public Methods and operators

    public string ToString(string? format)
    {
        return (format == null ? SignedMantissa.ToString() : SignedMantissa.ToString(format))
                + $"E{Exponent}";
    }

    public override string ToString() => ToString(null);

    public bool Equals(BigDecimal other)
    {
        if (Mantissa == 0 && other.Mantissa == 0)
            return true;
        else
            return Sign == other.Sign && Mantissa == other.Mantissa && Exponent == other.Exponent;
    }

    public override bool Equals([NotNullWhen(true)] object? obj)
    {
        if (obj is BigDecimal dec)
            return Equals(dec);
        else
            return false;
    }

    public override int GetHashCode() =>
        Sign.GetHashCode() ^ Mantissa.GetHashCode() ^ Exponent.GetHashCode();

    public int CompareTo(BigDecimal other)
    {
        if (Equals(other)) 
            return 0;
        else if (Sign != other.Sign)
            return Sign ? 1 : -1;
        else
        {
            if (Exponent == other.Exponent)
            {
                if (Sign)
                    return Mantissa > other.Mantissa ? 1 : -1;
                else
                    return Mantissa > other.Mantissa ? -1 : 1;
            }
            else
            {
                if (Sign)
                    return Exponent > other.Exponent ? 1 : -1;
                else
                    return Exponent > other.Exponent ? -1 : 1;
            }
        }
    }

    public static BigDecimal operator+ (BigDecimal a, BigDecimal b)
    {
        throw new NotImplementedException();
    }

    public static BigDecimal operator^ (BigDecimal a, int n)
    {

        if (n == 0)
        {
            if (a.Equals(0))
                throw new ArithmeticException("Attempted to calculate 0^0");
            else
                return BigDecimal.Zero;
        }
        else if (n == 1)
        {
            return a;
        }
        else if (n < 0)
        {
            throw new NotImplementedException("Negative exponents are not implemented yet");
        }
        else
        {
            decimal m = Math.Abs(a.Mantissa);
            int exp = a.Exponent;

            string binaryExp = Convert.ToString(n, 2);

            decimal result_m = 1;
            int result_exp = 0;
            
            for (int i = binaryExp.Length -1; i >= 0; i--)
            {                
                if (binaryExp[i] == '1')
                    (_, result_m, result_exp) = Normalize(result_m * m, result_exp + exp);
                
                m *= m;
                (_, m, exp) = Normalize(m, 2 * exp);
            }

            bool s = n % 2 == 0 ? true : a.Sign;

            return new(s, result_m, result_exp);
        }
    }

    public static BigDecimal operator *(BigDecimal a, BigDecimal b)
        => new (a.Sign == b.Sign, a.Mantissa * b.Mantissa, a.Exponent + b.Exponent);

    public static BigDecimal operator *(BigDecimal a, decimal d)
        => new (a.Sign, a.Mantissa * d, a.Exponent);
    
    public static BigDecimal operator *(decimal d, BigDecimal a) => a * d;

    public static BigDecimal operator /(BigDecimal a, decimal d) => a * (1.0m / d);

    public static BigDecimal operator -(BigDecimal a, BigDecimal b)
    {
        // Easy case: a and b have the same exponent, we can focus on the mantissa(+sign) only
        if (a.Exponent == b.Exponent)
        {
            return new BigDecimal(a.SignedMantissa - b.SignedMantissa, a.Exponent);
        }
        else
        {
            int biggerExp = Math.Max(a.Exponent, b.Exponent);
            int smallerExp = Math.Min(a.Exponent, b.Exponent);

            // we only have 28 significant digits. if the exponents differ by more than 28, we simply ignore
            // the smaller term
            if (biggerExp - smallerExp >= 28)
            {
                if (a.Exponent > b.Exponent)
                    return a;
                else
                    return new BigDecimal(!b.Sign, b.Mantissa, b.Exponent);
            }
            // if a and b are not too far appart, we need to conver the smaller term
            // to the base of the bigger one
            else
            {
                decimal deltaExp = (decimal)Math.Pow(10, biggerExp - smallerExp);
                if (a.Exponent > b.Exponent)
                    return new BigDecimal(a.SignedMantissa - b.SignedMantissa / deltaExp, biggerExp);
                else
                    return new BigDecimal(a.SignedMantissa / deltaExp - b.SignedMantissa, biggerExp);
            }
        }
    }

    // Conversions to/from decimal
    public static explicit operator BigDecimal(decimal d) => new (d);
    public static explicit operator decimal(BigDecimal d) 
        => (d.Sign ? 1 : -1) * d.Mantissa * (decimal)Math.Pow(10, d.Exponent);

    #endregion
}
