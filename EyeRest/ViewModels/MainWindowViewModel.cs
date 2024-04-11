using EyeRest.Models;
using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using static EyeRest.Models.AppModel;

namespace EyeRest.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        #region Fields

        private System.Timers.Timer rerfeshingTimer;

        private AppModel model;              

        #endregion
        #region Properties  
        public AppModel Model
        {
            get { return model; }
            set { model = value; }
        }
        public System.Timers.Timer Timer
        {
            get { return model.Timer; }
            set { model.Timer = value; }
        }

        private int seconds;

        public int Seconds
        {
            get { return model.SecondsOnClock; }
            set 
            { 
                model.SecondsOnClock=value;
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
        private string labelInFirstButton;

        public string LabelInFirstButton
        {
            get
            {
                return labelInFirstButton;
            }
            set
            {
                labelInFirstButton = value;
                OnPropertChanged("LabelInFirstButton");
            }
        }

        private string timeStringToDisplay;

        public string TimeStringToDisplay
        {
            get
            { 
                return model.TimeOnClockToString(); 
            }
            set 
            {
                timeStringToDisplay = value;
                OnPropertChanged("TimeStringToDisplay");
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
            }
        }

        private TimerStatus status;

        public TimerStatus Status
        {
            get { return model.Status; }
            set 
            { 
                model.Status = value;
                OnPropertChanged("Status");
            }
        }

        #endregion
        #region Constructor
        public MainWindowViewModel()
        {
            model = new AppModel();
            TimeStringToDisplay = "00:00";
            TitleStringToDisplay = "Hello!";
            LabelInFirstButton = "Hello";
        }
        #endregion
        #region Methods
        internal void StartRefreshingTimer()
        {
            if (rerfeshingTimer!=null)
            {
                rerfeshingTimer.Stop();
                rerfeshingTimer.Dispose();
            }
            
            rerfeshingTimer = new System.Timers.Timer(1000);
            rerfeshingTimer.Elapsed += OnTimedEvent;
            rerfeshingTimer.AutoReset = true;
            rerfeshingTimer.Enabled = true;            
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            OnPropertChanged("Seconds");
            OnPropertChanged("TimeStringToDisplay");
            OnPropertChanged("TimeStringToDisplay2");

            if (Seconds ==0)
            {
                TimeStringToDisplay = "00 : 00";
                OnPropertChanged("TimeStringToDisplay");
                OnPropertChanged("TimeStringToDisplay2");
                if (TitleStringToDisplay=="Work")
                {
                    StartBreakCommand.Execute(this);
                    TitleStringToDisplay = "Break";
                }
                else if (TitleStringToDisplay == "Break")
                {
                    startWorkCommand.Execute(this);
                    TitleStringToDisplay = "Work";
                }
            }
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
        private ICommand quitCommand;
        public ICommand QuitCommand
        {
            get
            {
                if (quitCommand == null) quitCommand = new QuitCommandClass();
                return quitCommand;
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
            if (mainWindowViewModel.Timer != null)
            {
                mainWindowViewModel.Timer.Stop();
                mainWindowViewModel.Timer.Dispose();
                mainWindowViewModel.TimeStringToDisplay = "00 : 00";
            }
            mainWindowViewModel.TitleStringToDisplay = "Work";
            mainWindowViewModel.Model.StartTimer(55*60);
            mainWindowViewModel.StartRefreshingTimer();            
            mainWindowViewModel.Status = TimerStatus.On;
            mainWindowViewModel.LabelInFirstButton = "Pause";

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
            if (mainWindowViewModel.Timer != null)
            {
                mainWindowViewModel.Timer.Stop();
                mainWindowViewModel.Timer.Dispose();
                mainWindowViewModel.TimeStringToDisplay = "00 : 00";
            }
            mainWindowViewModel.TitleStringToDisplay = "Break";
            mainWindowViewModel.Model.StartTimer(5*60);
            mainWindowViewModel.StartRefreshingTimer();
            mainWindowViewModel.Status = TimerStatus.Paused;

            mainWindowViewModel.LabelInFirstButton = "Pause";

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
            if (mainWindowViewModel.Timer == null)
                return;
            if (mainWindowViewModel.Status == TimerStatus.On)
            {
                mainWindowViewModel.Model.PauseTimer();
                mainWindowViewModel.TitleStringToDisplay += " (Pause)";
                mainWindowViewModel.LabelInFirstButton = "Resume";

            }
            else
            {
                mainWindowViewModel.Model.ResumeTimer();
                mainWindowViewModel.LabelInFirstButton = "Pause";
                if (mainWindowViewModel.TitleStringToDisplay.StartsWith('W'))
                    mainWindowViewModel.TitleStringToDisplay = "Work";
                else
                    mainWindowViewModel.TitleStringToDisplay = "Break";
            }
        }
    }
    internal class QuitCommandClass : ICommand
    {        
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
           if (MessageBox.Show("Are you sure to quit?","Quit",MessageBoxButton.YesNo,MessageBoxImage.Question)== MessageBoxResult.Yes)
                App.Current.Shutdown();
        }
    }
    #endregion

}
