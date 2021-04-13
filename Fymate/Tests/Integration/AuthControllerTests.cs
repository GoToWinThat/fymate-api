using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using System.Net;


namespace Fymate.Testing
{
    /// <summary>
    /// Controller integration tests
    /// </summary>
    public class AuthControllerTests : IClassFixture<WebTestFixture>
    {

        private readonly WebTestFixture _factory;
        public AuthControllerTests(WebTestFixture factory)
        {
            _factory = factory;
        }


        [Fact]
        public async Task RegisterTest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/auth/registerUser", JsonContent.Create(new
            {
                userName = "dog",
                email = "dog@email.com",
                password = "Keku#132ada"
            }));

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Fact]
        public async Task LoginTest()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsync("/api/auth/loginUser/", JsonContent.Create(new
            {
                email = "dog@email.com",
                password = "Keku#132ada"
            }));

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }


    }
}