namespace Motion.Interfaces
{
    /// <summary>
    ///     部件测试接口
    /// </summary>
    public interface ISupportTest
    {
        /// <summary>
        ///     运行测试
        /// </summary>
        void RunTest();

        /// <summary>
        ///     暂停测试
        /// </summary>
        void PauseTest();

        /// <summary>
        ///     停止测试
        /// </summary>
        void StopTest();
    }
}