using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    public interface IEulerProblem_Legacy
    {
        int ProblemNumber { get; }

        ulong ProblemSize { get; set; }
        
        ulong DefaultProblemSize { get; }

        ulong MinProblemSize { get; }

        ulong MaxProblemSize { get; }

        UInt64 Solve();
    }
}

