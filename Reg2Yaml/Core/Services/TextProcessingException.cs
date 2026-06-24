using System;

namespace Reg2Yaml.Core.Services
{
    public class TextProcessingException : Exception
    {
        public TextProcessingException(string message, Exception innerException)
            : base(message, innerException)
            {
            }
    }
}