using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HumanInheritance;
using System.Xml;

namespace SecondUnitTest
{
    [TestClass]
    public class UnitTest1a
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
            foreach(var item in vec.Find(null));
        }
        [ExpectedException(typeof(NotImplementedException))]
        [TestMethod]
        public void CloneHuman()
        {
            Patient p = new Patient("Podopitniy_Krolik", DateTime.Now, 001);
            (p as Human).Copy();
        }
        [TestMethod]
        public void KillerTest()
        {
            Patient p = new Patient("Victim", DateTime.Now, 001, new Doctor("Killer", DateTime.Parse("1990.04.21"), "<none>", 002));
            string heWasYoung = (p.Name).Clone() as string;
            p.kill();
            StringAssert.Equals(string.Format("†{0}†", heWasYoung), p.Name);
        }
    }
}
