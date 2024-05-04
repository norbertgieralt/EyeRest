using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeRest.Models.Languages
{
    public class SoundString :INotifyPropertyChanged
    {
        #region Fields and Properties	
        private string path;

        public string Path
        {
            get { return path; }
            set
            {
                path = value;
                OnPropertyChanged("Path");
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        private string translation;

        public string Translation
        {
            get { return translation; }
            set
            {
                translation = value;
                OnPropertyChanged("Translation");
            }
        }
        #endregion
        #region Constructor
        public SoundString(string path, string name)
        {
            this.Path = path;
            this.Name = name;
        }
        #endregion
        #region Methods
        public void SetTranslation(Dictionary<string, string> translation)
        {
            this.Translation = translation[Name];
        }
        #endregion
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(params string[] args)
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
}
