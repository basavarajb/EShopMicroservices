using Marten.Schema;

namespace Catalog.Api.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();
            if (await session.Query<Product>().AnyAsync())
                return;
            session.Store<Product>(GetPreConfiguredProducts());
            await session.SaveChangesAsync(cancellation);
        }

        private IEnumerable<Product> GetPreConfiguredProducts()=>      
             new List<Product>
                {
                    new Product
                    {
                        Name = "IPhone X",
                        Category = new List<string> { "Smart Phone" },
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc ultricie",
                        ImageFile = "product-1.png",
                        Price = 950
                    },
                    new Product
                    {
                        Name = "Samsung 10",
                        Category = new List<string> { "Smart Phone" },
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc ultricie",
                        ImageFile = "product-2.png",
                        Price = 840
                    },
                    new Product
                    {
                        Name = "Huawei Plus",
                        Category = new List<string> { "White Appliances" },
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc ultricie",
                        ImageFile = "product-3.png",
                        Price = 650
                    },
                    new Product
                    {
                        Name = "Xiaomi Mi 9",
                        Category = new List<string> { "White Appliances" },
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc ultricie",
                        ImageFile = "product-4.png",
                        Price = 470
                    },
                    new Product
                    {
                        Name = "HTC U11+ Plus",
                        Category = new List<string> { "Smart Phone" },
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc ultricie",
                        ImageFile = "product-5.png",
                        Price = 380
                    },
                    new Product
                    {
                        Name = "LG G7 ThinQ",
                        Category = new List<string> { "Home Kitchen" },
                        Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc ultricie",
                        ImageFile = "product-6.png",
                        Price = 240
                    }
                };
            
        
    }
}
