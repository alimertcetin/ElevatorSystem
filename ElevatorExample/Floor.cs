using AutamationSystem.ElevatorSystem;
using AutamationSystem.FloorElevatorIntegration;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AutamationSystem.BuildingSytem
{
    public class Floor
    {
        public int FloorIndex { get; private set; }
        public List<Elevator> CurrentElevatorsOnFloor;
        public bool EntranceFloor { get; private set; }
        public ElevatorCallButton[] elevatorButtons;

        public Floor(int floorIndex, int elevatorCount)
        {
            this.FloorIndex = floorIndex;

            elevatorButtons = new ElevatorCallButton[elevatorCount * 2];
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
            bool buttonPressed = false;
            for (int i = 0; i < elevatorButtons.Length; i++)
            {
                ElevatorCallButton button = elevatorButtons[i];
                if(button.ButtonDirection == dir)
                {
                    if (!buttonPressed && button.IsPressLegit())
                    {
                        //Press all buttons that shows same direction
                        button.Press();
                        buttonPressed = true;
                        continue;
                    }
                    button.Disable();
                }
            }
        }

        public void ElevatorMovedAway(Elevator elevator)
        {
            for (int i = 0; i < CurrentElevatorsOnFloor.Count; i++)
            {
                if(CurrentElevatorsOnFloor[i] == elevator)
                {
                    CurrentElevatorsOnFloor.RemoveAt(i);
                    break;
                }
            }
        }

        public void ElevatorArrived(Elevator elevator, Direction direction)
        {
            for (int i = 0; i < elevatorButtons.Length; i++)
            {
                ElevatorCallButton button = elevatorButtons[i];
                if (button.ButtonDirection == direction)
                {
                    if (!button.Enabled) button.Enable();

                    button.Clear();
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
