using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RealTimeDashboard.API;
using RealTimeDashboard.API.Configuration;
using RealTimeDashboard.API.Services;
using RealTimeDashboard.API.Services.Interfaces;
using System.Net;
using System.Net.WebSockets;
using System.Runtime;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:6969");

builder.Services.Configure<UpdateFrequenciesConfiguration>(builder.Configuration.GetSection("UpdateFrequencies"));

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
        var updateFrequencySettings = context.RequestServices.GetRequiredService<IOptions<UpdateFrequenciesConfiguration>>().Value;
        var cache = context.RequestServices.GetRequiredService<IMemoryCache>();
        var userService = context.RequestServices.GetRequiredService<IUserService>();
        await StartSending(
            ws,
            () =>
            {
                var activeUsers = userService.GetAmountOfActiveUsers();
                var utcUpdatedTime = DateTime.UtcNow;
                var response = new { activeUsers, utcUpdatedTime };
                string jsonString = JsonSerializer.Serialize(response);
                return jsonString;
            },
            updateFrequencySettings.ActiveUsers);
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
        var updateFrequencySettings = context.RequestServices.GetRequiredService<IOptions<UpdateFrequenciesConfiguration>>().Value;
        var salesService = context.RequestServices.GetRequiredService<ISalesService>();
        await StartSending(
            ws, 
            () => 
            {
                var totalSales = salesService.GetTotalSales();
                var utcUpdatedTime = DateTime.UtcNow;
                var response = new { totalSales, utcUpdatedTime };
                string jsonString = JsonSerializer.Serialize(response);
                return jsonString;
            },
            updateFrequencySettings.TotalSales);
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
        var updateFrequencySettings = context.RequestServices.GetRequiredService<IOptions<UpdateFrequenciesConfiguration>>().Value;
        var salesService = context.RequestServices.GetRequiredService<ISalesService>();
        await StartSending(
            ws,
            () =>
            {
                var topSellingProducts = salesService.GetTopSellingProducts();
                string jsonString = JsonSerializer.Serialize(topSellingProducts, new JsonSerializerOptions { WriteIndented = true });
                return jsonString;
            },
            updateFrequencySettings.TopSellingProducts);
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});

// Runs handler delegate in specified updatePeriod. On each iteration checks whether web socket is opened
async Task StartSending(WebSocket ws, Func<string> handler, int updatePeriod)
{
    while (true)
    {
        if (ws.State == WebSocketState.Open)
        {
            string message = handler();
            var bytes = Encoding.UTF8.GetBytes(message);
            var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
            await ws.SendAsync(arraySegment,
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None);
        }
        else if (ws.State == WebSocketState.Closed || ws.State == WebSocketState.Aborted)
        {
            break;
        }
        await Task.Delay(updatePeriod);
    }
}

app.Run();
