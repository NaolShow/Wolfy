using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Recognition;
using Wolfy.Files.Json;

namespace Wolfy.Classes.Recognition {
    public static class SpeechRecognition {

        // |-------[ Variables ]-------| //
        public static SpeechRecognitionEngine Recognizer;
        public static RecognizerInfo RecognizerInfo;

        public static void Init() {

            // Recognizer languages
            ReadOnlyCollection<RecognizerInfo> _InstalledRecognizers = GetInstalledRecognizers();

            // A recognizer is installed
            if (_InstalledRecognizers.Count > 0) {

                #region Speech language

                // Get speech language
                if (Reference.JsonSettings.Speech_language != null) {
                    foreach (RecognizerInfo _ri in _InstalledRecognizers) {
                        if (_ri.Id.Equals(Reference.JsonSettings.Speech_language, StringComparison.OrdinalIgnoreCase)) {
                            RecognizerInfo = _ri;
                            break;
                        }
                    }
                }
                // Set default speech language if it's not set
                if (RecognizerInfo == null) {
                    RecognizerInfo = _InstalledRecognizers[0];
                    Reference.JsonSettings.Speech_language = _InstalledRecognizers[0].Id;
                }

                #endregion

                #region Recognizer

                // Set voice
                Recognizer = new SpeechRecognitionEngine(RecognizerInfo.Id);

                // User has microphone
                try {
                    // Set microphone
                    Recognizer.SetInputToDefaultAudioDevice();
                } catch {
                    Utils.Log(Langs.Get("no_microphone"));
                    return;
                }

                // Events
                Recognizer.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognized);
                Recognizer.AudioLevelUpdated += new EventHandler<AudioLevelUpdatedEventArgs>(AudioLevelUpdated);

                // Recognition threshold limit
                float _Confidence = Reference.JsonSettings.Confidence;
                if ((_Confidence < 0) || (_Confidence > 100)) {
                    Reference.JsonSettings.Confidence = 80;
                }

                #endregion

                Utils.Log(string.Format(Langs.Get("recognizer_loaded"), RecognizerInfo.Culture.NativeName));

            } else {
                Utils.Log(Langs.Get("no_speech_language"));
            }

        }

        #region Events

        private static void SpeechRecognized(object sender, SpeechRecognizedEventArgs e) {

            // Recognition threshold
            if (e.Result.Confidence > Reference.JsonSettings.Confidence / 100) {
                string _Confidence = Math.Round(e.Result.Confidence * 100, 1).ToString();

                Utils.Log(string.Format(Langs.Get("command_recognized"), e.Result.Text, _Confidence));

                // Execute command
                Profiles.ExecuteCommand(e.Result.Text);

            }

        }

        private static void AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e) {
            // Set volume bar
            Reference.MainWindow.RecognizerBar.Value = (int)(10 * Math.Sqrt(e.AudioLevel));
        }

        #endregion

        #region Manage

        // Recognizer (private) state
        private static Boolean ActiveState = Reference.JsonSettings.Recognition_at_launch;

        // Recognizer (public) state
        public static Boolean State {
            get { return ActiveState; }
            set {

                ActiveState = value;

                // Recognition
                if (ActiveState && Profiles.GetProfileCommands().Count() > 0) {
                    Recognizer.RecognizeAsync(RecognizeMode.Multiple);
                    Reference.MainWindow.MicrophoneIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.MicrophoneVariant;
                } else {
                    Recognizer.RecognizeAsyncStop();
                    Reference.MainWindow.MicrophoneIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.MicrophoneVariantOff;
                }

                // Bar fix
                Reference.MainWindow.RecognizerBar.Value = 0;

            }
        }

        /// <summary>
        /// Load current profile commands
        /// </summary>
        public static void LoadCommands() {

            // Unload gammar
            Recognizer.UnloadAllGrammars();

            // Profile is valid
            if (Profiles.GetProfile() != null) {

                // There is commands
                List<JsonCommand> _ProfileCommands = Profiles.GetProfileCommands();
                if (_ProfileCommands.Count() > 0) {

                    // Commands
                    Choices _Commands = new Choices();

                    // Add commands
                    _ProfileCommands.ForEach(a => _Commands.Add(a.CommandName));

                    // Add grammar
                    GrammarBuilder _Builder = new GrammarBuilder();
                    _Builder.Append(_Commands);
                    _Builder.Culture = RecognizerInfo.Culture;
                    Grammar _Grammar = new Grammar(_Builder);

                    // Load grammar
                    Recognizer.LoadGrammar(_Grammar);

                }

                Utils.Log(String.Format(Langs.Get("profile_loaded"), Profiles.GetProfile(), _ProfileCommands.Count()));

            }
        }

        #endregion

        /// <summary>
        /// Returns all installed recognizers
        /// </summary>
        public static ReadOnlyCollection<RecognizerInfo> GetInstalledRecognizers() {
            return SpeechRecognitionEngine.InstalledRecognizers();
        }

    }
}
