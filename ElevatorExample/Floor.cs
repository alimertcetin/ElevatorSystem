using AutamationSystem.ElevatorSystem;
using AutamationSystem.FloorElevatorIntegration;
using AutamationSystem.HumanLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using static AutamationSystem.FloorElevatorIntegration.ElevatorCallButton;

namespace AutamationSystem.BuildingSytem
{
    public class Floor
    {
        public Building Building;
        public int FloorIndex { get; private set; }
        public List<Elevator> CurrentElevatorsOnFloor;
        public bool EntranceFloor { get; private set; }
        public ElevatorCallButton[] elevatorButtons;

        public Floor(Building building, int floorIndex, int elevatorButtonCount)
        {
            this.Building = building;
            this.FloorIndex = floorIndex;

            elevatorButtons = new ElevatorCallButton[elevatorButtonCount * 2];
            Direction startDir = Direction.Down;
            for (int i = 0; i < elevatorButtons.Length; i++)
            {
                if (startDir == Direction.Up) startDir = Direction.Down;
                else startDir = Direction.Up;
                elevatorButtons[i] = new ElevatorCallButton(this, startDir);
            }

            this.CurrentElevatorsOnFloor = new List<Elevator>();
        }

        public void CallElevator(Direction dir)
        {
            for (int i = 0; i < elevatorButtons.Length; i++)
            {
                var button = elevatorButtons[i];
                if(button.ButtonDirection == dir && button.IsPressLegit())
                {
                    button.Press();
                    break;
                }
            }
        }

        public void ElevatorMovedAway(Elevator elevator)
        {
            CurrentElevatorsOnFloor.Remove(elevator); //TODO : Use RemoveAt
        }

        public void ElevatorArrived(Elevator elevator, Direction direction)
        {
            for (int i = 0; i < elevatorButtons.Length; i++)
            {
                if (elevatorButtons[i].ButtonDirection == direction)
                {
                    elevatorButtons[i].Clear();
                }
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
