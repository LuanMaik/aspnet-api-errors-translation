
# Translation for API error messages

- AspNet 6
- FluentValidation

---

## Propose
In this project is demonstrate how to configure translation for api error messages.

It's possible switch the Culture by setting:
- Query param: `?culture=pt-BR` 
- Request header: `AcceptLanguage`

---

## Configuration

### Enable localization in `Program.cs`:
```c#
builder.Services.AddLocalization();
builder.Services.AddScoped<CustomerValidator>();

// ...

// Configure localization
var supportedCultures = new[] {"en-US", "pt-BR"};
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);
```

---

### Create resources files to represents translation dictionaries, inside `Resources` folder:
```
.
├── ...
├── Resources
│   ├── Messages.resx         # Default translation dictionary
│   ├── Messages.pt-BR.resx   # Portuguese Brazil translation dictionary
│   └── ...
└── ...
```

Example file `Messages.{culture}.resx` content:
```xml
<?xml version="1.0" encoding="utf-8"?>
<root>
    <data name="Hello">
        <value>Hola</value>
    </data>
    <data name="EmailMustBeGmail">
        <value>'{0}' must be @gmail.com</value>
    </data>
</root>
```
> :warning: **Warning:** Make sure your `Messages.{culture}.resx` are using `public` visibility.



---

### In `Validations\CustomerValidator.cd`:
```c#
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
```

---

### In `Controllers/CustomerController.cs`:

```c#
using System.Net.Security;
using AspNetI18N.Entities;
using AspNetI18N.Resources;
using AspNetI18N.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace AspNetI18N.Controllers;

[ApiController]
[Route("/customers")]
public class CustomerController: ControllerBase
{
    [HttpPost]
    public IActionResult addCustomer([FromBody] Customer customer, [FromServices] CustomerValidator validator)
    {
        var result = validator.Validate(customer);

        if (!result.IsValid)
        {
            return BadRequest(new {
                errors = result.Errors.Select(e => e.ErrorMessage)
            });
        }
        
        return Ok();
    }
}
```


