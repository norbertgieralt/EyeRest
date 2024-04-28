using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace EyeRest.Models.Languages
{
    public static class Languages
    {
        private static List<string> getKeys()
        {
            List<string> keys = new List<string>();

            XDocument xml = XDocument.Load("Models/Languages/Languages.xml");            

            var query = from element in xml.Root.Descendants("text")
                        select element.Element("name").Value;

            IEnumerable<string> results = new List<string>();
            results = query;

            foreach (string item in results)
            {
                keys.Add(item);
            }
            return keys;
        }
        private static List<string> getValues(Language language)
        {
            List<string> values = new List<string>();

            XDocument xml = XDocument.Load("Models/Languages/Languages.xml");

            var query = from element in xml.Root.Descendants("text")
                        select element.Element(language.Name).Value;

            IEnumerable<string> results = new List<string>();
            results = query;

            foreach (string item in results)
            {
                values.Add(item);
            }

            return values;
        }
        public static List<Language> GetPossibleLanguages()
        {
            List<Language> list = new List<Language>();
            list.Add(new Language("en"));
            list.Add(new Language ("ger"));
            list.Add(new Language("pl"));
            
            return list;
        }
        public static Dictionary<string,string> GetTranslationsDictionary(Language language)
        {
            Dictionary<string,string> dictionary = new Dictionary<string,string>();
            List<string> keys = getKeys();
            List<string> values = getValues(language);

            if (keys.Count!=values.Count)
            {
                throw new Exception("Something went wrong with translations");
            }

            for (int i = 0; i < keys.Count; i++)
            {
                dictionary[keys[i]] = values[i];
            }

            return dictionary;
        }
    }
}
