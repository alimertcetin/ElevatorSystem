using System;

namespace AutamationSystem.FloorElevatorIntegration
{
    public class Button
    {
        public bool IsPressed { get; protected set; }

        public Action<Button> onButtonPressed;
        public Action<Button> onButtonClear;

        public virtual void Press()
        {
            if (IsPressLegit())
            {
                IsPressed = true;
                onButtonPressed?.Invoke(this);
            }
        }

        public virtual void Clear()
        {
            IsPressed = false;
            onButtonClear?.Invoke(this);
        }

        public virtual bool IsPressLegit()
        {
            return !IsPressed;
        }
    }
}
