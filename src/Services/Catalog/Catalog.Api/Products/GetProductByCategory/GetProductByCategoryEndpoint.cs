using Microsoft.AspNetCore.Http.HttpResults;

namespace Catalog.Api.Products.GetProductByCategory
{
    //public record GetProductByCategoryRequest();
    public record GetProductByCategoryResponse(IEnumerable<Product>? Products);
    public class GetProductByCategoryEndpoint:ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/category/{category}", async (string Category,ISender sender) =>
            {
                var response = await sender.Send(new GetProductByCategoryQuery(Category));
                var result=response.Adapt<GetProductByCategoryResponse>();
                return Results.Ok( result);
            }).WithName("GetProductByCategory")
            .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Product By Category")
            .WithDescription("Get Product By Category");
        }
    }
}
