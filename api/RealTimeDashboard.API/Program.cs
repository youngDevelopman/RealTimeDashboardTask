using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using RealTimeDashboard.API;
using RealTimeDashboard.API.Services;
using RealTimeDashboard.API.Services.Interfaces;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:6969");
builder.Services.AddMemoryCache();
builder.Services.AddHostedService<CacheUpdater>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ISalesService, SalesService>();
var app = builder.Build();

app.UseWebSockets();

app.Map("/ws/active-users", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        var cache = context.RequestServices.GetRequiredService<IMemoryCache>();
        var userService = context.RequestServices.GetRequiredService<IUserService>();
        await StartSending(
            ws,
            async () =>
            {
                var activeUsers = userService.GetAmountOfActiveUsers();
                string message = $"Current amount of active users is {activeUsers}";
                var bytes = Encoding.UTF8.GetBytes(message);
                var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                await ws.SendAsync(arraySegment,
                                    WebSocketMessageType.Text,
                                    true,
                                    CancellationToken.None);
            },
            2000);
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

app.Map("/ws/total-sales", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        var salesService = context.RequestServices.GetRequiredService<ISalesService>();
        await StartSending(
            ws, 
            async () => 
            {
                var totalSales = salesService.GetTotalSales();
                string message = $"Current amount of total sales is {totalSales}";
                var bytes = Encoding.UTF8.GetBytes(message);
                var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                await ws.SendAsync(arraySegment,
                                    WebSocketMessageType.Text,
                                    true,
                                    CancellationToken.None);
            }, 
            2000);
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

app.Map("/ws/top-selling-products", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        var salesService = context.RequestServices.GetRequiredService<ISalesService>();
        await StartSending(
            ws,
            async () =>
            {
                var topSellingProducts = salesService.GetTopSellingProducts();
                string jsonString = JsonSerializer.Serialize(topSellingProducts, new JsonSerializerOptions { WriteIndented = true });
                var bytes = Encoding.UTF8.GetBytes(jsonString);
                var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                await ws.SendAsync(arraySegment,
                                    WebSocketMessageType.Text,
                                    true,
                                    CancellationToken.None);
            },
            2000);
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

// Runs handler delegate in specified updatePeriod. On each iteration checks whether web socket is opened
async Task StartSending(WebSocket ws, Action handler, int updatePeriod)
{
    while (true)
    {
        if (ws.State == WebSocketState.Open)
        {
            handler();
        }
        else if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
        {
            break;
        }
        await Task.Delay(updatePeriod);
    }
}

app.Run();
