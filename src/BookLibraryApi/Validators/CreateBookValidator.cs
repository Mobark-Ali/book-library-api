using BookLibraryApi.DTOs;
using FluentValidation;

namespace BookLibraryApi.Validators;

public class CreateBookValidator : AbstractValidator<CreateBookDto>
{
    public CreateBookValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ISBN).MaximumLength(20);
        RuleFor(x => x.PublishedYear).InclusiveBetween(1000, DateTime.UtcNow.Year);
        RuleFor(x => x.AuthorId).GreaterThan(0);
    }
}
