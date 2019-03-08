using System.Linq;
using System.Windows.Controls;
using Wolfy.Classes;
using Wolfy.Classes.Recognition;

namespace Wolfy.Windows.SettingsWindows {

    public partial class Recognition : UserControl {

        public Recognition() {
            InitializeComponent();

            // ----------------| Recognition language |---------------- //

            // List
            SpeechRecognition.GetInstalledRecognizers().ToList().ForEach(a => RecognitionLangCombo.Items.Add(new ListBoxItem() { Content = a.Culture.NativeName, Tag = a.Id }));
            // Default
            RecognitionLangCombo.SelectedValue = Reference.JsonSettings.Speech_language;
            // Event
            RecognitionLangCombo.SelectionChanged += delegate { Reference.JsonSettings.Speech_language = RecognitionLangCombo.SelectedValue.ToString(); };

            // ----------------| Recognition threshold |---------------- //

            // Default
            RecognitionThresholdSlider.Value = Reference.JsonSettings.Confidence;
            // Event
            RecognitionThresholdSlider.ValueChanged += delegate { Reference.JsonSettings.Confidence = (float)RecognitionThresholdSlider.Value; };

            // ----------------| Recognition at launch |---------------- //

            // Default
            RecognitionAtLaunchCb.IsChecked = Reference.JsonSettings.Recognition_at_launch;

            // ----------------| Synthesizer voice |---------------- //

            // List
            Synthesizer.GetInstalledVoices().ToList().ForEach(a => SynthesizerVoiceCombo.Items.Add(new ListBoxItem() { Content = a.VoiceInfo.Culture.NativeName, Tag = a.VoiceInfo.Id }));
            // Default
            SynthesizerVoiceCombo.SelectedValue = Reference.JsonSettings.Synthesizer_voice;
            // Event
            SynthesizerVoiceCombo.SelectionChanged += delegate { Reference.JsonSettings.Synthesizer_voice = SynthesizerVoiceCombo.SelectedValue.ToString(); };

        }

        #region Checkboxes

        // Recognition at launch
        private void RecognitionAtLaunchCb_Checked(object sender, System.Windows.RoutedEventArgs e) {
            Reference.JsonSettings.Recognition_at_launch = RecognitionAtLaunchCb.IsChecked.Value;
        }

        #endregion

    }

}
