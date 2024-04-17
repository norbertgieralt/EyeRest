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
        #endregion
        #region Constructor
        public MainWindowViewModel()
        {
            ViewModels = new ObservableCollection<BaseViewModel>();
            ViewModels.Add(new ClockScreenViewModel(this));
            ViewModels.Add(new SettingsViewModel(this));

            ActiveViewModel = ViewModels[0];

        }
        #endregion
        
    }

}
