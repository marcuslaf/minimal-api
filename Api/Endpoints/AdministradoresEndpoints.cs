using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using minimal_api.Dominio.Entidades;
using minimal_api.Dominio.Interfaces;
using minimal_api.Dominio.ModelViews;
using minimal_api.DTOs;

namespace minimal_api.Endpoints;

public static class AdministradoresEndpoints
{
    public static void MapAdministradoresEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/administradores")
            .WithTags("Administradores");

        group.MapPost("/login", async (
            [FromBody] LoginDTO loginDTO,
            IAdministradorServico administradorServico,
            IValidator<LoginDTO> validator,
            IConfiguration configuration) =>
        {
            var validation = await validator.ValidateAsync(loginDTO);
            if (!validation.IsValid)
            {
                return Results.BadRequest(new ErrosDeValidacao
                {
                    Mensagens = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }

            var adm = await administradorServico.LoginAsync(loginDTO);
            if (adm != null)
            {
                string token = GerarTokenJwt(adm, configuration);
                return Results.Ok(new AdministradorLogado
                {
                    Email = adm.Email,
                    Perfil = adm.Perfil,
                    Token = token
                });
            }
            else
                return Results.Unauthorized();
        })
        .AllowAnonymous()
        .WithName("Login")
        .WithDescription("Autentica um administrador e retorna um token JWT");

        group.MapGet("/", async (
            [FromQuery] int? pagina,
            IAdministradorServico administradorServico) =>
        {
            var administradores = await administradorServico.TodosAsync(pagina);
            var adms = administradores.Select(adm => new AdministradorModelView
            {
                Id = adm.Id,
                Email = adm.Email,
                Perfil = adm.Perfil
            }).ToList();
            return Results.Ok(adms);
        })
        .RequireAuthorization()
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
        .WithName("ListarAdministradores")
        .WithDescription("Lista todos os administradores paginados");

        group.MapGet("/{id}", async (
            [FromRoute] int id,
            IAdministradorServico administradorServico) =>
        {
            var administrador = await administradorServico.BuscaPorIdAsync(id);
            if (administrador == null) return Results.NotFound();
            return Results.Ok(new AdministradorModelView
            {
                Id = administrador.Id,
                Email = administrador.Email,
                Perfil = administrador.Perfil
            });
        })
        .RequireAuthorization()
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
        .WithName("BuscarAdministradorPorId")
        .WithDescription("Busca um administrador pelo ID");

        group.MapPost("/", async (
            [FromBody] AdministradorDTO administradorDTO,
            IAdministradorServico administradorServico,
            IValidator<AdministradorDTO> validator) =>
        {
            var validation = await validator.ValidateAsync(administradorDTO);
            if (!validation.IsValid)
            {
                return Results.BadRequest(new ErrosDeValidacao
                {
                    Mensagens = validation.Errors.Select(e => e.ErrorMessage).ToList()
                });
            }

            var administrador = new Administrador
            {
                Email = administradorDTO.Email,
                Senha = administradorDTO.Senha,
                Perfil = administradorDTO.Perfil.ToString() ?? "Editor"
            };

            await administradorServico.IncluirAsync(administrador);

            return Results.Created($"/administradores/{administrador.Id}", new AdministradorModelView
            {
                Id = administrador.Id,
                Email = administrador.Email,
                Perfil = administrador.Perfil
            });
        })
        .RequireAuthorization()
        .RequireAuthorization(new AuthorizeAttribute { Roles = "Adm" })
        .WithName("CriarAdministrador")
        .WithDescription("Cria um novo administrador");
    }

    private static string GerarTokenJwt(Administrador administrador, IConfiguration configuration)
    {
        var key = configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(key)) return string.Empty;

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var expiryDays = int.TryParse(configuration["Jwt:ExpiryDays"], out var days) ? days : 1;

        var claims = new List<Claim>()
        {
            new Claim("Email", administrador.Email),
            new Claim("Perfil", administrador.Perfil),
            new Claim(ClaimTypes.Role, administrador.Perfil),
        };

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddDays(expiryDays),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
