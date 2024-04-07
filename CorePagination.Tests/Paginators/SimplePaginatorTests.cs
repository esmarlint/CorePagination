using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CorePagination.Tests.DatabaseContexts;
using CorePagination.Extensions;
using CorePagination.Tests.Models;
using System.Threading.Tasks;
using FakeDbContext = CorePagination.Tests.DatabaseContexts.ApplicationDbContext;
using Product = CorePagination.Tests.Models.ProductTests;
using CorePagination.Tests.Seeds;
using CorePagination.Paginators.SimplePaginator;
using CorePagination.Paginators.Common;

namespace CorePagination.Tests.Paginators;

public class SimplePaginatorTests
{
    private static DbContextOptions<FakeDbContext> CreateInMemoryDatabaseOptions()
    {
        string databaseName = $"TestDatabase_{Guid.NewGuid()}";
        return new DbContextOptionsBuilder<FakeDbContext>()
            .UseInMemoryDatabase(databaseName)
            .Options;
    }

    private static void SeedTestData(FakeDbContext context, int amount)
    {
        ProductSeeder.SeedProducts(context, amount);
        context.SaveChanges();
    }

    #region Paginate
    [Fact]
    public void Paginate_ShouldCorrectlyPaginateData()
    {
        var options = CreateInMemoryDatabaseOptions();
        using var context = new FakeDbContext(options);
        var data = context.Products;
        SeedTestData(context, 50);

        var paginator = new SimplePaginator<ProductTests>();
        var pageSize = 10;
        var pageNumber = 2;
        var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };
        var paginationResult = paginator.Paginate(data, parameters);

        Assert.NotNull(paginationResult);
        Assert.Equal(pageSize, paginationResult.Items.Count());
        Assert.Equal(11, paginationResult.Items.First().Id);
    }

    [Fact]
    public void Paginate_ShouldReturnEmpty_WhenPageOutOfRange()
    {
        var options = CreateInMemoryDatabaseOptions();
        using var context = new FakeDbContext(options);
        SeedTestData(context, 10);

        var paginator = new SimplePaginator<ProductTests>();
        var parameters = new PaginatorParameters { Page = 5, PageSize = 10 };
        var paginationResult = paginator.Paginate(context.Products, parameters);

        Assert.NotNull(paginationResult);
        Assert.Empty(paginationResult.Items);
    }

    [Fact]
    public void Paginate_ShouldHandleEmptySource()
    {
        var options = CreateInMemoryDatabaseOptions();
        using var context = new FakeDbContext(options);

        var paginator = new SimplePaginator<ProductTests>();
        var parameters = new PaginatorParameters { Page = 1, PageSize = 10 };
        var paginationResult = paginator.Paginate(context.Products, parameters);

        Assert.NotNull(paginationResult);
        Assert.Empty(paginationResult.Items);
    }

    [Theory]
    [InlineData(-1, 10)] // Invalid number of page
    [InlineData(1, -10)] // Page size is invalid
    public void Paginate_ShouldThrowException_WhenGivenInvalidArguments(int pageNumber, int pageSize)
    {
        var options = CreateInMemoryDatabaseOptions();
        using var context = new FakeDbContext(options);
        SeedTestData(context, 10);

        var paginator = new SimplePaginator<ProductTests>();
        var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };

        Assert.Throws<ArgumentOutOfRangeException>(() => paginator.Paginate(context.Products, parameters));
    }

    #endregion Paginate

    #region PaginateAsync
    [Fact]
    public async Task PaginateAsync_ShouldCorrectlyPaginateData()
    {
        var options = CreateInMemoryDatabaseOptions();
        using var context = new FakeDbContext(options);
        SeedTestData(context, 50); 

        var paginator = new SimplePaginator<ProductTests>();
        var pageSize = 10;
        var pageNumber = 2;
        var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };
        var paginationResult = await paginator.PaginateAsync(context.Products, parameters);

        Assert.NotNull(paginationResult);
        Assert.Equal(pageSize, paginationResult.Items.Count());
        Assert.Equal(11, paginationResult.Items.First().Id);
    }

    [Theory]
    [InlineData(-1, 10)]  // Invalid page number
    [InlineData(1, -10)]  // Invalid page size
    public async Task PaginateAsync_ShouldThrowException_WhenGivenInvalidArguments(int pageNumber, int pageSize)
    {
        var options = CreateInMemoryDatabaseOptions();
        using var context = new FakeDbContext(options);
        SeedTestData(context, 10);

        var paginator = new SimplePaginator<ProductTests>();
        var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => paginator.PaginateAsync(context.Products, parameters));
    }

    [Fact]
    public async Task PaginateAsync_ShouldHandleEmptySource()
    {
        var options = CreateInMemoryDatabaseOptions();
        using var context = new FakeDbContext(options);

        var paginator = new SimplePaginator<ProductTests>();
        var parameters = new PaginatorParameters { Page = 1, PageSize = 10 };
        var paginationResult = await paginator.PaginateAsync(context.Products, parameters);

        Assert.NotNull(paginationResult);
        Assert.Empty(paginationResult.Items);
    }

    #endregion PaianteAsync
}