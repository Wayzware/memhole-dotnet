using Xunit;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Wayz.Memhole.WebApi;
using System.Net;

namespace MemholeApi.Tests
{
    public class MemholeDeviceControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public MemholeDeviceControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Connect_ShouldCallConnectMethod()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Post, "/memhole/connect");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        // Add other tests for other methods of MemholeDeviceController
    }
}
