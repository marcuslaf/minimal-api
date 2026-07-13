using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.DTOs;

namespace Test.Mocks;

public class VeiculoServicoMock : IVeiculoServico
{
    private static readonly List<Veiculo> veiculos = new List<Veiculo>();
    private static int _nextId = 1;

    public Task<Veiculo?> BuscaPorIdAsync(int id)
    {
        return Task.FromResult(veiculos.Find(v => v.Id == id));
    }

    public Task<Veiculo> IncluirAsync(Veiculo veiculo)
    {
        veiculo.Id = _nextId++;
        veiculos.Add(veiculo);
        return Task.FromResult(veiculo);
    }

    public Task<Veiculo> AtualizarAsync(Veiculo veiculo)
    {
        var existing = veiculos.Find(v => v.Id == veiculo.Id);
        if (existing != null)
        {
            existing.Nome = veiculo.Nome;
            existing.Marca = veiculo.Marca;
            existing.Ano = veiculo.Ano;
        }
        return Task.FromResult(veiculo);
    }

    public Task ApagarAsync(Veiculo veiculo)
    {
        veiculos.Remove(veiculo);
        return Task.CompletedTask;
    }

    public Task<List<Veiculo>> TodosAsync(int? pagina = 1, string? nome = null, string? marca = null)
    {
        var query = veiculos.AsQueryable();

        if (!string.IsNullOrEmpty(nome))
            query = query.Where(v => v.Nome.ToLower().Contains(nome.ToLower()));

        if (!string.IsNullOrEmpty(marca))
            query = query.Where(v => v.Marca.ToLower().Contains(marca.ToLower()));

        int itensPorPagina = 10;
        if (pagina != null)
            query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);

        return Task.FromResult(query.ToList());
    }
}
