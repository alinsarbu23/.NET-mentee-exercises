using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_Mentee_Project.Parking_lot.Models
{
    public class Motorcycle : Vehicle
    {
        public Motorcycle(string plate, DateTime enterTime) : base(plate, enterTime)
        {
        }
        public override decimal CalculateParkingFee()
        {
            if (ExitTime == null)
            {
                return 0;
            }
            var hours = Math.Ceiling((ExitTime.Value - EnterTime).TotalHours);
            return (decimal)hours * 3;
        }
        public override string PrintTicket()
        {
            if (ExitTime == null)
            {
                return "Vehicle has not exited yet.";
            }
            var hours = (int)Math.Ceiling((ExitTime.Value - EnterTime).TotalHours);
            var fee = hours * 3;

            return $"{Plate} | Entered: {EnterTime} | Exited: {ExitTime} | Duration: {hours} hours | Fee: ${fee}";

        }
    }
}
