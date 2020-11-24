// Copyright (c) 2008 Daniel Grunwald
// 
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.

using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;

namespace System.Patterns
{
    /// <summary>
    ///     管理弱事件的类.
    /// </summary>
    /// <see cref="http://www.codeproject.com/Articles/29922/Weak-Events-in-C" />
    public sealed class FastSmartWeakEvent<T> where T : class
    {
        private readonly List<EventEntry> _eventEntries = new List<EventEntry>();

        static FastSmartWeakEvent()
        {
            if (!typeof (T).IsSubclassOf(typeof (Delegate)))
                throw new ArgumentException("T must be a delegate type");
            var invoke = typeof (T).GetMethod("Invoke");
            if (invoke == null || invoke.GetParameters().Length != 2)
                throw new ArgumentException("T must be a delegate type taking 2 parameters");
            var senderParameter = invoke.GetParameters()[0];
            if (senderParameter.ParameterType != typeof (object))
                throw new ArgumentException("The first delegate parameter must be of type 'object'");
            var argsParameter = invoke.GetParameters()[1];
            if (!(typeof (EventArgs).IsAssignableFrom(argsParameter.ParameterType)))
                throw new ArgumentException("The second delegate parameter must be derived from type 'EventArgs'");
            if (invoke.ReturnType != typeof (void))
                throw new ArgumentException("The delegate return type must be void.");
        }

        public void Add(T eh)
        {
            if (eh != null)
            {
                var d = (Delegate) (object) eh;
                if (_eventEntries.Count == _eventEntries.Capacity)
                    RemoveDeadEntries();
                var targetMethod = d.Method;
                var targetInstance = d.Target;
                var target = targetInstance != null ? new WeakReference(targetInstance) : null;
                _eventEntries.Add(new EventEntry(FastSmartWeakEventForwarderProvider.GetForwarder(targetMethod),
                    targetMethod, target));
            }
        }

        private void RemoveDeadEntries()
        {
            _eventEntries.RemoveAll(ee => ee.TargetReference != null && !ee.TargetReference.IsAlive);
        }

        public void Remove(T eh)
        {
            if (eh != null)
            {
                var d = (Delegate) (object) eh;
                var targetInstance = d.Target;
                var targetMethod = d.Method;
                for (var i = _eventEntries.Count - 1; i >= 0; i--)
                {
                    var entry = _eventEntries[i];
                    if (entry.TargetReference != null)
                    {
                        var target = entry.TargetReference.Target;
                        if (target == null)
                        {
                            _eventEntries.RemoveAt(i);
                        }
                        else if (target == targetInstance && entry.TargetMethod == targetMethod)
                        {
                            _eventEntries.RemoveAt(i);
                            break;
                        }
                    }
                    else
                    {
                        if (targetInstance == null && entry.TargetMethod == targetMethod)
                        {
                            _eventEntries.RemoveAt(i);
                            break;
                        }
                    }
                }
            }
        }

        public void Raise(object sender, EventArgs e)
        {
            var needsCleanup = false;
            foreach (var ee in _eventEntries.ToArray())
            {
                needsCleanup |= ee.Forwarder(ee.TargetReference, sender, e);
            }
            if (needsCleanup)
                RemoveDeadEntries();
        }

        private struct EventEntry
        {
            public readonly FastSmartWeakEventForwarderProvider.ForwarderDelegate Forwarder;
            public readonly MethodInfo TargetMethod;
            public readonly WeakReference TargetReference;

            public EventEntry(FastSmartWeakEventForwarderProvider.ForwarderDelegate forwarder, MethodInfo targetMethod,
                WeakReference targetReference)
            {
                Forwarder = forwarder;
                TargetMethod = targetMethod;
                TargetReference = targetReference;
            }
        }
    }

    // 转发器生成的代码属于单独的类，因为它不依赖于类型T.
    internal static class FastSmartWeakEventForwarderProvider
    {
        private static readonly MethodInfo getTarget = typeof (WeakReference).GetMethod("get_Target");

        private static readonly Type[] forwarderParameters =
        {
            typeof (WeakReference), typeof (object),
            typeof (EventArgs)
        };

        private static readonly Dictionary<MethodInfo, ForwarderDelegate> forwarders =
            new Dictionary<MethodInfo, ForwarderDelegate>();

        internal static ForwarderDelegate GetForwarder(MethodInfo method)
        {
            lock (forwarders)
            {
                ForwarderDelegate d;
                if (forwarders.TryGetValue(method, out d))
                    return d;
            }

            if (method.DeclaringType.GetCustomAttributes(typeof (CompilerGeneratedAttribute), false).Length != 0)
                throw new ArgumentException("Cannot create weak event to anonymous method with closure.");
            var parameters = method.GetParameters();

            Debug.Assert(getTarget != null);

            var dm = new DynamicMethod(
                "FastSmartWeakEvent", typeof (bool), forwarderParameters, method.DeclaringType);

            var il = dm.GetILGenerator();

            if (!method.IsStatic)
            {
                il.Emit(OpCodes.Ldarg_0);
                il.EmitCall(OpCodes.Callvirt, getTarget, null);
                il.Emit(OpCodes.Dup);
                var label = il.DefineLabel();
                il.Emit(OpCodes.Brtrue, label);
                il.Emit(OpCodes.Pop);
                il.Emit(OpCodes.Ldc_I4_1);
                il.Emit(OpCodes.Ret);
                il.MarkLabel(label);
                //要验证生成的代码，这里的castclass是必需的.
                //我们可以忽略它，因为我们知道这个演员总是会成功的
                //(实例/方法对取自委托).
                //无法验证的代码是可以的，因为私有反射只在充分信任的情况下被允许.
                //il.Emit(OpCodes.Castclass, method.DeclaringType);
            }
            il.Emit(OpCodes.Ldarg_1);
            il.Emit(OpCodes.Ldarg_2);
                //为了防止在. net类型系统中创建漏洞，这里需要使用这个castclass.
                //程序.在“SmartWeakEventBenchmark”中查看效果的问题
                //这个类型没有使用.
                //如果你相信添加FastSmartWeakEvent，你可以移除这个强制类型.召集呼叫者去做
                //这是对的，但性能的小幅提升(约5%)通常不值得冒险.
            il.Emit(OpCodes.Castclass, parameters[1].ParameterType);

            il.EmitCall(OpCodes.Call, method, null);
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Ret);

            var fd = (ForwarderDelegate) dm.CreateDelegate(typeof (ForwarderDelegate));
            lock (forwarders)
            {
                forwarders[method] = fd;
            }
            return fd;
        }

        internal delegate bool ForwarderDelegate(WeakReference wr, object sender, EventArgs e);
    }
}