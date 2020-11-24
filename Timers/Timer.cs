using System.Collections.Generic;

namespace GS.Timers
{
    public class Timer
    {
        #region Attributes

        private float startTimer;
        private float pauseStart;
        private float pauseTime;
        private bool timerStopped;
        private bool paused;

        public bool Paused
        {
            get { return paused || timerStopped; }
        }
        public float GetTimer
        {
            get { return startTimer + pauseTime; }
        }

        private int laps;
        private float lapTime;
        private float lapPause;

        public int GetLaps
        {
            get { return laps; }
        }
        public float GetLapTime
        {
            get { return lapTime + lapPause; }
        }

        public Timer()
        {
            Reset(0f);
            timerStopped = true;
        }

        #endregion // Attributes

        #region Methods

        public void Reset(float time)
        {
            startTimer = time;
            paused = false;
            pauseStart = 0f;
            pauseTime = 0f;

            lapTime = time;
            lapPause = 0f;
            laps = 0;
        }

        public void Start(string tag, float time)
        {
            Reset(time);
            timerStopped = false;
        }

        public void Lap(string tag, float time)
        {
            if ( paused || timerStopped ) { return; }
            laps++;
            lapTime = time;
            lapPause = 0f;
        }

        public void Pause(string tag, float time)
        {
            if ( timerStopped ) { return; }
            paused = !paused;
            if ( paused )
            {
                pauseStart = time;
            }
            else
            {
                float calc = time - pauseStart;
                pauseTime += calc;
                lapPause += calc;
            }
        }

        public void Resume(string tag, float time)
        {
            if ( timerStopped ) { return; }
            if ( paused )
            {
                paused = false;
                float calc = time - pauseStart;
                pauseTime += calc;
                lapPause += calc;
            }
        }

        public void Stop(string tag, float time)
        {
            timerStopped = true;
        }

        #endregion // Methods
    }
}
