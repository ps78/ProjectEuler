using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;
using System.Numerics;
using System.IO;
using System.Diagnostics;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=91
    /// The points P(x1, y1) and Q(x2, y2) are plotted at integer co-ordinates and are joined to the origin, O(0,0), to form ΔOPQ.
    /// 
    /// 
    /// There are exactly fourteen triangles containing a right angle that can be formed when each 
    /// co-ordinate lies between 0 and 2 inclusive; that is,
    /// 
    /// 0 ≤ x1, y1, x2, y2 ≤ 2.
    /// 
    /// 
    /// Given that 0 ≤ x1, y1, x2, y2 ≤ 50, how many right triangles can be formed?
    /// </summary>
    public class Problem091 : EulerProblemBase
    {              
        public Problem091(): base(91, "Right triangles with integer coordinates", 0, 14234) { }

        public override long Solve(long n)
        {
            int m = 50;
            int count = 0;

            // count the easy cases, where two sides are parallel to the x and y axis:
            count += 3 * m * m;

            // count the cases where the right-angle corner is not an an axis
            // move the second corner P across the whole lattice
            for (int px = 0; px <= m; px++)
                for (int py = 1; py <= m; py++)
                    if (!(px == 0 && py == 0))
                        // the third corner Q must be on the line through P which is perpendicular to the line 0-P
                        // i.e. (px, py) * (qx-px, qy-pq) = 0, where qx and qy must be integers
                        // by testing all valid integer qx, the qy can be calculated and tested if it is integer
                        // qy = py - px * (qx - px) / py
                        for (int qx = 0; qx <= m; qx++)
                            if (px * (qx - px) % py == 0)
                            {
                                int qy = py - px * (qx - px) / py;
                                if ((qy >= 0) && (qy <= m) && (qy != py))
                                    count++;
                            }

            return count;
        }
    }
    
}
