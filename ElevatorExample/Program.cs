#define debugging

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using AutamationSystem.BuildingSytem;
using AutamationSystem.ElevatorSystem;
using AutamationSystem.HumanLogic;
using static AutamationSystem.FloorElevatorIntegration.ElevatorButton;

namespace AutamationSystem
{
    class Program
    {
        private static Building building = new Building(20, 6, 0);

        static void Main(string[] args)
        {
            Console.WriteLine(building.ToString());
            for (int i = 0; i < 100; i++)
            {
                Person person = Person.CreteRandomPerson(building.Floors.Length);
                building.AddPerson(person, 0);
                Console.WriteLine(person);
                System.Threading.Thread.Sleep(20);
            }
            Console.WriteLine("-----------------------------");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("-----------------------------");
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("-----------------------------/" + i);
                Person randomPerson = building.PersonsInBuilding[new Random().Next(0, building.PersonsInBuilding.Count)];

                string s = "Person current floor : " + randomPerson.TargetFloorIndex.ToString() + Environment.NewLine;

                Floor randomFloor = building.Floors[new Random().Next(0, building.Floors.Length)];
                int personCurrentFloorIndex = randomPerson.TargetFloorIndex;
                randomPerson.TargetFloorIndex = randomFloor.FloorIndex;
                s += " Target floor : " + randomPerson.TargetFloorIndex;
                Console.WriteLine(s);

                Direction direction = personCurrentFloorIndex > randomPerson.TargetFloorIndex ? Direction.Down : Direction.Up;
                randomFloor.CallElevator(direction, randomPerson);

                Console.WriteLine(randomFloor);
                Console.WriteLine("-----------------------------");
                System.Threading.Thread.Sleep(5000);
            }
            Console.WriteLine("-----------------------------");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("-----------------------------");
            for (int i = 0; i < 10; i++)
            {
                building.ElevatorUpdate();
            }
            Console.ReadKey();
        }
    }
}