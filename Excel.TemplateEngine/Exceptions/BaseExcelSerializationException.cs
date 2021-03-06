using System;
using System.Runtime.Serialization;

namespace SkbKontur.Excel.TemplateEngine.Exceptions
{
    public class BaseExcelSerializationException : Exception
    {
        public BaseExcelSerializationException()
        {
        }

        public BaseExcelSerializationException(string message)
            : base(message)
        {
        }

        public BaseExcelSerializationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected BaseExcelSerializationException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}