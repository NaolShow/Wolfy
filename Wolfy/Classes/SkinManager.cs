using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Wolfy.Classes {
    public static class SkinManager {

        // |-------[ Variables ]-------| //

        #region Colors

        // The colours are linked together (PRIMARY Amber with ACCENT Amber)..
        public static readonly List<String> Colors = new List<String>() {
            "amber",
            "blue",
            "cyan",
            "deeporange",
            "deeppurple",
            "green",
            "indigo",
            "lightblue",
            "lightgreen",
            "lime",
            "orange",
            "pink",
            "purple",
            "red",
            "teal",
            "yellow"
        };

        #endregion
        #region Themes

        public static readonly List<String> Themes = new List<String>() {
            "light",
            "dark"
        };

        #endregion

        /// <summary>
        /// Apply the theme
        /// </summary>
        public static void SetTheme(String _Theme) {

            // Theme is not valid
            _Theme = _Theme.ToLower();
            if (!Themes.Contains(_Theme))
                _Theme = "light";

            // Save
            Reference.JsonSettings.Theme = _Theme;
            // Apply
            ApplySkin();

        }

        /// <summary>
        /// Set the color
        /// </summary>
        public static void SetColor(String _Color) {

            // Color is not valid
            _Color = _Color.ToLower();
            if (!Colors.Contains(_Color))
                _Color = "deeppurple";

            // Save
            Reference.JsonSettings.Color = _Color;
            // Apply
            ApplySkin();

        }

        /// <summary>
        /// Applies the theme and color defined in the settings (Reference.JsonSettings.Color/Theme)
        /// </summary>
        public static void ApplySkin() {

            // Remove previous dictionaries
            Application.Current.Resources.MergedDictionaries.ToList().Where(a =>
                !a.Source.ToString().Contains("Localization") && !a.Source.ToString().Contains("Langs") && !a.Source.ToString().Contains("Defaults")).ToList().ForEach(a =>
                Application.Current.Resources.MergedDictionaries.Remove(a));

            // Add new dictionaries
            List<String> _NewDictionaries = new List<String>() {
                String.Format("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.{0}.xaml", Reference.JsonSettings.Theme),
                String.Format("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.{0}.xaml", Reference.JsonSettings.Color),
                String.Format("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.{0}.xaml", Reference.JsonSettings.Color)
            };
            _NewDictionaries.ForEach(a => Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(a) }));

        }

    }
}
