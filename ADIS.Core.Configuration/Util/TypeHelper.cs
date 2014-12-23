using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.ComponentServices;

namespace ADIS.Core.Configuration.Util
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
        public class PropertyAccessor
        {
            public delegate void PropertySetter(object instance, object value);
            public delegate object PropertyGetter(object instance);
            protected Type type;
            protected PropertySetter setter;
            protected PropertyGetter getter;
            protected Type genericType;
            protected bool isList;
            protected bool isDictionary;
            protected string name;
            protected ConfigurationProperty configProp = null;
            protected bool hasDataType;
            protected DbDataType dataType = DbDataType.INT;
            public ConfigurationProperty ConfigurationProperty
            {
                get
                {
                    return configProp;
                }
                
            }
            public bool HasDataType
            {
                get
                {
                    return hasDataType;
                }
            }
            public Type Type
            {
                get
                {
                    return type;
                }
            }
            public DbDataType DataType
            {
                get
                {
                    return dataType;
                }
            }
            public string Name
            {
                get
                {
                    return name;
                }
            }
            public PropertySetter Set
            {
                get
                {
                    return setter;
                }
            }
            public PropertyGetter Get
            {
                get
                {
                    return getter;
                }
            }
            public PropertyAccessor(Type t, PropertyInfo pi)
            {
                name = pi.Name;
                type = t;

                 var attributes = pi.GetCustomAttributes();
                 foreach (var attr in attributes)
                 {
                     if (attr is ConfigurationProperty)
                     {
                         configProp = attr as ConfigurationProperty;
                     }
                     if (attr is ConfigurationPropertyType)
                     {
                         dataType = ((ConfigurationPropertyType)attr).Type;
                         hasDataType = true;
                     }
                 }


                if (this.type.IsGenericType)
                {
                    foreach (var i in type.GetInterfaces())
                    {
                        if (i == typeof(IList))
                        {
                            this.isList = true;
                            this.genericType = this.type.GetGenericArguments()[0];
                            break;
                        }
                        if (i == typeof(IDictionary))
                        {
                            this.isDictionary = true;
                            this.genericType = this.type.GetGenericArguments()[1];
                        }
                    }
                }
                ILGenerator gen;
                DynamicMethod method;
                LocalBuilder loc;
                if (pi.CanWrite)
                {
                    method = new System.Reflection.Emit.DynamicMethod("__setter" + pi.Name, typeof(void), new Type[] { typeof(Object), typeof(Object) }, t, true);


                    gen = method.GetILGenerator();

                    loc = t.IsValueType ? gen.DeclareLocal(t) : null;
                    gen.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
                    TypeHelper.Cast(gen, t, loc);
                    gen.Emit(System.Reflection.Emit.OpCodes.Ldarg_1);
                    TypeHelper.Cast(gen, pi.PropertyType, null);
                    
                    gen.EmitCall(t.IsValueType ? OpCodes.Call : OpCodes.Callvirt, pi.GetSetMethod(true), null);
                    gen.Emit(System.Reflection.Emit.OpCodes.Ret);
                    setter = (PropertySetter)method.CreateDelegate(typeof(PropertySetter));
                }



                method = new System.Reflection.Emit.DynamicMethod("__getter" + pi.Name, typeof(object), new Type[] { typeof(object) }, t, true);

                gen = method.GetILGenerator();
                loc = t.IsValueType ? gen.DeclareLocal(t) : null;
                gen.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
                TypeHelper.Cast(gen, t, loc);
                gen.EmitCall(t.IsValueType ? OpCodes.Call : OpCodes.Callvirt, pi.GetGetMethod(), null);
                if (pi.PropertyType.IsValueType)
                {
                    gen.Emit(OpCodes.Box, pi.PropertyType);
                }
                //TypeHelper.Cast(gen, typeof(object), null);
                gen.Emit(System.Reflection.Emit.OpCodes.Ret);
                getter = (PropertyGetter)method.CreateDelegate(typeof(PropertyGetter));


            }
            public PropertyAccessor(Type t, FieldInfo fi)
            {
                name = fi.Name;
                type = t;
                var attributes = fi.GetCustomAttributes();
                foreach (var attr in attributes)
                {
                    if (attr is ConfigurationProperty)
                    {
                        configProp = attr as ConfigurationProperty;
                    }
                    if (attr is ConfigurationPropertyType)
                    {
                        dataType = ((ConfigurationPropertyType)attr).Type;
                        hasDataType = true;
                    }
                }
                if (this.type.IsGenericType)
                {
                    foreach (var i in type.GetInterfaces())
                    {
                        if (i == typeof(IList))
                        {
                            this.isList = true;
                            this.genericType = this.type.GetGenericArguments()[0];
                            break;
                        }
                        if (i == typeof(IDictionary))
                        {
                            this.isDictionary = true;
                            this.genericType = this.type.GetGenericArguments()[1];
                        }
                    }
                }
                DynamicMethod method = new System.Reflection.Emit.DynamicMethod("__setter" + fi.Name, typeof(void), new Type[] { typeof(Object), typeof(Object) }, t, true);

                ILGenerator gen = method.GetILGenerator();
                LocalBuilder loc = t.IsValueType ? gen.DeclareLocal(t) : null;
                gen.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
                TypeHelper.Cast(gen, t, loc);
                gen.Emit(System.Reflection.Emit.OpCodes.Ldarg_1);
                TypeHelper.Cast(gen, fi.FieldType, null);
                gen.Emit(System.Reflection.Emit.OpCodes.Stfld, fi);
                gen.Emit(System.Reflection.Emit.OpCodes.Ret);
                setter = (PropertySetter)method.CreateDelegate(typeof(PropertySetter));


                method = new System.Reflection.Emit.DynamicMethod("__getter" + fi.Name, typeof(object), new Type[] { typeof(object) }, t, true);

                gen = method.GetILGenerator();
                loc = t.IsValueType ? gen.DeclareLocal(t) : null;
                //gen.Emit(System.Reflection.Emit.OpCodes.Nop);
                gen.Emit(System.Reflection.Emit.OpCodes.Ldarg_0);
                TypeHelper.Cast(gen, t, loc);
                gen.Emit(OpCodes.Ldfld, fi);
                if (fi.FieldType.IsValueType)
                {
                    gen.Emit(OpCodes.Box, fi.FieldType);
                }
                gen.Emit(System.Reflection.Emit.OpCodes.Ret);
                getter = (PropertyGetter)method.CreateDelegate(typeof(PropertyGetter));


            }
        }
    }
}
