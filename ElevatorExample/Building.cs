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
                Floors[i] = new Floor(i, elevatorCount); //initilize floors inside of the array
                for (int j = 0; j < Floors[i].elevatorButtons.Length; j++)
                {
                    Floors[i].elevatorButtons[j].onButtonPressed += OnElevatorCalled;
                }
            }
        }

        private void OnElevatorCalled(Button button)
        {
            //TODO : It seems working but needs cleanup
            var callButton = button as ElevatorCallButton;

            List<Elevator> possibleAvailableList = new List<Elevator>();
            foreach (Elevator elevator in Elevators)
            {
                //Direction directionToThisFloor = elevator.GetDirection(callButton.Floor.FloorIndex);
                if (elevator.TargetFloors.Count == 0)
                {
                    possibleAvailableList.Add(elevator);
                }
            }

            Elevator selectedElevator = null;
            var closest = Elevator.GetClosest(Elevators, callButton.Floor.FloorIndex);

            if(closest.TargetFloors.Count == 0)
            {
                selectedElevator = closest;
                selectedElevator.AddTarget(callButton.Floor, callButton.ButtonDirection);
                return;
            }

            if(!possibleAvailableList.Contains(closest)) possibleAvailableList.Add(closest);

            foreach (Elevator elevator in possibleAvailableList)
            {
                if(elevator.TargetFloors.Count > 0 &&
                    callButton.ButtonDirection == elevator.CurrentDirection &&
                    elevator.GetDirection(callButton.Floor.FloorIndex) == elevator.CurrentDirection)
                {
                    selectedElevator = elevator;
                    break;
                }
                if (elevator.TargetFloors.Count == 0)
                {
                    selectedElevator = elevator;
                    break;
                }
            }

            if (selectedElevator == null)
            {
                selectedElevator = closest;
                /*
                 * Available conditions :
                 *      - Going to buttons direction ex : Up and elevator's direction to floor is also same (Up)
                 *      - If there is no elevator that matches the conditions
                 *          - Add to queue and next frame try to cleanup?
                 */
            }
            selectedElevator.AddTarget(callButton.Floor, callButton.ButtonDirection);
        }

        private void InıtializeElevators(int elevatorCount)
        {
            Elevators = new Elevator[elevatorCount];
            Floor firstFloor = Floors[0];
            for (int i = 0; i < elevatorCount; i++)
            {
                Elevators[i] = new Elevator(i, firstFloor, this.Floors); // all elevators will start at floor 0
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

        public void ElevatorUpdate(out List<Elevator> arrivedElevators)
        {
            arrivedElevators = new List<Elevator>();
            for (int i = 0; i < Elevators.Length; i++)
            {
                Elevator elevator = Elevators[i];
                if (elevator.Move())
                {
                    arrivedElevators.Add(elevator);
                }
            }
        }

        public bool IsSomeoneWaitingForElevator()
        {
            foreach (Elevator elevator in Elevators)
            {
                if(elevator.TargetFloors.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }


        public override string ToString()
        {
            return $"" +
                $"-------:-- {this.Name} --:-------{Environment.NewLine}" +
                $"Floor count : {Floors.Length}{Environment.NewLine}" +
                $"Elevator count : {Elevators.Length}";
        }
    }
}
