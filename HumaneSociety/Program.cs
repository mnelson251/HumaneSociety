using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class Program
    {
        static void Main(string[] args)
        {
            //  PointOfEntry.Run();
            //Query.GetCategoryId();
            Dictionary<int, string> tester = new Dictionary<int, string>();
            tester.Add(1, "Dog");
            tester.Add(2, "Polly");
            tester.Add(3, "2");
            Query.SearchForAnimalByMultipleTraits(tester);
        }
    }
}
