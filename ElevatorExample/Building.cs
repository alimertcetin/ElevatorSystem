using System;
using AutamationSystem.ElevatorSystem;
using AutamationSystem.HumanLogic;

namespace AutamationSystem.BuildingSytem
{
    public class Building
    {
        public Floor[] Floors { get; private set; }
        public Elevator[] Elevators { get; private set; }

        public Building(int floorCount, int elevatorCount, params int[] entranceFloorIndexes)
        {
            if (floorCount < 1)
            {
                throw new InvalidOperationException("Floor count cant be less than 1");
            }

            Floors = new Floor[floorCount]; //initialize array
            for (int i = 0; i < floorCount; i++)
            {
                Floors[i] = new Floor(this, i); //initilize floors inside of the array
            }

            Elevators = new Elevator[elevatorCount];
            for (int i = 0; i < elevatorCount; i++)
            {
                Elevators[i] = new Elevator(this, Floors[0]); // all elevators will start at floor 0
            }

            //Set entrance floors
            for (int i = 0; i < entranceFloorIndexes.Length; i++)
            {
                for (int j = 0; j < Floors.Length; j++)
                {
                    var floor = Floors[i];
                    if(floor.FloorIndex == entranceFloorIndexes[i])
                    {
                        floor.SetEntrance();
                    }
                }
            }

        }

        /// <summary>
        /// Simulate person that enters building
        /// </summary>
        /// <param name="person"></param>
        public void AddPerson(Person person, int entranceFloor)
        {
            for (int i = 0; i < Floors.Length; i++)
            {
                var floor = Floors[i];
                if (floor.FloorIndex == entranceFloor)
                {
                    if (floor.EntranceFloor)
                    {
                        //person is entered in this floor
                    }
                    else
                    {
                        throw new InvalidOperationException("This floor is not an entrance floor!");
                    }
                }
            }
        }

        public override string ToString()
        {
            return $"Floor count : {Floors.Length}{Environment.NewLine}" +
                $"Elevator count : {Elevators.Length}";
        }
    }
}
