using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HumanInheritance;
using System.Xml;

namespace SecondUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [ExpectedException(typeof(ApplicationException))]
        [TestMethod]
        public void PopulateNotHuman()
        {
            Vector.Vector<int> vec = new Vector.Vector<int>();
            vec.PopulateHuman("abrakadabra.xml");
        }
        [ExpectedException(typeof(System.IO.FileNotFoundException))]
        [TestMethod]
        public void NotExistedFile()
        {
            Vector.Vector<Human> vec = new Vector.Vector<Human>();
            vec.PopulateHuman("404");
        }
        [ExpectedException(typeof(XmlException))]
        [TestMethod]
        public void CorruptedFile()
        {
            Vector.Vector<Human> vec = new Vector.Vector<Human>();
            vec.PopulateHuman("Human.txt");
        }
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void AddNull()
        {
            Vector.Vector<Human> vec = new Vector.Vector<Human>();
            vec.Add(null);
        }
        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void FindNull()
        {
            Vector.Vector<Human> vec = new Vector.Vector<Human>();
            vec.Find(null);
        }
    }
}
