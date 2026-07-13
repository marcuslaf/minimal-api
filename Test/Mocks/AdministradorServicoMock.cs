using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.DTOs;

namespace Test.Mocks;

public class AdministradorServicoMock : IAdministradorServico
{
    private static readonly List<Administrador> administradores = new List<Administrador>()
    {
        new Administrador
        {
            Id = 1,
            Email = "adm@teste.com",
            Senha = BCrypt.Net.BCrypt.HashPassword("123456"),
            Perfil = "Adm"
        },
        new Administrador
        {
            Id = 2,
            Email = "editor@teste.com",
            Senha = BCrypt.Net.BCrypt.HashPassword("123456"),
            Perfil = "Editor"
        }
    };

    public Task<Administrador?> BuscaPorIdAsync(int id)
    {
        return Task.FromResult(administradores.Find(a => a.Id == id));
    }

    public Task<Administrador> IncluirAsync(Administrador administrador)
    {
        administrador.Id = administradores.Count + 1;
        administradores.Add(administrador);
        return Task.FromResult(administrador);
    }

    public Task<Administrador?> LoginAsync(LoginDTO loginDTO)
    {
        var adm = administradores.Find(a => a.Email == loginDTO.Email);
        if (adm != null && BCrypt.Net.BCrypt.Verify(loginDTO.Senha, adm.Senha))
            return Task.FromResult<Administrador?>(adm);

        return Task.FromResult<Administrador?>(null);
    }

    public Task<List<Administrador>> TodosAsync(int? pagina)
    {
        return Task.FromResult(administradores.ToList());
    }
}
