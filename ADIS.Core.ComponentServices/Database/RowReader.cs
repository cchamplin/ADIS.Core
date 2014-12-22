using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.ComponentServices.Database.Readers;

namespace ADIS.Core.ComponentServices.Database
{
    public abstract class RowReader
    {
        TypeCode[] typeCodes;

        public RowReader()
        {
        }

        protected void Init()
        {
            this.typeCodes = new TypeCode[this.FieldCount];
        }

        protected abstract int FieldCount { get; }
        protected abstract Type GetFieldType(int ordinal);
        protected abstract bool IsDBNull(int ordinal, object[] data);
        protected abstract T GetValue<T>(int ordinal, object[] data);
        protected abstract Byte GetByte(int ordinal, object[] data);
        protected abstract Char GetChar(int ordinal, object[] data);
        protected abstract DateTime GetDateTime(int ordinal, object[] data);
        protected abstract Decimal GetDecimal(int ordinal, object[] data);
        protected abstract Boolean GetBoolean(int ordinal, object[] data);
        protected abstract Double GetDouble(int ordinal, object[] data);
        protected abstract Single GetSingle(int ordinal, object[] data);
        protected abstract Guid GetGuid(int ordinal, object[] data);
        protected abstract Byte[] GetByteArray(int ordinal, object[] data);
        protected abstract Char[] GetCharArray(int ordinal, object[] data);
        protected abstract Int16 GetInt16(int ordinal, object[] data);
        protected abstract Int32 GetInt32(int ordinal, object[] data);
        protected abstract Int64 GetInt64(int ordinal, object[] data);
        protected abstract String GetString(int ordinal, object[] data);

        public T ReadValue<T>(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(T);
            }
            return this.GetValue<T>(ordinal, data);
        }

        public T? ReadNullableValue<T>(int ordinal, object[] data) where T : struct
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(T?);
            }
            return this.GetValue<T>(ordinal, data);
        }

        public Byte ReadByte(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Byte);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Byte)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Byte)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Byte)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return (Byte)this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return (Byte)this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return (Byte)this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Byte>(ordinal, data);
                }
            }
        }

        public Byte? ReadNullableByte(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Byte?);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Byte)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Byte)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Byte)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return (Byte)this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return (Byte)this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return (Byte)this.GetDecimal(ordinal, data);
                    default:
                        return (Byte)this.GetValue<Byte>(ordinal, data);
                }
            }
        }

        public Char ReadChar(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Char);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return (Char)this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Char)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Char)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Char)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return (Char)this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return (Char)this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return (Char)this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Char>(ordinal, data);
                }
            }
        }

        public Boolean ReadBoolean(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Boolean);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Boolean:
                        return this.GetBoolean(ordinal, data);
                    case TypeCode.Byte:
                        return (this.GetByte(ordinal, data) > 0) ? true : false;
                    case TypeCode.Int16:
                        return (this.GetInt16(ordinal, data) > 0) ? true : false;
                    case TypeCode.Int32:
                        return (this.GetInt32(ordinal, data) > 0) ? true : false;
                    case TypeCode.Int64:
                        return (this.GetInt64(ordinal, data) > 0) ? true : false;
                    case TypeCode.Double:
                        return (this.GetDouble(ordinal, data) > 0) ? true : false;
                    case TypeCode.Single:
                        return (this.GetSingle(ordinal, data) > 0) ? true : false;
                    case TypeCode.Decimal:
                        return (this.GetDecimal(ordinal, data) > 0) ? true : false;
                    default:
                        return this.GetValue<Boolean>(ordinal, data);
                }
            }
        }


        public Char? ReadNullableChar(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Char?);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return (Char)this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Char)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Char)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Char)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return (Char)this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return (Char)this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return (Char)this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Char>(ordinal, data);
                }
            }
        }

        public DateTime ReadDateTime(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(DateTime);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.DateTime:
                        return this.GetDateTime(ordinal, data);
                    default:
                        return this.GetValue<DateTime>(ordinal, data);
                }
            }
        }

        public DateTime? ReadNullableDateTime(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(DateTime?);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.DateTime:
                        return this.GetDateTime(ordinal, data);
                    default:
                        return this.GetValue<DateTime>(ordinal, data);
                }
            }
        }

        public Decimal ReadDecimal(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Decimal);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return (Decimal)this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Decimal)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Decimal)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Decimal)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return (Decimal)this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return (Decimal)this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Decimal>(ordinal, data);
                }
            }
        }

        public Decimal? ReadNullableDecimal(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Decimal?);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return (Decimal)this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Decimal)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Decimal)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Decimal)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return (Decimal)this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return (Decimal)this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Decimal>(ordinal, data);
                }
            }
        }

        public Double ReadDouble(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Double);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return (Double)this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Double)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Double)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Double)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return (Double)this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return (Double)this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Double>(ordinal, data);
                }
            }
        }

        public Double? ReadNullableDouble(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Double?);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return (Double)this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Double)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Double)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Double)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return (Double)this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return (Double)this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Double>(ordinal, data);
                }
            }
        }

        public Single ReadSingle(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Single);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return (Single)this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Single)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Single)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Single)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return (Single)this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return (Single)this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Single>(ordinal, data);
                }
            }
        }

        public Single? ReadNullableSingle(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Single?);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return (Single)this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Single)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Single)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Single)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return (Single)this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return (Single)this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Single>(ordinal, data);
                }
            }
        }

        public Guid ReadGuid(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Guid);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case tcGuid:
                        return this.GetGuid(ordinal, data);
                    default:
                        return this.GetValue<Guid>(ordinal, data);
                }
            }
        }

        public Guid? ReadNullableGuid(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Guid?);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case tcGuid:
                        return this.GetGuid(ordinal, data);
                    default:
                        return this.GetValue<Guid>(ordinal, data);
                }
            }
        }

        public Int16 ReadInt16(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Int16);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return (Int16)this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Int16)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Int16)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Int16)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return (Int16)this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return (Int16)this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return (Int16)this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Int16>(ordinal, data);
                }
            }
        }

        public Int16? ReadNullableInt16(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Int16?);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return (Int16)this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Int16)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Int16)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Int16)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return (Int16)this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return (Int16)this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return (Int16)this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Int16>(ordinal, data);
                }
            }
        }

        public Int32 ReadInt32(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Int32);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return (Int32)this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Int32)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Int32)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Int32)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return (Int32)this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return (Int32)this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return (Int32)this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Int32>(ordinal, data);
                }
            }
        }

        public Int32? ReadNullableInt32(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Int32?);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return (Int32)this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Int32)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Int32)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Int32)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return (Int32)this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return (Int32)this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return (Int32)this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Int32>(ordinal, data);
                }
            }
        }

        public Int64 ReadInt64(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Int64);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return (Int64)this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Int64)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Int64)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Int64)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return (Int64)this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return (Int64)this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return (Int64)this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Int64>(ordinal, data);
                }
            }
        }

        public Int64? ReadNullableInt64(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Int64?);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return (Int64)this.GetByte(ordinal, data);
                    case TypeCode.Int16:
                        return (Int64)this.GetInt16(ordinal, data);
                    case TypeCode.Int32:
                        return (Int64)this.GetInt32(ordinal, data);
                    case TypeCode.Int64:
                        return (Int64)this.GetInt64(ordinal, data);
                    case TypeCode.Double:
                        return (Int64)this.GetDouble(ordinal, data);
                    case TypeCode.Single:
                        return (Int64)this.GetSingle(ordinal, data);
                    case TypeCode.Decimal:
                        return (Int64)this.GetDecimal(ordinal, data);
                    default:
                        return this.GetValue<Int64>(ordinal, data);
                }
            }
        }

        public String ReadString(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(String);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = Type.GetTypeCode(this.GetFieldType(ordinal));
                        continue;
                    case TypeCode.Byte:
                        return this.GetByte(ordinal, data).ToString();
                    case TypeCode.Boolean:
                        return this.GetBoolean(ordinal, data).ToString();
                    case TypeCode.Int16:
                        return this.GetInt16(ordinal, data).ToString();
                    case TypeCode.Int32:
                        return this.GetInt32(ordinal, data).ToString();
                    case TypeCode.Int64:
                        return this.GetInt64(ordinal, data).ToString();
                    case TypeCode.Double:
                        return this.GetDouble(ordinal, data).ToString();
                    case TypeCode.Single:
                        return this.GetSingle(ordinal, data).ToString();
                    case TypeCode.Decimal:
                        return this.GetDecimal(ordinal, data).ToString();
                    case TypeCode.DateTime:
                        return this.GetDateTime(ordinal, data).ToString();
                    case tcGuid:
                        return this.GetGuid(ordinal, data).ToString();
                    case TypeCode.String:
                        return this.GetString(ordinal, data);
                    default:
                        return this.GetValue<String>(ordinal, data);
                }
            }
        }

        public Byte[] ReadByteArray(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Byte[]);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Byte:
                        return new Byte[] { this.GetByte(ordinal, data) };
                    case tcByteArray:
                        return this.GetByteArray(ordinal, data);
                    default:
                        return this.GetValue<Byte[]>(ordinal, data);
                }
            }
        }

        public Char[] ReadCharArray(int ordinal, object[] data)
        {
            if (this.IsDBNull(ordinal, data))
            {
                return default(Char[]);
            }
            while (true)
            {
                switch (typeCodes[ordinal])
                {
                    case TypeCode.Empty:
                        typeCodes[ordinal] = GetTypeCode(ordinal, data);
                        continue;
                    case TypeCode.Char:
                        return new Char[] { this.GetChar(ordinal, data) };
                    case tcCharArray:
                        return this.GetCharArray(ordinal, data);
                    default:
                        return this.GetValue<Char[]>(ordinal, data);
                }
            }
        }

        private const TypeCode tcGuid = (TypeCode)20;
        private const TypeCode tcByteArray = (TypeCode)21;
        private const TypeCode tcCharArray = (TypeCode)22;

        private TypeCode GetTypeCode(int ordinal, object[] data)
        {
            Type type = this.GetFieldType(ordinal);
            TypeCode tc = Type.GetTypeCode(type);
            if (tc == TypeCode.Object)
            {
                if (type == typeof(Guid))
                    tc = tcGuid;
                else if (type == typeof(Byte[]))
                    tc = tcByteArray;
                else if (type == typeof(Char[]))
                    tc = tcCharArray;
            }
            return tc;
        }

        /*public static MethodInfo GetReaderMethod(Type type)
        {
            if (_readerMethods == null)
            {
                var meths = typeof(RowReader).GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(m => m.Name.StartsWith("Read")).ToList();
                _readerMethods = meths.ToDictionary(m => m.ReturnType);
                _miReadValue = meths.Single(m => m.Name == "ReadValue");
                _miReadNullableValue = meths.Single(m => m.Name == "ReadNullableValue");
            }

            MethodInfo mi;
            _readerMethods.TryGetValue(type, out mi);
            if (mi == null)
            {
                if (TypeHelper.IsNullableType(type))
                {
                    mi = _miReadNullableValue.MakeGenericMethod(TypeHelper.GetNonNullableType(type));
                }
                else
                {
                    mi = _miReadValue.MakeGenericMethod(type);
                }
            }
            return mi;
        }
        */
        /*public static ITypedReader GetReaderClass(Type type)
        {
            if (_readerTypes == null)
            {
                var types = typeof(RowReader).Assembly.GetTypes().Where(m => m.GetInterface("TypedReader") != null).ToList();
                List<ITypedReader> readers = new List<ITypedReader>();
                foreach (Type t in types)
                {
                    if (t.Name.StartsWith("Value"))
                    {
                        _trReadValue = t;
                    }
                    else if (t.Name.StartsWith("NullableValue"))
                    {
                        _trReadNullableValue = t;
                    }
                    else
                    {
                        readers.Add((ITypedReader)Activator.CreateInstance(t));
                    }
                }
                _readerTypes = readers.ToDictionary(m => m.DataType);
            }
            ITypedReader tr;
            _readerTypes.TryGetValue(type, out tr);
            if (tr == null)
            {
                if (TypeHelper.IsNullableType(type))
                {
                    tr = (ITypedReader)Activator.CreateInstance(_trReadNullableValue.MakeGenericType(TypeHelper.GetNonNullableType(type)));
                }
                else
                {
                    tr = (ITypedReader)Activator.CreateInstance(_trReadValue.MakeGenericType(type));
                }
            }
            return tr;
        }*/

        //static Dictionary<Type, ITypedReader> _readerTypes;
        //static Dictionary<Type, MethodInfo> _readerMethods;
        //static Type _trReadValue;
       // static Type _trReadNullableValue;
        //static MethodInfo _miReadValue;
       // static MethodInfo _miReadNullableValue;
    }
}
