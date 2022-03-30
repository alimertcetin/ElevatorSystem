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

            //TODO : Create more reliable input handling
            if (Console.KeyAvailable) //Not much reliable, stores all input as Queue(not sure) processes later
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
        public static bool ReadInputForElevatorCall(out int floorNumber) //TODO : Find a better way to handle call input
        {
            bool isElevatorInput = false;
            floorNumber = int.MinValue;
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
    }
}
