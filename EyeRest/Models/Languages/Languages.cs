using System;
using System.CodeDom;
using System.Collections.Generic;
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
        private static List<string> getValues(string language)
        {
            List<string> values = new List<string>();

            XDocument xml = XDocument.Load("Models/Languages/Languages.xml");

            var query = from element in xml.Root.Descendants("text")
                        select element.Element(language.ToString()).Value;

            IEnumerable<string> results = new List<string>();
            results = query;

            foreach (string item in results)
            {
                values.Add(item);
            }

            return values;
        }
        public static List<string> GetPossibleLanguages()
        {
            List<string> list = new List<string>();
            list.Add("en");
            list.Add("ger");
            list.Add("pl");
            return list;
        }
        public static Dictionary<string,string> GetTranslationsDictionary(string language)
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
