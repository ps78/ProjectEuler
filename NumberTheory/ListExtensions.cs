using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumberTheory
{
    public static class ListExtensions
    {
        public static void Shuffle<T>(this List<T> list, Random? rand = null)
        {
            if (list == null)
                return;

            Random r = (rand == null ? new Random() : rand);

            var array = list.ToArray();
            var shuffleArray = new List<Tuple<T, double>>(array.Length);
            

            for (int i = 0; i < list.Count; i++)
                shuffleArray.Add(new Tuple<T, double>(array[i], r.NextDouble()));

            shuffleArray.Sort((a, b) => a.Item2.CompareTo(b.Item2));

            list.Clear();
            foreach (var e in shuffleArray)
                list.Add(e.Item1);
        }
    }
}
