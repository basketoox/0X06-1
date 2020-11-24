using System.Speech.Synthesis;

namespace System.Crosscutting.Speech
{
    public class TtsSynthesizer : ISynthesizer
    {
        private readonly SpeechSynthesizer _synth = new SpeechSynthesizer();
        private bool _enable = true;

        public bool Enable
        {
            get { return _enable; }
            set { _enable = value; }
        }

        public void Speak(string textToSpeak)
        {
            if (Enable)
            {
                _synth.Speak(textToSpeak);
            }
        }

        public void SpeakAsync(string textToSpeak)
        {
            if (Enable)
            {
                _synth.SpeakAsyncCancelAll();
                _synth.SpeakAsync(textToSpeak);
            }
        }
    }
}
