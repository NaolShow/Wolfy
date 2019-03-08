using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Wolfy.Classes {
    public static class SkinManager {

        // ----------------| Variables |---------------- //

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

        public static readonly List<String> Themes = new List<String>() {
            "light",
            "dark"
        };

        /// <summary>
        /// Apply the theme (This function checks if the theme exists, 
        /// if it does not exist, the default theme is applied)
        /// </summary>
        /// <param name="_Theme">Theme name</param>
        public static void SetTheme(String _Theme) {

            // If the theme does not exist, apply the default theme
            _Theme = _Theme.ToLower();
            if (!Themes.Contains(_Theme))
                _Theme = "light";

            // Save
            Reference.JsonSettings.Theme = _Theme;
            // Apply
            ApplySkin();

        }

        /// <summary>
        /// Set the color (This function checks if the color exists, 
        /// if it does not exist, the default color is applied)
        /// </summary>
        /// <param name="_Color">Color name</param>
        public static void SetColor(String _Color) {

            // If the color does not exist, apply the default color
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
        /// Warning: No verification of the existence of the theme/color
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
