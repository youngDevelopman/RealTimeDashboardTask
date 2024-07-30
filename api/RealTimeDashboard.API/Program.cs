using System.Net;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:6969");
var app = builder.Build();
app.UseWebSockets();

app.Map("/ws/active-users", async context =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        using var ws = await context.WebSockets.AcceptWebSocketAsync();
        var message = "Hello from Nazar!";
        var bytes = Encoding.UTF8.GetBytes(message);
        var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
        await ws.SendAsync(arraySegment,
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None);
    }
    else
    {
        context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
    }
});


app.Run();
