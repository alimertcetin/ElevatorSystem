using System.Timers;

namespace AutamationSystem.ElevatorSystem
{
    public class ElevatorDoor
    {
        public Elevator Elevator { get; private set; } //where this door belongs

        public bool IsOpen { get; private set; }

        public ElevatorDoor(Elevator elevator)
        {
            this.Elevator = elevator;
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
