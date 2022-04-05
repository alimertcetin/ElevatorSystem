using System;

namespace XIV
{
    public static class Timer
    {
        public static float DeltaTime;

        static TimeSpan previousTime;

        static Timer()
        {
            DeltaTime = 0;
            previousTime = DateTime.Now.TimeOfDay;
        }

        public static void Update()
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            DeltaTime = (float)currentTime.Subtract(previousTime).TotalMilliseconds / 1000;
            previousTime = currentTime;
        }
    }
}