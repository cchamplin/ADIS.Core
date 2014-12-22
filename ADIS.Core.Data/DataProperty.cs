using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.Util;

namespace ADIS.Core.Data
{
    public class DataProperty
    {
        public delegate void PropertySetter(object instance, object value);
        public delegate object PropertyGetter(object instance);
        protected Type type;
        protected PropertySetter setter;
        protected PropertyGetter getter;
        protected string accessFor;
        protected Type genericType;
        protected bool isList;
        protected bool isDictionary;
        protected string label;
        protected string name;
        public Type Type
        {
            get
            {
                return type;
            }
        }
        public string Label
        {
            get
            {
                return label;
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

        public DataProperty(Type t, PropertyInfo pi)
        {
            this.type = pi.PropertyType;
            this.name = pi.Name;
            foreach (Attribute attr in pi.GetCustomAttributes(false))
            {
                if (attr is PropertyLabel)
                {
                    this.label = ((PropertyLabel)attr).label;
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
                gen.EmitCall(t.IsValueType ? OpCodes.Call : OpCodes.Callvirt, pi.GetSetMethod(), null);
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
        public DataProperty(Type t, FieldInfo fi)
        {
            this.type = fi.FieldType;
            this.name = fi.Name;
            foreach (Attribute attr in fi.GetCustomAttributes(false))
            {
                if (attr is PropertyLabel)
                {
                    this.label = ((PropertyLabel)attr).label;
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
