namespace Catalog.Api.Products.DeleteProduct
{
    
    public record DeleteProductResponse(bool IsSuccess);
    public class DeleteProductEndpoint:ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/Products/{id}"
                , async (Guid Id,ISender sender) =>
            {
                
                var result = await sender.Send(new DeleteProductCommand(Id));
                var response = result.Adapt<DeleteProductResponse>();
                return Results.Ok(response);
            }).WithName("DeleteProducts")
            .Produces<DeleteProductResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithDescription("Delete Product")
            .WithSummary("Delete Product");
        }
    }
}
