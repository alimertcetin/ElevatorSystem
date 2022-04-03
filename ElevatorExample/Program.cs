using System;
using System.Collections.Generic;
using AutamationSystem.BuildingSytem;
using AutamationSystem.InputSystem;

namespace AutamationSystem
{
    class Program
    {
        private static Building building = new Building("Turp Tower", 20, 6, 0);
        private static Input input = new Input();
        
        static void Main(string[] args)
        {
            Console.WriteLine(building);
            Console.WriteLine("----------");
            Console.WriteLine("----------");
            Console.WriteLine("You can call elevator at anytime by pressing 'C'");
            Console.WriteLine("----------");
            Console.WriteLine("----------");

            System.Threading.Thread.Sleep(1000);

            while (true)
            {
                building.ElevatorUpdate();
                input.InputUpdate();
                if (input.GetKeyDown(ConsoleKey.C) && Input.ReadInputForElevatorCall(out int floorNumber, out var direction))
                {
                    building.Floors[floorNumber].CallElevator(direction);
                }

                //if (building.IsElevatorStateIdle())
                //{
                //    Console.WriteLine("Press 'C' to call elevator");
                //}
                System.Threading.Thread.Sleep(1000);
            }
        }

    }
}