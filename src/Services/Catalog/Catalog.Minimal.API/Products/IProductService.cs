namespace Catalog.Minimal.API.Products;

public interface IProductService
{
    void CreateProduct(Product? product);
    void UpdateProduct(Product? product);
    void DeleteProduct(string id);

    IEnumerable<Product> GetProducts();
    Product? ProductById(string id);
    IEnumerable<Product> GetProductsByName(string name);
    IEnumerable<Product> GetProductsByCategory(string categoryName);

}