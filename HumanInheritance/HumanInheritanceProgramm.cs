using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HumanInheritance
{
    class HumanInheritanceProgramm
    {
        static void Main(string[] args)
        {
            string[] data = File.ReadAllLines("Human.txt");
            Human[] lst = new Human[data.Length];
            for (int i = 0;i<data.Length;++i)
            {
                string[] line = data[i].Split(' ');
                if(line[0] == "Medic")
                {
                    lst[i] = new Doctor(line[1], new DateTime(int.Parse(line[2]), 1, 1), line[4], int.Parse(line[3]));
                }
                else if(line[0] == "Patient")
                {
                    Doctor pDoc = new Doctor(line[4], new DateTime(int.Parse(line[5]), 1, 1), line[7], int.Parse(line[6]));
                    Patient p = new Patient(line[1], new DateTime(int.Parse(line[2]), 1, 1), int.Parse(line[3]), pDoc);
                    lst[i] = p;
                }
                else
                {
                    Console.WriteLine("eeror");
                }
                Console.WriteLine(lst[i]);
            }
            Console.ReadKey();
        }
    }
}
