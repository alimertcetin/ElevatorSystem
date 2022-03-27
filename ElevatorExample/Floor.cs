using AutamationSystem.ElevatorSystem;
using AutamationSystem.FloorElevatorIntegration;
using AutamationSystem.HumanLogic;
using System;
using System.Collections;
using System.Collections.Generic;
using static AutamationSystem.FloorElevatorIntegration.ElevatorButton;

namespace AutamationSystem.BuildingSytem
{
    public class Floor
    {
        public Building Building;
        public int FloorIndex { get; private set; }
        public List<Elevator> CurrentElevatorsOnFloor;
        public bool EntranceFloor { get; private set; }
        public ElevatorButton[] elevatorButtons;
        List<Person> persons = new List<Person>();

        public Floor(Building building, int floorIndex, int elevatorButtonCount)
        {
            this.Building = building;
            this.FloorIndex = floorIndex;

            elevatorButtons = new ElevatorButton[elevatorButtonCount];
            for (int i = 0; i < elevatorButtonCount; i++)
            {
                elevatorButtons[i] = new ElevatorButton(this);
            }

            this.CurrentElevatorsOnFloor = new List<Elevator>();
        }

        /// <summary>
        /// How many buttons has pressed
        /// </summary>
        /// <returns>Returns how many buttons has pressed</returns>
        public int HowManyElevatorCalled()
        {
            int counter = 0;
            for (int i = 0; i < elevatorButtons.Length; i++)
            {
                if (elevatorButtons[i].IsPressed())
                {
                    counter++;
                }
            }
            return counter;
        }

        public void CallElevator(Direction direction, Person person)
        {
            persons.Add(person);
            for (int i = 0; i < elevatorButtons.Length; i++)
            {
                if (!elevatorButtons[i].IsPressed())
                {
                    elevatorButtons[i].Press(direction);
                    break;
                }
            }
        }

        public void TakeElevator(Person person)
        {
            persons.Remove(person);
            for (int i = 0; i < CurrentElevatorsOnFloor.Count; i++)
            {
                Elevator elevator = CurrentElevatorsOnFloor[i];
                //TODO : Check weight
                elevator.TakeTheElevator(person);
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
