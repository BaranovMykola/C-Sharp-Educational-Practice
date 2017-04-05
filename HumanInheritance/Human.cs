using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanInheritance
{
    abstract class Human : IComparable<Human>, INamedAndCopy<Human>, IPrint
    {
        public string Name { get; set; }
        public DateTime Born { get; set; }
        public int Id { get; set; }

        public Human(string name, DateTime born, int id)
        {
            Name = name;
            Born = born;
            Id = id;
        }
        public Human(string name): this(name, DateTime.Now, new Random().Next())
        {
        }

        public override string ToString()
        {
            return String.Format("Name: {0} Born: {1} Id: {2}", Name, Born.Year, Id);
        }

        virtual public void kill()
        {
            Name = "†" + Name + "†";
        }

        int IComparable<Human>.CompareTo(Human other)
        {
            return String.Compare(Name, other.Name);
        }

        public Human Copy()
        {
            throw new NotImplementedException();
        }

        public void Print()
        {
            Console.WriteLine(this);
        }
    }

    sealed class Doctor : Human, IComparable, INamedAndCopy<Doctor>, IPrint
    {
        public string Educational { get; set; }

        public Doctor(string name, DateTime born, string educational, int id): base(name, born, id)
        {
            Educational = educational;
        }
        public Doctor(string name, string educational): this(name, DateTime.Now, educational, new Random().Next())
        {
        }
        public override string ToString()
        {
            return base.ToString() + " Educational: " + Educational;
        }

        int IComparable.CompareTo(object obj)
        {
            return String.Compare(Educational, (obj as Doctor).Educational);
        }

        Doctor INamedAndCopy<Doctor>.Copy()
        {
            return new Doctor(Name, Educational);
        }

        public new void Print()
        {
            Console.WriteLine(this);
        }
    }

    sealed class Patient : Human, INamedAndCopy<Patient>, IPrint
    {
        public class DoctorName : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                return String.Compare((x as Patient).Name, (y as Patient).Name);
            }
        }
        public class DoctorEdiucationComparer : IComparer
        {
            int IComparer.Compare(object x, object y)
            {
                return String.Compare((x as Patient).Medic.Educational, (y as Patient).Medic.Educational);
            }
        }
        Doctor Medic { get; set; }

        public Patient(string name, DateTime born, int id) : base(name, born, id)
        {
        }
        public Patient(string name, DateTime born, int id, Doctor medic) : base(name, born, id)
        {
            Medic = medic;
        }

        public override string ToString()
        {
            return base.ToString() + " Doctor: " + Medic;
        }
        public override void kill()
        {
            base.kill();
            Console.Write("\t Doctor {0} have been arested\n", Medic.Name);
        }

        Patient INamedAndCopy<Patient>.Copy() => new Patient(Name, Born, Id, Medic);

        public new void Print() => Console.WriteLine(this);
    }
}
