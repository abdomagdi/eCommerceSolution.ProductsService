using BusinessLogicLayer.DTO;
using BusinessLogicLayer.ServiceContracts;
using BusinessLogicLayer.Validators;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace BusinessLogicLayer.APIEndpints
{
    public static class ProductAPIEndpints
    {
        public static IEndpointRouteBuilder MapProductsAPIEndpoints(this IEndpointRouteBuilder app)
        {
            //app.MapGet("/api/products", async (IProductsSerives productsSerives) =>
            //{
            //    List<ProductResponse> productResponses = await productsSerives.GetAllProducts();
            //    return Results.Ok(productResponses);
            //});

            app.MapGet("/api/products", async (IProductsSerives productsSerives) =>
            {
                List<ProductResponse?> products = await productsSerives.GetProducts();
                return Results.Ok(products);
            });
            
            app.MapGet("/api/products/serach/product-id/{ProductID:guid}", async (IProductsSerives productsSerives, Guid  ProductID) =>
            {
                ProductResponse? product = await productsSerives.GetProductByCondition(p=>p.ProductID== ProductID);
                return Results.Ok(product);
            });

            app.MapGet("/api/products/serach/{searchString}", async (IProductsSerives productsSerives, string searchString) =>
            {
                List<ProductResponse?> productsByProductName = await productsSerives.GetProducsByCondition(p => p.ProductName!= null && p.ProductName.Contains(searchString,StringComparison.OrdinalIgnoreCase));
                List<ProductResponse?> productsByProductCategory = await productsSerives.GetProducsByCondition(p => p.Category!= null && p.Category.Contains(searchString,StringComparison.OrdinalIgnoreCase));
                var products = productsByProductName.Union(productsByProductCategory);
                return Results.Ok(products);
            });

            app.MapPost("/api/products", async (IProductsSerives productsSerives,IValidator<ProductAddRequest> productAddRequestValidator, ProductAddRequest productAddRequest) =>
            {
                ValidationResult validationResult = await productAddRequestValidator.ValidateAsync(productAddRequest);

                if (!validationResult.IsValid)
                {
                    Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(p => p.PropertyName).ToDictionary(
                        grp => grp.Key, grp => grp.Select(err => err.ErrorMessage).ToArray());
                        
                    
                }
                
                ProductResponse? AddedproductResponse = await productsSerives.AddProduct(productAddRequest);
                if (AddedproductResponse != null)
                return Results.Created($"/api/products/product-id/{AddedproductResponse?.ProductID}",AddedproductResponse);
                return Results.Problem("Error in Adding Product");
            });

            app.MapPut("/api/products", async (IProductsSerives productsSerives, IValidator<ProductUpdateRequest> productAddRequestValidator, ProductUpdateRequest productUpdateRequest) =>
            {
                ValidationResult validationResult = await productAddRequestValidator.ValidateAsync(productUpdateRequest);

                if (!validationResult.IsValid)
                {
                    Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(p => p.PropertyName).ToDictionary(
                        grp => grp.Key, grp => grp.Select(err => err.ErrorMessage).ToArray());


                }

                ProductResponse? updatedProductResponse = await productsSerives.UpdateProduct(productUpdateRequest);
                if (updatedProductResponse != null)
                    return Results.Ok(updatedProductResponse);
                return Results.Problem("Error in Updating Product");
            });

            app.MapDelete("/api/products/{ProductId:guid}", async (IProductsSerives productsSerives, Guid ProductId) =>
            {
                
                bool isDeleted = await productsSerives.DeleteProduct(ProductId);
                if (isDeleted )
                    return Results.Ok(true);
                return Results.Problem("Error in Deleting Product");
            });
            return app;
        }
    }
}
