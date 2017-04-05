using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;

namespace HumanInheritance
{
    class HumanInheritanceProgramm
    {
        static void Main(string[] args)
        {
            Patient pat = new Patient("name", DateTime.Now, 0);
            Doctor doc = new Doctor("doc", "Strong");
            IPrint[] humans = { pat, doc };
            foreach (var item in humans)
            {
                item.Print();
            }
            Console.WriteLine();
            Console.WriteLine();

            int[] arr = { 1, 2, 3, 4, 5, 6 };

            Vector.Vector vec = new Vector.Vector(arr);
            vec.Add(3.4);
            vec.Add(pat);
            vec.Add(doc);
            vec.Add((pat as INamedAndCopy<Patient>).Copy());
            pat.Name = "CHANGE NAME";

            var integ = vec.GetTypeOf<Human>();
            foreach (var item in integ)
            {
                Console.WriteLine(item);
            }

            try
            {
                var bio = doc.Copy();
                Console.WriteLine("Type of simple Copy(): {0}", (bio is Human) ? "Human" : "Gumanoid");
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Copying human is forbiden by law");
            }

            Console.ReadKey();
        }
    }
}
