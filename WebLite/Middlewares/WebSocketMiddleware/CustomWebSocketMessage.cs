using System;
using System.Collections.Generic;
using System.Text;

namespace WebLite.Middlewares.WebSocketMiddleware
{
    public class CustomWebSocketMessage
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Time { get; set; }
        public string Username { get; set; }
    }
}
