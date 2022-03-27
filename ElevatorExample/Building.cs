using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutamationSystem.ElevatorSystem;
using AutamationSystem.HumanLogic;

namespace AutamationSystem.BuildingSytem
{
    public class Building
    {
        public Floor[] Floors { get; private set; }
        public Elevator[] Elevators { get; private set; }

        public List<Person> PersonsInBuilding = new List<Person>();

        private List<Floor> waitingForElevatorList = new List<Floor>();

        public Building(int floorCount, int elevatorCount, params int[] entranceFloorIndexes)
        {
            if (floorCount < 1)
            {
                throw new InvalidOperationException("Floor count cant be less than 1");
            }

            Floors = new Floor[floorCount]; //initialize array
            for (int i = 0; i < floorCount; i++)
            {
                Floors[i] = new Floor(this, i, elevatorCount); //initilize floors inside of the array
            }

            Elevators = new Elevator[elevatorCount];
            Floor firstFloor = Floors[0];
            for (int i = 0; i < elevatorCount; i++)
            {
                Elevators[i] = new Elevator(this, i, firstFloor); // all elevators will start at floor 0
                firstFloor.CurrentElevatorsOnFloor.Add(Elevators[i]);
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

        public void ClearWaitList()
        {
            for (int i = waitingForElevatorList.Count - 1; i >= 0; i--)
            {
                Floor floor = waitingForElevatorList[i];
                Elevator closestElevator = GetAvailableElevator(floor);
                if (closestElevator == null) break;

                closestElevator.TargetFloors.Enqueue(floor);
                waitingForElevatorList.RemoveAt(i);
            }
            if (waitingForElevatorList.Count == 0) return;

            Console.WriteLine("---------------");
            Console.WriteLine("Elevator Wait List");
            Console.WriteLine("---------------");
            for (int i = 0; i < waitingForElevatorList.Count; i++)
            {
                Console.WriteLine(waitingForElevatorList[i]);
            }
            Console.WriteLine("---------------");
            Console.WriteLine("Elevator Wait List");
            Console.WriteLine("---------------");
        }

        public void ElevatorUpdate()
        {
            for (int i = 0; i < Elevators.Length; i++)
            {
                Elevator elevator = Elevators[i];
                if (elevator.TargetFloors.Count > 0)
                {
                    elevator.Move();
                    Console.WriteLine(elevator);
                }
            }

            CheckPersons();
            ClearWaitList();
            CheckFloors();
        }

        private void CheckPersons()
        {

        }

        private void CheckFloors()
        {
            for (int i = 0; i < Floors.Length; i++)
            {
                Floor floor = Floors[i];
                int calledElevators = floor.HowManyElevatorCalled();
                if (calledElevators > 0)
                {
                    for (int j = 0; j < calledElevators; j++)
                    {
                        Elevator closestElevator = GetAvailableElevator(floor);
                        if (closestElevator == null) continue;

                        closestElevator.TargetFloors.Enqueue(floor);
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
                        PersonsInBuilding.Add(person);
                    }
                    else
                    {
                        throw new InvalidOperationException("This floor is not an entrance floor!");
                    }
                }
            }
        }

        public Elevator GetAvailableElevator(Floor floor)
        {
            //Check elevator state
            foreach (Elevator elevator in Elevators)
            {
                if (elevator.TargetFloors.Count > 0) continue;

                return elevator;
            }

            waitingForElevatorList.Add(floor);
            return null;
        }

        public override string ToString()
        {
            return $"Floor count : {Floors.Length}{Environment.NewLine}" +
                $"Elevator count : {Elevators.Length}";
        }
    }
}
