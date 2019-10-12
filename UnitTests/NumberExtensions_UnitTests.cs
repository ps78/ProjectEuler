using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using System.Numerics;
using NumberTheory;
using Xunit;
using FluentAssertions;
using Xunit.Abstractions;

namespace UnitTests
{ 
    public class NumberExtensions_UnitTests
    {
        #region Setup / Helper

        private readonly ITestOutputHelper testOutput;

        public NumberExtensions_UnitTests(ITestOutputHelper testOutputHelper)
        {
            testOutput = testOutputHelper;
        }
        #endregion
        #region Test Methods
        
        [Fact(DisplayName = "Power()")]
        public void TestPower()
        {
            // define the tests where <b, e, r> with b^e == r

            // ulong
            var tests_ulong = new Tuple<ulong, ulong, ulong>[] {
                new Tuple<ulong, ulong, ulong>(0, 1, 0),
                new Tuple<ulong, ulong, ulong>(2, 0, 1),
                new Tuple<ulong, ulong, ulong>(13, 1, 13),
                new Tuple<ulong, ulong, ulong>(29, 12, 353814783205469041L),
                new Tuple<ulong, ulong, ulong>(63248, 3, 253011575508992L),
                new Tuple<ulong, ulong, ulong>(2, 60, 1152921504606846976L),
                new Tuple<ulong, ulong, ulong>(2, 63, 9223372036854775808L),
                new Tuple<ulong, ulong, ulong>(13, 17, 8650415919381337933)
            };
            foreach (var t in tests_ulong)
                t.Item1.Power(t.Item2).Should().Be(t.Item3);

            Assert.ThrowsAny<Exception>(() => ((ulong)0).Power(0));

            // long 
            var tests_long = new Tuple<long, long, long>[] {
                new Tuple<long, long, long>(2, 0, 1),
                new Tuple<long, long, long>(13, 1, 13),
                new Tuple<long, long, long>(29, 12, 353814783205469041),
                new Tuple<long, long, long>(63248, 3, 253011575508992),
                new Tuple<long, long, long>(2, 60, 1152921504606846976),
                new Tuple<long, long, long>(13, 17, 8650415919381337933)
            };
            foreach (var t in tests_ulong)
                t.Item1.Power(t.Item2).Should().Be(t.Item3);
                        
            Assert.ThrowsAny<Exception>(() => ((long)0).Power(0));
        }

        [Fact(DisplayName = "BigPower()")]
        public void TestBigPower()
        {
            var tests = new Tuple<BigInteger, int, BigInteger>[] {
                new Tuple<BigInteger, int, BigInteger>(0, 1, 0),
                new Tuple<BigInteger, int, BigInteger>(2, 0, 1),
                new Tuple<BigInteger, int, BigInteger>(13, 1, 13),
                new Tuple<BigInteger, int, BigInteger>(29, 12, 353814783205469041L),
                new Tuple<BigInteger, int, BigInteger>(63248, 3, 253011575508992L),
                new Tuple<BigInteger, int, BigInteger>(2, 60, 1152921504606846976L),
                new Tuple<BigInteger, int, BigInteger>(2, 63, 9223372036854775808L),
                new Tuple<BigInteger, int, BigInteger>(13, 17, 8650415919381337933),
                new Tuple<BigInteger, int, BigInteger>(115, 14,BigInteger.Parse("70757057644889830377197265625"))
            };
            foreach (var t in tests)
                t.Item1.BigPower(t.Item2).Should().Be(t.Item3);

            Assert.ThrowsAny<Exception>(() => (BigInteger.Zero).BigPower(0));            
        }
     
        [Fact(DisplayName = "ModPower()")]
        public void TestModPower()
        {
            var tests = new Tuple<ulong, ulong, ulong, ulong>[] {
                new Tuple<ulong, ulong, ulong, ulong>(2, 0, 20, 1),
                new Tuple<ulong, ulong, ulong, ulong>(101, 1, 13, 10),
                new Tuple<ulong, ulong, ulong, ulong>(3, 63, 101, 63),
                new Tuple<ulong, ulong, ulong, ulong>(321324, 621343, 101, 64),
                new Tuple<ulong, ulong, ulong, ulong>(1000000, 2, 333333333333L, 1),
                new Tuple<ulong, ulong, ulong, ulong>(104729, 224737, 7919, 4996),
                new Tuple<ulong, ulong, ulong, ulong>(17, 100, 22801763489L, 20096797933L),
                new Tuple<ulong, ulong, ulong, ulong>(252097800623L, 384489816343L, 15482863, 57478)                
            };
            foreach (var t in tests)
                t.Item1.ModPower(t.Item2, t.Item3).Should().Be(t.Item4);
        }
        
        [Fact(DisplayName = "Factorial()")]
        public void TestFactorial()
        {
            // 10!
            ((long)10).Factorial().Should().Be(3628800);
            ((ulong)10).Factorial().Should().Be(3628800);

            // 0! 
            ((long)0).Factorial().Should().Be(1);
            ((ulong)0).Factorial().Should().Be(1);
        }

        [Fact(DisplayName = "BigFactorial()")]
        public void TestBigFactorial()
        {
            // 99!
            var factorialResult = BigInteger.Parse("933262154439441526816992388562667004907159682643816214685929638952175999932299156089414639761565182862536979208272237582511852109168640000000000000000000000");
            ((long)99).BigFactorial().Should().Be(factorialResult);
            ((ulong)99).BigFactorial().Should().Be(factorialResult);
            (new BigInteger(99)).BigFactorial().Should().Be(factorialResult);

            // 0!
            ((long)0).BigFactorial().Should().Be(1);
            ((ulong)0).BigFactorial().Should().Be(1);
            (new BigInteger(0)).BigFactorial().Should().Be(1);
        }


        #endregion
    }
}
