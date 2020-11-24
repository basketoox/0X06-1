using System.Diagnostics.Contracts;
using System.Reflection;

namespace System.Platform.Applications
{
    /// <summary>
    ///  弱事件处理程序
    /// </summary>
    /// <remarks>
    ///     使用它，而不是像这样注册一个事件回调:
    ///         someButton.Click += o.ClickHandler;
    ///     这样做:
    ///         someButton.Click += WeakEventHandler.Wrap(o.ClickHandler, eh => someButton.Click -= eh);
    /// </remarks>
    public static class WeakEventHandler
    {
        /// <summary>
        /// </summary>
        /// <param name="eh"></param>
        /// <param name="cleanup"></param>
        /// <returns></returns>
        /// <remarks>我们提供这种超载是因为它太常见了</remarks>
        public static EventHandler Wrap(EventHandler eh, Action<EventHandler> cleanup)
        {
            return WeakEventHandlerImpl<EventHandler>.Create(eh, cleanup);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <param name="eh"></param>
        /// <param name="cleanup"></param>
        /// <returns></returns>
        public static TEventHandler Wrap<TEventHandler>(TEventHandler eh, Action<TEventHandler> cleanup)
            where TEventHandler : class
        {
            return WeakEventHandlerImpl<TEventHandler>.Create(eh, cleanup);
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="TEventArgs"></typeparam>
        /// <param name="eh"></param>
        /// <param name="cleanup"></param>
        /// <returns></returns>
        public static EventHandler<TEventArgs> Wrap<TEventArgs>(EventHandler<TEventArgs> eh,
            Action<EventHandler<TEventArgs>> cleanup) where TEventArgs : EventArgs
        {
            return WeakEventHandlerImpl<EventHandler<TEventArgs>>.Create(eh, cleanup);
        }

        /// <summary>
        /// </summary>
        /// <param name="weakEventHandler"></param>
        /// <param name="strongEventHandler"></param>
        /// <returns></returns>
        public static Boolean Match(Delegate weakEventHandler, Delegate strongEventHandler)
        {
            return ((WeakEventHandlerImpl)weakEventHandler.Target).Match(strongEventHandler);
        }

        private delegate void OpenEventHandler<in TTarget, in TEventArgs>(
            TTarget target, Object sender, TEventArgs eventArgs)
            where TTarget : class
            where TEventArgs : EventArgs;

        private class WeakEventHandlerImpl
        {
            protected readonly WeakReference _wrTarget; //弱引用原始委托的目标对象
            protected Delegate _openEventHandler; // “OPEN”委托调用原始目标的委托方法

            public WeakEventHandlerImpl(Delegate d)
            {
                _wrTarget = new WeakReference(d.Target);
            }

            // Match用于将WeakEventHandlerImpl对象与实际委托进行比较. 
            // 通常用于从事件集合中删除WeakEventHandlerImpl. 
            public Boolean Match(Delegate strongEventHandler)
            {
                // 如果原始目标和方法与WeakEventHandlerImpl的目标和方法匹配，则返回true
                return (_wrTarget.Target == strongEventHandler.Target) &&
                       (_openEventHandler.Method == strongEventHandler.Method);
            }
        }

        private sealed class WeakEventHandlerImpl<TEventHandler> : WeakEventHandlerImpl where TEventHandler : class
        {
            // 引用一个方法，该方法在知道原始目标已被GC处理后，删除该代理对象的委托
            private readonly Action<TEventHandler> _cleanup;

            //这是传递给m_cleanup的委托，需要从事件中删除它
            private readonly TEventHandler _proxyHandler;

            private WeakEventHandlerImpl(Delegate d, Action<TEventHandler> cleanup)
                : base(d)
            {
                _cleanup = cleanup;

                var targetType = d.Target.GetType();
                var eventHandlerType = typeof(TEventHandler);
                var eventArgsType = eventHandlerType.IsGenericType
                    ? eventHandlerType.GetGenericArguments()[0]
                    : eventHandlerType.GetMethod("Invoke").GetParameters()[1].ParameterType;

                //创建代理调用方法的委托;此委托注册到事件
                var miProxy = typeof(WeakEventHandlerImpl<TEventHandler>)
                    .GetMethod("ProxyInvoke", BindingFlags.Instance | BindingFlags.NonPublic)
                    .MakeGenericMethod(targetType, eventArgsType);
                _proxyHandler = (TEventHandler)(Object)Delegate.CreateDelegate(eventHandlerType, this, miProxy);

                //创建原始委托方法的“打开”委托;ProxyInvoke调用这个
                var openEventHandlerType = typeof(OpenEventHandler<,>).MakeGenericType(d.Target.GetType(),
                    eventArgsType);
                _openEventHandler = Delegate.CreateDelegate(openEventHandlerType, null, d.Method);
            }

            public static TEventHandler Create(TEventHandler eh, Action<TEventHandler> cleanup)
            {
                Contract.Requires(eh != null && cleanup != null);
                // 我们不会为静态方法创建弱事件，因为类型不会得到GC
                var d = (Delegate)(Object)eh; //我们知道所有事件处理程序都是从委托派生的
                if (d.Target == null) return eh;

                var weh = new WeakEventHandlerImpl<TEventHandler>(d, cleanup);
                return weh._proxyHandler; //返回要添加到事件中的委托
            }

            private void ProxyInvoke<TTarget, TEventArgs>(Object sender, TEventArgs e)
                where TTarget : class
                where TEventArgs : EventArgs
            {
                //如果原始目标对象仍然存在，则调用它;否则调用m_cleanup取消对事件的委托注册
                var target = (TTarget)_wrTarget.Target;
                if (target != null)
                    ((OpenEventHandler<TTarget, TEventArgs>)_openEventHandler)(target, sender, e);
                else _cleanup(_proxyHandler);
            }
        }
    }
}