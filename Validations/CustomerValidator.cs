using AspNetI18N.Entities;
using AspNetI18N.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace AspNetI18N.Validations;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator(IStringLocalizer<Messages> localizer)
    {
        RuleFor(x => x.Name)
            .NotEmpty();
        
        RuleFor(x => x.Email)
            .EmailAddress();
        
        RuleFor(x => x.Email)
            .Must(x => x.EndsWith("@gmail.com"))
            .WithMessage(x => localizer["EmailMustBeGmail", nameof(x.Email)].Value);
    }
}