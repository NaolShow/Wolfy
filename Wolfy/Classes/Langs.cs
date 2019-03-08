using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Markup;
using System.Xml.Linq;

namespace Wolfy.Classes {

    public static class Langs {

        /// <summary>
        /// Initialization of the class (necessary)
        /// </summary>
        public static void Init() {

            // ----------------| Save locally all langs |---------------- //

            // Readme
            File.WriteAllText(Path.Combine(Reference.LangsPath, "readme.txt"),
@"If you want to change/add a new language, please go to the link below to take a template.
https://github.com/NaolShow/Wolfy/tree/master/Wolfy/Localization

(Default templates do not contain comments, and are in disorder due to problems in the code.)
Thank you - NaolShow");

            String[] _Langs = {
                "fr.xaml",
                "en.xaml",
                "it.xaml",
                "ru.xaml",
                "es.xaml",
                "de.xaml"
            };

            foreach (String _Lang in _Langs) {

                // Convert to resource dictionary
                ResourceDictionary _Dict = new ResourceDictionary() {
                    Source = new Uri("/Localization/" + _Lang, UriKind.Relative)
                };

                // TODO: Try to preserve comments & order
                File.WriteAllText(Path.Combine(Reference.LangsPath, _Lang), XDocument.Parse(XamlWriter.Save(_Dict)).ToString());
            }

            // ----------------| Default language |---------------- //
            SetLanguage(Reference.JsonSettings.Language); // en,fr,it,de,ru,es ...
            Dictionary = Application.Current.Resources.MergedDictionaries.FirstOrDefault(a => a.Contains("language_name") && a["language_name"].ToString() != "default");

        }

        // Dictionary variable
        private static ResourceDictionary Dictionary;

        #region Get

        /// <summary>
        /// Returns a string from a key in the current language
        /// </summary>
        /// <returns></returns>
        public static String Get(String _Key) {
            return (Dictionary.Contains(_Key)) ? Dictionary[_Key].ToString() : "N/A " + _Key;
        }

        /// <summary>
        /// Returns all available languages
        /// </summary>
        /// <returns></returns>
        public static List<ResourceDictionary> GetLanguages() {

            List<ResourceDictionary> _List = new List<ResourceDictionary>();
            // List all languages
            foreach (String _Lang in Directory.GetFiles(Reference.LangsPath, "*.xaml", SearchOption.TopDirectoryOnly)) {
                // If lang is correct
                ResourceDictionary _LangDict = new ResourceDictionary() { Source = new Uri(_Lang) };
                if (_LangDict.Contains("language_name") && _LangDict.Contains("language_display_name") && _LangDict["language_name"].ToString() != "default") {
                    // Add
                    _List.Add(_LangDict);
                }
            }
            return _List;

        }

        /// <summary>
        /// Returns the name of the current language
        /// Return null if there is no language
        /// </summary>
        /// <returns>Language name</returns>
        public static String GetLanguageName() {
            if (Dictionary != null) {
                return Dictionary["language_name"].ToString();
            }
            return null;
        }

        /// <summary>
        /// Returns the name of the current language
        /// Return null if there is no language
        /// </summary>
        /// <returns>Language name</returns>
        public static String GetLanguageDisplayName() {
            if (Dictionary != null) {
                return Dictionary["language_display_name"].ToString();
            }
            return null;
        }

        #endregion

        /// <summary>
        /// Changes the current language
        /// Does nothing if: the desired language is called default, and if the desired language does not exist
        /// </summary>
        public static void SetLanguage(String _Language) {

            // New language path
            String _LangFile = Path.Combine(Reference.LangsPath, _Language + ".xaml");

            if (_Language != "default" && File.Exists(_LangFile)) {

                // Retrieve dictionaries
                Collection<ResourceDictionary> _Dictionaries = Application.Current.Resources.MergedDictionaries;

                // Remove previous language
                if (Dictionary != null) {
                    _Dictionaries.Remove(Dictionary);
                }

                // Add new language
                Dictionary = new ResourceDictionary() { Source = new Uri(_LangFile) };
                _Dictionaries.Add(Dictionary);

            }
        }

    }

}
