using FluentValidation;
using noon.Application.DTOs;

namespace noon.Application.FluentValidation;
public class createProductValidator : AbstractValidator<createProductDto>
{
    public createProductValidator()
    {
        RuleFor(p=>p.Price)
            .NotEmpty()
            .WithMessage("Price cannot be empty")
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");
        
        RuleFor(p=>p.StockCount)
            .GreaterThan(0)
            .WithMessage("StockCount cannot be greater than 0");

        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Name cannot be empty");

        RuleFor(p => p.CategoryId)
            .NotEmpty()
            .WithMessage("CategoryId cannot be empty");
    }
}