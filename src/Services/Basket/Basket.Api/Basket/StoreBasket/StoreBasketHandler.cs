
using Discount.Grpc.Protos;
using JasperFx.Events.Daemon;

namespace Basket.Api.Basket.StoreBasket
{
    public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
    public record StoreBasketResult(string UserName);

    public class StoreBasketValidator: AbstractValidator<StoreBasketCommand>
    {
        public StoreBasketValidator()
        {
            RuleFor(x => x.Cart).NotNull().WithMessage("Cart is required");
            RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserId is required");
            //RuleFor(x => x.Cart.Items).NotEmpty().WithMessage("Cart must have at least one item");
            //RuleForEach(x => x.Cart.Items).ChildRules(items =>
            //{
            //    items.RuleFor(i => i.ProductId).NotEmpty().WithMessage("ProductId is required");
            //    items.RuleFor(i => i.ProductName).NotEmpty().WithMessage("ProductName is required");
            //    items.RuleFor(i => i.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero");
            //    items.RuleFor(i => i.Price).GreaterThan(0).WithMessage("Price must be greater than zero");
            //});
        }
    }
    public class StoreBasketCommandHandler
        (IBasketRepository repository,DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient)
        : ICommandHandler<StoreBasketCommand, StoreBasketResult>
    {
        public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
        {
            
            await DeductDiscount(command.Cart,cancellationToken);
            var result=await repository.StoreBasket(command.Cart,cancellationToken);
            return new StoreBasketResult(result.UserName);
        }

        private async Task DeductDiscount(ShoppingCart cart,CancellationToken cancellation)
        {
            foreach (var item in cart.Items)
            {
                var coupon = await discountProtoServiceClient.GetDiscountAsync(new GetDiscountRequest { ProductName = item.ProductName });
                item.Price -= coupon.Amount;
            }
        }
    }
}
