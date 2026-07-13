using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Servicos;
using minimal_api.Infraestrutura.Db;

namespace Test.Domain.Servicos;

[TestClass]
public class VeiculoServicoTest
{
    private DbContexto CriarContextoDeTeste()
    {
        var options = new DbContextOptionsBuilder<DbContexto>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new DbContexto(options);
    }

    [TestMethod]
    public async Task TestandoSalvarVeiculo()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        var veiculo = new Veiculo
        {
            Nome = "Civic",
            Marca = "Honda",
            Ano = 2022
        };

        var veiculoServico = new VeiculoServico(context);

        // Act
        await veiculoServico.IncluirAsync(veiculo);

        // Assert
        var resultado = await veiculoServico.TodosAsync(1);
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual("Civic", resultado[0].Nome);
    }

    [TestMethod]
    public async Task TestandoBuscaVeiculoPorId()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        var veiculo = new Veiculo
        {
            Nome = "Corolla",
            Marca = "Toyota",
            Ano = 2023
        };

        var veiculoServico = new VeiculoServico(context);

        // Act
        await veiculoServico.IncluirAsync(veiculo);
        var veiculoDoBanco = await veiculoServico.BuscaPorIdAsync(veiculo.Id);

        // Assert
        Assert.IsNotNull(veiculoDoBanco);
        Assert.AreEqual("Corolla", veiculoDoBanco.Nome);
        Assert.AreEqual("Toyota", veiculoDoBanco.Marca);
    }

    [TestMethod]
    public async Task TestandoAtualizarVeiculo()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        var veiculo = new Veiculo
        {
            Nome = "Onix",
            Marca = "Chevrolet",
            Ano = 2021
        };

        var veiculoServico = new VeiculoServico(context);
        await veiculoServico.IncluirAsync(veiculo);

        // Act
        veiculo.Nome = "Onix LTZ";
        veiculo.Ano = 2022;
        await veiculoServico.AtualizarAsync(veiculo);

        // Assert
        var veiculoAtualizado = await veiculoServico.BuscaPorIdAsync(veiculo.Id);
        Assert.IsNotNull(veiculoAtualizado);
        Assert.AreEqual("Onix LTZ", veiculoAtualizado.Nome);
        Assert.AreEqual(2022, veiculoAtualizado.Ano);
    }

    [TestMethod]
    public async Task TestandoApagarVeiculo()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        var veiculo = new Veiculo
        {
            Nome = "HB20",
            Marca = "Hyundai",
            Ano = 2020
        };

        var veiculoServico = new VeiculoServico(context);
        await veiculoServico.IncluirAsync(veiculo);

        // Act
        await veiculoServico.ApagarAsync(veiculo);

        // Assert
        var veiculoApagado = await veiculoServico.BuscaPorIdAsync(veiculo.Id);
        Assert.IsNull(veiculoApagado);
    }

    [TestMethod]
    public async Task TestandoFiltroPorNome()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        var veiculoServico = new VeiculoServico(context);

        await veiculoServico.IncluirAsync(new Veiculo { Nome = "Civic", Marca = "Honda", Ano = 2022 });
        await veiculoServico.IncluirAsync(new Veiculo { Nome = "Corolla", Marca = "Toyota", Ano = 2023 });
        await veiculoServico.IncluirAsync(new Veiculo { Nome = "Cruze", Marca = "Chevrolet", Ano = 2021 });

        // Act
        var resultado = await veiculoServico.TodosAsync(1, "civic");

        // Assert
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual("Civic", resultado[0].Nome);
    }

    [TestMethod]
    public async Task TestandoFiltroPorMarca()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        var veiculoServico = new VeiculoServico(context);

        await veiculoServico.IncluirAsync(new Veiculo { Nome = "Civic", Marca = "Honda", Ano = 2022 });
        await veiculoServico.IncluirAsync(new Veiculo { Nome = "Corolla", Marca = "Toyota", Ano = 2023 });
        await veiculoServico.IncluirAsync(new Veiculo { Nome = "CR-V", Marca = "Honda", Ano = 2021 });

        // Act
        var resultado = await veiculoServico.TodosAsync(1, null, "honda");

        // Assert
        Assert.AreEqual(2, resultado.Count);
        Assert.IsTrue(resultado.All(v => v.Marca.ToLower().Contains("honda")));
    }

    [TestMethod]
    public async Task TestandoBuscaVeiculoInexistente()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        var veiculoServico = new VeiculoServico(context);

        // Act
        var resultado = await veiculoServico.BuscaPorIdAsync(999);

        // Assert
        Assert.IsNull(resultado);
    }
}
