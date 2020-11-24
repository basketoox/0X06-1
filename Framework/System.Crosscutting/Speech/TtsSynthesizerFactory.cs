
namespace System.Crosscutting.Speech
{
    /// <summary>
    /// A SynthesizerFactory
    /// </summary>
    public class TtsSynthesizerFactory
        : ISynthesizerFactory
    {
        /// <summary>
        /// Create the synthesizer
        /// </summary>
        /// <returns>New ISynthesizer</returns>
        public ISynthesizer Create()
        {
            return new TtsSynthesizer();
        }
    }
}
