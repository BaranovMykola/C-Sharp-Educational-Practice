using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vector
{
    class VectorProgram
    {
        static void Main(string[] args)
        {
            int[] arr = { 1, 2, 3, 4 };
            Vector.Vector<int> vec = new Vector.Vector<int>(arr);
            foreach (var item in vec)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("Found {0} at {1} pos", 2, vec.Find(2));
            Console.ReadKey();
        }
    }
}
