using Bogus;
using Catalog.Core.Entities;

namespace Catalog.UnitTests.Fixtures;

public static class ProductsFixtures
{
    public static Faker<Product> GetFakeProduct { get; } =
        new Faker<Product>()
            .CustomInstantiator(f =>
                new Product(
                    f.Commerce.Ean8(),
                    f.Commerce.ProductName(),
                    f.Commerce.Department(),
                    f.Commerce.ProductDescription(),
                    f.Commerce.ProductDescription(),
                    f.Commerce.Ean8() + ".png",
                    f.Random.Number(100, 1000)));

    public static List<Product> GetProductsFixture()
    {
        return GetFakeProduct.Generate(5).ToList();
    }
}