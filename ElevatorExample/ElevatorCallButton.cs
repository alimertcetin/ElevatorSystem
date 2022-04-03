using AutamationSystem.BuildingSytem;

namespace AutamationSystem.FloorElevatorIntegration
{
    public enum Direction { None, Up, Down };

    public class ElevatorCallButton : Button
    {
        public Floor Floor { get; private set; }
        public Direction ButtonDirection { get; private set; }

        public ElevatorCallButton(Floor floor, Direction buttonDirection)
        {
            this.Floor = floor;
            this.ButtonDirection = buttonDirection;
        }
    }
}
