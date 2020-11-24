using System.Windows.Input;

namespace System.Platform.Applications
{
    /// <summary>
    /// 提供<see cref="ICommand"/>实现，该实现中继<see cref="Execute"/>和<see cref="CanExecute"/>
    /// 方法指定的委托.
    /// </summary>
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Func<object, bool> _canExecute;


        /// <summary>
        /// 初始化<see cref=" DelegateCommand "/>类的新实例
        /// </summary>
        /// <param name="execute">在命令中调用execute时委托执行.</param>
        /// <exception cref="ArgumentNullException">execute参数不可为空.</exception>
        public DelegateCommand(Action execute) : this(execute, null) { }

        /// <summary>
        /// 初始化<see cref=" DelegateCommand "/>类的新实例.
        /// </summary>
        /// <param name="execute">在命令中调用execute时委托执行.</param>
        /// <exception cref="ArgumentNullException">execute参数不可为空.</exception>
        public DelegateCommand(Action<object> execute) : this(execute, null) { }

        /// <summary>
        /// 初始化<see cref=" DelegateCommand "/>类的新实例.
        /// </summary>
        /// <param name="execute">在命令中调用execute时委托执行.</param>
        /// <param name="canExecute">在命令中调用CanExecute时委托执行.</param>
        /// <exception cref="ArgumentNullException">execute参数不可为空.</exception>
        public DelegateCommand(Action execute, Func<bool> canExecute)
            : this(execute != null ? p => execute() : (Action<object>)null, canExecute != null ? p => canExecute() : (Func<object, bool>)null)
        { }

        /// <summary>
        /// 初始化<see cref=" DelegateCommand "/>类的新实例.
        /// </summary>
        /// <param name="execute">在命令中调用execute时委托执行.</param>
        /// <param name="canExecute">在命令中调用CanExecute时委托执行.</param>
        /// <exception cref="ArgumentNullException">execute参数不可为空.</exception>
        public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            if (execute == null) { throw new ArgumentNullException("execute"); }

            _execute = execute;
            _canExecute = canExecute;
        }


        /// <summary>
        /// 当发生影响命令是否应该执行的更改时发生.
        /// </summary>
        public event EventHandler CanExecuteChanged;


        /// <summary>
        /// 定义确定命令能否在当前状态下执行的方法.
        /// </summary>
        /// <param name="parameter">命令使用的数据。如果命令不需要传递数据，则可以将该对象设置为null.</param>
        /// <returns>
        /// 如果该命令可以执行，则为真;否则,假
        /// </returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        /// <summary>
        /// 定义调用命令时要调用的方法.
        /// </summary>
        /// <param name="parameter">命令使用的数据。如果命令不需要传递数据，则可以将该对象设置为null.</param>
        /// <exception cref="InvalidOperationException"><see cref="CanExecute"/>方法返回<c>false.</c></exception>
        public void Execute(object parameter)
        {
            if (!CanExecute(parameter))
            {
                throw new InvalidOperationException("The command cannot be executed because the canExecute action returned false.");
            }

            _execute(parameter);
        }

        /// <summary>
        /// 提升<see cref="E:CanExecuteChanged"/>事件.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            OnCanExecuteChanged(EventArgs.Empty);
        }

        /// <summary>
        /// 提升<see cref="E:CanExecuteChanged"/>事件.
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/>包含事件数据的实例.</param>
        protected virtual void OnCanExecuteChanged(EventArgs e)
        {
            var handler = CanExecuteChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
