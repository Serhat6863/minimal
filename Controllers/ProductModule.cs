// Modules/ProductModule.cs
using Carter;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using WebApplication3.Models;

namespace MyMinimalApi.Modules
{
    public class ProductModule : CarterModule
    {
        public static readonly List<Product> Products = new List<Product>();

        public override void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/api/products", async (HttpContext context) =>
            {
                await context.Response.WriteAsJsonAsync(Products);
            }).WithName("GetAllProducts").WithOpenApi();

            app.MapGet("/api/products/{id}", async (HttpContext context, int id) =>
            {
                var product = Products.Find(p => p.ProductId == id);
                if (product == null)
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsJsonAsync(new { Message = "Product not found" });
                    return;
                }
                await context.Response.WriteAsJsonAsync(product);
            }).WithName("GetProductById").WithOpenApi();

            app.MapPost("/api/products", async (HttpContext context) =>
            {
                var product = await context.Request.ReadFromJsonAsync<Product>();
                Products.Add(product);
                await context.Response.WriteAsJsonAsync(product);
            }).WithName("CreateProduct").WithOpenApi();

            app.MapDelete("/api/products/{id}", async (HttpContext context, int id) =>
            {
                var product = Products.Find(p => p.ProductId == id);
                if (product == null)
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsJsonAsync(new { Message = "Product not found" });
                    return;
                }
                Products.Remove(product);
                await context.Response.WriteAsJsonAsync(product);
            }).WithName("DeleteProduct").WithOpenApi();

            app.MapPut("/api/products/{id}", async (HttpContext context, int id) =>
            {
                var updatedProduct = await context.Request.ReadFromJsonAsync<Product>();
                var index = Products.FindIndex(p => p.ProductId == id);
                if (index == -1)
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsJsonAsync(new { Message = "Product not found" });
                    return;
                }
                Products[index] = updatedProduct;
                await context.Response.WriteAsJsonAsync(updatedProduct);
            }).WithName("UpdateProduct").WithOpenApi();
        }
    }
}
