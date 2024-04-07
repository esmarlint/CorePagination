using CorePagination.Tests.Support.Seeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeDbContext = CorePagination.Tests.Support.DatabaseContexts.ApplicationDbContext;

namespace CorePagination.Tests.Support.Utils
{
    public static class DatabaseSupport
    {
        public static FakeDbContext SetupTestDatabase(int seedAmount)
        {
            var options = CreateInMemoryDatabaseOptions();
            var context = CreateContext(options);
            SeedTestData(context, seedAmount);
            return context;
        }

        private static DbContextOptions<FakeDbContext> CreateInMemoryDatabaseOptions()
        {
            string databaseName = $"TestDatabase_{Guid.NewGuid()}";
            return new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        private static FakeDbContext CreateContext(DbContextOptions<FakeDbContext> options)
        {
            return new FakeDbContext(options);
        }

        private static void SeedTestData(FakeDbContext context, int amount)
        {
            ProductSeeder.SeedProducts(context, amount);
            context.SaveChanges();
        }
    }

}
