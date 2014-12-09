using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ADIS.Core.Data.Util
{
    internal static class TypeHelper
    {
        internal static Dictionary<Type, ConstructorDelegate> _constructorCache;
        static TypeHelper()
        {
            _constructorCache = new Dictionary<Type, ConstructorDelegate>();

        }
        public delegate object ConstructorDelegate();
        public static ConstructorDelegate GetConstructor(Type type)
        {
            lock (_constructorCache)
            {
                ConstructorDelegate tDel;
                if (_constructorCache.TryGetValue(type, out tDel))
                    return tDel;

                ConstructorInfo ci = type.GetConstructor(Type.EmptyTypes);
                if (ci == null)
                {
                    ci = type.GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
                }
                if (ci != null)
                {
                    DynamicMethod method = new System.Reflection.Emit.DynamicMethod("__Ctor", type, Type.EmptyTypes, typeof(TypeHelper).Module, true);
                    ILGenerator gen = method.GetILGenerator();
                    gen.Emit(System.Reflection.Emit.OpCodes.Newobj, ci);
                    gen.Emit(System.Reflection.Emit.OpCodes.Ret);
                    tDel = (ConstructorDelegate)method.CreateDelegate(typeof(ConstructorDelegate));
                    _constructorCache.Add(type, tDel);
                    return tDel;
                }
                return () => Activator.CreateInstance(type);
            }
        }
        public static void Cast(ILGenerator il, Type type, LocalBuilder addr)
        {
            if (type == typeof(object)) { }
            else if (type.IsValueType)
            {
                il.Emit(OpCodes.Unbox_Any, type);
                if (addr != null)
                {
                    il.Emit(OpCodes.Stloc, addr);
                    il.Emit(OpCodes.Ldloca_S, addr);
                }
            }
            else
            {
                il.Emit(OpCodes.Castclass, type);
            }
        }
    }
}
