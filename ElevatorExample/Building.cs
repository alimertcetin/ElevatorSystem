using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutamationSystem.ElevatorSystem;
using AutamationSystem.FloorElevatorIntegration;
using AutamationSystem.HumanLogic;

namespace AutamationSystem.BuildingSytem
{
    public class Building
    {
        public string Name { get; private set; }
        public Floor[] Floors { get; private set; }
        public Elevator[] Elevators { get; private set; }

        public List<Person> PersonsInBuilding = new List<Person>();

        private List<Floor> waitingForElevatorList = new List<Floor>(20);
        private List<Elevator> arrivedElevators = new List<Elevator>(6);

        public Building(int floorCount, int elevatorCount, params int[] entranceFloorIndexes)
        {
            if (floorCount < 1)
            {
                throw new InvalidOperationException("Floor count cant be less than 1");
            }

            InitializeFloors(floorCount, elevatorCount);
            InıtializeElevators(elevatorCount);
            SetEntranceFloors(entranceFloorIndexes);
        }

        public Building(string buildingName, int floorCount, int elevatorCount, params int[] entranceFloorIndexes)
        {
            if (floorCount < 1)
            {
                throw new InvalidOperationException("Floor count cant be less than 1");
            }
            this.Name = buildingName;
            InitializeFloors(floorCount, elevatorCount);
            InıtializeElevators(elevatorCount);
            SetEntranceFloors(entranceFloorIndexes);
        }

        private void InitializeFloors(int floorCount, int elevatorCount)
        {
            Floors = new Floor[floorCount]; //initialize array
            for (int i = 0; i < floorCount; i++)
            {
                Floors[i] = new Floor(this, i, elevatorCount); //initilize floors inside of the array
                for (int j = 0; j < Floors[i].elevatorButtons.Length; j++)
                {
                    Floors[i].elevatorButtons[j].onButtonPressed += OnElevatorCalled;
                }
            }
        }

        private void OnElevatorCalled(Button button)
        {
            var callButton = button as ElevatorCallButton;

            Elevator selectedElevator = null;
            foreach (Elevator elevator in Elevators)
            {
                Direction directionToThisFloor = elevator.GetDirection(callButton.Floor.FloorIndex);
                if (elevator.TargetFloors.Count > 0)
                {
                    Direction directionToNextFloor = elevator.GetDirection(elevator.TargetFloors.Peek().Floor.FloorIndex);
                    if (directionToThisFloor == directionToNextFloor)
                    {
                        elevator.Call(callButton.Floor, elevator.CurrentDirection);
                        selectedElevator = elevator;
                        break;
                    }
                }
            }
            if (selectedElevator == null)
            {
                var closest = Elevators.GetClosest(callButton.Floor.FloorIndex);
                closest.Call(callButton.Floor, callButton.ButtonDirection);
            }
        }

        private void InıtializeElevators(int elevatorCount)
        {
            Elevators = new Elevator[elevatorCount];
            Floor firstFloor = Floors[0];
            for (int i = 0; i < elevatorCount; i++)
            {
                Elevators[i] = new Elevator(this, i, firstFloor); // all elevators will start at floor 0
                firstFloor.CurrentElevatorsOnFloor.Add(Elevators[i]);
            }
        }

        private void SetEntranceFloors(int[] entranceFloorIndexes)
        {
            for (int i = 0; i < entranceFloorIndexes.Length; i++)
            {
                for (int j = 0; j < Floors.Length; j++)
                {
                    var floor = Floors[i];
                    if (floor.FloorIndex == entranceFloorIndexes[i])
                    {
                        floor.SetEntrance();
                        break;
                    }
                }
            }
        }

        public void ElevatorUpdate()
        {
            arrivedElevators.Clear();
            for (int i = 0; i < Elevators.Length; i++)
            {
                Elevator elevator = Elevators[i];
                if (elevator.Move())
                {
                    arrivedElevators.Add(elevator);
                }
            }

            HandleArrivedElevators();
        }

        private void HandleArrivedElevators()
        {
            for (int i = 0; i < arrivedElevators.Count; i++)
            {
                Elevator elevator = arrivedElevators[i];
                Console.WriteLine($"Elevator is going {elevator.CurrentDirection.ToString()} Enter Target floors to {elevator.ElevatorIndex}. Elevator");
                Console.WriteLine("Input Example : 1,8,6,15 ----- Press Enter to skip...");
                string[] floorNumbersStr = Console.ReadLine().Split(','); //Read input
                int[] floorNumberArr = GetLegitFloorsFromInput(floorNumbersStr);

                if (floorNumberArr.Length != 0)
                {
                    elevator.EnterFloorNumbers(elevator.GetDirection(floorNumberArr[0]), floorNumberArr);
                }
            }
        }

        public bool IsElevatorStateIdle() => arrivedElevators.Count == 0 && !IsSomeoneWaitingForElevator();


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
            return $"" +
                $"-------:-- {this.Name} --:-------{Environment.NewLine}" +
                $"Floor count : {Floors.Length}{Environment.NewLine}" +
                $"Elevator count : {Elevators.Length}";
        }

        private static int[] GetLegitFloorsFromInput(string[] floorNumbersStr)
        {
            int[] floorNumberArr = new int[0];
            for (int i = 0; i < floorNumbersStr.Length; i++)
            {
                if (int.TryParse(floorNumbersStr[i], out var floorNumber))
                {
                    if(i == floorNumberArr.Length)
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
