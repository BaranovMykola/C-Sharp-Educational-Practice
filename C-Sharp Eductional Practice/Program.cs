using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Sharp_Eductional_Practice
{
    class Program
    {
        static void Main(string[] args)
        {
            string arrStr = Console.ReadLine();
            string[] arrStrSpit = arrStr.Split(' ');
            int[] arr = new int[arrStrSpit.Length];
            int index = 0;
            foreach(string i in arrStrSpit)
            {
                arr[index++] = int.Parse(i);
            }
            int f = 0;
            int s = arrStrSpit.Length - 1;
            do
            {
                while(arr[f] > 0)
                {
                    ++f;
                }
                while (arr[s] > 0)
                {
                    --s;
                }
                int t = arr[f];
                arr[f] = arr[s];
                arr[s] = t;
                ++f;
                --s;
            }
            while (f < s);
               foreach(int i in arr)
            {
                Console.Write(i);
                Console.Write(' ');
            }
            Console.Write('\n');
            Console.ReadLine();
        }
    }
}
