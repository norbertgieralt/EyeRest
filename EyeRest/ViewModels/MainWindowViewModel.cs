using EyeRest.Helpers;
using EyeRest.Models;
using EyeRest.Models.Languages;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Security.Cryptography.Xml;
using System.Timers;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.UI.Notifications;
using static EyeRest.Models.AppModel;
using static EyeRest.Models.Languages.Languages;

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
                OnPropertyChanged("ActiveViewModel");
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
                OnPropertyChanged("TitleStringToDisplay");
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
                OnPropertyChanged("LabelInFirstButton");
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
                OnPropertyChanged("TimeStringToDisplay");
            }
        }
        private string timeStringToDisplay2;

        public string TimeStringToDisplay2
        {
            get
            {
                return TimeStringToDisplay + " "+titleStringToDisplay;
            }
            set
            {
                timeStringToDisplay2 = value;
            }
        }
        private AppStatus appStatus;

        public AppStatus AppStatus
        {
            get { return appStatus; }
            set { appStatus = value; }
        }


        private TimerStatus status;

        public TimerStatus Status
        {
            get { return model.Status; }
            set
            {
                model.Status = value;
                OnPropertyChanged("Status");
            }
        }
        private int workPeriodInMinutes;

        public int WorkPeriodInMinutes
        {
            get { return workPeriodInMinutes; }
            set 
            { 
                workPeriodInMinutes = value;
                OnPropertyChanged("WorkPeriodInMinutes");
                saveSettings("workPeriodInMinutes", WorkPeriodInMinutes);
            }
        }
        private int breakPeriodInMinutes;

        public int BreakPeriodInMinutes
        {
            get { return breakPeriodInMinutes; }
            set
            {
                breakPeriodInMinutes = value;
                OnPropertyChanged("BreakPeriodInMinutes");
                saveSettings("breakPeriodInMinutes", BreakPeriodInMinutes);
            }
        }
        private Dictionary<string,string> translations;

        public Dictionary<string, string> Translations
        {
            get { return translations; }
            set
            {
                translations = value;
                OnPropertyChanged("Translations");
            }
        }
        private Language language;

        public Language Language
        {
            get { return language; }
            set
            {
                language = value;
                OnPropertyChanged("Language");
                if (Translations!=null)
                {
                    saveSettings("language", Language.Name);
                    Translations = GetTranslationsDictionary(Language);                    
                    foreach (Language item in PossibleLanguages)
                    {
                        item.SetTranslation(Translations);                        
                    }
                    foreach (SoundString item in SoundsStrings)
                    {
                        item.SetTranslation(Translations);
                    }
                    if (AppStatus == AppStatus.Initial)
                    {
                        TitleStringToDisplay = Translations["Hello!"];
                    }
                    else if (AppStatus == AppStatus.Work)
                    {
                        TitleStringToDisplay = Translations["Work"];
                        LabelInFirstButton = Translations["Pause"];
                    }
                    else if (AppStatus == AppStatus.Break)
                    {
                        TitleStringToDisplay = Translations["Break"];
                        LabelInFirstButton = Translations["Pause"];
                    }
                    else if (AppStatus == AppStatus.WorkPaused)
                    {
                        TitleStringToDisplay = Translations["Work (Paused)"];
                        LabelInFirstButton = Translations["Resume"];
                    }
                    else if (AppStatus == AppStatus.BreakPaused)
                    {
                        TitleStringToDisplay = Translations["Break (Paused)"];
                        LabelInFirstButton = Translations["Resume"];
                    }
                    OnPropertyChanged("TimeStringToDisplay2");
                }

            }
        }
        private List<Language> possibleLanguages;

        public List<Language> PossibleLanguages
        {
            get { return possibleLanguages; }
            set 
            { 
                possibleLanguages = value;
                OnPropertyChanged("PossibleLanguages");
            }
        }
        private List<SoundString> soundsStrings;

        public List<SoundString> SoundsStrings
        {
            get { return soundsStrings; }
            set
            {
                soundsStrings = value;
                OnPropertyChanged("SoundsStrings");
            }
        }
        private SoundString soundString;

        public SoundString SoundString
        {
            get { return soundString; }
            set
            {
                soundString = value;
                OnPropertyChanged("SoundString");
            }
        }
        #endregion
        #region Constructor
        public MainWindowViewModel()
        {                  

            ViewModels = new ObservableCollection<BaseViewModel>();
            ViewModels.Add(new ClockScreenViewModel());
            ViewModels.Add(new SettingsViewModel());
            ViewModels.Add(new QuitViewModel());

            AppStatus = AppStatus.Initial;
            readSettings();
            loadLanguage();
            loadSoundsStrings();
            SoundString = soundsStrings[0];
            
            

            ShowClock();
            model = new AppModel();
            TimeStringToDisplay = "00:00";
            TitleStringToDisplay = Translations["Hello!"];
            LabelInFirstButton = "";



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
            OnPropertyChanged("Seconds");
            OnPropertyChanged("TimeStringToDisplay");
            OnPropertyChanged("TimeStringToDisplay2");

            if (Seconds == 0)
            {
                TimeStringToDisplay = "00 : 00";
                OnPropertyChanged("TimeStringToDisplay");
                OnPropertyChanged("TimeStringToDisplay2");
                if (TitleStringToDisplay == Translations["Work"])
                {
                    StartBreakCommand.Execute(this);
                    TitleStringToDisplay = Translations["Break"];
                }
                else if (TitleStringToDisplay == Translations["Break"])
                {
                    StartWorkCommand.Execute(this);
                    TitleStringToDisplay = Translations["Work"];
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
            TitleStringToDisplay = Translations["Work"];
            Model.StartTimer(WorkPeriodInMinutes * 60);
            startRefreshingTimer();
            AppStatus = AppStatus.Work;
            LabelInFirstButton = Translations["Pause"]; ;

            showToastNotification(Translations["It's time to work."]);           
        }
        private void startBreak()
        {
            if (Timer != null)
            {
                Timer.Stop();
                Timer.Dispose();
                TimeStringToDisplay = "00 : 00";
            }
            TitleStringToDisplay = Translations["Break"];
            Model.StartTimer(BreakPeriodInMinutes * 60);
            startRefreshingTimer();
            AppStatus = AppStatus.Break;
            LabelInFirstButton = Translations["Pause"];

            showToastNotification(Translations["It's time to have a break."]);
        }
        private void startPauseOrResume()
        {
            if (Timer == null)
                return;
            else if (AppStatus==AppStatus.Work)
            {
                Model.PauseTimer();
                TitleStringToDisplay = Translations["Work (Paused)"];
                LabelInFirstButton = Translations["Resume"];
                AppStatus = AppStatus.WorkPaused;
            }
            else if (AppStatus == AppStatus.Break)
            {
                Model.PauseTimer();
                TitleStringToDisplay = Translations["Break (Paused)"];
                LabelInFirstButton = Translations["Resume"];
                AppStatus = AppStatus.BreakPaused;
            }
            else if (AppStatus == AppStatus.WorkPaused)
            {
                Model.ResumeTimer();
                TitleStringToDisplay = Translations["Work"];
                LabelInFirstButton = Translations["Pause"];
                AppStatus = AppStatus.Work;
            }
            else if (AppStatus == AppStatus.BreakPaused)
            {
                Model.ResumeTimer();
                TitleStringToDisplay = Translations["Break"];
                LabelInFirstButton = Translations["Pause"];
                AppStatus = AppStatus.Break;
            }
        }
        private void quit()
        {      
            App.Current.Shutdown();
        }
        private void askWhetherToQuit()
        {
            ActiveViewModel = ViewModels[2];
        }
        private void showSettings()
        {
            ActiveViewModel = ViewModels[1];
        }
        private void ShowClock()
        {
            ActiveViewModel= ViewModels[0];
        }
        private void readSettings()
        {
            XDocument settings = XDocument.Load("Models/Settings.xml");
            WorkPeriodInMinutes = int.Parse((settings.Root.Element("workPeriodInMinutes").Value));
            BreakPeriodInMinutes = int.Parse((settings.Root.Element("breakPeriodInMinutes").Value));
            
        }
        private void saveSettings(string elementName, object value, string path = "Models/Settings.xml")
        {
            XDocument settings = XDocument.Load(path);
            settings.Root.Element(elementName).Value = value.ToString();
            settings.Save(path);
        }
        private void loadLanguage()
        {
            XDocument settings = XDocument.Load("Models/Settings.xml");
            PossibleLanguages = Languages.GetPossibleLanguages();
            foreach (Language item in PossibleLanguages)
            {
                if (item.Name == settings.Root.Element("language").Value)
                    Language = item;
            }            
            Translations = new Dictionary<string, string>();
            Translations = Languages.GetTranslationsDictionary(Language);
            foreach (Language item in possibleLanguages)
            {
                item.SetTranslation(Translations);
            }
            if (Language==null)
            {
                throw new Exception("Language can't be null!");
            }
        }
        private void loadSoundsStrings()
        {
            SoundsStrings = new List<SoundString>();
            SoundsStrings.Add(new SoundString ("Sounds/Bell 1.wav","Bell 1"));
            SoundsStrings.Add(new SoundString("Sounds/Bell 2.wav", "Bell 2"));
            SoundsStrings.Add(new SoundString("Sounds/Bell 3.wav", "Bell 3"));
            SoundsStrings.Add(new SoundString("Sounds/Birds.wav", "Birds"));
            SoundsStrings.Add(new SoundString("Sounds/Notification 1.wav", "Notification 1"));
            SoundsStrings.Add(new SoundString("Sounds/Notification 2.wav", "Notification 2"));
            SoundsStrings.Add(new SoundString("Sounds/Whistling.wav", "Whistling"));

            foreach (var item in SoundsStrings)
            {
                item.SetTranslation(Translations);
            }

        }
        private void showToastNotification(string message)
        {
            SoundPlayer soundPlayer = new SoundPlayer(SoundString.Path);
            soundPlayer.Load();
            soundPlayer.Play();   

            new ToastContentBuilder()
                .AddText(message)
                .SetToastScenario(ToastScenario.Reminder)
                .AddAudio(new Uri("ms-winsoundevent:Notification.Default"),false,true)
                .AddButton(new ToastButton()
                .SetContent("OK"))
                .Show(toast =>
                {
                    toast.ExpirationTime = DateTime.Now.AddMinutes(5);
                    toast.Activated += toast_Activated;
                    
                });
            void toast_Activated(Windows.UI.Notifications.ToastNotification sender, object args)
            {
                soundPlayer.Stop();                
            }
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
        public ICommand AskWhethetToQuitCommand
        {
            get
            {
                return new BaseCommand(askWhetherToQuit);
            }
        }
        #endregion
    }

}
