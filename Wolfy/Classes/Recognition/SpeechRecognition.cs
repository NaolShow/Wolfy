using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading.Tasks;

namespace Wolfy.Classes.Recognition
{
    public static class SpeechRecognition
    {

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
                }
                catch {
                    Utils.Log(Translation.Get("noMicrophone"));
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

                Utils.Log(String.Format(Translation.Get("recognizerLoaded"), recognizerInfo.Culture.NativeName));
                IsReady = true;

                // Temporary
                LoadCommands();
                
            } else {
                Utils.Log(Translation.Get("noSpeechLanguage"));
            }

        }

        #region Events

        private static void SpeechRecognized(object sender, SpeechRecognizedEventArgs e) {

            if (e.Result.Confidence > Reference.JsonSettings.Confidence / 100) {
                String _Confidence = Math.Round(e.Result.Confidence * 100, 1).ToString();

                Utils.Log(String.Format(Translation.Get("commandRecognized"), e.Result.Text, _Confidence));
                if (e.Result.Text == "Hello") {
                    System.Diagnostics.Process.Start("http://google.com/");
                    Synthesizer.SpeakText("Hello, opening google...");
                }
            }

        }

        private static void AudioLevelUpdated(object sender, AudioLevelUpdatedEventArgs e) {
            Reference.MainWindow.RecognizerBar.Value = (int)(10 * Math.Sqrt(e.AudioLevel));
        }

        #endregion

        #region Manage

        // Enable or disable recognizer
        public static void SetActive(Boolean _State) {
            if (_State) {
                recognizer.RecognizeAsync(RecognizeMode.Multiple);
            } else {
                recognizer.RecognizeAsyncStop();
            }
            Reference.MainWindow.RecognizerBar.Value = 0;
        }

        // Load commands
        public static void LoadCommands() {

            // Disable recognition
            SetActive(false);

            // Commands
            Choices commands = new Choices();

            // Unload grammer
            recognizer.UnloadAllGrammars();

            // Add commands
            commands.Add("Hello");

            // Add grammar
            GrammarBuilder gBuilder = new GrammarBuilder();
            gBuilder.Append(commands);
            gBuilder.Culture = recognizerInfo.Culture;
            Grammar grammar = new Grammar(gBuilder);

            // Load grammar
            recognizer.LoadGrammar(grammar);

            // Enable
            SetActive(true);

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
