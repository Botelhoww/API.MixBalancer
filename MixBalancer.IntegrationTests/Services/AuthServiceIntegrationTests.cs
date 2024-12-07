//using Microsoft.AspNetCore.Mvc.Testing;
//using System.Net;
//using System.Text;

//namespace MixBalancer.IntegrationTests.Services
//{
//    public class AuthServiceIntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
//    {
//        private readonly HttpClient _client;

//        public AuthServiceIntegrationTests(WebApplicationFactory<startup> factory)
//        {
//            _client = factory.CreateClient();
//        }

//        [Fact]
//        public async Task RegisterAsync_ShouldReturnSuccess_WhenValidData()
//        {
//            // Arrange
//            var content = new StringContent("{\"username\":\"testuser\",\"email\":\"test@example.com\",\"password\":\"password\"}", Encoding.UTF8, "application/json");

//            // Act
//            var response = await _client.PostAsync("/api/auth/register", content);

//            // Assert
//            response.EnsureSuccessStatusCode();
//            var responseString = await response.Content.ReadAsStringAsync();
//            Assert.Contains("success", responseString);
//            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
//        }

//        // Additional integration tests for LoginAsync, etc.
//    }
//}