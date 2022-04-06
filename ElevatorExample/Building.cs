using AutamationSystem.ElevatorSystem;
using AutamationSystem.FloorElevatorIntegration;
using AutamationSystem.HumanLogic;
using System;
using System.Collections.Generic;

namespace AutamationSystem.BuildingSytem
{
    public class Building
    {
        public string Name { get; private set; }
        public Floor[] Floors { get; private set; }
        public Elevator[] Elevators { get; private set; }

        //public List<Person> PersonsInBuilding = new List<Person>();
        //private List<Floor> waitingForElevatorList = new List<Floor>(20);

        public Building(Floor[] floors, Elevator[] elevators, string buildingName = "Default")
        {
            this.Name = buildingName;
            this.Floors = floors;
            this.Elevators = elevators;
            RegisterToElevatorCall();
        }

        private void RegisterToElevatorCall()
        {
            for (int i = 0; i < Floors.Length; i++)
            {
                for (int j = 0; j < Floors[i].elevatorButtons.Length; j++)
                {
                    Floors[i].elevatorButtons[j].onButtonPressed += OnElevatorCalled;
                }
            }
        }

        private void OnElevatorCalled(Button button)
        {
            //TODO : It seems working but needs cleanup
            /*
             * Available conditions :
             *      - Going to buttons direction ex : Up and elevator's direction to floor is also same (Up)
             *      - If there is no elevator that matches the conditions
             *          - Add to queue and next frame try to cleanup?
             */
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

            if (closest.TargetFloors.Count == 0)
            {
                selectedElevator = closest;
                selectedElevator.AddTarget(callButton.Floor, callButton.ButtonDirection);
                return;
            }

            if (!possibleAvailableList.Contains(closest)) possibleAvailableList.Add(closest);

            foreach (Elevator elevator in possibleAvailableList)
            {
                if (elevator.TargetFloors.Count > 0 &&
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
            }
            selectedElevator.AddTarget(callButton.Floor, callButton.ButtonDirection);
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
                if (elevator.TargetFloors.Count > 0)
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
