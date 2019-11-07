using System;
using System.Collections.Generic;
using System.Text;

namespace WebLite
{
    public class TransResult
    {
        public TransResult(StatusCode statusCode, string reasonPhrase, dynamic data)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
            Data = data;
        }

        public TransResult(StatusCode statusCode, string reasonPhrase) : this(statusCode, reasonPhrase, null) { }

        public TransResult(StatusCode statusCode) : this(statusCode, null, null) { }

        public TransResult() { }

        public StatusCode StatusCode { get; set; }

        public string ReasonPhrase { get; set; }

        public dynamic Data { get; set; }
    }
}
