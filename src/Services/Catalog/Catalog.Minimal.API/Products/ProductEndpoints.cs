﻿using Catalog.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;

namespace Catalog.Minimal.API.Products;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("api/v1/catalog", GetAllProducts);
        app.MapGet("api/v1/catalog/{id}", GetProductById).WithName("GetProduct");
        app.MapGet("api/v1/catalog/GetProductByCategory/{id}", GetProductsByCategory);
        app.MapGet("api/v1/catalog/GetProductByName/{id}", GetProductsByName);
        app.MapPost("api/v1/catalog", CreateProduct);
        app.MapPut("api/v1/catalog", UpdateProduct);
        app.MapDelete("api/v1/catalog", DeleteProduct);
    }

    public static async Task<IResult> GetAllProducts(IProductService service)
    {
        var products = await service.GetProducts();
        if (!products.Any())
        {
            Results.NotFound();
        }
        return Results.Ok(products);
    }

    public static async Task<IResult> GetProductById(IProductService service, string id)
    {
        var product = await service.GetProductById(id);
        if (product == null)
        {
            Results.NotFound();
        }
        return Results.Ok(product);
    }

    public static async Task<IResult> GetProductsByCategory(IProductService service, string category)
    {
        var products = await service.GetProductsByCategory(category);
        if (!products.Any())
        {
            Results.NotFound();
        }
        return Results.Ok(products);
    }
    public static async Task<IResult> GetProductsByName(IProductService service, string name)
    {
        var products = await service.GetProductsByName(name);
        if (!products.Any())
        {
            Results.NotFound();
        }
        return Results.Ok(products);
    }

    public static IResult CreateProduct(IProductService service, [FromBody]Product product)
    {
        service.CreateProduct(product);
        return Results.CreatedAtRoute("GetProduct", new { id = product.Id }, product);
    }

    public static IResult UpdateProduct(IProductService service, [FromBody] Product product)
    {
        service.UpdateProduct(product);
        return Results.AcceptedAtRoute("GetProduct", new { id = product.Id }, product);
    }

    public static IResult DeleteProduct(IProductService service, string id)
    {
        service.DeleteProduct(id);
        return Results.Ok();
    }

}