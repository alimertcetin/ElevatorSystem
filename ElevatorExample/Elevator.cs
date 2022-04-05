using AutamationSystem.BuildingSytem;
using AutamationSystem.FloorElevatorIntegration;
using AutamationSystem.HumanLogic;
using System;
using System.Collections.Generic;

namespace AutamationSystem.ElevatorSystem
{
    public class Elevator
    {
        public Floor[] Floors { get; private set; }
        public int ElevatorIndex { get; private set; }
        public Floor CurrentFloor { get; private set; }

        public List<Floor> TargetFloors { get; private set; }
        public ElevatorDoor Door { get; private set; }
        private List<Person> personsCarrying = new List<Person>();

        public Direction CurrentDirection;
        public ElevatorFloorButton[] FloorButtons;

        public Elevator(Elevator elevator)
        {
            this.Floors = elevator.Floors;
            this.ElevatorIndex = elevator.ElevatorIndex;
            this.CurrentFloor = elevator.CurrentFloor;
            this.TargetFloors = elevator.TargetFloors;
            this.Door = elevator.Door;
            this.personsCarrying = elevator.personsCarrying;
            this.CurrentDirection = elevator.CurrentDirection;
            this.FloorButtons = elevator.FloorButtons;
        }

        public Elevator(int elevatorIndex, Floor currentFloor, Floor[] floors)
        {
            this.Floors = floors;
            this.ElevatorIndex = elevatorIndex;
            this.CurrentFloor = currentFloor;
            this.TargetFloors = new List<Floor>();
            this.Door = new ElevatorDoor(this);
            this.FloorButtons = new ElevatorFloorButton[Floors.Length];

            InitializeFloorButtons();
        }

        private void InitializeFloorButtons()
        {
            for (int i = 0; i < FloorButtons.Length; i++)
            {
                FloorButtons[i] = new ElevatorFloorButton(this, Floors[i]);
                FloorButtons[i].onButtonPressed += OnFloorButtonPressed;
            }
        }

        private void OnFloorButtonPressed(Button button)
        {
            ElevatorFloorButton elevatorFloorButton = button as ElevatorFloorButton;
            AddTarget(elevatorFloorButton.Floor, GetDirection(elevatorFloorButton.Floor.FloorIndex));
        }

        public void AddTarget(Floor floor, Direction direction)
        {
            TargetFloors.Add(floor);
            CurrentDirection = direction;
        }

        public void EnterFloorNumbers(params int[] floorNumbers)
        {
            if (floorNumbers.Length == 0)
            {
                throw new InvalidOperationException("There is no floor number but you are still trying to enter");
            }

            IntArrayExtentions.Sort(ref floorNumbers);

            for (int i = floorNumbers.Length - 1; i >= 0; i--)
            {
                if (GetDirection(floorNumbers[i]) != CurrentDirection) continue;

                AddTarget(Floors[floorNumbers[i]], CurrentDirection);
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

            if (TargetFloors.Count == 0)
            {
                CurrentDirection = Direction.None;
                return false;
            }

            var targetFloor = TargetFloors[0];
            if (CurrentFloor.FloorIndex == targetFloor.FloorIndex)
            {
                TargetFloors.RemoveAt(0);
                Door.Open();
                return true;
            }

            CurrentFloor.ElevatorMovedAway(this);

            int floorIndex = CurrentFloor.FloorIndex > targetFloor.FloorIndex ? CurrentFloor.FloorIndex - 1 : CurrentFloor.FloorIndex + 1;
            CurrentFloor = Floors[floorIndex];
            CurrentFloor.ElevatorArrived(this, CurrentDirection);
            if (TargetFloors.Contains(CurrentFloor))
            {
                //We arrived to floor
                TargetFloors.Remove(CurrentFloor);
                Console.WriteLine($"Elevator {ElevatorIndex} has arrived to : " + CurrentFloor.ToString());
                Console.WriteLine();
                Door.Open();
                return true;
            }
            else
            {
                Console.WriteLine($"Elevator {ElevatorIndex} is not arrived to {targetFloor.FloorIndex} yet, current floor : {CurrentFloor.FloorIndex}");
                Console.WriteLine();
                return false;
            }
        }

        public static void SortByFloor(ref Elevator[] arr)
        {
            for (var i = 0; i < arr.Length; i++)
            {
                var min = i;
                for (var j = i + 1; j < arr.Length; j++)
                {
                    if (arr[min].CurrentFloor.FloorIndex > arr[j].CurrentFloor.FloorIndex)
                    {
                        min = j;
                    }
                }
                if (min != i)
                {
                    var lowerValue = arr[min];
                    arr[min] = arr[i];
                    arr[i] = lowerValue;
                }
            }
        }

        public static Elevator GetClosest(Elevator[] arr, int floorIndex)
        {
            Elevator[] newArr = new Elevator[arr.Length];
            for (int i = 0; i < newArr.Length; i++)
            {
                newArr[i] = new Elevator(arr[i]);
            }
            SortByFloor(ref newArr);

            Elevator current = newArr[0];
            int diff = Math.Abs(floorIndex - current.CurrentFloor.FloorIndex);
            for (int i = 0; i < newArr.Length; i++)
            {
                var idxValue = newArr[i];
                var newDiff = Math.Abs(idxValue.CurrentFloor.FloorIndex - floorIndex);
                if (newDiff < diff)
                {
                    diff = newDiff;
                    current = idxValue;
                }
            }
            for (int i = 0; i < arr.Length; i++)
            {
                if (current.ElevatorIndex == arr[i].ElevatorIndex)
                {
                    current = arr[i];
                    break;
                }
            }

            return current;
        }

        public override string ToString()
        {
            return $"Elevator Index : {ElevatorIndex}{Environment.NewLine}" +
                $"Current Floor : {CurrentFloor.FloorIndex}";
        }
    }
}
