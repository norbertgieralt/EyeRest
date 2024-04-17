using EyeRest.Helpers;
using EyeRest.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Cryptography.Xml;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using static EyeRest.Models.AppModel;

namespace EyeRest.ViewModels
{
    internal class MainWindowViewModel : BaseViewModel
    {
        #region Fields and Properties
        private ObservableCollection<BaseViewModel> viewModels;

        public ObservableCollection<BaseViewModel> ViewModels
        {
            get { return viewModels; }
            set { viewModels = value; }
        }        

        private BaseViewModel activeViewModel;

        public BaseViewModel ActiveViewModel
        {
            get { return activeViewModel; }
            set
            {
                activeViewModel = value;
                OnPropertChanged("ActiveViewModel");
            }
        }
        private MainWindowViewModel mainWindowViewModel;

        private System.Timers.Timer rerfeshingTimer;

        private AppModel model;

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
                model.SecondsOnClock = value;
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
                return TimeStringToDisplay + titleStringToDisplay;
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
            ViewModels = new ObservableCollection<BaseViewModel>();
            ViewModels.Add(new ClockScreenViewModel());
            ViewModels.Add(new SettingsViewModel());

            ShowClock();
            model = new AppModel();
            TimeStringToDisplay = "00:00";
            TitleStringToDisplay = "Hello!";
            LabelInFirstButton = "Hello";

        }
        #endregion
        #region Methods
        private void startRefreshingTimer()
        {
            if (rerfeshingTimer != null)
            {
                rerfeshingTimer.Stop();
                rerfeshingTimer.Dispose();
            }

            rerfeshingTimer = new System.Timers.Timer(1000);
            rerfeshingTimer.Elapsed += onTimedEvent;
            rerfeshingTimer.AutoReset = true;
            rerfeshingTimer.Enabled = true;
        }
        private void onTimedEvent(object source, ElapsedEventArgs e)
        {
            OnPropertChanged("Seconds");
            OnPropertChanged("TimeStringToDisplay");
            OnPropertChanged("TimeStringToDisplay2");

            if (Seconds == 0)
            {
                TimeStringToDisplay = "00 : 00";
                OnPropertChanged("TimeStringToDisplay");
                OnPropertChanged("TimeStringToDisplay2");
                if (TitleStringToDisplay == "Work")
                {
                    StartBreakCommand.Execute(this);
                    TitleStringToDisplay = "Break";
                }
                else if (TitleStringToDisplay == "Break")
                {
                    StartWorkCommand.Execute(this);
                    TitleStringToDisplay = "Work";
                }
            }
        }
        private void startWork()
        {
            if (Timer != null)
            {
                Timer.Stop();
                Timer.Dispose();
                TimeStringToDisplay = "00 : 00";
            }
            TitleStringToDisplay = "Work";
            Model.StartTimer(55 * 60);
            startRefreshingTimer();
            Status = TimerStatus.On;
            LabelInFirstButton = "Pause";

            Console.Beep(500, 200);
            Console.Beep(1000, 200);
            MessageBox.Show("It's time to work.");
        }
        private void startBreak()
        {
            if (Timer != null)
            {
                Timer.Stop();
                Timer.Dispose();
                TimeStringToDisplay = "00 : 00";
            }
            TitleStringToDisplay = "Break";
            Model.StartTimer(5 * 60);
            startRefreshingTimer();
            Status = TimerStatus.Paused;

            LabelInFirstButton = "Pause";

            Console.Beep(1000, 200);
            Console.Beep(500, 200);
            MessageBox.Show("It's time to have a break.");
        }
        private void startPauseOrResume()
        {
            if (Timer == null)
                return;
            if (Status == TimerStatus.On)
            {
                Model.PauseTimer();
                TitleStringToDisplay += " (Pause)";
                LabelInFirstButton = "Resume";

            }
            else
            {
                Model.ResumeTimer();
                LabelInFirstButton = "Pause";
                if (TitleStringToDisplay.StartsWith('W'))
                    TitleStringToDisplay = "Work";
                else
                    TitleStringToDisplay = "Break";
            }
        }
        private void quit()
        {
            if (MessageBox.Show("Are you sure to quit?", "Quit", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                App.Current.Shutdown();
        }
        private void showSettings()
        {
            ActiveViewModel = ViewModels[1];
        }
        private void ShowClock()
        {
            ActiveViewModel= ViewModels[0];
        }
        #endregion       
        #region Commands
        public ICommand StartWorkCommand
        {
            get
            {
                return new BaseCommand(startWork);
            }
        }
        public ICommand StartBreakCommand
        {
            get
            {
                return new BaseCommand(startBreak);
            }
        }
        public ICommand StartPauseOrResumeCommand
        {
            get
            {
                return new BaseCommand(startPauseOrResume);
            }
        }
        public ICommand QuitCommand
        {
            get
            {
                return new BaseCommand(quit);
            }
        }
        public ICommand ShowSettingsCommand
        {
            get
            {
                return new BaseCommand(showSettings);
            }
        }
        public ICommand ShowClockCommand
        {
            get
            {
                return new BaseCommand(ShowClock);
            }
        }
        #endregion

    }

}
