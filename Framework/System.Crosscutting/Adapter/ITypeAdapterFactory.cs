namespace System.Crosscutting.Adapter
{
    /// <summary>
    /// 适配器工厂的基本接口
    /// </summary>
    public interface ITypeAdapterFactory
    {
        /// <summary>
        /// 创建类型适配器
        /// </summary>
        /// <returns>The created ITypeAdapter</returns>
        ITypeAdapter Create();
    }
}