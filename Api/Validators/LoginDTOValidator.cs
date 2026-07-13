using FluentValidation;
using minimal_api.DTOs;

namespace minimal_api.Validators;

public class LoginDTOValidator : AbstractValidator<LoginDTO>
{
    public LoginDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email não pode ser vazio")
            .EmailAddress().WithMessage("Email deve ser válido");

        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("Senha não pode ser vazia");
    }
}
