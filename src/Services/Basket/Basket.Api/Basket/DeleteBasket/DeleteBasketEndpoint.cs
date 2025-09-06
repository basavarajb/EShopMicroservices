
namespace Basket.Api.Basket.DeleteBasket
{
    public record DeleteBasketRequest(string UserName);
    public record DeleteBasketResponse(bool Success);
    public class DeleteBasketEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/basket/{UserName}", async (string UserName, ISender sender) =>
            {
                var command = new DeleteBasketCommand(UserName);
                var result = await sender.Send(command);
                var response = result.Adapt<DeleteBasketResponse>();
                return Results.Ok(response);
            }).WithName("DeleteBasket")
            .Produces<DeleteBasketResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Basket by username")
            .WithDescription("Delete Basket by username");
        }
    }
}
