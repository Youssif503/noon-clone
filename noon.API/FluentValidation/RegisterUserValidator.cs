using FluentValidation;
using noon.Application.DTOs;

namespace noon.API.FluentValidation;
public class RegisterUserValidator:AbstractValidator<RegisterDto>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email cannot be empty Yasta");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty Yasta");
        RuleFor(x=>x.ConfirmPassword)
            .NotEmpty()
            .WithMessage("Confirm password cannot be empty Yasta");
        
        RuleFor(x=>x.FirstName)
            .NotEmpty()
            .WithMessage("Feen esmak Ya3m");
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Feen esmak Ya3m");
        
        
        RuleFor(y => y.ConfirmPassword)
            .Matches(x=>x.Password)
            .WithMessage("Mosh Zay ba3d Yasta ");
        
    }
}