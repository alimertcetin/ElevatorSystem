using System;

namespace AutamationSystem.FloorElevatorIntegration
{
    public class Button
    {
        public bool IsPressed { get; protected set; }
        public bool Enabled { get; protected set; }

        public Action<Button> onButtonPressed;
        public Action<Button> onButtonClear;

        public Button()
        {
            this.Enabled = true;
        }

        public virtual void Press()
        {
            if (Enabled && IsPressLegit())
            {
                IsPressed = true;
                onButtonPressed?.Invoke(this);
            }
        }

        public virtual void Enable()
        {
            Enabled = true;
        }

        public virtual void Disable()
        {
            Enabled = false;
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
