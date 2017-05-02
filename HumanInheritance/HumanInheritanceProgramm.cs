using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;

namespace HumanInheritance
{
    class HumanInheritanceProgramm
    {
        static void Main(string[] args)
        {
            try
            {
                Vector.Vector<Human> vec = new Vector.Vector<Human>();
                Console.Write("Open file: ");
                vec.PopulateHuman(Console.ReadLine());
                Console.WriteLine("Sorting...");
                vec.Sort(new Human.HumanComparerName());
                Console.WriteLine("Finding last element...");
                Human last = null;

                foreach (var item in vec)
                {
                    (item as Human)?.kill();
                    Console.WriteLine(item);
                    last = item as Human;
                }
                vec.WriteHuman("FILE.txt");
                Console.WriteLine();
                Console.WriteLine("Find {0} in collection: ", last);
                foreach (var item in vec.Find(last))
                {
                    Console.WriteLine(item);
                }
                Vector.Vector<int> vecInt = new Vector.Vector<int>();
                vecInt.PopulateHuman("");
                
            }
            catch (ArgumentNullException error)
            {
                Console.WriteLine();
                Console.WriteLine("Fatal exception:");
                Console.WriteLine(error.Message);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.GetType());
                Console.WriteLine();
                Console.WriteLine("Some error occured:");
                Console.WriteLine(error.Message);
                var data = error.Data;
                foreach (var item in data)
                {
                    Console.WriteLine(item);
                }
            }
            Console.ReadKey();
        }
    }
}
