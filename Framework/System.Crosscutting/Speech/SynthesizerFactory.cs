//===================================================================================
// Microsoft Developer & Platform Evangelism
//=================================================================================== 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
namespace System.Crosscutting.Speech
{
    /// <summary>
    /// 合成器的工厂模式
    /// </summary>
    public static class SynthesizerFactory
    {
        #region Members

        private static ISynthesizerFactory _currentSynthesizerFactory;

        #endregion

        #region Public Methods

        /// <summary>
        /// 设置要使用的日志工厂
        /// </summary>
        /// <param name="synthesizerFactory">Synthesizer factory to use</param>
        public static void SetCurrent(ISynthesizerFactory synthesizerFactory)
        {
            _currentSynthesizerFactory = synthesizerFactory;
        }

        /// <summary>
        /// Createt a new <ref name="System.Crosscutting.Speech.ILogger"/>
        /// </summary>
        /// <returns>Created ISynthesizer</returns>
        public static ISynthesizer CreateSynthesizer()
        {
            return (_currentSynthesizerFactory != null) ? _currentSynthesizerFactory.Create() : null;
        }

        #endregion
    }
}