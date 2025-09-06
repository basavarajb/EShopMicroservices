using FluentValidation;

namespace Catalog.Api.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Category, string Description, string ImageFile, int Price) : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid Id);

    public class CreateProductCommandValidator:AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(100).WithMessage("Name Required");
            RuleFor(x => x.Category).NotEmpty().WithMessage("Category Required");
            RuleFor(x => x.Description).NotEmpty().MaximumLength(500).WithMessage("Description Required");
            RuleFor(x => x.ImageFile).NotEmpty().MaximumLength(100).WithMessage("ImageFile Required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price Required");
        }
    }

    public class CreateProductCommandHandler
        (IDocumentSession session) 
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            
            var product = new Product()
            {
                Name = command.Name,
                Category = command.Category,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);
            return new CreateProductResult(product.Id);
        }
    }
}