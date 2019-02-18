using Wolfy.Files.Json;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Wolfy.Classes {
    public static class Translation {

        // ----------------| Variables |---------------- //
        // Define which tags are identified as translation, (tags that start with this 'yourstring:')
        // You must leave the ':'
        public static String TranslationTag = "tr:";

        public static JsonLang CurrentLang;

        /// <summary>
        /// Apply the tongue. If the language exists, it is applied.
        /// Otherwise the default language is applied
        /// (_Lang == null == Reference.JsonSettings.Language)
        /// </summary>
        /// <param name="_Lang">Lang file name</param>
        /// <returns></returns>
        public static void LoadLang(String _Lang = null) {

            // Retrieve settings lang if(_Lang == null)
            _Lang = _Lang ?? Reference.JsonSettings.Language;

            // If the lang does not exist, apply the default lang
            if (!File.Exists(Reference.LangsPath + _Lang + ".json"))
                _Lang = "en";

            // Apply lang
            CurrentLang = JsonConvert.DeserializeObject<JsonLang>(File.ReadAllText(Reference.LangsPath + _Lang + ".json"));

        }

        /// <summary>
        /// This function allows you to translate windows that have been instantiated
        /// </summary>
        public static void Translate() {

            // Loop between every windows
            foreach (Window _Window in Application.Current.Windows) {
                // Translate
                TranslateWindow(_Window);
            }

        }

        /// <summary>
        /// This function allows you to translate a specific window
        /// </summary>
        /// <param name="_Window">Window</param>
        public static void TranslateWindow(Window _Window) {

            // Check if the window has translations
            if (_Window is ITranslation) {

                // Loop controls
                foreach (Control _Control in Utils.GetControlsOf(_Window)) {

                    // Translate controls
                    if (HasTranslationTag(_Control.Tag)) {
                        _Control.SetValue(ContentControl.ContentProperty, Get(_Control.Tag.ToString()));
                    }

                }

                // Translate window
                if (HasTranslationTag(_Window.Tag)) {
                    _Window.Title = Get(_Window.Tag.ToString());
                }

                // Call OnTranslation event
                (_Window as ITranslation).OnTranslation();
            }

        }

        /// <summary>
        /// Allows you to retrieve a translation from an ID
        /// </summary>
        /// <param name="_ID">ID or TAG</param>
        /// <returns></returns>
        public static String Get(String _ID) {

            // Retrieve ID
            if (_ID.StartsWith(TranslationTag)) {
                String[] _parts = _ID.Split(':');
                _ID = _ID.Substring(_parts[0].Length + 1).Trim();
            }

            // Return translation
            if (CurrentLang.Translations.ContainsKey(_ID)) {
                return CurrentLang.Translations[_ID].Replace("\n", Environment.NewLine);
            }
            // If the translation doesn't exist
            return "N/A tr:" + _ID;

        }

        /// <summary>
        /// Checks if the tag has a translation
        /// </summary>
        /// <param name="_Tag">Tag</param>
        /// <returns></returns>
        public static Boolean HasTranslationTag(object _Tag) {
            return (_Tag != null && HasTranslationTag(_Tag.ToString())) ? true : false;
        }
        /// <summary>
        /// Checks if the string has a translation
        /// </summary>
        /// <param name="_Tag">String</param>
        /// <returns></returns>
        public static Boolean HasTranslationTag(String _Tag) {
            return (_Tag.StartsWith(TranslationTag)) ? true : false;
        }

    }
}
