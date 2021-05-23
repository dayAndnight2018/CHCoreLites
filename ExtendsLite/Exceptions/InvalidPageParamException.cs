using System;

namespace ExtendsLite.Exceptions
{
    public class InvalidPageParamException : Exception
    {
        /// <summary>
        /// default exception message
        /// </summary>
        private static readonly string DEFAULT_MESSAGE = "invalid page or size";
        
        public InvalidPageParamException() : this(DEFAULT_MESSAGE) { }

        public InvalidPageParamException(string message) : base(message) { }
    }
}