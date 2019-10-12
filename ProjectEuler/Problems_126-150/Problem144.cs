using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NumberTheory;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=144
    /// 
    /// In laser physics, a "white cell" is a mirror system that acts as a delay line for the 
    /// laser beam.The beam enters the cell, 
    /// bounces around on the mirrors, and eventually works its way back out.
    /// 
    /// The specific white cell we will be considering is an ellipse with the equation 4x2 + y2 = 100
    /// 
    /// The section corresponding to −0.01 ≤ x ≤ +0.01 at the top is missing, allowing the light 
    /// to enter and exit through the hole.
    /// 
    /// The light beam in this problem starts at the point (0.0,10.1) just outside the white cell, 
    /// and the beam first impacts the mirror at(1.4,-9.6).
    /// 
    /// Each time the laser beam hits the surface of the ellipse, it follows the usual law of 
    /// reflection "angle of incidence equals angle of reflection." That is, both the incident and 
    /// reflected beams make the same angle with the normal line at the point of incidence.
    /// 
    /// In the figure on the left, the red line shows the first two points of contact between the
    /// laser beam and the wall of the white cell; the blue line shows the line tangent to the 
    /// ellipse at the point of incidence of the first bounce.
    /// 
    /// The slope m of the tangent line at any point(x, y) of the given ellipse is: m = −4x/y
    /// The normal line is perpendicular to this tangent line at the point of incidence.
    /// The animation on the right shows the first 10 reflections of the beam.
    /// 
    /// How many times does the beam hit the internal surface of the white cell before exiting?
    /// </summary>
    public class Problem144 : EulerProblemBase
    {
        public Problem144() : base(144, "Investigating multiple reflections of a laser beam", 0, 354) { }
                
        public override long Solve(long n)
        {
            double x = 1.4;
            double y = -9.6;
            double m = -197.0 / 14;

            long i = 1;
            while(true)
            {                
                m = MirrorSlope(m, x, y);
                var p = Intersect(x, y, m);
                x = p.xs;
                y = p.ys;
                if (x > -0.01 && x < 0.01 && y > 0)
                    break;
                i++;
            }

            return i;
        }

        /// <summary>
        /// Calculate the slope of the outgoing beam, given an ingoing beam with
        /// slope m, hitting the mirror at (x,y)
        /// </summary>
        private double MirrorSlope(double m, double x, double y)
        {
            double mp = y / (4 * x);
            return (m * (1 - mp * mp) - 2 * mp) / (mp * mp - 2 * m * mp - 1);
        }

        private (double xs, double ys) Intersect(double xi, double yi, double m)
        {
            double z = m * xi - yi;
            double R = Math.Sqrt((5 * m + 10) * (5 * m + 10) - z * z - 100 * m);
            double xs1 = (m * z - 2 * R) / (m * m + 4);
            double xs2 = (m * z + 2 * R) / (m * m + 4);

            // problem: due to rounding errors, there can be xs1 and xs2 with the same sign already after a few iterations
            // need a solution with integers!
            double xs = Math.Abs(xs1 - xi) > 0.01 ? xs1 : xs2; 
            double ys = m * (xs - xi) + yi;
            return (xs, ys);
        }

    }
}
