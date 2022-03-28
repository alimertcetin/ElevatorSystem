using AutamationSystem.ElevatorSystem;
using AutamationSystem.FloorElevatorIntegration;
using AutamationSystem.HumanLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using static AutamationSystem.FloorElevatorIntegration.ElevatorFloorButton;

namespace AutamationSystem.BuildingSytem
{
    public class Floor
    {
        public Building Building;
        public int FloorIndex { get; private set; }
        public List<Elevator> CurrentElevatorsOnFloor;
        public bool EntranceFloor { get; private set; }
        public ElevatorFloorButton[] elevatorButtons;

        public Floor(Building building, int floorIndex, int elevatorButtonCount)
        {
            this.Building = building;
            this.FloorIndex = floorIndex;

            elevatorButtons = new ElevatorFloorButton[elevatorButtonCount];
            for (int i = 0; i < elevatorButtonCount; i++)
            {
                elevatorButtons[i] = new ElevatorFloorButton(this);
            }

            this.CurrentElevatorsOnFloor = new List<Elevator>();
        }

        public void CallElevator()
        {
            Elevator selectedElevator = null;
            foreach (Elevator elevator in Building.Elevators)
            {
                var direction = elevator.GetDirection(this.FloorIndex);
                if(elevator.TargetFloors.Count > 0)
                {
                    Direction elevatorDir = elevator.GetDirection(elevator.TargetFloors.Peek().Floor.FloorIndex);
                    if(elevatorDir == direction)
                    {
                        elevator.Call(this);
                        selectedElevator = elevator;
                        break;
                    }
                }
            }
            if(selectedElevator == null)
            {
                Building.Elevators.GetClosest(this.FloorIndex).Call(this);
            }
        }

        public void ElevatorMovedAway(Elevator elevator)
        {
            CurrentElevatorsOnFloor.Remove(elevator); //TODO : Use RemoveAt
        }

        public void ElevatorArrived(Elevator elevator)
        {
            for (int i = 0; i < elevatorButtons.Length; i++)
            {
                elevatorButtons[i].SetButtonStateToNone();
            }
            CurrentElevatorsOnFloor.Add(elevator);
        }

        public void SetEntrance()
        {
            this.EntranceFloor = true;
        }

        public override string ToString()
        {
            return $"Floor Index : {this.FloorIndex}{Environment.NewLine}" +
                $"Is Entrance : {this.EntranceFloor}{Environment.NewLine}" +
                $"Current Elevators On Floor : {CurrentElevatorsOnFloor.Count}";
        }
    }
}
