using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UnhandledException
{
    public class CustomComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            throw new NotImplementedException();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                ArrayList lst = new ArrayList();
                lst.Add(3);
                lst.Add(0);
                lst.Add(4);
                lst.Sort(new CustomComparer());
            }
            catch (NotImplementedException)
            {
                Console.WriteLine("Deal with it");
            }
            catch(Exception)
            {
                Console.WriteLine("Are you surprized?");
            }
            finally
            {
                Console.WriteLine("It dosen't metter - you'll never see this line");
            }
        }
    }
}
