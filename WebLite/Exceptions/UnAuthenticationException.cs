using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebLite.Exceptions
{
    public class UnAuthenticationException :Exception
    {
        public UnAuthenticationException() { }

        public UnAuthenticationException(string message) : base(message) { }
    }
}
