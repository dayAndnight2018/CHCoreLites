using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace WebLite.Middlewares.WebSocketMiddleware
{
    public class CustomWebSocket
    {
        public WebSocket WebSocket { get; set; }
        public string SocketId { get; set; }
        public string Username { get; set; }
    }
}
