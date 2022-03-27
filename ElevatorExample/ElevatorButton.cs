using AutamationSystem.BuildingSytem;
using System;

namespace AutamationSystem.FloorElevatorIntegration
{
    public class ElevatorButton
    {
        public enum Direction { None, Up, Down };

        public Floor Floor { get; private set; }
        public Direction PressedDirection { get; private set; }

        public ElevatorButton(Floor floor)
        {
            this.Floor = floor;
            PressedDirection = Direction.None;
        }

        public bool IsPressed() => PressedDirection != Direction.None;

        public void Press(Direction direction)
        {
            if (direction == Direction.None) throw new InvalidOperationException("You cant press none direction");

            PressedDirection = direction;
        }

        public void SetButtonStateToNone() => PressedDirection = Direction.None;
    }
}
