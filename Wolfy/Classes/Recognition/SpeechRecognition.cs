using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Speech.Recognition;

namespace Wolfy.Classes.Recognition {
    public static class SpeechRecognition {

        // ----------------| Variables |---------------- //
        public static SpeechRecognitionEngine recognizer;
        public static RecognizerInfo recognizerInfo;

        public static Boolean IsReady = false;

        public static void Init() {

            // -----| Recognizer language |----- //
            ReadOnlyCollection<RecognizerInfo> _InstalledRecognizers = GetInstalledRecognizers();

            // Check if a recognizer is installed
            if (_InstalledRecognizers.Count > 0) {

                // Retrieve speech language
                // If a speech language is set
                if (Reference.JsonSettings.Speech_language != null) {
                    foreach (RecognizerInfo _ri in _InstalledRecognizers) {
                        if (_ri.Id.Equals(Reference.JsonSettings.Speech_language, StringComparison.OrdinalIgnoreCase)) {
                            recognizerInfo = _ri; break;
                        }
                    }
                }
                // Set default speech language
                if (recognizerInfo == null) {
                    recognizerInfo = _InstalledRecognizers[0];
                    Reference.JsonSettings.Speech_language = _InstalledRecognizers[0].Id;
                }

                // -----| Recognition engine |----- //

                recognizer = new SpeechRecognitionEngine(recognizerInfo.Id);

                // Check if the user has a microphone
                try {
                    // Set microphone
                    recognizer.SetInputToDefaultAudioDevice();
                } catch {
                    Utils.Log(Langs.Get("no_microphone"));
                    return;
                }

                // Events
                recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);
                recognizer.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(AudioLevelUpdated);

                // Check the recognition threshold
                float _Confidence = Reference.JsonSettings.Confidence;
                if ((_Confidence < 0) || (_Confidence > 100)) {
                    Reference.JsonSettings.Confidence = 80;
                }

                Utils.Log(String.Format(Langs.Get("recognizer_loaded"), recognizerInfo.Culture.NativeName));
                IsReady = true;

            } else {
                Utils.Log(Langs.Get("no_speech_language"));
            }

        }

        #region Events

        private static void SpeechRecognized(object sender, SpeechRecognizedEventArgs e) {

            if (e.Result.Confidence > Reference.JsonSettings.Confidence / 100) {
                String _Confidence = Math.Round(e.Result.Confidence * 100, 1).ToString();

                Utils.Log(String.Format(Langs.Get("command_recognized"), e.Result.Text, _Confidence));

                // Execute command
                Profiles.ExecuteCommand(e.Result.Text);

            }

        }

        private static void AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e) {
            Reference.MainWindow.RecognizerBar.Value = (int)(10 * Math.Sqrt(e.AudioLevel));
        }

        #endregion

        #region Manage

        // Enable or disable recognizer
        private static Boolean ActiveState = Reference.JsonSettings.Recognition_at_launch;
        public static Boolean State {
            get { return ActiveState; }
            set {

                ActiveState = value;

                // Recognition
                if (ActiveState && Profiles.GetProfileCommands().Count() > 0) {
                    recognizer.RecognizeAsync(RecognizeMode.Multiple);
                    Reference.MainWindow.MicrophoneIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.MicrophoneVariant;
                } else {
                    recognizer.RecognizeAsyncStop();
                    Reference.MainWindow.MicrophoneIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.MicrophoneVariantOff;
                }

                // Bar fix
                Reference.MainWindow.RecognizerBar.Value = 0;

            }
        }

        // Load commands
        public static void LoadCommands() {

            // Unload gammar
            recognizer.UnloadAllGrammars();

            // Check if profile is valid
            if (Profiles.GetProfile() != null) {

                // Check if there is commands
                String[] _ProfileCommands = Profiles.GetProfileCommands();

                if (_ProfileCommands.Count() > 0) {

                    // Commands
                    Choices _Commands = new Choices();

                    // Add commands
                    Profiles.GetProfileCommands().ToList().ForEach(a => _Commands.Add(Path.GetFileNameWithoutExtension(a)));

                    // Add grammar
                    GrammarBuilder _Builder = new GrammarBuilder();
                    _Builder.Append(_Commands);
                    _Builder.Culture = recognizerInfo.Culture;
                    Grammar _Grammar = new Grammar(_Builder);

                    // Load grammar
                    recognizer.LoadGrammar(_Grammar);

                }

                // End
                Utils.Log(String.Format(Langs.Get("profile_loaded"), Profiles.GetProfile(), _ProfileCommands.Count()));

            }
        }

        #endregion

        /// <summary>
        /// Returns all installed recognizers
        /// </summary>
        /// <returns>Recognizers</returns>
        public static ReadOnlyCollection<RecognizerInfo> GetInstalledRecognizers() {
            return SpeechRecognitionEngine.InstalledRecognizers();
        }

    }
}
