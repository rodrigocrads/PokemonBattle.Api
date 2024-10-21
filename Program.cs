using System;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<SessionBattleService>();
var app = builder.Build();
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(10)
};
app.UseWebSockets();
app.MapGet("/event-battle", async (HttpContext context, SessionBattleService chatService) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        await chatService.HandleWebSocketConnection(webSocket);
    }
    else
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("Expected a WebSocket request");
    }
});

app.Run();

