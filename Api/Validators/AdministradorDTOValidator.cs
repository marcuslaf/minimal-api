using FluentValidation;
using minimal_api.DTOs;

namespace minimal_api.Validators;

public class AdministradorDTOValidator : AbstractValidator<AdministradorDTO>
{
    public AdministradorDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email não pode ser vazio")
            .EmailAddress().WithMessage("Email deve ser válido")
            .MaximumLength(255).WithMessage("Email deve ter no máximo 255 caracteres");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha não pode ser vazia")
            .MinimumLength(6).WithMessage("Senha deve ter no mínimo 6 caracteres")
            .MaximumLength(50).WithMessage("Senha deve ter no máximo 50 caracteres");

        RuleFor(x => x.Perfil)
            .NotNull().WithMessage("Perfil não pode ser vazio")
            .IsInEnum().WithMessage("Perfil deve ser Adm ou Editor");
    }
}
