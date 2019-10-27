using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using System;
using System.Windows.Media;

namespace Wolfy.Classes.App {

    public static class Appearance {

        private static readonly PaletteHelper Helper = new PaletteHelper();
        private static ITheme Current;

        /// <summary>
        /// Initialize application appearance
        /// </summary>
        public static void Initialize() {

            // Get application theme
            Current = Helper.GetTheme();

            // Set color & theme
            SetColor(Settings.Data.Color);
            SetTheme(Settings.Data.Theme);

            // Apply
            Apply();

        }

        /// <summary>
        /// Set application color
        /// Set the default color if the specified one doesn't exist
        /// </summary>
        public static void SetColor(string colorName) {

            // Get secondary color
            Enum.TryParse(colorName, true, out SecondaryColor color);

            // Color exists
            if (Enum.IsDefined(typeof(SecondaryColor), color)) {

                // Get color
                Color finalColor = SwatchHelper.Lookup[(MaterialDesignColor)color];

                // Set application color
                Current.SetPrimaryColor(finalColor);
                Current.SetSecondaryColor(finalColor);

            } else {

                // Set the application color to the default one
                Settings.Data.Color = Settings.DefaultData.Color;
                SetColor(Settings.Data.Color);

            }

        }

        /// <summary>
        /// Set application theme
        /// Set the default theme if the specified one doesn't exist
        /// </summary>
        public static void SetTheme(string themeName) {

            // Set theme
            if (themeName.Equals("dark", StringComparison.OrdinalIgnoreCase)) {
                Current.SetBaseTheme(Theme.Dark);
            } else if (themeName.Equals("light", StringComparison.OrdinalIgnoreCase)) {
                Current.SetBaseTheme(Theme.Light);
            } else {

                // Set the application theme to the default one
                Settings.Data.Theme = Settings.DefaultData.Theme;
                SetTheme(Settings.Data.Theme);

            }

        }

        /// <summary>
        /// Apply the application appearance
        /// </summary>
        public static void Apply() {
            Helper.SetTheme(Current);
        }

    }

}
