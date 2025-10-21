using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_Mentee_Project.Parking_lot.Models
{
    public class Truck : Vehicle
    {
        public Truck(string plate, DateTime enterTime) : base(plate, enterTime)
        {}

        public override decimal CalculateParkingFee()
        {
            if (ExitTime == null)
            {
                return 0;
            }
            
            var hours = (ExitTime.Value - EnterTime).TotalHours;
            if(hours >4)
            {
                return (decimal)hours * 10 + 15;
            }
            else
            {
                return (decimal)hours * 10;
            }
        }

        public override string PrintTicket()
        {
            if (ExitTime == null)
            {
                return "Vehicle has not exited yet.";
            }
            var hours = (int)Math.Ceiling((ExitTime.Value - EnterTime).TotalHours);
            if (hours > 4) 
            {
                var fee = (hours * 10) + 15;
                return $"{Plate} | Entered: {EnterTime} | Exited: {ExitTime} | Duration: {hours} hours | Fee: ${fee}";
            }
            else
            {
                var fee = hours * 10;
                return $"{Plate} | Entered: {EnterTime} | Exited: {ExitTime} | Duration: {hours} hours | Fee: ${fee}";
            }
        }

    }
}
