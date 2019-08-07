using System;
using System.Collections.Generic;
using System.Text;

namespace WebLite.Middlewares.WebSocketMiddleware
{
    public interface ICustomWebSocketFactory
    {
        void Add(CustomWebSocket uws);
        void Remove(string socketId);
        List<CustomWebSocket> All();
        List<CustomWebSocket> Others(CustomWebSocket client);
        CustomWebSocket Client(string socketId);
    }
}
