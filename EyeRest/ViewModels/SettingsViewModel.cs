using EyeRest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EyeRest.ViewModels
{
    internal class SettingsViewModel : BaseViewModel
    {
        #region Fields and Properties
        private MainWindowViewModel mainWindowViewModel;
        #endregion
        #region Constructor
        public SettingsViewModel(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }
        #endregion
        #region Methods
        private void back()
        {
            mainWindowViewModel.ActiveViewModel = mainWindowViewModel.ViewModels[0];
        }
        #endregion
        #region Commands
        public ICommand BackFromSettingsCommand
        {
            get
            {
                return new BaseCommand(back);
            }
        }
        #endregion
    }
}
