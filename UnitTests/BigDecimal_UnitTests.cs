using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using NumberTheory;

namespace UnitTests;

public class BigDecimal_UnitTests
{
    [Theory(DisplayName = "BigDecimal: Constructor(d)")]
    [InlineData(1.0, true, 1.0, 0)]
    [InlineData(10.0, true, 1.0, 1)]
    [InlineData(0.01, true, 1.0, -2)]
    [InlineData(-3254.214, false, 3.254214, 3)]
    public void TestConstructorDouble(double d, bool sign, double mantissa, int exponent)
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
    public void TestConstructorMantissaExponent(double m, int e, bool sign, double mantissa, int exponent)
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
    public void TestConstructorSignMantissaExponent(bool s, double m, int e, bool sign, double mantissa, int exponent)
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
    public void TestToString(double d, string s)
    {
        new BigDecimal(d).ToString().Should().Be(s);
    }

    [Theory(DisplayName = "BigDecimal: ^")]
    [InlineData(true, 2, 0, 5, true, 3.2, 1)]
    public void TestOperatorPower(bool a_sign, double a_m, int a_exp, int n, bool c_sign, double c_m, int c_exp)
    {
        var result = new BigDecimal(a_sign, a_m, a_exp) ^ n;
        result.Should().Equals(new BigDecimal(c_sign, c_m, c_exp));
    }
}
