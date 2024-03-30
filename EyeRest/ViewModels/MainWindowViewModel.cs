using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace EyeRest.ViewModels
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Fields
        private int seconds;
        public Timer timer;
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
            }
        }
        #endregion
        #region Constructor
        public MainWindowViewModel()
        {
            seconds = 0;
            TimeStringToDisplay = "00:00";

        }
        #endregion
        #region Timer
        internal void SetTimer()
        {
            initTime=DateTime.Now;
            timer = new Timer(1000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {      
            seconds += 1;

            if (seconds >55*60 )
            {
                timer.Stop();
                timer.Dispose();
                seconds = 0;
            }

            int tempMinutes=seconds/60;
            int tempSeconds=seconds-tempMinutes*60;
            string minutesString;
            string secondsString;

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
            //if (mainWindowViewModel.timer!=null)
            //    return;
            mainWindowViewModel.TitleStringToDisplay = "Work";
            mainWindowViewModel.SetTimer();
        }
    }
    #endregion

}
