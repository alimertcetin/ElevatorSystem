using AutamationSystem.BuildingSytem;
using AutamationSystem.ElevatorSystem;

namespace AutamationSystem.FloorElevatorIntegration
{
    //TODO : Integrate to Elevator
    public class ElevatorFloorButton : Button
    {
        public Elevator Elevator { get; private set; }
        public Floor Floor { get; private set; }

        public ElevatorFloorButton(Elevator elevator, Floor floor)
        {
            this.Elevator = elevator;
            this.Floor = floor;
        }
    }
}
