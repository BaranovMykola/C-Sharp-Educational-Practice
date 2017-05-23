using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnesCentre.FitnesClub
{
    [Serializable]
    public class Client
    {
        #region Fields
        private DateTime timeLine;
        DateTime endLine;
        #endregion
        #region Properties
        public int Year => timeLine.Year;
        public int Month => timeLine.Month;
        public int DurationDays => (int)(endLine - timeLine).TotalDays;
        public string Id { get; set; }
        #endregion
        #region Constructors
        public Client()
        {
        }
        public Client(int year, int month, int duration)
        {
            timeLine = new DateTime(year, month, 1);
            endLine = timeLine.AddDays(duration);
        }
        #endregion
        #region Methods
        public override string ToString() => $"Client [{Id}]\tYear [{Year}]\tMonth [{Month}]\tDuraton [{DurationDays}] days";
        #endregion


    }
}
