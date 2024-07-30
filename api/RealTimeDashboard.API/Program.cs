using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using RealTimeDashboard.API;
using System.Net;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:6969");
builder.Services.AddMemoryCache();
builder.Services.AddHostedService<CacheUpdater>();
var app = builder.Build();

app.UseWebSockets();

app.Map("/ws/active-users", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        var cache = context.RequestServices.GetRequiredService<IMemoryCache>();
        while (true)
        {
            if(ws.State == WebSocketState.Open)
            {
                var activeUsers = cache.Get<int>("active-users");
                string message = $"Current amount of active users is {activeUsers}";
                var bytes = Encoding.UTF8.GetBytes(message);
                var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                await ws.SendAsync(arraySegment,
                                    WebSocketMessageType.Text,
                                    true,
                                    CancellationToken.None);
            }
            else if(ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
            {
                break;
            }
            await Task.Delay(5000);
        }
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});


app.Run();
