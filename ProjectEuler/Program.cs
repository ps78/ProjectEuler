using System.Text;

namespace ProjectEuler;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;

        var pm = new ProblemManager(
            //Enumerable.Range(1, 106).Union([121, 126, 144, 146, 148, 169, 200, 233, 243, 307, 543])
            [107]
        );
        pm.Run();
    }
}
