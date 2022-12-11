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
    private static List<Customer> customers = new();

    [HttpGet]
    public List<Customer> listCustomers()
    {
        return customers;
    }
    [HttpGet("teste")]
    public string teste([FromServices] IStringLocalizer<Messages> localizer, IStringLocalizerFactory localizerFactory)
    {
        //var localizer = localizerFactory.Create("Messages", "pt-BR");
        return localizer.GetString("").Value;
    }

    [HttpPost]
    public IActionResult addCustomer([FromBody] Customer customer, [FromQuery] string culture, [FromServices] CustomerValidator validator)
    {
        var result = validator.Validate(customer);

        if (!result.IsValid)
        {
            return BadRequest(new {
                errors = result.Errors.Select(e => e.ErrorMessage)
            });
        }
        
        customers.Add(customer);
        return Ok(customer);
    }
}