using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_Mentee_Project.Parking_lot
{
    public class ParkingLot
    {
        private readonly List<Vehicle> parkedVehicles = new(); 
        private readonly List<string> receipts = new();
        private decimal totalEarnings = 0m;
        public ParkingLot() { }

        public string EnterVehicle(Vehicle vehicle)
        {
            bool allreadyParked = parkedVehicles.Any(v => v.Plate.Equals(vehicle.Plate, StringComparison.OrdinalIgnoreCase) && v.ExitTime == null);
            if (allreadyParked)
            {
                return $"Vehicle with plate {vehicle.Plate} is already parked.";
            }
            else
            {
                parkedVehicles.Add(vehicle);
                return "OK";
            }
        }

        public string ExitVehicle(string plate, DateTime exitTime)
        {
            var vehicle = parkedVehicles.FirstOrDefault(v => v.Plate.Equals(plate, StringComparison.OrdinalIgnoreCase) && v.ExitTime == null); //is already inside
            if (vehicle == null)
            {
                return $"No parked vehicle found with plate {plate}.";
            }
            else
            {
                vehicle.ExitTime = exitTime;
                decimal fee = vehicle.CalculateParkingFee();
                string message = vehicle.PrintTicket();

                totalEarnings+= fee;
                receipts.Add(message);
                return message;

            }
        }

        public void Report()
        {
            foreach (var receipt in receipts)
            {
                Console.WriteLine(receipt);
            }

            Console.WriteLine($"Total earnings: {totalEarnings}");

            bool anyActive = parkedVehicles.Any(v => v.ExitTime == null);
            if(!anyActive)
            {
                Console.WriteLine("No active vehicles in the parking lot.");
            }
            else
            {
                Console.WriteLine("Active vehicles in the parking lot:");
                foreach (var vehicle in parkedVehicles.Where(v => v.ExitTime == null))
                {
                    Console.WriteLine($"- Plate: {vehicle.Plate}, Entered at: {vehicle.EnterTime}");
                }
            }
        }
    }
}
