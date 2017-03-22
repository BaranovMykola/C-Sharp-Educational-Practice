using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanInheritance
{
    class Human
    {
        public string Name { get; set; }
        DateTime Born { get; set; }
        int Id { get; set; }

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
    }

    sealed class Doctor : Human
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
    }

    sealed class Patient : Human
    {
        Doctor Medic { get; set; }

        public Patient(string name, DateTime born, int id): base(name, born, id)
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
    }
}
