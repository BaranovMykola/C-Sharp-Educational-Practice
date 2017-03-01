using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace String
{
    class Program
    {
        public static int getCountSpecificGroups(string str, char ch)
        {
            string t;
            int count = -2;
            do
            {
                t = Regex.Match(str, @"[^0-9\+\-\=\*][^0-9\+\-\=\*]*" + ch+ @"[0-9\+\-\=\*]").Value;
                //Console.WriteLine(t);
                //Console.WriteLine(str.IndexOf(t) + t.Length);
                str = str.Remove(0, str.IndexOf(t) + t.Length);
                //Console.WriteLine(str);
                Console.WriteLine("{0} ", t);
                ++count;
            }
            while (t.Length > 0);
            Console.WriteLine("count: {0}", count);
            return 0;
        }
        public static char getFirstGroupLastChar(string str)
        {
            str = Regex.Match(str, @"[^0-9\+\-\=\*][^0-9\+\-\=\*]+[^0-9\+\-\=\*]").Value;
            //Console.WriteLine(str);
            return getLastLetter(str);
        }
        public static char getLastLetter(string group)
        {
            Console.WriteLine(group[group.Length - 1]);
            return group[group.Length - 1];
        }
        static void Main(string[] args)
        {
            string str = "0kkk0bbk9ccc7gfk0kke2gtk2";
            char ch = getFirstGroupLastChar(str);
            getCountSpecificGroups(str, ch);
            Console.ReadKey();
        }
    }
}
