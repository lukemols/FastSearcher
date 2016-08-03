using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Phone.Speech.Recognition;

namespace FastSearcher.Classes.Support
{
    class SpeechHelper
    {
        static SpeechHelper instance;
        public static SpeechHelper Instance { get { if (instance == null) instance = new SpeechHelper(); return instance; } }

        private SpeechHelper() { }

        public async Task<string> GetMicrophoneSpeech(string SpeechListenText, string SpeechExampleText)
        {
            try
            {
                SpeechRecognizerUI sr = new SpeechRecognizerUI();
                sr.Settings.ListenText = SpeechListenText;
                sr.Settings.ExampleText = SpeechExampleText;
                sr.Settings.ReadoutEnabled = false;
                sr.Settings.ShowConfirmation = false;

                SpeechRecognitionUIResult result = await sr.RecognizeWithUIAsync();
                if (result.ResultStatus == SpeechRecognitionUIStatus.Succeeded)
                {
                    return result.RecognitionResult.Text;
                }
            }
            catch { }
            throw new Exception();
        }
    }
}
