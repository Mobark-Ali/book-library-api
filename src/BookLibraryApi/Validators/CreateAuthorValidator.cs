using BookLibraryApi.DTOs;
using FluentValidation;

namespace BookLibraryApi.Validators;

public class CreateAuthorValidator : AbstractValidator<CreateAuthorDto>
{
    public CreateAuthorValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Country).MaximumLength(100);
    }
}
