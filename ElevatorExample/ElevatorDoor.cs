using System.Timers;

namespace AutamationSystem.ElevatorSystem
{
    public class ElevatorDoor
    {
        public Elevator Elevator { get; private set; } //where this door belongs

        public bool IsOpen { get; private set; }
        public double MoveSpeed { get; private set; }

        public ElevatorDoor(Elevator elevator, double moveSpeed)
        {
            this.Elevator = elevator;
            this.MoveSpeed = moveSpeed;
        }

        public void Open()
        {
            IsOpen = true; //reverse open state
            System.Console.WriteLine("Door opened");
        }

        public void Close()
        {
            IsOpen = false; //reverse open state
            System.Console.WriteLine("Door closed");
        }

    }
}
