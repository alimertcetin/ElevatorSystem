using AutamationSystem.ElevatorSystem;
using AutamationSystem.HumanLogic;
using System.Collections.Generic;

namespace AutamationSystem.BuildingSytem
{
    public class Floor
    {
        public Building Building;
        public int FloorIndex { get; private set; }
        public List<Person> PeopleWaiting;
        public List<Elevator> CurrentElevatorsOnFloor;
        public bool EntranceFloor { get; private set; }

        public Floor(Building building, int floorIndex)
        {
            this.Building = building;
            this.FloorIndex = floorIndex;

            this.PeopleWaiting = new List<Person>();
            this.CurrentElevatorsOnFloor = new List<Elevator>();
        }

        public void SetEntrance()
        {
            this.EntranceFloor = true;
        }
    }
}
