using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NumberTheory;
using Xunit.Abstractions;

namespace UnitTests;

public class BigDecimal_UnitTests(ITestOutputHelper output) : UnitTestBase(output)
{
    [Theory(DisplayName = "BigDecimal: Constructor(d)")]
    [InlineData(1.0, true, 1.0, 0)]
    [InlineData(10.0, true, 1.0, 1)]
    [InlineData(0.01, true, 1.0, -2)]
    [InlineData(-3254.214, false, 3.254214, 3)]
    public void TestConstructorDouble(decimal d, bool sign, decimal mantissa, int exponent)
    {
        var bd = new BigDecimal(d);

        bd.Sign.Should().Be(sign);
        bd.Mantissa.Should().Be(mantissa);
        bd.Exponent.Should().Be(exponent); 
    }

    [Theory(DisplayName = "BigDecimal: Constructor(m,e)")]
    [InlineData(1.0, 0, true, 1.0, 0)]
    [InlineData(-65473, 3, false, 6.5473, 7)]
    [InlineData(-0.00547, -1, false, 5.47, -4)]
    public void TestConstructorMantissaExponent(decimal m, int e, bool sign, decimal mantissa, int exponent)
    {
        var bd = new BigDecimal(m, e);

        bd.Sign.Should().Be(sign);
        bd.Mantissa.Should().Be(mantissa);
        bd.Exponent.Should().Be(exponent);
    }

    [Theory(DisplayName = "BigDecimal: Constructor(s,m,e)")]
    [InlineData(true, 5.2, 4, true, 5.2, 4)]
    [InlineData(false, 5.2, 4, false, 5.2, 4)]
    [InlineData(true, -5.2, 4, false, 5.2, 4)]
    [InlineData(false, -5.2, 4, true, 5.2, 4)]
    public void TestConstructorSignMantissaExponent(bool s, decimal m, int e, bool sign, decimal mantissa, int exponent)
    {
        var bd = new BigDecimal(s, m, e);

        bd.Sign.Should().Be(sign);
        bd.Mantissa.Should().Be(mantissa);
        bd.Exponent.Should().Be(exponent);
    }


    [Theory(DisplayName = "BigDecimal: ToString()")]
    [InlineData(3.14, "3.14E0")]
    [InlineData(-3.14, "-3.14E0")]
    [InlineData(50003, "5.0003E4")]
    [InlineData(-0.007, "-7E-3")]
    public void TestToString(decimal d, string s)
    {
        new BigDecimal(d).ToString().Should().Be(s);
    }

    [Theory(DisplayName = "BigDecimal: Equals()")]
    [InlineData(false, 3, 7, false, 30, 6, true)]
    [InlineData(true, 1, 0, false, 1, 0, false)]
    [InlineData(false, 3.54, -7, false, 0.0354, -5, true)]
    public void TestEquals(bool s1, decimal m1, int exp1, bool s2, decimal m2, int exp2, bool isequal)
    {
        var a = new BigDecimal(s1, m1, exp1);
        var b = new BigDecimal(s2, m2, exp2);
        a.Equals(b).Should().Be(isequal);
    }

    [Fact(DisplayName = "BigDecimal: Compare")]
    public void TestCompare()
    {
        var rand = new Random();
        var numsDouble = new List<(int, decimal)>();

        // generate 10k random numbers in the range -150..+150
        for (int i = 1; i <= 10000; i++)
            numsDouble.Add((i, (decimal)rand.NextDouble() * 300 - 150));

        // copy the same list as BigDecimal
        var numsBigDec = numsDouble.Select(x => (x.Item1, new BigDecimal(x.Item2))).ToList();

        // ordering both lists should result in the same sequence
        var orderedDoubles = numsDouble.OrderBy(x => x.Item2).Select(x => x.Item1).ToArray();
        var orderedBigDec = numsBigDec.OrderBy(x => x.Item2).Select(x => x.Item1).ToArray();

        orderedBigDec.Should().Equal(orderedDoubles);
    }

    [Theory(DisplayName = "BigDecimal: Operator BigDecimal^int")]
    [InlineData(false, 5, 0, 21, false, 4.76837158203125, 14)]
    [InlineData(true, 2, 0, 5, true, 3.2, 1)]
    [InlineData(false, 7, 0, 2, true, 4.9, 1)]
    [InlineData(true, -0.3, 0, 7, true, -0.0002187,0)]
    public void TestOperatorPower(bool a_sign, decimal a_m, int a_exp, int n, bool c_sign, decimal c_m, int c_exp)
    {
        var result = new BigDecimal(a_sign, a_m, a_exp) ^ n;
        result.Should().Be(new BigDecimal(c_sign, c_m, c_exp));
    }

    [Theory(DisplayName = "BigDecimal: Operator BigDecimal*BigDecimal")]
    [InlineData(true, 2, 3, true, 7, 1, true, 140000, 0)]
    [InlineData(false, 77, -5, true, 13, 1, false, 0.1001, 0)]
    public void TestOperatorMultiplyBigDec(
        bool a_sign, decimal a_m, int a_exp,
        bool b_sign, decimal b_m, int b_exp,
        bool c_sign, decimal c_m, int c_exp)
    {
        var a = new BigDecimal(a_sign, a_m, a_exp);
        var b = new BigDecimal(b_sign, b_m, b_exp);
        var c = new BigDecimal(c_sign, c_m, c_exp);

        (a * b).Should().Be(c);
        (b * a).Should().Be(c);
    }

    [Theory(DisplayName = "BigDecimal: Operator BigDecimal*Double")]
    [InlineData(true, 2, 3, 70.0, true, 140000, 0)]
    [InlineData(false, 77, -5, 130.0, false, 0.1001, 0)]
    public void TestOperatorMultiplyDecimal(
        bool a_sign, decimal a_m, int a_exp,
        decimal b,
        bool c_sign, decimal c_m, int c_exp)
    {
        var a = new BigDecimal(a_sign, a_m, a_exp);
        var c = new BigDecimal(c_sign, c_m, c_exp);

        (a * b).Should().Be(c);
        (b * a).Should().Be(c);
    }

    [Theory(DisplayName = "BigDecimal: Operator Minus")]
    [InlineData(true, 3.3, 4, true, 1.1, 4, true, 2.2, 4)]
    [InlineData(false, 3.3, 4, true, 1.1, 4, false, 4.4, 4)]
    [InlineData(false, 3.3, 4, false, 1.1, 4, false, 2.2, 4)]
    [InlineData(true, 3.3, 4, false, 1.1, 4, true, 4.4, 4)]
    [InlineData(true, 33000, 0, true, 11, 0, true, 32989, 0)]
    [InlineData(true, 33000, 0, false, 11, 0, true, 33011, 0)]
    [InlineData(false, 33000, 0, true, 11, 0, false, 33011, 0)]
    [InlineData(false, 33000, 0, false, 11, 0, false, 32989, 0)]
    [InlineData(false, 11, 0, false, 33000, 0, true, 32989, 0)]
    [InlineData(true, 0.53, 0, true, 32.6, 0, false, 32.07, 0)]
    public void TestOperatorMinus(bool a_sign, decimal a_m, int a_exp,
                                  bool b_sign, decimal b_m, int b_exp,
                                  bool c_sign, decimal c_m, int c_exp)
    {
        var a = new BigDecimal(a_sign, a_m, a_exp);
        var b = new BigDecimal(b_sign, b_m, b_exp);
        var c = new BigDecimal(c_sign, c_m, c_exp);

        (a - b).Should().Be(c);
    }
}
