using System.Text;

namespace ProjectEuler;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;

        var pm = new ProblemManager(
            //Enumerable.Range(1, 110).Union([121, 126, 144, 146, 148, 169, 200, 206, 233, 243, 307, 543])
            [684]
        );

        // 50 easies problems: need 684, 686, 700, 719, 751, 800, 808, 816, 836, 853, 872, 

        pm.Run();
    }
}
