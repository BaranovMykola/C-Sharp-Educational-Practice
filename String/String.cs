using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace String
{
    class String
    {
        string groupRegex = @"[^0-9\+\-\=\*]";
        public int getCountSpecificGroups(string str, char ch)
        {
            //string t;
            //int count = -2;
            //do
            //{
            //    t = Regex.Match(str, groupRegex+groupRegex+"*"+ch+@"[0-9\+\-\=\*]").Value;
            //    str = str.Remove(0, str.IndexOf(t) + t.Length);
            //    Console.Write("{0} ", t);
            //    ++count;
            //}
            //while (t.Length > 0);
            //Console.WriteLine("count: {0}", count);
            int count = Regex.Matches(str, groupRegex + groupRegex + "*" + ch + @"[0-9\+\-\=\*]").Count - 1;
            Console.WriteLine("count {0}", count);
            return count;
        }
        public char getFirstGroupLastChar(string str)
        {
            str = Regex.Match(str, groupRegex+groupRegex+"+"+groupRegex).Value;
            return getLastLetter(str);
        }
        public static char getLastLetter(string group)
        {
            Console.WriteLine(group[group.Length - 1]);
            return group[group.Length - 1];
        }
        static void Main(string[] args)
        {
            String pr = new String();
            string str = "0kkk0bbk9ccc=gfk0kke2gtk2";
            Console.WriteLine(str);
            char ch = pr.getFirstGroupLastChar(str);
            pr.getCountSpecificGroups(str, ch);
            Console.ReadKey();
        }
    }
}
