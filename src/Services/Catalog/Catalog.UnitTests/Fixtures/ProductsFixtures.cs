using Bogus;
using Catalog.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.UnitTests.Fixtures;
public static class ProductsFixtures
{
    public static List<Product> GetProductsFixture()
    {
        return GetFakeProduct.Generate(5).ToList();
    }

    public static Faker<Product> GetFakeProduct { get; } =
        new Faker<Product>()
            .CustomInstantiator(f =>
                new Product(
                    id: f.Commerce.Ean8(),
                    name: f.Commerce.ProductName(),
                    category: f.Commerce.Department(),
                    summary: f.Commerce.ProductDescription(),
                    description: f.Commerce.ProductDescription(),
                    imageFile: f.Commerce.Ean8() + ".png",
                    price: f.Random.Number(100, 1000)));
    
}
