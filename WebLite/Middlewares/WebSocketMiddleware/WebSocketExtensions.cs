using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebLite.Middlewares.WebSocketMiddleware
{
    public static class WebSocketExtensions
    {
        public static IApplicationBuilder UserCustomWebSocketManager(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomWebSocketManager>();
        }
    }
}
