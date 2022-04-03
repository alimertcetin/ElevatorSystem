using AutamationSystem.BuildingSytem;
using AutamationSystem.FloorElevatorIntegration;
using AutamationSystem.HumanLogic;
using System;
using System.Collections.Generic;

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

        public Direction CurrentDirection;

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

        public void Call(Floor floor, Direction direction)
        {
            TargetFloors.Enqueue(new ElevatorMoveData() 
            {
                Direction = direction,
                Floor = floor,
            });
            CurrentDirection = direction;
        }

        public void Call(List<int> floorIndexes, Direction direction)
        {
            for (int i = 0; i < floorIndexes.Count; i++)
            {
                Call(Building.Floors[floorIndexes[i]], direction);
            }
        }

        public void EnterFloorNumbers(Direction direction, params int[] floorNumbers)
        {
            if (floorNumbers.Length == 0)
            {
                throw new InvalidOperationException("There is no floor number but you are still trying to enter");
            }

            IntArrayExtentions.RemoveDuplicates(ref floorNumbers);
            IntArrayExtentions.RemoveValueAll(ref floorNumbers, CurrentFloor.FloorIndex);

            if (floorNumbers.Length == 0) return;

            IntArrayExtentions.Sort(ref floorNumbers);
            if(CurrentDirection == Direction.None)
            {
                CurrentDirection = direction;
            }

            List<int> legitDirections = new List<int>();
            //List<int> downDirectionList = new List<int>();

            for (int i = 0; i < floorNumbers.Length; i++)
            {
                if (GetDirection(floorNumbers[i]) == CurrentDirection)
                {
                    legitDirections.Add(floorNumbers[i]);
                }
                //else
                //{
                //    downDirectionList.Add(floorNumbers[i]);
                //}
            }
            //downDirectionList.Reverse();
            Call(legitDirections, CurrentDirection);
            //switch (dir)
            //{
            //    case Direction.Up:
            //        Call(upDirectionList);
            //        Call(downDirectionList);
            //        break;
            //    case Direction.Down:
            //        Call(downDirectionList);
            //        Call(upDirectionList);
            //        break;
            //    default:
            //        Call(upDirectionList);
            //        Call(downDirectionList);
            //        break;
            //}

        }

        public Direction GetDirection(int floorIndex)
        {
            if (CurrentFloor.FloorIndex == floorIndex) return Direction.None;

            return CurrentFloor.FloorIndex > floorIndex ? Direction.Down : Direction.Up;
        }

        public bool Move()
        {
            if(Door.IsOpen) Door.Close();

            if (TargetFloors.Count == 0)
            {
                CurrentDirection = Direction.None;
                return false;
            }

            ElevatorMoveData currentMoveData = TargetFloors.Peek();
            if (CurrentFloor.FloorIndex == currentMoveData.Floor.FloorIndex)
            {
                TargetFloors.Dequeue();
                Door.Open();
                return true;
            }

            CurrentFloor.ElevatorMovedAway(this);

            int floorIndex = CurrentFloor.FloorIndex > currentMoveData.Floor.FloorIndex ? CurrentFloor.FloorIndex - 1 : CurrentFloor.FloorIndex + 1;
            CurrentFloor = Building.Floors[floorIndex];
            CurrentFloor.ElevatorArrived(this, CurrentDirection);
            if (CurrentFloor == currentMoveData.Floor)
            {
                //We arrived to floor
                TargetFloors.Dequeue();
                Console.WriteLine($"Elevator {ElevatorIndex} has arrived to : " + CurrentFloor.ToString());
                Console.WriteLine();
                Door.Open();

                //if (TargetFloors.Count == 0)
                //{
                //    CurrentDirection = Direction.None;
                //}

                return true;
            }
            else
            {
                Console.WriteLine($"Elevator {ElevatorIndex} is not arrived to {currentMoveData.Floor.FloorIndex} yet, current floor : {CurrentFloor.FloorIndex}");
                Console.WriteLine();
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
