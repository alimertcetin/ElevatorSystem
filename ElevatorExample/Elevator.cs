using AutamationSystem.BuildingSytem;
using AutamationSystem.HumanLogic;
using System;
using System.Collections.Generic;
using System.Timers;
using Direction = AutamationSystem.FloorElevatorIntegration.ElevatorButton.Direction;

namespace AutamationSystem.ElevatorSystem
{
    public class Elevator
    {
        public Building Building { get; private set; }
        public int ElevatorIndex { get; private set; }
        public Floor CurrentFloor { get; private set; }

        private Queue<Floor> TargetFloors { get; set; }
        private ElevatorDoor Door { get; set; }
        private List<Person> personsCarrying = new List<Person>();

        public Elevator(Building building, int elevatorIndex, Floor currentFloor, double moveSpeed = 1)
        {
            this.Building = building;
            this.ElevatorIndex = elevatorIndex;
            this.CurrentFloor = currentFloor;
            this.TargetFloors = new Queue<Floor>();
            this.Door = new ElevatorDoor(this, 1);
        }

        public void Move()
        {
            for (int i = personsCarrying.Count - 1; i >= 0; i--)
            {
                var person = personsCarrying[i];
                if(person.TargetFloorIndex == CurrentFloor.FloorIndex)
                {
                    personsCarrying.RemoveAt(i);
                }
            }

            CurrentFloor.ElevatorMovedAway(this);
            CurrentFloor = TargetFloors.Dequeue();
            CurrentFloor.ElevatorArrived(this);
            Door.Open();

            if (TargetFloors.Count == 0 && Door.IsOpen)
            {
                Door.Close();
            }
        }

        public void TakeTheElevator(Person person)
        {
            personsCarrying.Add(person);
        }

        public override string ToString()
        {
            return $"Elevator Index : {ElevatorIndex}{Environment.NewLine}" +
                $"Current Floor : {CurrentFloor.FloorIndex}";
        }
    }
}
