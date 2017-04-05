using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanInheritance;

namespace Vector
{
    class Vector : IEnumerable
    {
        ArrayList lst;
        public Vector(IEnumerable seq)
        {
            lst = new ArrayList();
            foreach (var item in seq)
            {
                lst.Add(item);
            }
        }

        public IEnumerator GetEnumerator() => ((IEnumerable)lst).GetEnumerator();

        public IEnumerable<T> GetTypeOf<T>()
        {
            foreach (var item in this)
            {
                if(item is T)
                {
                    yield return (T)item;
                }
            }
        }

        public void Add<T>(T obj)
        {
            lst.Add(obj);
        }
    }
}
