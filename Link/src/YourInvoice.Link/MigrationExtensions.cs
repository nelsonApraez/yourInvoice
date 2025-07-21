///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using yourInvoice.Link.Infrastructure.Persistence;

namespace yourInvoice.Link
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrationsLink(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<LinkDbContext>();

            dbContext.Database.Migrate();
        }
    }
}