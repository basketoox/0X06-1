using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace System.Toolkit.Helpers
{
    /// <summary>
    /// 定义字段克隆类型的枚举.
    /// 与CloneAttribute组合使用
    /// </summary>
    public enum CloneType
    {
        None,
        ShallowCloning,
        DeepCloning
    }

    /// <summary>
    /// 用于指定字段的cloneproperties的CloningAttribute.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class CloneAttribute : Attribute
    {
        private CloneType _clonetype;
        public CloneAttribute()
        {

        }

        public CloneType CloneType
        {
            get { return _clonetype; }
            set { _clonetype = value; }
        }
    }

    /// <summary>
    /// 克隆对象的类
    /// </summary>
    public static class CloneHelper<T>
    where T : class
    {
        #region Declarations
        // 用于缓存(预编译)编译后的IL代码的字典.
        private static Dictionary<Type, Delegate> _cachedILShallow = new Dictionary<Type, Delegate>();
        private static Dictionary<Type, Delegate> _cachedILDeep = new Dictionary<Type, Delegate>();
        // 这用于设置固定的克隆，该克隆为null，然后应该调用自定义克隆。
        // (使用Clone(T obj)进行定制克隆)
        private static CloneType? _globalCloneType = CloneType.ShallowCloning;

        #endregion

        #region Public Methods

        /// <summary>
        /// 使用深度克隆或自定义策略克隆对象 
        /// 例如浅克隆和/或深克隆组合(使用CloneAttribute)
        /// </summary>
        /// <param name="obj">对象以执行克隆.</param>
        /// <returns>克隆的对象.</returns>
        public static T Clone(T obj)
        {
            _globalCloneType = null;
            return CloneObjectWithILDeep(obj);
        }

        /// <summary>
        /// 用一种策略克隆对象(DeepClone或ShallowClone)
        /// </summary>
        /// <param name="obj">对象以执行克隆.</param>
        /// <param name="cloneType">类型的克隆</param>
        /// <returns>克隆的对象.</returns>
        /// <exception cref="InvalidOperationException">传递cloningtype的错误枚举时.</exception>
        public static T Clone(T obj, CloneType cloneType)
        {
            if (_globalCloneType != null)
                _globalCloneType = cloneType;
            switch (cloneType)
            {
                case CloneType.None:
                    throw new InvalidOperationException("No need to call this method?");
                case CloneType.ShallowCloning:
                    return CloneObjectWithILShallow(obj);
                case CloneType.DeepCloning:
                    return CloneObjectWithILDeep(obj);
                default:
                    break;
            }
            return default(T);
        }

        #endregion

        #region Private Methods

        /// <summary>    
        /// 用IL克隆对象的通用克隆方法.    
        /// 只有特定类型的第一个调用才会影响性能.    
        /// 在第一次调用之后，执行已编译的IL.    
        /// </summary>
        /// <typeparam name="T">要克隆的对象类型</typeparam>    
        /// <param name="myObject">对象克隆</param>    
        /// <returns>克隆的对象(shallow)</returns>    
        private static T CloneObjectWithILShallow(T myObject)
        {
            Delegate myExec = null;
            if (!_cachedILShallow.TryGetValue(typeof(T), out myExec))
            {
                var dymMethod = new DynamicMethod("DoShallowClone", typeof(T), new Type[] { typeof(T) }, Assembly.GetExecutingAssembly().ManifestModule, true);
                var cInfo = myObject.GetType().GetConstructor(new Type[] { });
                var generator = dymMethod.GetILGenerator();
                var lbf = generator.DeclareLocal(typeof(T));
                generator.Emit(OpCodes.Newobj, cInfo);
                generator.Emit(OpCodes.Stloc_0);
                foreach (var field in myObject.GetType().GetFields(System.Reflection.BindingFlags.Instance
                | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public))
                {
                    generator.Emit(OpCodes.Ldloc_0);
                    generator.Emit(OpCodes.Ldarg_0);
                    generator.Emit(OpCodes.Ldfld, field);
                    generator.Emit(OpCodes.Stfld, field);
                }
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Ret);
                myExec = dymMethod.CreateDelegate(typeof(Func<T, T>));
                _cachedILShallow.Add(typeof(T), myExec);
            }
            return ((Func<T, T>)myExec)(myObject);
        }

        /// <summary>
        /// 用IL克隆对象的通用克隆方法.
        /// 只有特定类型的第一个调用才会影响性能.
        /// 在第一次调用之后，执行已编译的IL. 
        /// </summary>
        /// <param name="myObject">要克隆的对象类型</param>
        /// <returns>克隆对象(深度克隆)</returns>
        private static T CloneObjectWithILDeep(T myObject)
        {
            Delegate myExec = null;
            if (!_cachedILDeep.TryGetValue(typeof(T), out myExec))
            {
                // Create ILGenerator            
                var dymMethod = new DynamicMethod("DoDeepClone", typeof(T), new Type[] { typeof(T) }, Assembly.GetExecutingAssembly().ManifestModule, true);
                var generator = dymMethod.GetILGenerator();
                var cloneVariable = generator.DeclareLocal(myObject.GetType());

                var cInfo = myObject.GetType().GetConstructor(Type.EmptyTypes);
                generator.Emit(OpCodes.Newobj, cInfo);
                generator.Emit(OpCodes.Stloc, cloneVariable);

                foreach (var field in typeof(T).GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public))
                {
                    if (_globalCloneType == CloneType.DeepCloning)
                    {
                        if (field.FieldType.IsValueType || field.FieldType == typeof(string))
                        {
                            generator.Emit(OpCodes.Ldloc, cloneVariable);
                            generator.Emit(OpCodes.Ldarg_0);
                            generator.Emit(OpCodes.Ldfld, field);
                            generator.Emit(OpCodes.Stfld, field);
                        }
                        else if (field.FieldType.IsClass)
                        {
                            CopyReferenceType(generator, cloneVariable, field);
                        }
                    }
                    else
                    {
                        switch (GetCloneTypeForField(field))
                        {
                            case CloneType.ShallowCloning:
                                {
                                    generator.Emit(OpCodes.Ldloc, cloneVariable);
                                    generator.Emit(OpCodes.Ldarg_0);
                                    generator.Emit(OpCodes.Ldfld, field);
                                    generator.Emit(OpCodes.Stfld, field);
                                    break;
                                }
                            case CloneType.DeepCloning:
                                {
                                    if (field.FieldType.IsValueType || field.FieldType == typeof(string))
                                    {
                                        generator.Emit(OpCodes.Ldloc, cloneVariable);
                                        generator.Emit(OpCodes.Ldarg_0);
                                        generator.Emit(OpCodes.Ldfld, field);
                                        generator.Emit(OpCodes.Stfld, field);
                                    }
                                    else if (field.FieldType.IsClass)
                                        CopyReferenceType(generator, cloneVariable, field);
                                    break;
                                }
                            case CloneType.None:
                                {
                                    // 这里什么都不做，字段不克隆.
                                }
                                break;
                        }
                    }
                }
                generator.Emit(OpCodes.Ldloc_0);
                generator.Emit(OpCodes.Ret);
                myExec = dymMethod.CreateDelegate(typeof(Func<T, T>));
                _cachedILDeep.Add(typeof(T), myExec);
            }
            return ((Func<T, T>)myExec)(myObject);
        }

        /// <summary>
        /// 帮助方法来克隆引用类型.
        /// 这个方法克隆了IList和IEnumerables和其他引用类型(类)
        /// 不支持数组 (ex. string[])
        /// </summary>
        /// <param name="generator">IL发生器发出代码到.</param>
        /// <param name="cloneVar">克隆对象所在的本地存储. (or child of)</param>
        /// <param name="field">要克隆的引用类型的字段定义.</param>
        private static void CopyReferenceType(ILGenerator generator, LocalBuilder cloneVar, FieldInfo field)
        {
            if (field.FieldType.IsSubclassOf(typeof(Delegate)))
            {
                return;
            }
            var lbTempVar = generator.DeclareLocal(field.FieldType);

            if (field.FieldType.GetInterface("IEnumerable") != null && field.FieldType.GetInterface("IList") != null)
            {
                if (field.FieldType.IsGenericType)
                {
                    var argumentType = field.FieldType.GetGenericArguments()[0];
                    var genericTypeEnum = Type.GetType("System.Collections.Generic.IEnumerable`1[" + argumentType.FullName + "]");

                    var ci = field.FieldType.GetConstructor(new Type[] { genericTypeEnum });
                    if (ci != null && GetCloneTypeForField(field) == CloneType.ShallowCloning)
                    {
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldfld, field);
                        generator.Emit(OpCodes.Newobj, ci);
                        generator.Emit(OpCodes.Stloc, lbTempVar);
                        generator.Emit(OpCodes.Ldloc, cloneVar);
                        generator.Emit(OpCodes.Ldloc, lbTempVar);
                        generator.Emit(OpCodes.Stfld, field);
                    }
                    else
                    {
                        ci = field.FieldType.GetConstructor(Type.EmptyTypes);
                        if (ci != null)
                        {
                            generator.Emit(OpCodes.Newobj, ci);
                            generator.Emit(OpCodes.Stloc, lbTempVar);
                            generator.Emit(OpCodes.Ldloc, cloneVar);
                            generator.Emit(OpCodes.Ldloc, lbTempVar);
                            generator.Emit(OpCodes.Stfld, field);
                            CloneList(generator, field, argumentType, lbTempVar);
                        }
                    }
                }
            }
            else
            {
                var cInfo = field.FieldType.GetConstructor(new Type[] { });
                generator.Emit(OpCodes.Newobj, cInfo);
                generator.Emit(OpCodes.Stloc, lbTempVar);
                generator.Emit(OpCodes.Ldloc, cloneVar);
                generator.Emit(OpCodes.Ldloc, lbTempVar);
                generator.Emit(OpCodes.Stfld, field);
                foreach (var fi in field.FieldType.GetFields(System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public))
                {
                    if (fi.FieldType.IsValueType || fi.FieldType == typeof(string))
                    {
                        generator.Emit(OpCodes.Ldloc_1);
                        generator.Emit(OpCodes.Ldarg_0);
                        generator.Emit(OpCodes.Ldfld, field);
                        generator.Emit(OpCodes.Ldfld, fi);
                        generator.Emit(OpCodes.Stfld, fi);
                    }
                }
            }
        }

        /// <summary>
        /// 制作一个可列举的列表
        /// 创建列表的新对象并包含对象。(使用默认构造函数)
        /// 通过调用上面定义的deepclone方法. (recursive)
        /// </summary>
        /// <param name="generator">IL发生器发出代码到.</param>
        /// <param name="listField">要克隆的列表的引用类型的字段定义.</param>
        /// <param name="typeToClone">要克隆的基类型(List<T>的参数)</param>
        /// <param name="cloneVar">克隆对象所在的本地存储. (or child of)</param>
        private static void CloneList(ILGenerator generator, FieldInfo listField, Type typeToClone, LocalBuilder cloneVar)
        {
            var genIEnumeratorTyp = Type.GetType("System.Collections.Generic.IEnumerator`1[" + typeToClone.FullName + "]");
            var genIEnumeratorTypLocal = Type.GetType(listField.FieldType.Namespace + "." + listField.FieldType.Name + "+Enumerator[[" + typeToClone.FullName + "]]");
            var lbEnumObject = generator.DeclareLocal(genIEnumeratorTyp);
            var lbCheckStatement = generator.DeclareLocal(typeof(bool));
            var checkOfWhile = generator.DefineLabel();
            var startOfWhile = generator.DefineLabel();
            var miEnumerator = listField.FieldType.GetMethod("GetEnumerator");
            generator.Emit(OpCodes.Ldarg_0);
            generator.Emit(OpCodes.Ldfld, listField);
            generator.Emit(OpCodes.Callvirt, miEnumerator);
            if (genIEnumeratorTypLocal != null)
            {
                generator.Emit(OpCodes.Box, genIEnumeratorTypLocal);
            }
            generator.Emit(OpCodes.Stloc, lbEnumObject);
            generator.Emit(OpCodes.Br_S, checkOfWhile);
            generator.MarkLabel(startOfWhile);
            generator.Emit(OpCodes.Nop);
            generator.Emit(OpCodes.Ldloc, cloneVar);
            generator.Emit(OpCodes.Ldloc, lbEnumObject);
            var miCurrent = genIEnumeratorTyp.GetProperty("Current").GetGetMethod();
            generator.Emit(OpCodes.Callvirt, miCurrent);
            var cloneHelper = Type.GetType(typeof(CloneHelper<T>).Namespace + "." + typeof(CloneHelper<T>).Name + "[" + miCurrent.ReturnType.FullName + "]");
            var miDeepClone = cloneHelper.GetMethod("CloneObjectWithILDeep", BindingFlags.Static | BindingFlags.NonPublic);
            generator.Emit(OpCodes.Call, miDeepClone);
            var miAdd = listField.FieldType.GetMethod("Add");
            generator.Emit(OpCodes.Callvirt, miAdd);
            generator.Emit(OpCodes.Nop);
            generator.MarkLabel(checkOfWhile);
            generator.Emit(OpCodes.Nop);
            generator.Emit(OpCodes.Ldloc, lbEnumObject);
            var miMoveNext = typeof(IEnumerator).GetMethod("MoveNext");
            generator.Emit(OpCodes.Callvirt, miMoveNext);
            generator.Emit(OpCodes.Stloc, lbCheckStatement);
            generator.Emit(OpCodes.Ldloc, lbCheckStatement);
            generator.Emit(OpCodes.Brtrue_S, startOfWhile);
        }

        /// <summary>
        /// 返回在自定义模式下应用于特定字段的克隆类型.
        /// 否则返回主克隆方法.
        /// 您可以通过调用该Clone(T obj)方法来调用自定义模式 
        /// </summary>
        /// <param name="field">现场检查</param>
        /// <returns>用于此字段的克隆类型.</returns>
        private static CloneType GetCloneTypeForField(FieldInfo field)
        {
            var attributes = field.GetCustomAttributes(typeof(CloneAttribute), true);
            if (attributes == null || attributes.Length == 0)
            {
                if (!_globalCloneType.HasValue)
                    return CloneType.ShallowCloning;
                else
                    return _globalCloneType.Value;
            }
            return (attributes[0] as CloneAttribute).CloneType;
        }

        #endregion
    }
}