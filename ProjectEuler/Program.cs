using System.Text;

namespace ProjectEuler;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;

        var pm = new ProblemManager(
            Enumerable.Range(1, 111).Union([121, 126, 144, 146, 148, 169, 200, 206, 233, 243, 307, 543, 1012, 1013])
        );

        pm.Run();
    }
}
