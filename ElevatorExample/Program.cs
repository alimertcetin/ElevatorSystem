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
using static AutamationSystem.FloorElevatorIntegration.ElevatorFloorButton;

namespace AutamationSystem
{
    class Program
    {
        private static Building building = new Building(20, 6, 0);

        static void Main(string[] args)
        {
            Console.WriteLine(building);
            Console.WriteLine("Enter current floor");
            if (!int.TryParse(Console.ReadLine(), out var number)) Console.WriteLine("You must Enter floor");

            //call elevator to floor
            building.Floors[number].CallElevator();

            while (true)
            {
                System.Threading.Thread.Sleep(500);
                building.ElevatorUpdate(out List<Elevator> arrivedElevators);

                foreach (Elevator elevator in arrivedElevators)
                {
                    Console.WriteLine("Enter Target floors to " + elevator.ElevatorIndex + ". Elevator");
                    Console.WriteLine("Input Example : 1,8,6,15 ----- If you wanna skip just type " + elevator.CurrentFloor.FloorIndex);
                    string[] floorNumbersStr = Console.ReadLine().Split(','); //Read input
                    int[] floorNumberArr = GetLegitFloorsFromInput(floorNumbersStr);

                    if (floorNumberArr.Contains(elevator.CurrentFloor.FloorIndex))
                    {
                        floorNumberArr = IntArrayExtentions.Remove(floorNumberArr, elevator.CurrentFloor.FloorIndex);
                    }

                    elevator.EnterFloorNumbers(floorNumberArr);
                }

                if(arrivedElevators.Count == 0 && !building.IsSomeoneWaitingForElevator())
                {
                    Console.WriteLine(building);
                    Console.WriteLine("Waiting for elevator call");
                    if (!int.TryParse(Console.ReadLine(), out int floorNumber))
                    {
                        Console.WriteLine("You must Enter floor");
                    }

                    //call elevator to floor
                    building.Floors[floorNumber].CallElevator();
                }
            }
        }

        private static int[] GetLegitFloorsFromInput(string[] floorNumbersStr)
        {
            int[] floorNumberArr = new int[floorNumbersStr.Length];
            for (int i = 0; i < floorNumbersStr.Length; i++)
            {
                if (int.TryParse(floorNumbersStr[i], out var floorNumber))
                {
                    floorNumberArr[i] = floorNumber;
                }
                else if (string.IsNullOrWhiteSpace(floorNumbersStr[i]))
                {
                    continue;
                }
                else
                {
                    Console.WriteLine($"{floorNumbersStr[i]} is not a legit floor number");
                }
            }

            return floorNumberArr;
        }
    }
}
