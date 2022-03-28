using AutamationSystem.BuildingSytem;
using AutamationSystem.HumanLogic;
using System;
using System.Collections.Generic;
using Direction = AutamationSystem.FloorElevatorIntegration.ElevatorFloorButton.Direction;

namespace AutamationSystem.ElevatorSystem
{
    public class Elevator
    {
        public struct ElevatorMoveData
        {
            public Direction Direction;
            public Floor Floor;
        }
        //TODO : Add elevator buttons
        /*
         * All buttons will have floor index
         *  button will be set as dirty when someone pressed to it
         *      if button gets pressed elevator targetfloors will be updated
         *  We dont want to call elevator directly
         *  We want elevator to move according its surroundings
         */
        //TODO : Create an elevator system that manages all elevators at one pass
        /*
         * Its gonna be like an Update loop
         */

        public Building Building { get; private set; }
        public int ElevatorIndex { get; private set; }
        public Floor CurrentFloor { get; private set; }

        public Queue<ElevatorMoveData> TargetFloors { get; private set; }
        private ElevatorDoor Door { get; set; }
        private List<Person> personsCarrying = new List<Person>();

        public Elevator(Building building, int elevatorIndex, Floor currentFloor, double moveSpeed = 1)
        {
            this.Building = building;
            this.ElevatorIndex = elevatorIndex;
            this.CurrentFloor = currentFloor;
            this.TargetFloors = new Queue<ElevatorMoveData>();
            this.Door = new ElevatorDoor(this);
        }

        public Elevator(Elevator elevator)
        {
            this.Building = elevator.Building;
            this.ElevatorIndex = elevator.ElevatorIndex;
            this.CurrentFloor = elevator.CurrentFloor;
            this.TargetFloors = elevator.TargetFloors;
            this.Door = elevator.Door;
        }

        public void Call(Floor floor)
        {
            Direction direction = CurrentFloor.FloorIndex > floor.FloorIndex ? Direction.Down : Direction.Up;
            
            TargetFloors.Enqueue(new ElevatorMoveData() 
            {
                Direction = direction,
                Floor = floor,
            });
        }

        public void Call(List<int> floorIndexes)
        {
            for (int i = 0; i < floorIndexes.Count; i++)
            {
                Call(Building.Floors[floorIndexes[i]]);
            }
        }

        public void EnterFloorNumbers(params int[] floorNumbers)
        {
            floorNumbers.Sort();

            Direction dir = Direction.None;
            if(TargetFloors.Count > 0)
            {
                ElevatorMoveData currentMoveData = TargetFloors.Peek();
                dir = CurrentFloor.FloorIndex > currentMoveData.Floor.FloorIndex ? Direction.Down : Direction.Up;
            }

            List<int> upDirectionList = new List<int>();
            List<int> downDirectionList = new List<int>();

            for (int i = 0; i < floorNumbers.Length; i++)
            {
                if (GetDirection(floorNumbers[i]) == Direction.Up)
                {
                    upDirectionList.Add(floorNumbers[i]);
                }
                else
                {
                    downDirectionList.Add(floorNumbers[i]);
                }
            }
            downDirectionList.Reverse();

            switch (dir)
            {
                case Direction.Up:
                    Call(upDirectionList);
                    Call(downDirectionList);
                    break;
                case Direction.Down:
                    Call(downDirectionList);
                    Call(upDirectionList);
                    break;
                default:
                    Call(upDirectionList);
                    Call(downDirectionList);
                    break;
            }

        }

        public Direction GetDirection(int floorIndex)
        {
            if (CurrentFloor.FloorIndex == floorIndex) return Direction.None;

            return CurrentFloor.FloorIndex > floorIndex ? Direction.Down : Direction.Up;
        }

        public bool Move()
        {
            if(Door.IsOpen) Door.Close();

            if (TargetFloors.Count == 0) return true;

            ElevatorMoveData currentMoveData = TargetFloors.Peek();
            if (CurrentFloor.FloorIndex == currentMoveData.Floor.FloorIndex)
            {
                TargetFloors.Dequeue();
                return true;
            }

            CurrentFloor.ElevatorMovedAway(this);

            int floorIndex = CurrentFloor.FloorIndex > currentMoveData.Floor.FloorIndex ? CurrentFloor.FloorIndex - 1 : CurrentFloor.FloorIndex + 1;
            CurrentFloor = Building.Floors[floorIndex];
            CurrentFloor.ElevatorArrived(this);
            if (CurrentFloor == currentMoveData.Floor)
            {
                //We arrived to floor
                TargetFloors.Dequeue();
                Console.WriteLine($"{ElevatorIndex} Elevator has arrived to : " + CurrentFloor.ToString());
                Door.Open();
                return true;
            }
            else
            {
                Console.WriteLine($"{ElevatorIndex} Elevator is not arrived to {currentMoveData.Floor.FloorIndex} yet, current floor is {CurrentFloor.FloorIndex}");
                return false;
            }
        }

        public override string ToString()
        {
            return $"Elevator Index : {ElevatorIndex}{Environment.NewLine}" +
                $"Current Floor : {CurrentFloor.FloorIndex}";
        }
    }
}
