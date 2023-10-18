using Core.Test.Configurations.Integrations;
using DevShop.Identity.Application.Features.User.Dtos;
using DevShop.Identity.Application.Models.Requesties;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace DevShop.Identity.Integration.Test.Configurations
{
    [CollectionDefinition(nameof(IntegrationWebApiTestFixtureCollection))]
    public class IntegrationWebApiTestFixtureCollection : ICollectionFixture<IntegrationTestsFixture<Program>>
    { }

    public class IntegrationTestsFixture<TProgram> : IDisposable where TProgram : class
    {
        public string UserEmail = string.Empty;
        public string UserPassword = string.Empty;

        public string UserToken = string.Empty;


        public readonly WebTestFactory<TProgram> Factory;
        public HttpClient Client;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {
                //AllowAutoRedirect = true,
                //BaseAddress = new Uri("http://localhost"),
                //HandleCookies = true,
                //MaxAutomaticRedirections = 7
            };

            Factory = new WebTestFactory<TProgram>();
            Client = Factory.CreateClient();
        }

        public async Task Authentication()
        {
            var userData = new LoginRequest
            {
                Login = "administrador@oficinadev.kinghost.net",
                Password = "Teste@123"
            };

            Client = Factory.CreateClient();
            var response = await Client.PostAsJsonAsync("api/v1/auth", userData);
            response.EnsureSuccessStatusCode();

            var userReponse = JsonConvert.DeserializeObject<UserResponseLoginDto>(await response.Content.ReadAsStringAsync());
            UserToken = userReponse.AccessToken;

        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}