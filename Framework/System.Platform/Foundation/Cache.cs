namespace System.Platform.Foundation
{
    /// <summary>
    /// 提供对缓存值的支持.
    /// </summary>
    /// <typeparam name="T">正在缓存的对象类型.</typeparam>
    public sealed class Cache<T>
    {
        private readonly Func<T> valueFactory;
        private T value;
        private bool isDirty;

        /// <summary>
        /// 初始化<see cref="Cache{T}"/>类的一个新实例.
        /// </summary>
        /// <param name="valueFactory">当需要值时调用的委托.</param>
        public Cache(Func<T> valueFactory)
        {
            if (valueFactory == null) { throw new ArgumentNullException("valueFactory"); }
            this.valueFactory = valueFactory;
            this.isDirty = true;
        }

        /// <summary>
        /// 得到的值.
        /// </summary>
        public T Value
        {
            get
            {
                if (isDirty)
                {
                    value = valueFactory();
                    isDirty = false;
                }
                return value;
            }
        }

        /// <summary>
        /// 指示缓存是不良的，并将在下次读取值时更新自身.
        /// </summary>
        public bool IsDirty { get { return isDirty; } }

        /// <summary>
        /// 设置缓存为不良。这确保缓存在下一次读取值时被更新.
        /// </summary>
        public void SetDirty()
        {
            isDirty = true;
        }
    }
}
