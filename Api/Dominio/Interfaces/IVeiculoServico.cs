using minimal_api.Dominio.Entidades;
using minimal_api.DTOs;

namespace minimal_api.Dominio.Interfaces;

public interface IVeiculoServico
{
    Task<List<Veiculo>> TodosAsync(int? pagina = 1, string? nome = null, string? marca = null);
    Task<Veiculo?> BuscaPorIdAsync(int id);
    Task<Veiculo> IncluirAsync(Veiculo veiculo);
    Task<Veiculo> AtualizarAsync(Veiculo veiculo);
    Task ApagarAsync(Veiculo veiculo);
}
