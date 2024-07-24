using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ProjectEuler
{
    public class ProblemManager
    {
        private readonly List<IEulerProblem> problems = new List<IEulerProblem>();

        public IEnumerable<IEulerProblem> Problems => problems;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="problemNumbers"></param>
        public ProblemManager(IEnumerable<int>? problemNumbers = null)
        {
            problems.AddRange(LoadProblems(problemNumbers));
        }

        /// <summary>
        /// Load the problems with the given numbers. 
        /// If null, load all
        /// Numbers that don't exist are simply not loaded
        /// </summary>
        /// <param name="problemNumbers"></param>
        private IEnumerable<IEulerProblem> LoadProblems(IEnumerable<int>? problemNumbers = null)
        {
            // get the types through Reflection:
            List<Type> types = [];

            // get all types:
            if (problemNumbers == null)
            {
                var interfaceType = typeof(IEulerProblem);
                types.AddRange(AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => a.FullName.StartsWith("ProjectEuler,"))
                    .SelectMany(s => s.GetTypes())
                    .Where(p => interfaceType.IsAssignableFrom(p) && !p.IsInterface));
            }
            // get types by number
            else
            {
                foreach (int n in problemNumbers.Distinct())
                {
                    string typeName = $"ProjectEuler.Problem{n:000}, ProjectEuler, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                    var type = Type.GetType(typeName);
                    if (type != null)
                        types.Add(type);
                }
            }

            foreach (var type in types.OrderBy(t => t.Name))
            {
                IEulerProblem instance = null;
                try
                {                    
                    instance = (IEulerProblem)Activator.CreateInstance(type);
                }
                catch
                {
                    Console.WriteLine($"Error instantiating {type.Name}");
                }
                if (instance != null)
                    yield return instance;
            }
        }

        public void Run()
        {
            var sw = new Stopwatch();
            double totalRunTime = 0;
            int testsFailed = 0;
            int wrongSolutions = 0;
            IEulerProblem? slowestProblem = null;
            TimeSpan longestRuntime = TimeSpan.Zero;

            Console.WriteLine("=======================================");
            Console.WriteLine("|    P R O J E C T     E U L E R      |");
            Console.WriteLine("=======================================");            
            Console.WriteLine($"{problems.Count} problems found");
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");

            foreach (var p in problems.OrderBy(p => p.ProblemNumber))
            {
                string lineStart = $"{p.ProblemNumber,3:D3}: {p.Title,-50}";
                
                // run test first, skip if this already fails
                if (!p.Test())
                {
                    Console.WriteLine($"{lineStart}                        -- |       --     TEST FAILED!");
                    testsFailed++;
                    continue;
                }

                // solve
                sw.Restart();
                var solution = (long)p.Solve(p.ProblemSize);
                var runtime = sw.Elapsed;
                totalRunTime += runtime.TotalMilliseconds;

                // print solution & runtime 
                Console.Write($"{lineStart}{solution,26:N0} | {runtime.TotalMilliseconds,8:N1} ms  ");

                // add slow indicator if runtime is over 1 sec
                if (runtime.TotalMilliseconds > 1000)
                    Console.Write("SLOW!! ");

                if (slowestProblem == null || runtime >= longestRuntime)
                {
                    longestRuntime = runtime;
                    slowestProblem = p;
                }

                // test if solution is correct, in case it is available
                if (p.IsSolved)
                    if (solution != (long)p.Solution)
                    {
                        Console.Write($"WRONG!! ");
                        wrongSolutions++;
                    }
                Console.WriteLine();
            }

            // print summary
            Console.WriteLine("---------------------------------------------------------------------------------------------------------------");
            Console.WriteLine($"Total solution time             : {totalRunTime / 1000,10:N1} sec");
            Console.WriteLine($"Average runtime per problem     : {totalRunTime / problems.Count,10:N1} ms");
            Console.WriteLine($"Problem{slowestProblem.ProblemNumber,3:D3} took longest         : {longestRuntime.TotalMilliseconds,10:N1} ms");
            Console.WriteLine($"Number of tests failed          : {testsFailed,10}");
            Console.WriteLine($"Number of wrong solutions       : {wrongSolutions,10}");            
        }
    }
}
