using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Servicos;
using minimal_api.Infraestrutura.Db;

namespace Test.Domain.Entidades;

[TestClass]
public class AdministradorServicoTest
{
    private DbContexto CriarContextoDeTeste()
    {
        var options = new DbContextOptionsBuilder<DbContexto>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new DbContexto(options);
    }

    [TestMethod]
    public async Task TestandoSalvarAdministrador()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        var adm = new Administrador
        {
            Email = "teste@teste.com",
            Senha = "teste123",
            Perfil = "Adm"
        };

        var administradorServico = new AdministradorServico(context);

        // Act
        await administradorServico.IncluirAsync(adm);

        // Assert
        var resultado = await administradorServico.TodosAsync(1);
        Assert.AreEqual(1, resultado.Count);
    }

    [TestMethod]
    public async Task TestandoBuscaPorId()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        var adm = new Administrador
        {
            Email = "teste@teste.com",
            Senha = "teste123",
            Perfil = "Adm"
        };

        var administradorServico = new AdministradorServico(context);

        // Act
        await administradorServico.IncluirAsync(adm);
        var admDoBanco = await administradorServico.BuscaPorIdAsync(adm.Id);

        // Assert
        Assert.IsNotNull(admDoBanco);
        Assert.AreEqual("teste@teste.com", admDoBanco.Email);
    }

    [TestMethod]
    public async Task TestandoLoginComSenhaCorreta()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        var adm = new Administrador
        {
            Email = "login@teste.com",
            Senha = "senha123",
            Perfil = "Adm"
        };

        var administradorServico = new AdministradorServico(context);
        await administradorServico.IncluirAsync(adm);

        // Act
        var resultado = await administradorServico.LoginAsync(new minimal_api.DTOs.LoginDTO
        {
            Email = "login@teste.com",
            Senha = "senha123"
        });

        // Assert
        Assert.IsNotNull(resultado);
        Assert.AreEqual("login@teste.com", resultado.Email);
    }

    [TestMethod]
    public async Task TestandoLoginComSenhaIncorreta()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        var adm = new Administrador
        {
            Email = "login@teste.com",
            Senha = "senha123",
            Perfil = "Adm"
        };

        var administradorServico = new AdministradorServico(context);
        await administradorServico.IncluirAsync(adm);

        // Act
        var resultado = await administradorServico.LoginAsync(new minimal_api.DTOs.LoginDTO
        {
            Email = "login@teste.com",
            Senha = "senhaincorreta"
        });

        // Assert
        Assert.IsNull(resultado);
    }

    [TestMethod]
    public async Task TestandoBuscaPorIdInexistente()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        var administradorServico = new AdministradorServico(context);

        // Act
        var resultado = await administradorServico.BuscaPorIdAsync(999);

        // Assert
        Assert.IsNull(resultado);
    }
}
