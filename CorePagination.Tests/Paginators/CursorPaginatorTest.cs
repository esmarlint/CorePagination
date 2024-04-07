using CorePagination.Paginators.CursorPaginator;
using CorePagination.Tests.Models;
using CorePagination.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeDbContext = CorePagination.Tests.DatabaseContexts.ApplicationDbContext;

namespace CorePagination.Tests.Paginators
{
    public class CursorPaginatorTests
    {
        private DbContextOptions<FakeDbContext> CreateInMemoryDatabaseOptions()
        {
            string databaseName = $"TestDatabase_{Guid.NewGuid()}";
            return new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        private void SeedTestData(FakeDbContext context, int amount)
        {
            ProductSeeder.SeedProducts(context, amount);
            context.SaveChanges();
        }

        [Fact]
        public void Constructor_ShouldInitializeProperly()
        {
            var options = CreateInMemoryDatabaseOptions();
            using var context = new FakeDbContext(options);
            var paginator = new CursorPaginator<ProductTests, int>(x => x.Id);

            Assert.NotNull(paginator); // Verify the paginator is created.
        }

        [Fact]
        public void Paginate_ShouldCorrectlyPaginateData()
        {
            var options = CreateInMemoryDatabaseOptions();
            using var context = new FakeDbContext(options);
            SeedTestData(context, 50);  // Assume this adds 50 test items

            var paginator = new CursorPaginator<ProductTests, int>(x => x.Id);
            var pageSize = 10;
            var currentCursor = 20;  // Assuming the cursor is on the item with ID 20
            var paginationResult = paginator.Paginate(context.Products, new CursorPaginationParameters<int> { PageSize = pageSize, CurrentCursor = currentCursor });

            Assert.NotNull(paginationResult);
            Assert.Equal(pageSize, paginationResult.Items.Count());
            // Assuming the items are ordered by ID, the next item should have ID 21
            Assert.Equal(21, paginationResult.Items.First().Id);
        }

        [Fact]
        public void Paginate_ShouldHandleEmptySource()
        {
            var options = CreateInMemoryDatabaseOptions();
            using var context = new FakeDbContext(options);

            var paginator = new CursorPaginator<ProductTests, int>(x => x.Id);
            var paginationResult = paginator.Paginate(context.Products, new CursorPaginationParameters<int> { PageSize = 10, CurrentCursor = 0 });

            Assert.NotNull(paginationResult);
            Assert.Empty(paginationResult.Items);
        }

        [Fact]
        public async Task PaginateAsync_ShouldCorrectlyPaginateData()
        {
            var options = CreateInMemoryDatabaseOptions();
            using var context = new FakeDbContext(options);
            SeedTestData(context, 50);

            var paginator = new CursorPaginator<ProductTests, int>(x => x.Id);
            var pageSize = 10;
            var currentCursor = 20;
            var paginationResult = await paginator.PaginateAsync(context.Products, new CursorPaginationParameters<int> { PageSize = pageSize, CurrentCursor = currentCursor });

            Assert.NotNull(paginationResult);
            Assert.Equal(pageSize, paginationResult.Items.Count());
            Assert.Equal(21, paginationResult.Items.First().Id);
        }

        [Fact]
        public async Task PaginateAsync_ShouldHandleEmptySource()
        {
            var options = CreateInMemoryDatabaseOptions();
            using var context = new FakeDbContext(options);

            var paginator = new CursorPaginator<ProductTests, int>(x => x.Id);
            var paginationResult = await paginator.PaginateAsync(context.Products, new CursorPaginationParameters<int> { PageSize = 10, CurrentCursor = 0 });

            Assert.NotNull(paginationResult);
            Assert.Empty(paginationResult.Items);
        }
    }

}
