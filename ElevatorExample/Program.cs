using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutamationSystem.BuildingSytem;
using AutamationSystem.ElevatorSystem;

namespace AutamationSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create building
            Building building = new Building(20, 6);
            Console.WriteLine(building.ToString());

            //Check floors is there anyone waiting for elevator

            //if not do nothing
            //else get closest and empty elevator
            //move elevator to the floor that people waits
            //handle other elevators' state
            //if selected elevator has arrived open elevator door
            //else continue
            Console.ReadKey();
        }
    }
}