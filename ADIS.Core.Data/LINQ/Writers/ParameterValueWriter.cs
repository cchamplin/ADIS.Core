using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADIS.Core.Data.LINQ.Writers.Interfaces;

namespace ADIS.Core.Data.LINQ.Writers
{

    public static class ParameterValueWriter
    {
        private static readonly IList<IValueWriter> _valueWriters;
        private static readonly IList<IResolvedValueWriter> _resolvedValueWriters;
        static ParameterValueWriter()
        {
            _valueWriters = new List<IValueWriter>
							{
                                new EnumValueWriter(),
								new StringValueWriter(),
								new BooleanValueWriter(),
								new IntValueWriter(),
								new LongValueWriter(),
								new ShortValueWriter(),
								new UnsignedIntValueWriter(),
								new UnsignedLongValueWriter(),
								new UnsignedShortValueWriter(),
								new ByteArrayValueWriter(),
								new StreamValueWriter(),
								new DecimalValueWriter(),
								new DoubleValueWriter(),
								new SingleValueWriter(),
								new ByteValueWriter(),
								new GuidValueWriter(),
								new DateTimeValueWriter(),
								new TimeSpanValueWriter(),
								new DateTimeOffsetValueWriter()
							};
            _resolvedValueWriters = new List<IResolvedValueWriter>
                            {
                                new ExpansionWriter(),
                            };
        }

        public static string Write(object value, INameResolver nameResolver, FragmentWriterType writerType)
        {
            if (value == null)
            {
                return "null";
            }


            var type = value.GetType();

            if (type.IsEnum)
            {
                return value.ToString();
            }

            var writer = _valueWriters.FirstOrDefault(x => x.Handles(type));

            if (writer != null)
            {
                switch (writerType)
                {
                    case FragmentWriterType.SQL:
                        return writer.WriteSqlFragment(value);
                    case FragmentWriterType.URI:
                        return writer.WriteUriFragment(value);
                }

            }

            var resolvedWriter = _resolvedValueWriters.FirstOrDefault(x => x.Handles(type));

            if (resolvedWriter != null)
            {
                switch (writerType)
                {
                    case FragmentWriterType.SQL:
                        return resolvedWriter.WriteSqlFragment(value, nameResolver);
                    case FragmentWriterType.URI:
                        return resolvedWriter.WriteUriFragment(value,nameResolver);
                }

            }


            if (typeof(Nullable<>).IsAssignableFrom(type))
            {
                var genericParameter = type.GetGenericArguments()[0];

                return Write(Convert.ChangeType(value, genericParameter, CultureInfo.CurrentCulture),nameResolver, writerType);
            }


            return value.ToString();
        }
    }
}
