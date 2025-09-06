using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data
{
    public static class Extensions
    {
        public static async  Task<IApplicationBuilder> UseMigration(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DiscountContext>();
            await db.Database.MigrateAsync();
            return app;
        }
    }
}
