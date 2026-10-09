# ProjectEuler

C# solutions to problems from [Project Euler](https://projecteuler.net/), together with a small
library of reusable number-theory and algorithm helpers.

All of the first 110 problems are solved, plus a selection of later ones (121, 126, 144, 146, 148,
169, 200, 206, 233, 243, 307, 543, …).
The goal is to solve every problem in **under one second** — so far that holds for all solved
problems (measured on a 24-core AMD CPU; many solutions use `Parallel` / PLINQ).

> Per Project Euler's [guidelines](https://projecteuler.net/about), please try the problems yourself
> before reading the solutions.

## Solution layout

```
ProjectEuler.sln
├── ProjectEuler/          Console app: the problem solutions and the runner
│   ├── Problems_001-025/  Problem001.cs … Problem025.cs   (one class per problem,
│   ├── Problems_026-050/  …                                 grouped in blocks of 25)
│   ├── Resources/         Input data files (problemNNN.txt) and analysis notes (.xlsx/.svg)
│   ├── IEulerProblem.cs   Interface every problem implements
│   ├── EulerProblemBase.cs  Abstract base class for problems
│   ├── ProblemManager.cs  Discovers problems via reflection, runs and times them
│   └── Program.cs         Entry point – choose which problems to run here
├── NumberTheory/          Class library of reusable math / algorithm helpers
├── SudokuGame/            Sudoku model, solver and generator (used by Problem 96)
└── UnitTests/             xUnit tests for the library and for all problem solutions
```

### ProjectEuler (runner + solutions)

Every problem is a class `ProblemNNN` deriving from `EulerProblemBase`. The constructor passes the
problem number, title, the problem size `n` as stated on the website, and the known answer
(`0` = not solved yet):

```csharp
public class Problem001 : EulerProblemBase
{
    public Problem001() : base(1, "Multiples of 3 and 5", 1000, 233168) { }

    // optional sanity check on a smaller instance given in the problem statement
    public override bool Test() => Solve(10) == 23;

    public override long Solve(long n) { ... }
}
```

`ProblemManager` loads problems by number (or all of them) via reflection, then for each problem:

1. runs `Test()` and skips the problem if it fails,
2. times `Solve(ProblemSize)`,
3. compares the result with the stored `Solution`.

It prints a table with result and runtime per problem, flags anything slower than 1 s (`SLOW!!`) or
incorrect (`WRONG!!`), and ends with a summary (total/average runtime, slowest problem, failures).

`EulerProblemBase_Legacy` / `IEulerProblem_Legacy` are the older `ulong`-based problem API and are
no longer used by any problem.

### NumberTheory (helper library)

Reusable building blocks shared across solutions, for example:

| Area | Types |
| --- | --- |
| Primes | `SieveOfEratosthenes` (bit-packed, prime factors), `MillerRabinTest`, `SimplePrimeTest`, `GeneralPrimeTest` (all `IPrimeTest`), `CountPrimes` (fast prime counting π(N)) |
| Arithmetic | `NumberExtensions` (`Power`, `ModPower`, `Factorial`, `BigInteger.Sqrt`, base conversion, digit sums, palindromes), `GCD`, `Totient`, `LongDivison` |
| Big numbers | `BigDecimal` (arbitrary-precision decimal on top of `BigInteger`) |
| Sequences | `Fibonacci`, `PolygonalNumber` (triangle, square, pentagonal, …), `PartitionMath` (partition function p(n)) |
| Combinatorics | `Permutation`, `Combination` |
| Algebra | `LinearEquationSystem` (exact solving with `BigInteger` coefficients) |
| Graphs & search | `UndirectedGraph` (connectivity, bridges, minimal connectivity), `AStarSearch`, `PriorityQueue` |
| Misc | `RomanNumeral`, string / enumerable / list extensions, `IOExtensions.ReadMatrix` |

### SudokuGame

A general Sudoku library supporting layouts from 4×4 up to 12×12 (including rectangular boxes):
`Sudoku`/`SudokuState` model, a backtracking `SudokuSolver` that always explores the cell with the
fewest candidates first, a random puzzle `Generator`, and an Excel export (`ExcelWriter`, via EPPlus).
Problem 96 uses it to solve the 50 puzzles from the input file in parallel.

This is an older-style **.NET Framework 4.8** project that pulls its NuGet packages into the
`packages/` folder (`packages.config`), so it builds on Windows only.

### UnitTests

xUnit + FluentAssertions tests covering the NumberTheory library (primes, `BigDecimal`,
permutations, graphs, number extensions – including some performance tests) and two
end-to-end tests over all problems:

- **Run Test() methods of all problems** – quick check of every solved problem's `Test()`.
- **Run Solve() methods of all problems** – solves every problem at full size and compares
  against the stored solution.

## Requirements

- Windows (because of the .NET Framework–based `SudokuGame` project)
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) and the .NET Framework 4.8 developer pack
- Visual Studio 2022 (17.10+) is recommended

## Getting started

### Resource path

Problems that read input files use `EulerProblemBase.ResourcePath`, which is currently hard-coded:

```csharp
protected string ResourcePath => @"D:\Code\ProjectEuler\ProjectEuler\Resources";
```

If you clone the repository to a different location, adjust this path in
[EulerProblemBase.cs](ProjectEuler/EulerProblemBase.cs) first.

### Choosing which problems to run

Edit the list passed to `ProblemManager` in [Program.cs](ProjectEuler/Program.cs):

```csharp
var pm = new ProblemManager([1, 2, 3, 96]);                        // specific problems
var pm = new ProblemManager(Enumerable.Range(1, 110));             // a range
var pm = new ProblemManager();                                     // everything
pm.Run();
```

Problem numbers without a matching class are silently ignored.

### Build and run

Open `ProjectEuler.sln` in Visual Studio, set **ProjectEuler** as the startup project and run it —
use the **Release** configuration for meaningful timings. From the command line:

```bash
dotnet run -c Release --project ProjectEuler
```

### Run the tests

Run them from Visual Studio's Test Explorer, or:

```bash
dotnet test
```

Note that the "Run Solve() methods of all problems" test solves every problem at full size and
therefore takes a while.

## Adding a new problem

1. Create `ProblemNNN.cs` in the matching `Problems_XXX-YYY` folder (create the folder if needed).
2. Derive from `EulerProblemBase`, pass number, title and problem size to the base constructor,
   and leave the solution at `0` until it is known.
3. Implement `Solve(long n)`; optionally override `Test()` with the small example from the problem
   statement.
4. Put any input data in `Resources/` and load it via `Path.Combine(ResourcePath, "problemNNN.txt")`.
5. Add the problem number to `Program.cs`. Once the answer is confirmed, enter it as the solution in
   the constructor so the runner and unit tests can verify it from then on.

Generic, reusable logic belongs in the `NumberTheory` library rather than in the problem class.

## License

[GNU General Public License v3.0](LICENSE.MD)
