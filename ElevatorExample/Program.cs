using System;
using System.Collections.Generic;
using AutamationSystem.BuildingSytem;
using AutamationSystem.ElevatorSystem;
using AutamationSystem.InputSystem;

namespace AutamationSystem
{
    class Program
    {
        //Floor 20 - Elevator 6
        private static Floor[] floors = new Floor[]
        {
            new Floor(0, 6), new Floor(1, 6), new Floor(2, 6),
            new Floor(3, 6), new Floor(4, 6), new Floor(5, 6),
            new Floor(6, 6), new Floor(7, 6), new Floor(8, 6),
            new Floor(9, 6), new Floor(10, 6), new Floor(11, 6),
            new Floor(12, 6), new Floor(13, 6), new Floor(14, 6),
            new Floor(15, 6), new Floor(16, 6), new Floor(17, 6),
            new Floor(18, 6), new Floor(19, 6),
        };
        private static Elevator[] elevators = new Elevator[]
        {
            new Elevator(0, floors[0], floors), new Elevator(1, floors[0], floors),
            new Elevator(2, floors[0], floors), new Elevator(3, floors[0], floors),
            new Elevator(4, floors[0], floors), new Elevator(5, floors[0], floors),
            new Elevator(6, floors[0], floors),
        };
        private static Building building = new Building(floors, elevators, "Turp Tower");
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
                XIV.Timer.Update();
                Console.WriteLine(XIV.Timer.DeltaTime);

                building.ElevatorUpdate(out var arrivedElevators);
                input.HandleArrivedElevators(arrivedElevators);
                input.InputUpdate();
                if (input.GetKeyDown(ConsoleKey.C) && Input.ReadInputForElevatorCall(out int floorNumber, out var direction))
                {
                    building.Floors[floorNumber].CallElevator(direction);
                }

                System.Threading.Thread.Sleep(1000);
            }
        }

    }
}