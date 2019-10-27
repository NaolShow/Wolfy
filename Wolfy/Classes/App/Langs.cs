using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Markup;
using System.Xml.Linq;

namespace Wolfy.Classes.App {

    public static class Langs {

        /// <summary>
        /// Supported languages
        /// </summary>
        private static readonly string[] Languages = {
            "fr",
            "en",
            "es",
            "it",
            "de",
            "ru"
        };

        private static readonly Collection<ResourceDictionary> Dictionaries = Application.Current.Resources.MergedDictionaries;
        private static ResourceDictionary Current;
        private static ResourceDictionary Default;


        /// <summary>
        /// Initialize application langs
        /// </summary>
        public static void Initialize() {

            // Remove default english lang

            #region Write languages

            try {

                foreach (string lang in Languages) {

                    // Get lang path
                    string path = Path.Combine(Reference.Folders.Langs, lang + ".xaml");

                    // Get dictionary
                    ResourceDictionary dict = new ResourceDictionary() {
                        Source = new Uri("/Localization/" + lang + ".xaml", UriKind.Relative)
                    };

                    // Write
                    File.WriteAllText(path, XDocument.Parse(XamlWriter.Save(dict)).ToString());

                }

            } catch (Exception ex) {
                MessageBox.Show(ex.Message, Reference.AppName, MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }

            #endregion
            #region User language

            // User language is not set
            if (Settings.Data.Language == null) {

                // Get user language
                string lang = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;

                // Set language or default language
                Settings.Data.Language = (File.Exists(Path.Combine(Reference.Folders.Langs, lang + ".xaml"))) ? lang : "en";

            }

            #endregion

            // Default language
            Default = Dictionaries[0];
            // Load lang
            Set(Settings.Data.Language);

        }

        /// <summary>
        /// Returns string of the corresponding key
        /// </summary>
        public static string Get(string key) {
            return (Current.Contains(key)) ? Current[key].ToString() : (Default.Contains(key)) ? Default[key].ToString() : string.Format("TR.ERR: '{0}'", key);
        }

        /// <summary>
        /// Set application language
        /// Returns true if the language has been applied
        /// Returns false if the language isn't valid
        /// </summary>
        public static bool Set(string lang) {

            // Language path
            string path = Path.Combine(Reference.Folders.Langs, lang + ".xaml");

            // Language exists
            if (File.Exists(path)) {

                // Check if file content is valid
                try { XElement.Load(path); } catch { return false; }

                // Remove previous language
                Dictionaries.Remove(Current);

                // Set new language
                Current = new ResourceDictionary() {
                    Source = new Uri(path)
                };
                Dictionaries.Add(Current);

                return true;

            }
            return false;

        }

    }

}
