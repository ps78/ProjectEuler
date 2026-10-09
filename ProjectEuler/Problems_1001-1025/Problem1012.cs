namespace ProjectEuler;

/// <summary>
/// https://projecteuler.net/problem=1012
///
/// In RPS(n) both players secretly pick a number from 1..n. Equal numbers are a draw. Otherwise the
/// smaller number wins if the difference is odd and the larger number wins if it is even; a winner
/// who picked m receives 2m-1 dollars from the loser.
/// P(n) is the probability that a player using the unique Nash equilibrium strategy of RPS(n)
/// picks n, e.g. P(3) = 1/9 and P(4) = 1/5. S(N) = P(3) + ... + P(N), S(10) ~ 1.1546112276,
/// S(100) ~ 4.8779925686. Find S(10^5) rounded to ten places after the decimal point.
///
/// REASONING
///
/// 1) The game
///    Write w(x) = 2x-1 for the prize of x. For x != y, "x beats y" iff x<y and y-x is odd, or
///    x > y and x-y is even. The payoff matrix A(x,y) = w(x) if x beats y, -w(y) if y beats x and 0
///    on the diagonal is skew-symmetric, so RPS(n) is a symmetric zero-sum game with value 0.
///    Therefore p is the equilibrium iff p >= 0, sum(p) = 1 and (Ap)(y) <= 0 for every pure
///    strategy y, with equality on the support of p.
///
/// 2) Shape of the equilibrium (found by solving small cases exactly, verified in Test())
///    The support is always a block of consecutive numbers [a, n] at the top, of odd length
///    k = 2m+1. As n grows, m stays the same for a while and then grows by one. At such a transition
///    n, the equilibrium is still the one for n-1, so n is never played and P(n) = 0.
///    On the support (Ap) = 0, so p is the kernel vector of the k x k block of A. A skew-symmetric
///    matrix of odd order is singular, and its kernel is spanned by the signed sub-Pfaffians
///        x_i = (-1)^i Pf(block with row/column i removed).
///    All prizes in the block are W, W+2, ..., W+4m with W = w(a), so every entry is linear in W and
///    every Pfaffian is a polynomial in W:
///      - x_top    = Pf([a, n-1])          =: U_m(W),  of degree m, monic
///      - sum(x_i) = Pf(A bordered by ones) =: V_m(W),  of degree m
///    which gives P(n) = U_m(W) / V_m(W).
///    For the strategy a-1 just below the block, expanding Pf([a-1, n]) along its first row gives
///    (A x)(a-1) = Pf([a-1, n]) = U_(m+1)(W-2). So the block stays an equilibrium while
///    U_(m+1)(W-2) &lt;= 0. When that becomes positive, a-1 would gain against the block, and this is
///    exactly the transition to m+1.
///
/// 3) Closed forms (found by exact rational computations of U and V for m <= 10, verified for all
///    n <= 1000 in Test() by an independent equilibrium computation)
///      V_m(W) = (2m+1) * prod_(j=0..m-1) (W + 2 + 4j)
///    The factors are the prizes of the numbers a+1, a+3, ..., n-1 in the block. Writing U_m in
///    partial fractions with respect to these factors gives
///      U_m(W) / prod_j (W + 2 + 4j) = 1 - sum_(j=0..m-1) rho(m,j) / (W + 2 + 4j)
///      rho(m,j) = 2/3 * (2m^2+1) * (2j+1) * c(j) * c(m-1-j),   c(j) = binomial(2j, j) / 4^j.
///    With F_m(a) = 1 - sum_j rho(m,j) / w(a+2j+1) this leads to
///      P(n) = F_m(n-2m) / (2m+1)        while the block [n-2m, n] is the equilibrium,
///      transition at n  <=  F_(m+1)(n-2m-1) > 0;   then P(n) = 0 and m becomes m+1.
///    Examples:
///      - m = 1: P(n) = (1 - 2/w(n-1)) / 3, so P(3) = 1/9 and P(4) = 1/5. The kernel is
///        (w(a+1), w(a+2), w(a)), i.e. classic Rock Paper Scissors.
///      - The first transition is at n = 8, since F_2(4) = 1 - 3/9 - 9/13 &lt; 0 but
///        F_2(5) = 1 - 3/11 - 9/15 > 0. The following ones are at 21, 47, 88, 150, ...
///    Since sum_j (2j+1) c(j) c(m-1-j) = m, the largest root of U_m is about (4/3) m^3, so m grows
///    like n^(1/3). For n = 10^5 the support has 107 numbers (m = 53).
///
/// 4) Cost and precision
///    Every P(n) takes O(m) operations, about 10^7 in total, which runs in milliseconds. All
///    rho(m,j) are positive, so F is 1 minus a sum of positive terms, and its absolute error is a
///    few ulp (~1e-16). The terms are added with Kahan summation. A 50-digit reference
///    computation gives S(10^5) = 75.00739367538100..., and the double result is within ~2e-12.
///    Like Problem 307, Solve() returns the rounded answer without its decimal point, i.e.
///    round(S(n) * 10^10).
/// </summary>
public class Problem1012 : EulerProblemBase
{
    /// <summary>
    /// Test() checks the closed form against a direct equilibrium computation for all n up to this
    /// </summary>
    private const int TestLimit = 1000;

    public Problem1012() : base(1012, "Rock Paper Scissors", 100_000, 750_073_936_754) { }

    public override bool Test()
    {
        var equilibria = Equilibria(TestLimit).ToArray();

        // examples from the problem statement
        if (Math.Abs(equilibria[0].P - 1.0 / 9) > 1e-15 || Math.Abs(equilibria[1].P - 1.0 / 5) > 1e-15)
            return false;
        if (Solve(10) != 11_546_112_276 || Solve(100) != 48_779_925_686)
            return false;

        // the closed form (support, transitions and P(n)) must describe the actual equilibria
        return equilibria.All(IsEquilibrium);
    }

    /// <summary>
    /// S(n) rounded to 10 decimal places, returned as an integer (i.e. S(n) * 10^10)
    /// </summary>
    public override long Solve(long n) => (long)Math.Round(S(n) * 1e10);

    /// <summary>
    /// S(n) = P(3) + ... + P(n), with Kahan summation
    /// </summary>
    private static double S(long n)
    {
        double sum = 0, compensation = 0;
        foreach (var e in Equilibria(n))
        {
            double y = e.P - compensation;
            double t = sum + y;
            compensation = (t - sum) - y;
            sum = t;
        }
        return sum;
    }

    /// <summary>
    /// Equilibrium of RPS(N): it is supported on the numbers Low..High, and P is the probability of N
    /// </summary>
    private readonly record struct Equilibrium(long N, long Low, long High, double P);

    /// <summary>
    /// Equilibria of RPS(n) for n = 3..maxN, via the closed form (see 3) above). m is the current
    /// half-width: in regime m the support is [n-2m, n].
    /// </summary>
    private static IEnumerable<Equilibrium> Equilibria(long maxN)
    {
        int m = 1;
        double[] rho = Residues(m), rhoNext = Residues(m + 1);

        for (long n = 3; n <= maxN; n++)
        {
            long a = n - 2 * m; // lowest number of the block [a, n]

            if (a > 1 && F(rhoNext, a - 1) > 0)
            {
                // strategy a-1 would beat the block [a, n]: transition. The equilibrium of n-1, [a-1, n-1],
                // remains the equilibrium, n itself is not played. From n+1 on the block [n-2m-2, n+1] is used.
                yield return new Equilibrium(n, a - 1, n - 1, 0.0);
                m++;
                rho = rhoNext;
                rhoNext = Residues(m + 1);
            }
            else
                yield return new Equilibrium(n, a, n, F(rho, a) / (2 * m + 1));
        }
    }

    /// <summary>
    /// rho(m,j) = 2/3 (2m^2+1) (2j+1) c(j) c(m-1-j) for j = 0..m-1, with c(j) = binomial(2j,j) / 4^j
    /// </summary>
    private static double[] Residues(int m)
    {
        var c = new double[m];
        c[0] = 1;
        for (int j = 1; j < m; j++)
            c[j] = c[j - 1] * (2 * j - 1) / (2 * j);

        double factor = 2.0 / 3 * (2.0 * m * m + 1);
        var rho = new double[m];
        for (int j = 0; j < m; j++)
            rho[j] = factor * (2 * j + 1) * c[j] * c[m - 1 - j];
        return rho;
    }

    /// <summary>
    /// F_m(a) = 1 - sum_j rho(m,j) / w(a+2j+1) with w(x) = 2x-1, i.e. (2m+1) times the probability of the
    /// top number a+2m when the block [a, a+2m] is played. m is given implicitly by rho.Length.
    /// </summary>
    private static double F(double[] rho, long a)
    {
        double sum = 0;
        for (int j = 0; j < rho.Length; j++)
            sum += rho[j] / (2 * a + 4 * j + 1);
        return 1 - sum;
    }

    /// <summary>
    /// Payoff for the player choosing x against y in RPS(n)
    /// </summary>
    private static long Payoff(long x, long y)
    {
        if (x == y)
            return 0;
        long winner = Math.Abs(x - y) % 2 == 1 ? Math.Min(x, y) : Math.Max(x, y);
        return winner == x ? 2 * x - 1 : -(2 * y - 1);
    }

    /// <summary>
    /// Independent check of an equilibrium returned by the closed form, that does not use any of the
    /// formulas in 3): solve (A x)(i) = 0 for the block [Low, High] together with sum(x) = 1 by
    /// Gaussian elimination, then verify x > 0, that no pure strategy 1..N gains against x (which
    /// makes x the equilibrium, see 1)), and that x(N) equals P.
    /// </summary>
    private static bool IsEquilibrium(Equilibrium e)
    {
        int k = (int)(e.High - e.Low + 1);

        // k-1 kernel equations plus normalisation. As x > 0 is the only dependency between the rows
        // of the skew-symmetric block, any k-1 of them are independent.
        var M = new double[k, k + 1];
        for (int i = 0; i < k - 1; i++)
            for (int j = 0; j < k; j++)
                M[i, j] = Payoff(e.Low + i, e.Low + j);
        for (int j = 0; j <= k; j++)
            M[k - 1, j] = 1;

        var x = SolveLinearSystem(M);
        if (x.Any(v => v <= 0))
            return false;

        for (long y = 1; y <= e.N; y++)
        {
            double payoff = 0;
            for (int j = 0; j < k; j++)
                payoff += Payoff(y, e.Low + j) * x[j];
            if (payoff > 1e-9)
                return false;
        }

        double pN = e.High == e.N ? x[k - 1] : 0;
        return Math.Abs(pN - e.P) < 1e-12;
    }

    /// <summary>
    /// Solves the k x k system given as augmented k x (k+1) matrix with Gaussian elimination
    /// (partial pivoting). The matrix is overwritten.
    /// </summary>
    private static double[] SolveLinearSystem(double[,] M)
    {
        int k = M.GetLength(0);
        for (int col = 0; col < k; col++)
        {
            int pivot = col;
            for (int r = col + 1; r < k; r++)
                if (Math.Abs(M[r, col]) > Math.Abs(M[pivot, col]))
                    pivot = r;
            for (int j = col; j <= k; j++)
                (M[col, j], M[pivot, j]) = (M[pivot, j], M[col, j]);

            for (int r = col + 1; r < k; r++)
            {
                double f = M[r, col] / M[col, col];
                for (int j = col; j <= k; j++)
                    M[r, j] -= f * M[col, j];
            }
        }

        var x = new double[k];
        for (int i = k - 1; i >= 0; i--)
        {
            double s = M[i, k];
            for (int j = i + 1; j < k; j++)
                s -= M[i, j] * x[j];
            x[i] = s / M[i, i];
        }
        return x;
    }
}
