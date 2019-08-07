using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace WebLite.Middlewares.WebSocketMiddleware
{
    public class CustomWebSocketFactory : ICustomWebSocketFactory
    {
        ConcurrentDictionary<string, CustomWebSocket> sockets = new ConcurrentDictionary<string, CustomWebSocket>();

        /// <summary>
        /// 添加socket
        /// </summary>
        /// <param name="uws"></param>
        public void Add(CustomWebSocket uws)
        {
            sockets.TryAdd(uws.SocketId, uws);
        }

        /// <summary>
        /// 获取所有socket
        /// </summary>
        /// <returns></returns>
        public List<CustomWebSocket> All()
        {
            return sockets.Values as List<CustomWebSocket>;
        }

        /// <summary>
        /// 根据id获取socket
        /// </summary>
        /// <param name="socketId"></param>
        /// <returns></returns>
        public CustomWebSocket Client(string socketId)
        {
            CustomWebSocket cws;
            sockets.TryGetValue(socketId, out cws);
            return cws;
        }

        /// <summary>
        /// 获取除指定socket外的socket
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        public List<CustomWebSocket> Others(CustomWebSocket client)
        {
            var list = (sockets.Values as List<CustomWebSocket>);
            list.Remove(client);
            return list;
        }

        /// <summary>
        /// 删除指定socket
        /// </summary>
        /// <param name="socketId"></param>
        public void Remove(string socketId)
        {
            CustomWebSocket cws;
            sockets.TryRemove(socketId,out cws);
        }
    }
}
