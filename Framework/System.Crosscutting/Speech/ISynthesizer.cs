namespace System.Crosscutting.Speech
{
    public interface ISynthesizer
    {
        /// <summary>
        /// 是否禁用
        /// </summary>
        bool Enable { get; set; }

        /// <summary>
        /// 同步朗读
        /// </summary>
        /// <param name="textToSpeak"></param>
        void Speak(string textToSpeak);
        /// <summary>
        /// 异步朗读
        /// </summary>
        /// <param name="textToSpeak"></param>
        void SpeakAsync(string textToSpeak);
    }
}
