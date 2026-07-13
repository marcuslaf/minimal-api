using Microsoft.EntityFrameworkCore;
using minimal_api.Dominio.Entidades;
using minimal_api.DTOs;
using minimal_api.Infraestrutura.Db;
using minimal_api.Dominio.Interfaces;

namespace minimal_api.Dominio.Servicos;

public class VeiculoServico : IVeiculoServico
{
    private readonly DbContexto _contexto;
    public VeiculoServico(DbContexto contexto)
    {
        _contexto = contexto;
    }

    public async Task ApagarAsync(Veiculo veiculo)
    {
        _contexto.Veiculos.Remove(veiculo);
        await _contexto.SaveChangesAsync();
    }

    public async Task<Veiculo> AtualizarAsync(Veiculo veiculo)
    {
        _contexto.Veiculos.Update(veiculo);
        await _contexto.SaveChangesAsync();
        return veiculo;
    }

    public async Task<Veiculo?> BuscaPorIdAsync(int id)
    {
        return await _contexto.Veiculos.FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<Veiculo> IncluirAsync(Veiculo veiculo)
    {
        _contexto.Veiculos.Add(veiculo);
        await _contexto.SaveChangesAsync();
        return veiculo;
    }

    public async Task<List<Veiculo>> TodosAsync(int? pagina = 1, string? nome = null, string? marca = null)
    {
        var query = _contexto.Veiculos.AsQueryable();

        if (!string.IsNullOrEmpty(nome))
        {
            query = query.Where(v => v.Nome.ToLower().Contains(nome.ToLower()));
        }

        if (!string.IsNullOrEmpty(marca))
        {
            query = query.Where(v => v.Marca.ToLower().Contains(marca.ToLower()));
        }

        int itensPorPagina = 10;

        if (pagina != null)
            query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);

        return await query.ToListAsync();
    }
}
