using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.Entidades;
using minimal_api.DTOs;
using minimal_api.Infraestrutura.Db;
using minimal_api.Dominio.Interfaces;

namespace minimal_api.Dominio.Servicos;

public class AdministradorServico : IAdministradorServico
{
    private readonly DbContexto _contexto;
    public AdministradorServico(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public async Task<Administrador?> BuscaPorIdAsync(int id)
    {
        return await _contexto.Administradores.FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Administrador> IncluirAsync(Administrador administrador)
    {
        administrador.Senha = BCrypt.Net.BCrypt.HashPassword(administrador.Senha);
        _contexto.Administradores.Add(administrador);
        await _contexto.SaveChangesAsync();
        return administrador;
    }

    public async Task<Administrador?> LoginAsync(LoginDTO loginDTO)
    {
        var adm = await _contexto.Administradores.FirstOrDefaultAsync(a => a.Email == loginDTO.Email);
        if (adm == null) return null;

        if (!BCrypt.Net.BCrypt.Verify(loginDTO.Senha, adm.Senha))
            return null;

        return adm;
    }

    public async Task<List<Administrador>> TodosAsync(int? pagina)
    {
        var query = _contexto.Administradores.AsQueryable();

        int itensPorPagina = 10;

        if (pagina != null)
            query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);

        return await query.ToListAsync();
    }
}
