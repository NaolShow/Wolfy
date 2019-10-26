using System;
using System.Collections.ObjectModel;
using System.Speech.Synthesis;

namespace Wolfy.Classes.Recognition {
    public static class Synthesizer {

        // |-------[ Variables ]-------| //
        public static SpeechSynthesizer SpeechSynthesizer;
        public static VoiceInfo SynthesizerVoice;
        public static bool IsReady = false;

        public static void Init() {

            // Initialize synthesizer
            SpeechSynthesizer = new SpeechSynthesizer();

            // A voice is installed 
            if (GetInstalledVoices().Count > 0) {

                // Get voice
                SynthesizerVoice = GetVoice(Reference.JsonSettings.Synthesizer_voice);
                Reference.JsonSettings.Synthesizer_voice = SynthesizerVoice.Id;

                Utils.Log(string.Format(Langs.Get("synthesizer_loaded"), SynthesizerVoice.Culture.NativeName));

                IsReady = true;

            } else {
                Utils.Log(Langs.Get("synthesizer_no_voice"));
            }

        }

        #region Get

        /// <summary>
        /// Trying to get a voice from a String 
        /// </summary>
        public static VoiceInfo GetVoice(string _Voice) {
            VoiceInfo _ResultVoice = null;

            // Get installed voices
            ReadOnlyCollection<InstalledVoice> _InstalledVoices = GetInstalledVoices();

            // Retrieve the Voice by name
            foreach (var _VoiceInfo in _InstalledVoices) {

                if (_VoiceInfo.VoiceInfo.Id.Equals(_Voice, StringComparison.OrdinalIgnoreCase)) {
                    _ResultVoice = _VoiceInfo.VoiceInfo; break;
                }

            }

            // Voice is not installed, get another one
            if (_ResultVoice == null && _InstalledVoices.Count != 0) {
                _ResultVoice = _InstalledVoices[0].VoiceInfo;
            }

            return _ResultVoice;
        }

        /// <summary>
        /// Returns all installed synthesizer voices
        /// </summary>
        public static ReadOnlyCollection<InstalledVoice> GetInstalledVoices() {
            return SpeechSynthesizer.GetInstalledVoices();
        }

        #endregion

        /// <summary>
        /// Plays the _Text has a voice synthesizer
        /// </summary>
        public static void SpeakText(string _Text, VoiceInfo _Voice = null) {

            // Speech synthesizer is ready
            if (IsReady) {

                PromptBuilder builder = new PromptBuilder();

                // Get default voice if not specified
                if (_Voice == null) { _Voice = SynthesizerVoice; }

                // Speech
                builder.StartVoice(_Voice);
                builder.StartSentence();
                builder.AppendText(_Text);
                builder.EndSentence();
                builder.EndVoice();

                // Speak
                SpeechSynthesizer.SpeakAsync(builder);

            }
        }
    }
}
