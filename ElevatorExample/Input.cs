using AutamationSystem.ElevatorSystem;
using AutamationSystem.FloorElevatorIntegration;
using System;
using System.Collections.Generic;

namespace AutamationSystem.InputSystem
{
    public class Input
    {
        private Dictionary<ConsoleKey, bool> keyMap = new Dictionary<ConsoleKey, bool>()
        {
            //Type keys that you want to handle
            { ConsoleKey.C, false },
            { ConsoleKey.NumPad0, false },
            { ConsoleKey.NumPad1, false },
            { ConsoleKey.NumPad2, false },
            { ConsoleKey.NumPad3, false },
            { ConsoleKey.NumPad4, false },
            { ConsoleKey.NumPad5, false },
            { ConsoleKey.NumPad6, false },
            { ConsoleKey.NumPad7, false },
            { ConsoleKey.NumPad8, false },
            { ConsoleKey.NumPad9, false },
        };

        public Input()
        {
            //Can be useful
            //Array enumMembers = Enum.GetValues(typeof(ConsoleKey));
            //for (int i = 0; i < enumMembers.Length; i++)
            //{
            //    keyMap.Add((ConsoleKey)enumMembers.GetValue(i), false);
            //}
        }

        /// <summary>
        /// True if any key press happens
        /// </summary>
        /// <returns>Returns true when supported key press happens, otherwise false</returns>
        public bool InputUpdate()
        {
            //TODO : Use a better pattern, implement a wrapper around Dictionary maybe?
            var tempList = new List<KeyValuePair<ConsoleKey, bool>>(keyMap);
            foreach (KeyValuePair<ConsoleKey, bool> item in tempList)
            {
                keyMap[item.Key] = false;
            }

            while (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (keyMap.ContainsKey(key))
                {
                    keyMap[key] = true;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// True if input is legit for elevator call, if not returns false
        /// </summary>
        /// <returns>Returns true if input is legit for elevator call, if not returns false</returns>
        /// <param name="floorNumber">Legit input from user</param>
        public static bool ReadInputForElevatorCall(out int floorNumber, out Direction direction) 
        {
            //TODO : Find a better way to handle call input
            bool isElevatorInput = false;
            floorNumber = int.MinValue;
            direction = Direction.None;
            while (!isElevatorInput)
            {
                Console.WriteLine("You must Enter floor, to skip press 'S'");
                var lineInput = Console.ReadLine();
                isElevatorInput = int.TryParse(lineInput, out floorNumber);

                Console.WriteLine("Select a direction using up and down keys");
                var dirInput = Console.ReadKey(true).Key;
                if (dirInput == ConsoleKey.UpArrow)
                {
                    direction = Direction.Up;
                }
                else if (dirInput == ConsoleKey.DownArrow)
                {
                    direction = Direction.Down;
                }
                else
                {
                    isElevatorInput = false;
                }

                if (!isElevatorInput && lineInput.ToLower() == "s")
                {
                    break;
                }
            }
            return isElevatorInput;
        }

        /// <summary>
        /// Is key pressed
        /// </summary>
        /// <param name="consoleKey">Key to check</param>
        /// <returns>True if <paramref name="consoleKey"/> is pressed</returns>
        public bool GetKeyDown(ConsoleKey consoleKey)
        {
            if(keyMap.TryGetValue(consoleKey, out bool isPressed) && isPressed == true)
            {
                return true;
            }
            return false;
        }

        public void HandleArrivedElevators(List<Elevator> arrivedElevators)
        {
            for (int i = 0; i < arrivedElevators.Count; i++)
            {
                Elevator elevator = arrivedElevators[i];
                Console.WriteLine($"Elevator is going {elevator.CurrentDirection.ToString()} Enter Target floors to {elevator.ElevatorIndex}. Elevator");
                Console.WriteLine("Input Example : 1,8,6,15 ----- Press Enter to skip...");
                string[] floorNumbersStr = Console.ReadLine().Split(','); //Read input
                int[] floorNumberArr = GetLegitFloorsFromInput(floorNumbersStr);

                IntArrayExtentions.RemoveDuplicates(ref floorNumberArr);
                IntArrayExtentions.RemoveValueAll(ref floorNumberArr, elevator.CurrentFloor.FloorIndex);

                if (floorNumberArr.Length != 0)
                {
                    elevator.EnterFloorNumbers(floorNumberArr);
                }
            }
        }

        private int[] GetLegitFloorsFromInput(string[] floorNumbersStr)
        {
            int[] floorNumberArr = new int[0];
            for (int i = 0; i < floorNumbersStr.Length; i++)
            {
                if (int.TryParse(floorNumbersStr[i], out var floorNumber))
                {
                    if (i == floorNumberArr.Length)
                    {
                        IntArrayExtentions.Resize(ref floorNumberArr, i + 1);
                    }
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
