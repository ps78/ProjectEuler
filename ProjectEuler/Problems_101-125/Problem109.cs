using NumberTheory;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectEuler;

/// <summary>
/// https://projecteuler.net/problem=109
/// </summary>
public class Problem109 : EulerProblemBase
{
    public Problem109() : base(109, "Darts", 100, 38182) { }

    public override long Solve(long n)
    {
        List<(string shot, int points)> singleShots = 
            Enumerable.Range(1, 20).Select(n => ("S" + n.ToString(), n)).Union([("S25", 25)]).ToList();

        List<(string shot, int points)> doubleShots =
            Enumerable.Range(1, 20).Select(n => ("D" + (2 * n).ToString(), 2 * n)).Union([("D25", 50)]).ToList();

        List<(string shot, int points)> tripleShots = 
            Enumerable.Range(1, 20).Select(n => ("T" + (3 * n).ToString(), 3 * n)).ToList();

        var allShots = singleShots.Union(doubleShots).Union(tripleShots).ToList();

        var finishes = new List<(string[] shots, int points)>();

        // triple finishes
        for (int firstIdx = 0; firstIdx < allShots.Count; firstIdx++)
            for (int secondIdx = firstIdx; secondIdx < allShots.Count; secondIdx++)
                for (int thirdIdx = 0; thirdIdx < doubleShots.Count; thirdIdx++)
                {
                    var points = allShots[firstIdx].points + allShots[secondIdx].points + doubleShots[thirdIdx].points;
                    finishes.Add(([allShots[firstIdx].shot, allShots[secondIdx].shot, doubleShots[thirdIdx].shot], points));
                }

        // double finishes
        for (int firstIdx = 0; firstIdx < allShots.Count; firstIdx++)
            for (int secondIdx = 0; secondIdx < doubleShots.Count; secondIdx++)
            {
                var points = allShots[firstIdx].points + doubleShots[secondIdx].points;
                finishes.Add(([allShots[firstIdx].shot, doubleShots[secondIdx].shot], points));
            }

        // single finished
        finishes.AddRange(doubleShots.Select(s => (new string[] { s.shot }, s.points)));

        return finishes.Count(x => x.points < (int)n);
    }
}
