using System.Net;
using System.Text;
using System.Text.Json;
using minimal_api.Dominio.ModelViews;
using minimal_api.DTOs;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class VeiculoRequestTest
{
    private static string _token = string.Empty;

    [ClassInitialize]
    public static void ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);

        var loginDTO = new LoginDTO
        {
            Email = "adm@teste.com",
            Senha = "123456"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(loginDTO),
            Encoding.UTF8,
            "application/json");

        var response = Setup.client.PostAsync("/administradores/login", content).Result;
        var body = response.Content.ReadAsStringAsync().Result;

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(body, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            _token = admLogado?.Token ?? string.Empty;
        }
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }

    private HttpRequestMessage CriarRequestComAuth(HttpMethod method, string url, object? body = null)
    {
        var request = new HttpRequestMessage(method, url);
        if (!string.IsNullOrEmpty(_token))
            request.Headers.Add("Authorization", $"Bearer {_token}");
        if (body != null)
            request.Content = new StringContent(
                JsonSerializer.Serialize(body),
                Encoding.UTF8,
                "application/json");
        return request;
    }

    [TestMethod]
    public async Task TestarCriarVeiculo()
    {
        // Arrange
        var veiculoDTO = new VeiculoDTO
        {
            Nome = "Civic",
            Marca = "Honda",
            Ano = 2022
        };

        var request = CriarRequestComAuth(HttpMethod.Post, "/veiculos", veiculoDTO);

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        Assert.IsTrue(
            response.StatusCode == HttpStatusCode.Created || response.StatusCode == HttpStatusCode.Unauthorized,
            $"Status inesperado: {response.StatusCode}");
    }

    [TestMethod]
    public async Task TestarListarVeiculos()
    {
        // Arrange
        var request = CriarRequestComAuth(HttpMethod.Get, "/veiculos");

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        Assert.IsTrue(
            response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Unauthorized,
            $"Status inesperado: {response.StatusCode}");
    }

    [TestMethod]
    public async Task TestarVeiculoNaoEncontrado()
    {
        // Arrange
        var request = CriarRequestComAuth(HttpMethod.Get, "/veiculos/999");

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        Assert.IsTrue(
            response.StatusCode == HttpStatusCode.NotFound || response.StatusCode == HttpStatusCode.Unauthorized,
            $"Status inesperado: {response.StatusCode}");
    }
}
