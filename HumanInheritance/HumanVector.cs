using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanInheritance;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Vector
{
    public class Vector<T> : IEnumerable
    {
        ArrayList lst;
        public Vector(IEnumerable<T> seq)
        {
            lst = new ArrayList();
            foreach (var item in seq)
            {
                lst.Add(item);
            }
        }
        public Vector()
        {
            lst = new ArrayList();
        }

        public IEnumerator GetEnumerator() => ((IEnumerable)lst).GetEnumerator();

        public IEnumerable<TType> GetTypeOf<TType>() where TType : T
        {
            foreach (var item in this)
            {
                if (item is TType)
                {
                    yield return (TType)item;
                }
            }
        }

        public void Add(T newElement)
        {
            if(newElement == null)
            {
                throw new ArgumentNullException(nameof(newElement), "Null reference cannot be added to collection");
            }
            lst.Add(newElement);
        }

        public void PopulateHuman(string file)
        {
            if (typeof(T) != typeof(Human))
            {
                throw new ApplicationException("Cannot populate non Human collection by Humans");
            }
            XDocument xel = XDocument.Load(file);
            var doc = xel.XPathSelectElements("Root/Doctor");
            foreach (var item in doc)
            {
                lst.Add(Doctor.Parse(item));
            }
            var pat = xel.XPathSelectElements("Root/Patient");
            foreach (var item in pat)
            {
                lst.Add(Patient.Parse(item));
            }
        }

        public void Sort(IComparer comparer)
        {
            lst.Sort(comparer);
        }

        public void WriteHuman(string file)
        {
            if (typeof(T) != typeof(Human))
            {
                throw new ApplicationException("Cannot wirte non Human");
            }
            XElement root = new XElement("Root", null);
            foreach (var item in lst)
            {
                if (item is Doctor)
                {
                    Doctor docItem = (item as Doctor);
                    root.Add(docItem.ToXElement("Doctor"));
                }
                else if (item is Patient)
                {
                    root.Add((item as Patient).ToXElement("Patient"));
                }
            }
            root.Save(file);
        }

        public IEnumerable<T> Find(T obj)
        {
            if(obj == null)
            {
                throw new ArgumentNullException(nameof(obj), "Value cannot be null");
            }
            foreach (var item in lst)
            {
                if (item.Equals(obj))
                {
                    yield return (T)item;
                }
            }
        }
    }
}