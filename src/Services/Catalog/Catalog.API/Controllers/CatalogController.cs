using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController: ControllerBase
{
    private readonly ICatalogRepository _repository;

    public CatalogController(ICatalogRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult> GetProducts()
    {
        var products = await _repository.GetProducts();

        if (!products.Any())
        {
            return NotFound();
        }
        return Ok(products);
    }
}
