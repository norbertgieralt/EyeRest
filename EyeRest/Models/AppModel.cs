using System.Timers;

namespace EyeRest.Models
{
    public class AppModel
    {
        #region Fields

		#endregion
		#region Properties		

		private Timer timer;

		public Timer Timer
		{
			get { return timer; }
			set { timer = value; }
		}

        private int secondsOnClock;

        public int SecondsOnClock
        {
            get { return secondsOnClock; }
            set { secondsOnClock = value;}
        }
        private TimerStatus status;

        public TimerStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        #endregion
        #region Constructor
        public AppModel()
        {
            SecondsOnClock= 0;
            Status = TimerStatus.Off;
        }
        #endregion
        #region Methods
        public void StartTimer(int lengthInSeconds)
        {
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }
            timer = new Timer(1000);
            SecondsOnClock = lengthInSeconds;
            timer.Elapsed += onTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
            Status = TimerStatus.On;
        }
        public void PauseTimer()
        {
            Timer.Stop();
            Status = TimerStatus.Paused;
        }
        public void ResumeTimer()
        {
            Timer.Start();
            Status = TimerStatus.On;
        }
        private void onTimedEvent(object source, ElapsedEventArgs e)
        {
            SecondsOnClock--;

            if (secondsOnClock == 0)
            {
                timer.Stop();
                timer.Dispose();
                
            }
        }
        public string TimeOnClockToString()
        {
            int tempMinutes = SecondsOnClock / 60;
            int tempSeconds = SecondsOnClock - tempMinutes * 60;
            string minutesString, secondsString;

            if (tempMinutes == 0)
            {
                minutesString = "00";
            }
            else if (tempMinutes < 10)
            {
                minutesString = "0" + tempMinutes.ToString();
            }
            else
            {
                minutesString = tempMinutes.ToString();
            }

            if (tempSeconds == 0)
            {
                secondsString = "00";
            }
            else if (tempSeconds < 10)
            {
                secondsString = "0" + tempSeconds.ToString();
            }
            else
            {
                secondsString = tempSeconds.ToString();
            }
            return minutesString + " : " + secondsString;
        }
        #endregion
        #region Enums
        public enum TimerStatus
        {
            On, Paused, Off
        }
        #endregion
    }
}
