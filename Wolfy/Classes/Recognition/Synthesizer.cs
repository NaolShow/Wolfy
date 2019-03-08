using System;
using System.Collections.ObjectModel;
using System.Speech.Synthesis;

namespace Wolfy.Classes.Recognition {
    public static class Synthesizer {

        // ----------------| Variables |---------------- //
        public static SpeechSynthesizer synthesizer;
        public static VoiceInfo synthesizerVoice;

        public static Boolean IsReady = false;

        public static void Init() {

            // Initialize synthesizer
            synthesizer = new SpeechSynthesizer();

            // Checks if a synthesizer voice is installed
            if (GetInstalledVoices().Count > 0) {
                IsReady = true;

                // Retrieve voice
                synthesizerVoice = GetVoice(Reference.JsonSettings.Synthesizer_voice);
                Reference.JsonSettings.Synthesizer_voice = synthesizerVoice.Id;

                Utils.Log(String.Format(Langs.Get("synthesizer_loaded"), synthesizerVoice.Culture.NativeName));
            }
            else {
                Utils.Log(Langs.Get("synthesizer_no_voice"));
            }

        }

        /// <summary>
        /// Trying to get a voice from a String. 
        /// 
        /// If no voice is installed, returns null.
        /// If the given voice does not exist/is not installed, returns null.
        /// </summary>
        /// <param name="_Voice">Voice</param>
        /// <returns></returns>
        public static VoiceInfo GetVoice(String _Voice) {
            VoiceInfo _ResultVoice = null;

            // Get installed voices
            ReadOnlyCollection<InstalledVoice> _InstalledVoices = GetInstalledVoices();

            // Try to retrieve voice
            foreach (var _VoiceInfo in _InstalledVoices) {

                if (_VoiceInfo.VoiceInfo.Id.Equals(_Voice, StringComparison.OrdinalIgnoreCase)) {
                    _ResultVoice = _VoiceInfo.VoiceInfo; break;
                }

            }

            // If the selected voice is not installed, try to take an installed voice 
            if (_ResultVoice == null && _InstalledVoices.Count != 0) {
                _ResultVoice = _InstalledVoices[0].VoiceInfo;
            }

            return _ResultVoice;
        }

        /// <summary>
        /// Returns all installed synthesizer voices
        /// </summary>
        /// <returns>Voices</returns>
        public static ReadOnlyCollection<InstalledVoice> GetInstalledVoices() {
            return synthesizer.GetInstalledVoices();
        }

        /// <summary>
        /// Plays the _Text has a voice synthesizer. 
        /// If no voice is given, use the default voice of the software (given in the settings)
        /// </summary>
        /// <param name="_Text">Text that will be read</param>
        /// <param name="_Voice">Voice that will read the text</param>
        public static void SpeakText(String _Text, VoiceInfo _Voice = null) {

            if (IsReady) {

                PromptBuilder builder = new PromptBuilder();

                // Get default voice if not specified
                if (_Voice == null) { _Voice = synthesizerVoice; }

                // Add text
                builder.StartVoice(_Voice);
                builder.StartSentence();
                builder.AppendText(_Text);
                builder.EndSentence();
                builder.EndVoice();

                // Speak
                synthesizer.SpeakAsync(builder);

            }
        }
    }
}
