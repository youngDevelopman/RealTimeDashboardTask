using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using RealTimeDashboard.API.Services.Interfaces;
using System;
using System.Data.Common;
using System.Net.WebSockets;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;

namespace RealTimeDashboard.API.IntegrationTests
{
    public class WebSocketEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;
        public WebSocketEndpointsTests(WebApplicationFactory<Program> applicationFactory)
        {
            _applicationFactory = applicationFactory;
        }

        [Fact]
        public async Task GetActiveUsers_ShouldReturnOk()
        {
            using var hostBuilder = _applicationFactory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureTestServices(services =>
                    {
                        var userServiceDescriptor = services.SingleOrDefault(
                            d => d.ServiceType ==
                                typeof(IUserService));

                        var cacheUpdaterDescriptor = services.SingleOrDefault(
                            d => d.ImplementationType ==
                                typeof(CacheUpdater));

                        services.Remove(userServiceDescriptor);
                        services.Remove(cacheUpdaterDescriptor);

                        var mockService = new Mock<IUserService>();
                        mockService.SetupSequence(service => service.GetAmountOfActiveUsers())
                            .Returns(50)
                            .Returns(30)
                            .Returns(20);

                        // No need to remove IUserService, it would be overriden
                        services.AddScoped(provider => mockService.Object);
                    });
                });

            using var client = hostBuilder.CreateClient();

            var baseUri = client.BaseAddress;
            var webSocketUri = new Uri($"{baseUri.Scheme.Replace("http", "ws")}://{baseUri.Host}:{baseUri.Port}/ws/active-users");

            var webSocketClient = hostBuilder.Server.CreateWebSocketClient();
            var webSocket = await webSocketClient.ConnectAsync(webSocketUri, CancellationToken.None);

            var buffer = new byte[1024 * 4];

            var messages = new List<string>();

            var cts = new CancellationTokenSource(TimeSpan.FromSeconds(25));
            while (!cts.Token.IsCancellationRequested)
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                messages.Add(message);

                if (result.CloseStatus.HasValue)
                {
                    break;
                }
            }

            Assert.Equal(3, messages.Count);

            // Freezes for some reason
            //await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Test completed", CancellationToken.None);
        }
    }
}