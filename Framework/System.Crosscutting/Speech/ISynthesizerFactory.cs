
namespace System.Crosscutting.Speech
{
    /// <summary>
    /// 合成器抽象工厂的基本契约
    /// </summary>
    public interface ISynthesizerFactory
    {
        /// <summary>
        /// Create a new ISynthesizer
        /// </summary>
        /// <returns>The ISynthesizer created</returns>
        ISynthesizer Create();
    }
}