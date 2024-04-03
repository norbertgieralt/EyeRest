using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace EyeRest.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Fields
        internal int seconds;
        internal Timer timer;
        internal string timerStatus;
        
        #endregion
        #region Properties       

        private DateTime initTime;
        public DateTime InitTime
        {
            get
            {
                return initTime;
            }
            set
            {
                initTime = value;
                OnPropertChanged("InitTime");
            }
        }
        private string titleStringToDisplay;

        public string TitleStringToDisplay
        {
            get
            {
                return titleStringToDisplay;
            }
            set
            {
                titleStringToDisplay = value;
                OnPropertChanged("TitleStringToDisplay");
            }
        }
        private string titleStringToDisplay2;

        public string TitleStringToDisplay2
        {
            get
            {
                return titleStringToDisplay2;
            }
            set
            {
                titleStringToDisplay2 = value;
                OnPropertChanged("TitleStringToDisplay2");
            }
        }

        private string timeStringToDisplay;

        public string TimeStringToDisplay
        {
            get
            { 
                return timeStringToDisplay; 
            }
            set 
            {
                timeStringToDisplay = value;
                OnPropertChanged("TimeStringToDisplay");
                OnPropertChanged("TimeStringToDisplay2");
            }
        }
        private string timeStringToDisplay2;

        public string TimeStringToDisplay2
        {
            get
            {
                return "EyeRest "+TimeStringToDisplay;
            }
            set
            {
                timeStringToDisplay2 = value;
                OnPropertChanged("TimeStringToDisplay2");
            }
        }
        #endregion
        #region Constructor
        public MainWindowViewModel()
        {
            seconds = 0;
            TimeStringToDisplay = "00:00";
            TitleStringToDisplay = "Hello!";
            titleStringToDisplay2 = "Hello";            
        }
        #endregion
        #region Timer
        internal void SetTimer(int secs)
        {
            seconds = secs;
            initTime=DateTime.Now;
            timer = new Timer(1000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;

        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {      
            seconds -= 1;

            if (seconds <=0 )
            {
                timer.Stop();
                timer.Dispose();
                seconds = 0;
                if (TitleStringToDisplay == "Work")
                {
                    StartBreakCommand.Execute(seconds);
                }
                else
                {
                    StartWorkCommand.Execute(seconds);
                }
            }

            int tempMinutes=seconds/60;
            int tempSeconds=seconds-tempMinutes*60;
            string minutesString, secondsString;

            if (tempMinutes==0)
            {
                minutesString = "00";
            }
            else if (tempMinutes<10)
            {
                minutesString= "0"+tempMinutes.ToString();
            }
            else
            {
                minutesString= tempMinutes.ToString();
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
            TimeStringToDisplay = minutesString + " : " + secondsString;            
        }
        #endregion
        #region Commands
        private ICommand startWorkCommand;
        public ICommand StartWorkCommand
        {
            get
            {
                if (startWorkCommand == null) startWorkCommand = new StartWorkCommandClass(this);
                return startWorkCommand;
            }
        }
        private ICommand startBreakCommand;
        public ICommand StartBreakCommand
        {
            get
            {
                if (startBreakCommand == null) startBreakCommand = new StartBreakCommandClass(this);
                return startBreakCommand;
            }
        }
        private ICommand startPauseOrResumeCommand;
        public ICommand StartPauseOrResumeCommand
        {
            get
            {
                if (startPauseOrResumeCommand == null) startPauseOrResumeCommand = new StartPauseOrResumeCommandClass(this);
                return startPauseOrResumeCommand;
            }
        }
        #endregion
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertChanged(params string[] args)
        {
            if (PropertyChanged != null)
            {
                foreach (string arg in args)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(arg));
                }
            }
        }
        #endregion
    }
    #region CommandClasses    
    internal class StartWorkCommandClass : ICommand
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        public StartWorkCommandClass(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (mainWindowViewModel.timer != null)
            {
                mainWindowViewModel.timer.Stop();
                mainWindowViewModel.timer.Dispose();
                mainWindowViewModel.TimeStringToDisplay = "00 : 00";
                //mainWindowViewModel.seconds = 0;

            }
            mainWindowViewModel.TitleStringToDisplay = "Work";
            mainWindowViewModel.SetTimer(6);
            mainWindowViewModel.timerStatus = "On";
            mainWindowViewModel.TitleStringToDisplay2 = "Pause";
            Console.Beep(500, 200);
            Console.Beep(1000, 200);
            MessageBox.Show("It's time to work.");            
        }
    }
    internal class StartBreakCommandClass : ICommand
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        public StartBreakCommandClass(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (mainWindowViewModel.timer != null)
            {
                mainWindowViewModel.timer.Stop();
                mainWindowViewModel.timer.Dispose();
                mainWindowViewModel.TimeStringToDisplay = "00 : 00";
                //mainWindowViewModel.seconds = 0;
            }

            mainWindowViewModel.TitleStringToDisplay = "Break";
            //mainWindowViewModel.SetTimer(5*60);
            mainWindowViewModel.SetTimer(3);
            mainWindowViewModel.timerStatus = "On";
            mainWindowViewModel.TitleStringToDisplay2 = "Pause";

            Console.Beep(1000, 200);
            Console.Beep(500, 200);
            MessageBox.Show("It's time to have a break.");
        }
    }
    internal class StartPauseOrResumeCommandClass : ICommand
    {
        private readonly MainWindowViewModel mainWindowViewModel;
        public StartPauseOrResumeCommandClass(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (mainWindowViewModel.timer == null)
                return;            
            if (mainWindowViewModel.timerStatus=="On")
            {
                mainWindowViewModel.timer.Stop();
                mainWindowViewModel.timerStatus = "Off";
                mainWindowViewModel.TitleStringToDisplay += " (Pause)";
                mainWindowViewModel.TitleStringToDisplay2 = "Resume";

            }
            else
            {
                mainWindowViewModel.timer.Start();
                mainWindowViewModel.timerStatus = "On";
                mainWindowViewModel.TitleStringToDisplay2 = "Pause";
                if (mainWindowViewModel.TitleStringToDisplay.StartsWith('W'))
                    mainWindowViewModel.TitleStringToDisplay = "Work";
                else
                    mainWindowViewModel.TitleStringToDisplay = "Break";
            }
        }
    }
    #endregion

}
