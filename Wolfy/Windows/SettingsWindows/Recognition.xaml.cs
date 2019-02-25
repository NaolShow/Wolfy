using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wolfy.Classes;

namespace Wolfy.Windows.SettingsWindows
{

    public partial class Recognition : UserControl
    {

        public Recognition()
        {
            InitializeComponent();

            // ----------------| Recognition language |---------------- //
            InitRecLang();

            // ----------------| Recognition threshold |---------------- //
            RecognitionThresholdSlider.Value = Reference.JsonSettings.Confidence;

            // ----------------| Synthesizer voice |---------------- //
            InitSynth();

        }

        #region Recognition language

        private void InitRecLang() {

            // List all langs
            foreach (System.Speech.Recognition.RecognizerInfo _Lang in Classes.Recognition.SpeechRecognition.GetInstalledRecognizers()) {

                ComboBoxItem _Item = new ComboBoxItem() {
                    Content = _Lang.Culture.NativeName,
                    Tag = _Lang.Id
                };
                RecognitionLangCombo.Items.Add(_Item);

                // Check if this lang is in settings
                if (_Lang.Id == Reference.JsonSettings.Speech_language)
                    RecognitionLangCombo.Text = _Lang.Culture.NativeName;

            }

        }

        private void RecognitionLangCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            Reference.JsonSettings.Speech_language = RecognitionLangCombo.SelectedValue.ToString();

        }


        #endregion

        #region Recognition Threshold

        private void RecognitionThresholdSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {

            Reference.JsonSettings.Confidence = (float)RecognitionThresholdSlider.Value;

        }

        #endregion

        #region Synthesizer voice

        private void InitSynth() {

            // List all langs
            foreach (System.Speech.Synthesis.InstalledVoice _Voice in Classes.Recognition.Synthesizer.GetInstalledVoices()) {

                ComboBoxItem _Item = new ComboBoxItem() {
                    Content = _Voice.VoiceInfo.Culture.NativeName,
                    Tag = _Voice.VoiceInfo.Id
                };
                SynthesizerVoiceCombo.Items.Add(_Item);

                // Check if this lang is in settings
                if (_Voice.VoiceInfo.Id == Reference.JsonSettings.Synthesizer_voice)
                    SynthesizerVoiceCombo.Text = _Voice.VoiceInfo.Culture.NativeName;

            }

        }

        private void SynthesizerVoiceCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) {

            Reference.JsonSettings.Synthesizer_voice = SynthesizerVoiceCombo.SelectedValue.ToString();

        }

        #endregion

    }

}
