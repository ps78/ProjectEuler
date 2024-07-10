using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace ProjectEuler
{
    /// <summary>
    /// Solves https://projecteuler.net/problem=093
    /// By using each of the digits from the set, {1, 2, 3, 4}, exactly once, 
    /// and making use of the four arithmetic operations (+, −, *, /) and brackets/parentheses, 
    /// it is possible to form different positive integer targets.
    /// 
    /// For example,
    /// 
    /// 8 = (4 * (1 + 3)) / 2
    /// 14 = 4 * (3 + 1 / 2)
    /// 19 = 4 * (2 + 3) − 1
    /// 36 = 3 * 4 * (2 + 1)
    /// 
    /// Note that concatenations of the digits, like 12 + 34, are not allowed.
    /// 
    /// Using the set, {1, 2, 3, 4}, it is possible to obtain thirty-one different target numbers 
    /// of which 36 is the maximum, and each of the numbers 1 to 28 can be obtained before encountering
    /// the first non-expressible number.
    /// 
    /// Find the set of four distinct digits, a<b<c<d, 
    /// for which the longest set of consecutive positive integers, 1 to n, can be obtained, giving your answer as a string: abcd.
    /// </summary>
    public class Problem093 : EulerProblemBase
    {
        public Problem093() : base(93, "Arithmetic expressions", 0, 1258) { }

        #region Operations

        private delegate double Operation(double a, double b, ref bool hasError);

        private static double Add(double a, double b, ref bool hasError) => !hasError ? a + b : 0;

        private static double Multiply(double a, double b, ref bool hasError) => !hasError ? a * b : 0;

        private static double Subtract(double a, double b, ref bool hasError) => !hasError ? a - b : 0;

        private static double Divide(double a, double b, ref bool hasError)
        {
            if (b == 0)
            {
                hasError = true;
                return 0;
            }
            else
                return a / b;
        }

        #endregion
        #region Parser Trees

        private delegate int ParserTree(Tuple<int, int, int, int> val, Tuple<Operation, Operation, Operation> op, out bool isValid);

        /// <summary>
        /// Parser tree 
        ///        A
        ///      /   \
        ///     B     C
        ///    / \   / \
        ///   1   2 3   4
        /// </summary>
        /// <returns></returns>
        private static int TreeS(Tuple<int, int, int, int> val, Tuple<Operation, Operation, Operation> op, out bool isValid)
        {
            bool hasError = false;
            double result = op.Item2(val.Item1, val.Item2, ref hasError);
            double result2 = 0;
            if (!hasError)
                result2 = op.Item3(val.Item3, val.Item4, ref hasError);
            if (!hasError)
                result = op.Item1(result, result2, ref hasError);
            if (!hasError)
            {
                isValid = IsValid(result, out int res);
                return res;
            }
            isValid = false;
            return 0;
        }

        /// <summary>
        /// Parser tree 
        ///       A
        ///      / \
        ///     B   4
        ///    / \
        ///   C   3
        ///  / \
        /// 1   2
        /// </summary>
        /// <returns></returns>
        private static int TreeLL(Tuple<int, int, int, int> val, Tuple<Operation, Operation, Operation> op, out bool isValid)
        {
            bool hasError = false;            
            double result = op.Item3(val.Item1, val.Item2, ref hasError);
            if (!hasError)
                result = op.Item2(result, val.Item3, ref hasError);
            if (!hasError)
                result = op.Item1(result, val.Item4, ref hasError);                
            if (!hasError)
            {
                isValid = IsValid(result, out int res);
                return res;
            }
            isValid = false;
            return 0;
        }

        /// <summary>
        /// Parser tree 
        ///       A
        ///      / \
        ///     1   B
        ///        / \
        ///       2   C
        ///          / \
        ///         3   4
        /// </summary>
        /// <returns></returns>
        private static int TreeRR(Tuple<int, int, int, int> val, Tuple<Operation, Operation, Operation> op, out bool isValid)
        {
            bool hasError = false;
            double result = op.Item3(val.Item3, val.Item4, ref hasError);
            if (!hasError)
                result = op.Item2(val.Item2, result, ref hasError);
            if (!hasError)
                result = op.Item1(val.Item1, result, ref hasError);
            if (!hasError)
            {
                isValid = IsValid(result, out int res);
                return res;
            }
            isValid = false;
            return 0;
        }

        /// <summary>
        /// Parser tree 
        ///       A
        ///      / \
        ///     1   B
        ///        / \
        ///       C   4
        ///      / \
        ///     2   3
        /// </summary>
        /// <returns></returns>
        private static int TreeRL(Tuple<int, int, int, int> val, Tuple<Operation, Operation, Operation> op, out bool isValid)
        {
            bool hasError = false;
            double result = op.Item3(val.Item2, val.Item3, ref hasError);
            if (!hasError)
                result = op.Item2(result, val.Item4, ref hasError);
            if (!hasError)
                result = op.Item1(val.Item1, result, ref hasError);
            if (!hasError)
            {
                isValid = IsValid(result, out int res);
                return res;
            }
            isValid = false;
            return 0;
        }

        /// <summary>
        /// Parser tree 
        ///       A
        ///      / \
        ///     B   1
        ///    / \
        ///   2   C   
        ///      / \
        ///     3   4
        /// </summary>
        /// <returns></returns>
        private static int TreeLR(Tuple<int, int, int, int> val, Tuple<Operation, Operation, Operation> op, out bool isValid)
        {
            bool hasError = false;
            double result = op.Item3(val.Item3, val.Item4, ref hasError);
            if (!hasError)
                result = op.Item2(val.Item2, result, ref hasError);
            if (!hasError)
                result = op.Item1(result, val.Item1, ref hasError);
            if (!hasError)
            {
                isValid = IsValid(result, out int res);
                return res;
            }
            isValid = false;
            return 0;
        }

        #endregion
        #region Helper methods
        private static bool IsValid(double result, out int res)
        {
            if (result - Math.Floor(result) > 0.000001)
            {
                res = 0;
                return false;
            }
            res = (int)result;
            if (res <= 0)
            {
                res = 0;
                return false;
            }
            return true;
        }

        /// <summary>
        /// returns all combinations of 4 distinct elements out of digits, order is not relevant
        /// </summary>
        private IEnumerable<Tuple<int, int, int, int>> CreateDigitCombinations(int[] digits)
        {
            int n = digits.Length;
            for (int p1 = 0; p1 < n; p1++)
                for (int p2 = p1 + 1; p2 < n; p2++)
                    for (int p3 = p2 + 1; p3 < n; p3++)
                        for (int p4 = p3 + 1; p4 < n; p4++)
                            yield return new Tuple<int, int, int, int>(digits[p1], digits[p2], digits[p3], digits[p4]);
        }        

        /// <summary>
        /// creates all permutions of the given 4 digits
        /// </summary>
        private IEnumerable<Tuple<int, int, int, int>> CreateDigitPermutations(Tuple<int, int, int, int> d)
        {
            var digits = new int[] { d.Item1, d.Item2, d.Item3, d.Item4 };
            for (int p1 = 0; p1 < 4; p1++)
                for (int p2 = 0; p2 < 4; p2++)
                    if (p2 != p1)
                        for (int p3 = 0; p3 < 4; p3++)
                            if (p3 != p2 && p3 != p1)
                                for (int p4 = 0; p4 < 4; p4++)
                                    if (p4 != p3 && p4 != p2 && p4 != p1)
                                        yield return new Tuple<int, int, int, int>(digits[p1], digits[p2], digits[p3], digits[p4]);
        }

        private IEnumerable<Tuple<Operation, Operation, Operation>> CreateOpsPermutations(Operation[] ops)
        {            
            int n = ops.Length;
            for (int p1 = 0; p1 < n; p1++)
                for (int p2 = 0; p2 < n; p2++)
                    for (int p3 = 0; p3 < n; p3++)
                        yield return new Tuple<Operation, Operation, Operation>(ops[p1], ops[p2], ops[p3]);
        }

        private string FormatExpression(ParserTree tree, Tuple<int, int, int, int> digits, Tuple<Operation, Operation, Operation> ops, int result)
        {
            string[] o = (new Operation[] { ops.Item1, ops.Item2, ops.Item3 }).ToList().Select((op) => 
             {
                 if (op == Add) return "+";
                 else if (op == Subtract) return "-";
                 else if (op == Multiply) return "*";
                 else if (op == Divide) return "/";
                 return "";
 
             }).ToArray();
            
            if (tree == TreeS)
                return string.Format("( {0} {1}   {2}) {3} ({4}   {5} {6})   = {7}", digits.Item1, o[1], digits.Item2, o[0], digits.Item3, o[2], digits.Item4, result);
            if (tree == TreeLL)
                return string.Format("(({0} {1}   {2}) {3}  {4})  {5} {6}    = {7}", digits.Item1, o[2], digits.Item2, o[1], digits.Item3, o[0], digits.Item4, result);
            if (tree == TreeLR)
                return string.Format("( {0} {1} ( {2}  {3}  {4})) {5} {6}    = {7}", digits.Item2, o[1], digits.Item3, o[2], digits.Item4, o[0], digits.Item1, result);
            if (tree == TreeRR)
                return string.Format("( {0} {1} ( {2}  {3} ({4}   {5} {6}))) = {7}", digits.Item1, o[0], digits.Item2, o[1], digits.Item3, o[2], digits.Item4, result);
            if (tree == TreeRL)
                return string.Format("  {0} {1} (({2}  {3}  {4})  {5} {6})   = {7}", digits.Item1, o[0], digits.Item2, o[2], digits.Item3, o[1], digits.Item4, result);
            return "";
        }

        #endregion
        public override long Solve(long n)
        {
            var digitCombs = CreateDigitCombinations(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 }).ToList();
            var opsPermutations = CreateOpsPermutations(new Operation[] { Add, Subtract, Multiply, Divide }).ToList();
            var parserTrees = new ParserTree[] { TreeS, TreeLL, TreeLR, TreeRR, TreeRL };

            //Console.WriteLine("{0} ops permuations", opsPermutations.Count());
            //Console.WriteLine("{0} digit combinations", digitCombs.Count());

            int bestConseqCount = 0;
            string bestTuple = "";

            foreach (var digitComb in digitCombs)
            {
                var resVector = new bool[6562]; // the maximum number can be obtained as 9x9x9x9 = 6561
                int maxResult = 0;

                var digitPermutations = CreateDigitPermutations(digitComb).ToList();
                foreach (var opsPerm in opsPermutations)
                    foreach (var digitPerm in digitPermutations)                    
                        foreach (var tree in parserTrees)
                        {
                            int result = tree(digitPerm, opsPerm, out bool isValid);
                            if (isValid)
                            {
                                resVector[result] = true;
                                maxResult = Math.Max(maxResult, result);
                            }
                            //Console.WriteLine("{0}   Valid: {1}", FormatExpression(tree, digitPerm, opsPerm, result), isValid);
                        }

                int consequtiveCount = 0;
                for (int i = 1; i < resVector.Length; i++)
                    if (resVector[i])
                        consequtiveCount++;
                    else
                        break;

                // Console.WriteLine("{0} consequtive: {1} max-value: {2}", string.Format("{0}{1}{2}{3}", digitComb.Item1, digitComb.Item2, digitComb.Item3, digitComb.Item4), consequtiveCount, maxResult);
                
                if (consequtiveCount > bestConseqCount)
                {
                    bestConseqCount = consequtiveCount;
                    bestTuple = string.Format("{0}{1}{2}{3}", digitComb.Item1, digitComb.Item2, digitComb.Item3, digitComb.Item4);
                    //Console.WriteLine("{0} - count = {1}", bestTuple, bestConseqCount);
                }                
            }
            
            return long.Parse(bestTuple);
        }        
    }
}
