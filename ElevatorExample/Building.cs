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
                        break;
                    }
                }
            }

        }


        public void ElevatorUpdate(out List<Elevator> arrivedElevators)
        {
            arrivedElevators = new List<Elevator>();
            foreach (Elevator elevator in Elevators)
            {
                if (elevator.Move())
                {
                    arrivedElevators.Add(elevator);
                }
            }
        }

        public bool IsSomeoneWaitingForElevator()
        {
            bool flag = false;
            foreach (Elevator elevator in Elevators)
            {
                if(elevator.TargetFloors.Count > 0)
                {
                    flag = true;
                    break;
                }
            }
            return flag;
        }


        public override string ToString()
        {
            return $"Floor count : {Floors.Length}{Environment.NewLine}" +
                $"Elevator count : {Elevators.Length}";
        }
    }
}
