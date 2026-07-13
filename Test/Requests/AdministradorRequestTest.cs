using System.Net;
using System.Text;
using System.Text.Json;
using minimal_api.Dominio.ModelViews;
using minimal_api.DTOs;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class AdministradorRequestTest
{
    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }

    [TestMethod]
    public async Task TestarLoginComSucesso()
    {
        // Arrange
        var loginDTO = new LoginDTO
        {
            Email = "adm@teste.com",
            Senha = "123456"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(loginDTO),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await Setup.client.PostAsync("/administradores/login", content);

        // Assert
        var body = await response.Content.ReadAsStringAsync();
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, $"Login falhou: {body}");

        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(body, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.IsNotNull(admLogado);
        Assert.IsFalse(string.IsNullOrEmpty(admLogado.Email));
        Assert.IsFalse(string.IsNullOrEmpty(admLogado.Token));
    }

    [TestMethod]
    public async Task TestarLoginComCredenciaisInvalidas()
    {
        // Arrange
        var loginDTO = new LoginDTO
        {
            Email = "adm@teste.com",
            Senha = "senhaerrada"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(loginDTO),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await Setup.client.PostAsync("/administradores/login", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task TestarHomeEndpoint()
    {
        // Act
        var response = await Setup.client.GetAsync("/");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(body.Contains("Bem vindo"));
    }
}
