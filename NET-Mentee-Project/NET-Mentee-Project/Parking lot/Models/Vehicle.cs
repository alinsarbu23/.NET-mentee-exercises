using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_Mentee_Project.Parking_lot
{
    public abstract class Vehicle
    {
        public string Plate { get; set; }
        public DateTime EnterTime { get;set; }
        public DateTime? ExitTime { get; set; }

        public Vehicle(string plate, DateTime enterTime)
        {
            Plate = plate;
            EnterTime = enterTime;
            ExitTime = null;
        }

        public abstract decimal CalculateParkingFee();
        public abstract string PrintTicket();
    }
}
