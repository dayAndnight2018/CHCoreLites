using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebLite.Middlewares.WebSocketMiddleware
{
    public class CustomWebSocketMessageHandler : ICustomWebSocketMessageHandler
    {
        public async Task BroadcastAll(byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory)
        {
            var all = wsFactory.All();
            foreach (var uws in all)
            {
                await uws.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task BroadcastOthers(byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory)
        {
            var others = wsFactory.Others(userWebSocket);
            foreach (var uws in others)
            {
                await uws.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        public async Task HandleMessage(WebSocketReceiveResult result, byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory)
        {
            string msg = Encoding.UTF8.GetString(buffer);
            try
            {
                var message = JsonConvert.DeserializeObject<CustomWebSocketMessage>(msg);
                await userWebSocket.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                //if (message.Type == WSMessageType.anyType)
                //{
                //    await BroadcastOthers(buffer, userWebSocket, wsFactory);
                //}
            }
            catch (Exception e)
            {
                await userWebSocket.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
            }
        }

        /// <summary>
        /// 发送初识内容
        /// </summary>
        /// <param name="userWebSocket"></param>
        /// <returns></returns>
        public async Task SendInitialMessages(CustomWebSocket userWebSocket)
        {
            WebSocket webSocket = userWebSocket.WebSocket;
            var msg = new CustomWebSocketMessage
            {
                Time = DateTime.Now,
                Content = "测试内容",
                 Title = "测试",
                 Username = "server"
            };

            string serialisedMessage = JsonConvert.SerializeObject(msg);
            byte[] bytes = Encoding.UTF8.GetBytes(serialisedMessage);
            await webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// 向用户发送消息
        /// </summary>
        /// <param name="userWebSocket"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SendMessageToUser(CustomWebSocket userWebSocket, Object data)
        {
            WebSocket webSocket = userWebSocket.WebSocket;          
            string serialisedMessage = JsonConvert.SerializeObject(data);
            byte[] bytes = Encoding.UTF8.GetBytes(serialisedMessage);
            await webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        /// <summary>
        /// 向指定用户发送消息
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="userid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task SendMessageToUser(ICustomWebSocketFactory factory, string userid, Object data)
        {
            var user = factory.Client(userid);
            if(user != null)
            {
                string serialisedMessage = JsonConvert.SerializeObject(data);
                byte[] bytes = Encoding.UTF8.GetBytes(serialisedMessage);
                await user.WebSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

    }
}
