using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using NumberTheory;
using System.Numerics;
using System.Collections.Concurrent;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.ComponentModel;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=102
    /// 
    /// Three distinct points are plotted at random on a Cartesian plane, for which -1000 <= x,y y= 1000,
    /// such that a triangle is formed.
    /// 
    /// Consider the following two triangles:
    /// 
    ///     A(-340,495), B(-153,-900), C(835, -947)
    ///     X(-175,41), Y(-421,-714), Z(574,-645)
    /// 
    /// It can be verified that triangle ABC contains the origin, whereas triangle XYZ does not.
    /// 
    /// Using triangles.txt (right click and 'Save Link/Target As...'), a 27K text file 
    /// containing the co-ordinates of one thousand "random" triangles, 
    /// find the number of triangles for which the interior contains the origin.
    /// 
    /// NOTE: The first two examples in the file represent the triangles in the example given above.
    /// </summary>
    public class Problem102 : EulerProblemBase
    {
        public struct Point
        {
            public int X;
            public int Y;
        }

        public Problem102() : base(102, "Triangle Containment", 0, 228) { }

        public override long Solve(long n)
        {
            var lines = File.ReadAllLines(Path.Combine(ResourcePath, "problem102.txt"));
        
            int containsOrigin = 0;
            var P = new Point() { X = 0, Y = 0 };

            foreach (var line in lines)
            {
                var coord = line.Split(',').Select(x => int.Parse(x)).ToArray();
                var A = new Point() { X = coord[0], Y = coord[1] };
                var B = new Point() { X = coord[2], Y = coord[3] };
                var C = new Point() { X = coord[4], Y = coord[5] };

                var d1 = Distance(P, A, B);
                var d2 = Distance(P, B, C);
                var d3 = Distance(P, C, A);

                if ((d1 >= 0 && d2 >= 0 && d3 >= 0) ||
                    (d1 <= 0 && d2 <= 0 && d3 <= 0))
                    containsOrigin++;
            }

            return containsOrigin;
        }

        /// <summary>
        /// Computes the distance of P to the line defined by AB
        /// the distance can be positive or negative, depending on which side the point lies
        /// </summary>
        private int Distance(Point p, Point A, Point B) => 
             (p.X - A.X) * (B.Y - A.Y) - (p.Y - A.Y) * (B.X - A.X);

        
    }
}
