using CorePagination.Extensions;
using CorePagination.Tests.Models;
using CorePagination.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FakeDbContext = CorePagination.Tests.DatabaseContexts.ApplicationDbContext;

namespace CorePagination.Tests.Extensions
{
    public class PaginatorExtensionsTests
    {

        private static DbContextOptions<FakeDbContext> CreateInMemoryDatabaseOptions()
        {
            string databaseName = $"TestDatabase_{Guid.NewGuid()}";
            return new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
        }

        private static void SeedTestData(FakeDbContext context)
        {
            ProductSeeder.SeedProducts(context, 30);
            context.SaveChanges();
        }

        #region SimplePaginate

        [Fact]
        public void SimplePaginate_ShouldThrowArgumentNullException_WhenQueryIsNull()
        {
            var pageNumber = 1;
            var pageSize = 10;

            Assert.Throws<ArgumentNullException>(() => PaginatorExtensions.SimplePaginate<object>(null, pageNumber, pageSize));
        }

        [Theory]
        [InlineData(0, 10)] // pageNumber is less than 1
        [InlineData(1, 0)]  // pageSize is less than 1
        public void SimplePaginate_ShouldThrowArgumentOutOfRangeException_WhenPageNumberOrPageSizeIsInvalid(int pageNumber, int pageSize)
        {
            var query = new List<object>().AsQueryable();

            Assert.Throws<ArgumentOutOfRangeException>(() => query.SimplePaginate(pageNumber, pageSize));
        }

        [Fact]
        public void SimplePaginate_ShouldPaginateCorrectly()
        {
            var options = CreateInMemoryDatabaseOptions();
            using var context = new FakeDbContext(options);
            SeedTestData(context);

            var data = context.Products;
            var pageNumber = 2;
            var pageSize = 10;

            var result = data.SimplePaginate(pageNumber, pageSize);

            Assert.Equal(pageSize, result.Items.Count());
            Assert.Equal(11, result.Items.First().Id); // Check that pagination starts correctly on page 2
        }

        [Fact]
        public async Task SimplePaginateAsync_ShouldThrowArgumentNullException_WhenQueryIsNull()
        {
            var pageNumber = 1;
            var pageSize = 10;

            await Assert.ThrowsAsync<ArgumentNullException>(() => PaginatorExtensions.SimplePaginateAsync<object>(null, pageNumber, pageSize));
        }

        [Theory]
        [InlineData(0, 10)] // pageNumber is less than 1
        [InlineData(1, 0)]  // pageSize is less than 1
        public async Task SimplePaginateAsync_ShouldThrowArgumentOutOfRangeException_WhenPageNumberOrPageSizeIsInvalid(int pageNumber, int pageSize)
        {
            var query = new List<object>().AsQueryable();

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => query.SimplePaginateAsync(pageNumber, pageSize));
        }

        [Fact]
        public async Task SimplePaginateAsync_ShouldPaginateCorrectly()
        {
            var options = CreateInMemoryDatabaseOptions();
            using var context = new FakeDbContext(options);
            SeedTestData(context);
            var data = context.Products;
            var pageNumber = 2;
            var pageSize = 10;

            var result = await data.SimplePaginateAsync(pageNumber, pageSize);

            Assert.Equal(pageSize, result.Items.Count());
            Assert.Equal(11, result.Items.First().Id);// Check that pagination starts correctly on page 2
        }

        #endregion SimplePaginate
    }
}
