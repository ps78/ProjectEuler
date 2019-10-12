using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectEuler
{
    /// <summary>
    /// https://projecteuler.net/problem=19
    /// 
    /// 1 Jan 1900 was a Monday.
    /// Thirty days has September, April, June and November.
    /// All the rest have thirty-one, Saving February alone,
    /// Which has twenty-eight, rain or shine.
    /// And on leap years, twenty-nine.
    /// A leap year occurs on any year evenly divisible by 4, but not on a century unless it is divisible by 400.
    /// 
    /// How many Sundays fell on the first of the month during the twentieth century (1 Jan 1901 to 31 Dec 2000)?
    /// </summary>
    public class Problem019 : EulerProblemBase
    {
        public Problem019() : base(19, "Counting Sundays", 2000, 171) { }

        public override long Solve(long n)
        {
            uint startYear = 1900;
            uint endYear = (uint)n;
                        
            uint weekDay = 0; // 0 = monday

            ulong result = 0;

            uint currentYear = startYear;
            while (currentYear <= endYear)
            {
                for (uint month = 1; month <= 12; month++)
                {
                    if ((weekDay == 6) && (currentYear >= 1901))
                        result++;

                    switch (month)
                    {
                        // april, june, september november, 
                        case 4:                            
                        case 6:
                        case 9:
                        case 11:
                            weekDay = (weekDay + 30) % 7;
                            break;

                        // jan, mar, may, july, aug, oct, dec
                        case 1:
                        case 3:
                        case 5:
                        case 7:
                        case 8:
                        case 10:
                        case 12:
                            weekDay = (weekDay + 31) % 7;
                            break;

                        // february
                        case 2:
                            if ((currentYear % 4 == 0) && (
                                (currentYear % 100 != 0) || (currentYear % 400 == 0)))
                                weekDay = (weekDay + 29) % 7;
                            else
                                weekDay = (weekDay + 28) % 7;
                            break;
                    }
                }
                currentYear++;
            }

            return (long)result;
        }
    }
}
