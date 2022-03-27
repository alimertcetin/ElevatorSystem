using System.Timers;

namespace AutamationSystem.ElevatorSystem
{
    public class ElevatorDoor
    {
        public Elevator Elevator { get; private set; } //where this door belongs

        public bool IsOpen { get; private set; }
        public Timer doorTimer { get; private set; } //how much time it will take to close and open the door
        public double MoveSpeed { get; private set; }

        public ElevatorDoor(Elevator elevator, double moveSpeed)
        {
            this.Elevator = elevator;
            this.MoveSpeed = moveSpeed;
            this.doorTimer = new Timer(MoveSpeed);// every x second timer will fire Timer.Elapsed event
        }

        public void OpenDoor()
        {
            doorTimer.Start();
            doorTimer.Elapsed += DoorTimer_Elapsed; //fires every MoveSpeed second
        }

        public void CloseDoor()
        {
            doorTimer.Start();
            doorTimer.Elapsed += DoorTimer_Elapsed; //fires every MoveSpeed second
        }

        private void DoorTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            doorTimer.Stop();
            doorTimer.Elapsed -= DoorTimer_Elapsed; //unregister to event
                                                    //if IsOpen == true it will be false
                                                    //if IsOpen == false it will be true
            IsOpen = !IsOpen; //reverse open state
        }

    }
}
