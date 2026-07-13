using FluentValidation;
using minimal_api.DTOs;
using minimal_api.Dominio.Enuns;
using minimal_api.Validators;

namespace Test.Domain.Validadores;

[TestClass]
public class ValidatorTest
{
    [TestMethod]
    public async Task TestarAdministradorDTOValido()
    {
        // Arrange
        var validator = new AdministradorDTOValidator();
        var dto = new AdministradorDTO
        {
            Email = "teste@teste.com",
            Senha = "123456",
            Perfil = Perfil.Adm
        };

        // Act
        var resultado = await validator.ValidateAsync(dto);

        // Assert
        Assert.IsTrue(resultado.IsValid);
    }

    [TestMethod]
    public async Task TestarAdministradorDTOEmailVazio()
    {
        // Arrange
        var validator = new AdministradorDTOValidator();
        var dto = new AdministradorDTO
        {
            Email = "",
            Senha = "123456",
            Perfil = Perfil.Adm
        };

        // Act
        var resultado = await validator.ValidateAsync(dto);

        // Assert
        Assert.IsFalse(resultado.IsValid);
        Assert.IsTrue(resultado.Errors.Any(e => e.ErrorMessage.Contains("Email")));
    }

    [TestMethod]
    public async Task TestarAdministradorDTOSenhaCurta()
    {
        // Arrange
        var validator = new AdministradorDTOValidator();
        var dto = new AdministradorDTO
        {
            Email = "teste@teste.com",
            Senha = "123",
            Perfil = Perfil.Adm
        };

        // Act
        var resultado = await validator.ValidateAsync(dto);

        // Assert
        Assert.IsFalse(resultado.IsValid);
        Assert.IsTrue(resultado.Errors.Any(e => e.ErrorMessage.Contains("mínimo")));
    }

    [TestMethod]
    public async Task TestarVeiculoDTOValido()
    {
        // Arrange
        var validator = new VeiculoDTOValidator();
        var dto = new VeiculoDTO
        {
            Nome = "Civic",
            Marca = "Honda",
            Ano = 2022
        };

        // Act
        var resultado = await validator.ValidateAsync(dto);

        // Assert
        Assert.IsTrue(resultado.IsValid);
    }

    [TestMethod]
    public async Task TestarVeiculoDTONomeVazio()
    {
        // Arrange
        var validator = new VeiculoDTOValidator();
        var dto = new VeiculoDTO
        {
            Nome = "",
            Marca = "Honda",
            Ano = 2022
        };

        // Act
        var resultado = await validator.ValidateAsync(dto);

        // Assert
        Assert.IsFalse(resultado.IsValid);
        Assert.IsTrue(resultado.Errors.Any(e => e.ErrorMessage.Contains("nome")));
    }

    [TestMethod]
    public async Task TestarVeiculoDTOAnoInvalido()
    {
        // Arrange
        var validator = new VeiculoDTOValidator();
        var dto = new VeiculoDTO
        {
            Nome = "Civic",
            Marca = "Honda",
            Ano = 1940
        };

        // Act
        var resultado = await validator.ValidateAsync(dto);

        // Assert
        Assert.IsFalse(resultado.IsValid);
        Assert.IsTrue(resultado.Errors.Any(e => e.ErrorMessage.Contains("1950")));
    }

    [TestMethod]
    public async Task TestarLoginDTOValido()
    {
        // Arrange
        var validator = new LoginDTOValidator();
        var dto = new LoginDTO
        {
            Email = "teste@teste.com",
            Senha = "123456"
        };

        // Act
        var resultado = await validator.ValidateAsync(dto);

        // Assert
        Assert.IsTrue(resultado.IsValid);
    }

    [TestMethod]
    public async Task TestarLoginDTOEmailInvalido()
    {
        // Arrange
        var validator = new LoginDTOValidator();
        var dto = new LoginDTO
        {
            Email = "email-invalido",
            Senha = "123456"
        };

        // Act
        var resultado = await validator.ValidateAsync(dto);

        // Assert
        Assert.IsFalse(resultado.IsValid);
        Assert.IsTrue(resultado.Errors.Any(e => e.ErrorMessage.Contains("válido")));
    }
}
