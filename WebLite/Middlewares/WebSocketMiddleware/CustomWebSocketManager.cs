using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebLite.Middlewares.WebSocketMiddleware
{
    public class CustomWebSocketManager
    {
        private readonly RequestDelegate next;

        public CustomWebSocketManager(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context, ICustomWebSocketFactory wsFactory, ICustomWebSocketMessageHandler wsmHandler)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await next(context);
                return;
            }

            if (context.Request.Path == "/ws")
            {
                //var auth = context.Request.Headers.Where(header => header.Key == "Authorization").FirstOrDefault();
                //if (auth.Key == null || string.IsNullOrWhiteSpace(auth.Value))
                //{
                //    context.Response.StatusCode = 401;
                //    return;
                //}

                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                CustomWebSocket userWebSocket = new CustomWebSocket()
                {
                    WebSocket = webSocket,
                    SocketId = new Random().Next(100000).ToString()
                };
                wsFactory.Add(userWebSocket);                
                await Listen(context, userWebSocket, wsFactory, wsmHandler);
            }
            else
            {
                context.Response.StatusCode = 400;
                return;
            }
        }

        private async Task Listen(HttpContext context, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory, ICustomWebSocketMessageHandler wsmHandler)
        {
            //await wsmHandler.SendInitialMessages(userWebSocket);

            WebSocket webSocket = userWebSocket.WebSocket;
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!result.CloseStatus.HasValue)
            {
                //await wsmHandler.HandleMessage(result, buffer, userWebSocket, wsFactory);
                //await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                await wsmHandler.SendInitialMessages(userWebSocket);
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }

            wsFactory.Remove(userWebSocket.SocketId);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
