using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Xml.Linq;
using Wolfy.Properties;

namespace Wolfy.Classes {

    public static class Langs {

        public static void Init() {

            // Readme file
            File.WriteAllText(Path.Combine(Reference.LangsPath, "readme.txt"), Resources.lang_readme);

            #region Create lang files

            // Langs files
            string[] _Langs = {
                "fr.xaml",
                "en.xaml",
                "it.xaml",
                "ru.xaml",
                "es.xaml",
                "de.xaml"
            };

            // Create lang files
            foreach (string _Lang in _Langs) {

                // Convert to resource dictionary
                ResourceDictionary _Dict = new ResourceDictionary() {
                    Source = new Uri("/Localization/" + _Lang, UriKind.Relative)
                };

                // Lang path
                string _Path = Path.Combine(Reference.LangsPath, _Lang);

                // If lang file doesn't exist, create it
                if (!File.Exists(_Path)) {

                    // TODO: Try to preserve comments & order
                    File.WriteAllText(Path.Combine(Reference.LangsPath, _Lang), XDocument.Parse(XamlWriter.Save(_Dict)).ToString());

                }

            }

            #endregion

            // Load language
            SetLanguage(Reference.JsonSettings.Language); // en,fr,it,de,ru,es ...
            Dictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault(a => a.Contains("language_name") && a["language_name"].ToString() != "default");

        }

        // Dictionary variable
        private static ResourceDictionary Dictionary;

        #region Get

        /// <summary>
        /// Returns a string from a key in the current language
        /// </summary>
        public static string Get(string _Key) {
            return (Dictionary.Contains(_Key)) ? Dictionary[_Key].ToString() : "N/A " + _Key;
        }

        /// <summary>
        /// Returns all available languages
        /// </summary>
        public static List<ResourceDictionary> GetLanguages() {

            List<ResourceDictionary> _List = new List<ResourceDictionary>();
            // List all languages
            foreach (string _Lang in Directory.GetFiles(Reference.LangsPath, "*.xaml", SearchOption.TopDirectoryOnly)) {
                // Lang is correct
                ResourceDictionary _LangDict = new ResourceDictionary() { Source = new Uri(_Lang) };
                if (_LangDict.Contains("language_name") && _LangDict.Contains("language_display_name") && _LangDict["language_name"].ToString() != "default") {
                    // Add lang
                    _List.Add(_LangDict);
                }
            }
            return _List;

        }

        /// <summary>
        /// Returns the name of the current language
        /// </summary>
        public static string GetLanguageName() {
            return Dictionary["language_name"].ToString();
        }

        /// <summary>
        /// Returns the display name of the current language
        /// </summary>
        public static string GetLanguageDisplayName() {
            return Dictionary["language_display_name"].ToString();
        }

        #endregion
        #region Set

        /// <summary>
        /// Changes the current language
        /// </summary>
        public static void SetLanguage(string _Language) {

            // Lang path
            string _LangFile = Path.Combine(Reference.LangsPath, _Language + ".xaml");

            // Lang is valid
            if (_Language != "default" && File.Exists(_LangFile)) {

                // Retrieve dictionaries
                Collection<ResourceDictionary> _Dictionaries = Application.Current.Resources.MergedDictionaries;

                // Remove previous language
                _Dictionaries.Remove(Dictionary);

                // Add lang
                Dictionary = new ResourceDictionary() { Source = new Uri(_LangFile) };
                _Dictionaries.Add(Dictionary);

            }
        }

        #endregion

    }

}
