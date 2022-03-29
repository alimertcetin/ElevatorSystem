using System;

namespace AutamationSystem.InputSystem
{
    public class Input
    {
        public const int DEFAULT_INT = int.MinValue;
        public int CalledFrom = int.MinValue;

        /// <summary>
        /// True if input is legit for elevator call, if not returns false
        /// </summary>
        /// <returns>Returns true if input is legit for elevator call, if not returns false</returns>
        public bool InputUpdate()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.C)
                {
                    Console.WriteLine("Enter floor number");
                    ReadInputForElevator(out var isElevatorInput, out var floorNumber);
                    if (isElevatorInput)
                    {
                        CalledFrom = floorNumber;
                        return true;
                    }
                    else
                    {
                        CalledFrom = DEFAULT_INT;
                        return false;
                    }
                }
            }
            return false;
        }
        private static void ReadInputForElevator(out bool isElevatorInput, out int floorNumber)
        {
            isElevatorInput = int.TryParse(Console.ReadLine(), out floorNumber);
            while (!isElevatorInput)
            {
                Console.WriteLine("You must Enter floor, to skip press 'S'");
                var lineInput = Console.ReadLine();
                isElevatorInput = int.TryParse(lineInput, out floorNumber);

                if (!isElevatorInput && lineInput.ToLower() == "s")
                {
                    break;
                }
            }
        }
    }
}
