using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Wolfy.Classes {
    public static class SkinManager {

        // ----------------| Variables |---------------- //

        // The colours are linked together (PRIMARY Amber with ACCENT Amber)..
        public static readonly String[] Colors = {
            "amber",
            "blue",
            "bluegrey",
            "brown",
            "cyan",
            "deeporange",
            "deeppurple",
            "green",
            "grey",
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
        public static readonly String[] Themes = {
            "light",
            "dark"
        };

        /// <summary>
        /// Load the module managing the visual of the application
        /// </summary>
        public static void Init() {

            // Load theme/color
            ApplySkin();

        }

        /// <summary>
        /// Apply the theme (This function checks if the theme exists, 
        /// if it does not exist, the default theme is applied)
        /// </summary>
        /// <param name="_Theme">Theme name</param>
        public static void ApplyTheme(String _Theme, Boolean _ApplySkin = true) {

            // If the theme does not exist, apply the default theme
            _Theme = _Theme.ToLower();
            if (!Themes.Contains(_Theme)) {
                _Theme = "light";
            }

            // Save
            Reference.JsonSettings.Theme = _Theme;
            // Apply
            if (_ApplySkin)
                ApplySkin();

        }

        /// <summary>
        /// Apply the color (This function checks if the color exists, 
        /// if it does not exist, the default color is applied)
        /// </summary>
        /// <param name="_Color">Color name</param>
        public static void ApplyColor(String _Color, Boolean _ApplySkin = true) {

            // If the color does not exist, apply the default color
            _Color = _Color.ToLower();
            if (!Colors.Contains(_Color))
                _Color = "deeppurple";

            // Save
            Reference.JsonSettings.Color = _Color;
            // Apply
            if (_ApplySkin)
                ApplySkin();

        }

        /// <summary>
        /// Applies the theme and color defined in the settings (Reference.JsonSettings.Color/Theme)
        /// Warning: No verification of the existence of the theme/color
        /// </summary>
        public static void ApplySkin() {

            List<String> _ResourceDictionaries = new List<String>() {
                String.Format("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.{0}.xaml", Reference.JsonSettings.Theme),
                String.Format("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.{0}.xaml", Reference.JsonSettings.Color),
                String.Format("pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Accent/MaterialDesignColor.{0}.xaml", Reference.JsonSettings.Color)
            };
            // Needed
            _ResourceDictionaries.Add("pack://application:,,,/MaterialDesignThemes.Wpf;component/Themes/MaterialDesignTheme.Defaults.xaml");

            Application.Current.Resources.MergedDictionaries.Clear();
            foreach (String _ResourceDictionary in _ResourceDictionaries) {
                Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() {
                    Source = new Uri(_ResourceDictionary)
                });
            }

        }

    }
}
