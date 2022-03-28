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
            Elevator elevator = new Elevator(building, -1, building.Floors[0]);
            Console.WriteLine(building);
            Console.WriteLine("Enter current floor");
            if (!int.TryParse(Console.ReadLine(), out var number)) Console.WriteLine("You must Enter floor");

            //call elevator to floor
            elevator.Call(building.Floors[number]);

            while (true)
            {
                while (!elevator.Move() || elevator.TargetFloors.Count > 0)
                {
                    System.Threading.Thread.Sleep(500);
                }

                Console.WriteLine("Enter Target floors by seperating them with comma Example : \" 1,8,6,15 \"");
                string[] floorNumbersStr = Console.ReadLine().Split(','); //Read input
                int[] floorNumberArr = GetLegitFloorsFromInput(floorNumbersStr);
                elevator.EnterFloorNumbers(floorNumberArr);
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
