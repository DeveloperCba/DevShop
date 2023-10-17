using Core.Test.Helpers;
using DevShop.Identity.Application.Features.User.Dtos;
using DevShop.Identity.Application.Models.Requesties;
using DevShop.Identity.Integration.Test.Configurations;
using MongoDB.Bson.IO;
using System.Net.Http.Json;
using Newtonsoft.Json;

namespace DevShop.Identity.Integration.Test.Auth
{

    [TestCaseOrderer("Core.Test.Helpers.XUnitPriorityOrderer", "Core.Test.Helpers")]
    [Collection(nameof(IntegrationWebApiTestFixtureCollection))]
    public  class AuthTest
    {
        private readonly IntegrationTestsFixture<Program> _testsFixture;

        public AuthTest(IntegrationTestsFixture<Program> testsFixture)
        {
            _testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Valid login"),TestPriority(1)]
        [Trait("Auth","Auth Login")]
        public async Task Auth_Login_ShouldReturnSuccessFully()
        {
            //Arrange
            var userData = new LoginRequest
            {
                Login = "administrador@oficinadev.kinghost.net",
                Password = "Teste@123"
            };

            _testsFixture.Client = _testsFixture.Factory.CreateClient();

            //Act
            var response = await _testsFixture.Client.PostAsJsonAsync("api/v1/auth", userData);

            //Assert
            response.EnsureSuccessStatusCode();
            var userReponse = Newtonsoft.Json.JsonConvert.DeserializeObject<UserResponseLoginDto>(await response.Content.ReadAsStringAsync());
            Assert.NotEmpty(userReponse.AccessToken);
        }
    }
}
