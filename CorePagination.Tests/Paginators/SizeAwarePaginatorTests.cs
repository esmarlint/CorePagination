using System.Linq;
using System.Threading.Tasks;
using CorePagination.Paginators.Common;
using CorePagination.Paginators.SizeAwarePaginator;
using Microsoft.EntityFrameworkCore;
using FakeDbContext = CorePagination.Tests.Support.DatabaseContexts.ApplicationDbContext;
using Xunit;
using CorePagination.Tests.Support.Seeds;
using CorePagination.Tests.Support.Models;

namespace CorePagination.Tests.Paginators
{
    public class SizeAwarePaginatorTests
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

        #region Constructor and Initialization

        [Fact]
        public void Constructor_ShouldInitializeProperly()
        {
            var options = CreateInMemoryDatabaseOptions();
            using var context = new FakeDbContext(options);
            var paginator = new SizeAwarePaginator<ProductTests>();

            Assert.NotNull(paginator); // Verify paginator is created.
        }

        #endregion

        #region Paginate Method

        [Fact]
        public void Paginate_ShouldCorrectlyPaginateData()
        {
            var options = CreateInMemoryDatabaseOptions();
            using var context = new FakeDbContext(options);
            SeedTestData(context, 50);  // Assume this adds 50 test items

            var paginator = new SizeAwarePaginator<ProductTests>();
            var pageSize = 10;
            var pageNumber = 2;
            var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };
            var paginationResult = paginator.Paginate(context.Products, parameters);

            Assert.NotNull(paginationResult);
            Assert.Equal(pageSize, paginationResult.Items.Count());
            Assert.Equal(50, paginationResult.TotalItems);
            Assert.Equal(5, paginationResult.TotalPages);
            Assert.Equal(11, paginationResult.Items.First().Id);
        }

        [Fact]
        public void Paginate_ShouldHandleEmptySource()
        {
            var options = CreateInMemoryDatabaseOptions();
            using var context = new FakeDbContext(options);

            var paginator = new SizeAwarePaginator<ProductTests>();
            var parameters = new PaginatorParameters { Page = 1, PageSize = 10 };
            var paginationResult = paginator.Paginate(context.Products, parameters);

            Assert.NotNull(paginationResult);
            Assert.Empty(paginationResult.Items);
            Assert.Equal(0, paginationResult.TotalItems);
            Assert.Equal(0, paginationResult.TotalPages);
        }

        [Theory]
        [InlineData(-1, 10)]
        [InlineData(1, -10)]
        public void Paginate_ShouldThrowException_WhenGivenInvalidArguments(int pageNumber, int pageSize)
        {
            var options = CreateInMemoryDatabaseOptions();
            using var context = new FakeDbContext(options);
            SeedTestData(context, 10);

            var paginator = new SizeAwarePaginator<ProductTests>();
            var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };

            Assert.Throws<ArgumentOutOfRangeException>(() => paginator.Paginate(context.Products, parameters));
        }

        #endregion

        #region PaginateAsync Method

        [Fact]
        public async Task PaginateAsync_ShouldCorrectlyPaginateData()
        {
            var options = CreateInMemoryDatabaseOptions();
            using var context = new FakeDbContext(options);
            SeedTestData(context, 50);

            var paginator = new SizeAwarePaginator<ProductTests>();
            var pageSize = 10;
            var pageNumber = 2;
            var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };
            var paginationResult = await paginator.PaginateAsync(context.Products, parameters);

            Assert.NotNull(paginationResult);
            Assert.Equal(pageSize, paginationResult.Items.Count());
            Assert.Equal(50, paginationResult.TotalItems);
            Assert.Equal(5, paginationResult.TotalPages);
            Assert.Equal(11, paginationResult.Items.First().Id);
        }

        [Fact]
        public async Task PaginateAsync_ShouldHandleEmptySource()
        {
            var options = CreateInMemoryDatabaseOptions();
            using var context = new FakeDbContext(options);

            var paginator = new SizeAwarePaginator<ProductTests>();
            var parameters = new PaginatorParameters { Page = 1, PageSize = 10 };
            var paginationResult = await paginator.PaginateAsync(context.Products, parameters);

            Assert.NotNull(paginationResult);
            Assert.Empty(paginationResult.Items);
            Assert.Equal(0, paginationResult.TotalItems);
            Assert.Equal(0, paginationResult.TotalPages);
        }

        [Theory]
        [InlineData(-1, 10)]
        [InlineData(1, -10)]
        public async Task PaginateAsync_ShouldThrowException_WhenGivenInvalidArguments(int pageNumber, int pageSize)
        {
            var options = CreateInMemoryDatabaseOptions();
            using var context = new FakeDbContext(options);
            SeedTestData(context, 10);

            var paginator = new SizeAwarePaginator<ProductTests>();
            var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => paginator.PaginateAsync(context.Products, parameters));
        }

        #endregion
    }

}