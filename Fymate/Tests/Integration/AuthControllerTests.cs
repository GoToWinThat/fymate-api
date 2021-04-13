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

        [Fact]
        public async Task ChangePassword()
        {
            // Arrange
            var client = _factory.CreateClient();

            //Try to login with new email
            var authToken = await client.PostAsync("/api/auth/loginUser/", JsonContent.Create(new
            {
                email = "dog@email.com",
                password = "Keku#132ada"
            }));

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", await authToken.Content.ReadAsStringAsync());

            // Act
            var response = await client.PatchAsync("/api/auth/changePassword/", JsonContent.Create(new
            {
                Email = "dog@email.com",
                OldPassword = "Keku#132ada",
                Username = "dog",
                NewPassword = "Nenu#123ada"
            }));

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            //Try to login with new password
            response = await client.PatchAsync("/api/auth/loginUser/", JsonContent.Create(new
            {
                email = "dog@email.com",
                password = "Nenu#123ada"
            }));

            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Fact]
        public async Task ChangeEmail()
        {
            // Arrange
            var client = _factory.CreateClient();
            //Get auth token
            //Try to login with new email
            var authToken = await client.PostAsync("/api/auth/loginUser/", JsonContent.Create(new
            {
                email = "dog@email.com",
                password = "Nenu#123ada"
            }));

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Authorization", await authToken.Content.ReadAsStringAsync());
            // Act
            var response = await client.PatchAsync("/api/auth/changeEmail/", JsonContent.Create(new
            {
                email = "dog@email.com",
                newEmail = "dog@email.com",
                username = "dog"
            }));

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            //Try to login with new email
            response = await client.PostAsync("/api/auth/loginUser/", JsonContent.Create(new
            {
                email = "cat@email.com",
                password = "Nenu#123ada"
            }));

            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }


    }
}