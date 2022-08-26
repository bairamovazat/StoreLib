using System;

namespace StoreLib.Exceptions
{
    public class CommonException : StoreLibException
    {
        public CommonException()
        {
        }

        public CommonException(string message)
            : base(message)
        {
        }

        public CommonException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}