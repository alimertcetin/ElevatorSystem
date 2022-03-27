using AutamationSystem.BuildingSytem;
using System.Timers;

namespace AutamationSystem.ElevatorSystem
{
    public class Elevator
    {
        public Building Building;
        public bool IsMoving { get; private set; }
        public Floor CurrentFloor { get; private set; }
        public Floor TargetFloor { get; private set; }
        public ElevatorDoor Door { get; private set; }
        public double MoveSpeed = 1;
        public Timer moveTimer; //how much time it will take to move

        public Elevator(Building building, Floor currentFloor, double moveSpeed = 1)
        {
            this.Building = building;
            this.CurrentFloor = currentFloor;
            this.MoveSpeed = moveSpeed;
            this.IsMoving = false;
            this.Door = new ElevatorDoor(this, 1);
            this.moveTimer = new Timer(MoveSpeed);
        }

        public void Move(Floor floor)
        {
            TargetFloor = floor;
            if (TargetFloor.FloorIndex == CurrentFloor.FloorIndex)
            {
                //Gets called from same floor
                //Wait for caller
                return;
            }

            if (TargetFloor.FloorIndex < CurrentFloor.FloorIndex)
            {
                //Going down
            }
            else
            {
                //Going up
            }
            moveTimer.Elapsed += MovedOneUnit;
            moveTimer.Start();
        }

        private void MovedOneUnit(object sender, ElapsedEventArgs e)
        {
            //Check if elevator arrived to target floor
            //if arrived open the door
            //else keep moving
        }
    }
}
