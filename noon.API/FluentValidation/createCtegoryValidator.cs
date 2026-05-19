using FluentValidation;
using noon.Application.DTOs;
namespace noon.Application.FluentValidation;
public class createCtegoryValidator:AbstractValidator<createCategoryDto>
{
    public createCtegoryValidator()
    {
        RuleFor(x => x.name)
            .NotEmpty()
            .WithMessage("Category name cannot be empty");

        RuleFor(x => x.name)
            .Length(3, 100)
            .WithMessage("Category name must be between 3 and 100 characters");
    }
}