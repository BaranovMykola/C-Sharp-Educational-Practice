using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanInheritance
{
    interface INamedAndCopy<T>
    {
        string Name { get; set; }
        T Copy();
    }

    interface IPrint
    {
        void Print();
    }
}
