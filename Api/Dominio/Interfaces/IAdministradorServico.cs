using minimal_api.Dominio.Entidades;
using minimal_api.DTOs;

namespace minimal_api.Dominio.Interfaces;

public interface IAdministradorServico
{
    Task<Administrador?> LoginAsync(LoginDTO loginDTO);
    Task<Administrador> IncluirAsync(Administrador administrador);
    Task<Administrador?> BuscaPorIdAsync(int id);
    Task<List<Administrador>> TodosAsync(int? pagina);
}
