using System;
using System.Collections.Generic;
using System.Text;

namespace WebLite.Exceptions
{
    public class UnAuthorizationException :Exception
    {
        public UnAuthorizationException()
        {

        }

        public UnAuthorizationException(string message):base(message)
        {

        }
    }
}
