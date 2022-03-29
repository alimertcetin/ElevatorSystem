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
        //dummy update
        static void Main(string[] args)
        {
            Console.WriteLine(building);
            Console.WriteLine("----------");
            Console.WriteLine("----------");
            Console.WriteLine("You can elevator at anytime by pressing 'C'");
            Console.WriteLine("----------");
            Console.WriteLine("----------");

            System.Threading.Thread.Sleep(1000);

            while (true)
            {
                System.Threading.Thread.Sleep(500);
                building.ElevatorUpdate();
                bool thereIsInput = input.InputUpdate();
                if (thereIsInput)
                {
                    building.Floors[input.CalledFrom].CallElevator();
                }

                //if (building.IsElevatorStateIdle())
                //{
                //    Console.WriteLine("Press 'C' to call elevator");
                //}
            }
        }

    }
}