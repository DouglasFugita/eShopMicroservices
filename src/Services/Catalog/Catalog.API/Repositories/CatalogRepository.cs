using Catalog.API.Entities;

namespace Catalog.API.Repositories;

public class CatalogRepository: ICatalogRepository
{
	public CatalogRepository()
	{

	}

    public Task<IEnumerable<Product>> GetProducts()
    {
        throw new NotImplementedException();
    }
}
