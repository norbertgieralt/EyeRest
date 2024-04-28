using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EyeRest.Models.Languages
{
    public class Language
    {
		#region Fields and Properties		
		private string name;

		public string Name
		{
			get { return name; }
			set { name = value; }
		}
		private string translation;

		public string Translation
		{
			get { return translation; }
			set { translation = value; }
		}
        #endregion
        #region Constructor
        public Language(string name)
        {
            this.Name= name;
        }
        public Language(string name,string translation)
        {
            this.Name = name;
			this.Translation = translation;
        }
        #endregion
        #region Methods
        public void SetTranslation(Dictionary<string, string> translation)
		{
			this.Translation = translation[Name];
		}
		#endregion
	}
}
