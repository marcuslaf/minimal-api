using FluentValidation;
using minimal_api.DTOs;

namespace minimal_api.Validators;

public class VeiculoDTOValidator : AbstractValidator<VeiculoDTO>
{
    public VeiculoDTOValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome não pode ser vazio")
            .MaximumLength(150).WithMessage("Nome deve ter no máximo 150 caracteres");

        RuleFor(x => x.Marca)
            .NotEmpty().WithMessage("A marca não pode ficar em branco")
            .MaximumLength(100).WithMessage("Marca deve ter no máximo 100 caracteres");

        RuleFor(x => x.Ano)
            .InclusiveBetween(1950, DateTime.Now.Year + 1)
            .WithMessage("Veículo muito antigo, aceito somente anos superiores a 1950");
    }
}
