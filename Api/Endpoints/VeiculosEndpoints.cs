using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Dominio.ModelViews;
using minimal_api.DTOs;

namespace minimal_api.Endpoints;

public static class VeiculosEndpoints
{
    public static void MapVeiculosEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/veiculos")
            .WithTags("Veiculos");

        group.MapGet("/", async (
            [FromQuery] int? pagina,
            [FromQuery] string? nome,
            [FromQuery] string? marca,
            IVeiculoServico veiculoServico) =>
        {
            var veiculos = await veiculoServico.TodosAsync(pagina, nome, marca);
            return Results.Ok(veiculos);
        })
        .RequireAuthorization()
        .WithName("ListarVeiculos")
        .WithDescription("Lista veículos paginados com filtro opcional por nome e marca");

        group.MapGet("/{id}", async (
            [FromRoute] int id,
            IVeiculoServico veiculoServico) =>
        {
            var veiculo = await veiculoServico.BuscaPorIdAsync(id);
            if (veiculo == null) return Results.NotFound();
            return Results.Ok(veiculo);
        })
        .RequireAuthorization()
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
        .WithName("BuscarVeiculoPorId")
        .WithDescription("Busca um veículo pelo ID");

        group.MapPost("/", async (
            [FromBody] VeiculoDTO veiculoDTO,
            IVeiculoServico veiculoServico,
            IValidator<VeiculoDTO> validator) =>
        {
            var validation = await validator.ValidateAsync(veiculoDTO);
            if (!validation.IsValid)
            {
                return Results.BadRequest(new ErrosDeValidacao
                {
                    Mensagens = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }

            var veiculo = new Veiculo
            {
                Nome = veiculoDTO.Nome,
                Marca = veiculoDTO.Marca,
                Ano = veiculoDTO.Ano
            };

            await veiculoServico.IncluirAsync(veiculo);

            return Results.Created($"/veiculos/{veiculo.Id}", veiculo);
        })
        .RequireAuthorization()
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm,Editor" })
        .WithName("CriarVeiculo")
        .WithDescription("Cria um novo veículo");

        group.MapPut("/{id}", async (
            [FromRoute] int id,
            [FromBody] VeiculoDTO veiculoDTO,
            IVeiculoServico veiculoServico,
            IValidator<VeiculoDTO> validator) =>
        {
            var veiculo = await veiculoServico.BuscaPorIdAsync(id);
            if (veiculo == null) return Results.NotFound();

            var validation = await validator.ValidateAsync(veiculoDTO);
            if (!validation.IsValid)
            {
                return Results.BadRequest(new ErrosDeValidacao
                {
                    Mensagens = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }

            veiculo.Nome = veiculoDTO.Nome;
            veiculo.Marca = veiculoDTO.Marca;
            veiculo.Ano = veiculoDTO.Ano;

            await veiculoServico.AtualizarAsync(veiculo);

            return Results.Ok(veiculo);
        })
        .RequireAuthorization()
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
        .WithName("AtualizarVeiculo")
        .WithDescription("Atualiza um veículo existente");

        group.MapDelete("/{id}", async (
            [FromRoute] int id,
            IVeiculoServico veiculoServico) =>
        {
            var veiculo = await veiculoServico.BuscaPorIdAsync(id);
            if (veiculo == null) return Results.NotFound();

            await veiculoServico.ApagarAsync(veiculo);

            return Results.NoContent();
        })
        .RequireAuthorization()
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
        .WithName("DeletarVeiculo")
        .WithDescription("Deleta um veículo");
    }
}
