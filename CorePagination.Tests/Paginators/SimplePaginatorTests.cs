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
        return new DbContextOptionsBuilder<FakeDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
    }

    private static void SeedTestData(FakeDbContext context)
    {
        ProductSeeder.SeedProducts(context, 20);
    }

    [Fact]
    public async Task SimplePaginator_Paginate_ReturnsCorrectPage()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        using var context = new FakeDbContext(options);
        SeedTestData(context);

        var paginator = new SimplePaginator<Product>();
        var parameters = new PaginatorParameters { Page = 2, PageSize = 5 };

        // Act
        var result = await paginator.PaginateAsync(context.Products, parameters);

        // Assert
        Assert.Equal(5, result.Items.Count());
        Assert.Equal(2, result.Page);
        Assert.Equal(5, result.PageSize);
        Assert.Equal(6, result.Items.First().Id);
        Assert.Equal(10, result.Items.Last().Id);
    }

    [Fact]
    public async Task SimplePaginator_Paginate_ReturnsEmptyPage_WhenPageExceedsTotal()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        using var context = new FakeDbContext(options);
       

        var paginator = new SimplePaginator<Product>();
        var parameters = new PaginatorParameters { Page = 5, PageSize = 5 };

        // Act
        var result = await paginator.PaginateAsync(context.Products, parameters);

        // Assert
        Assert.Empty(result.Items);
        Assert.Equal(5, result.Page);
        Assert.Equal(5, result.PageSize);
    }

    [Fact]
    public async Task SimplePaginator_Paginate_ThrowsArgumentNullException_WhenQueryIsNull()
    {
        // Arrange
        var paginator = new SimplePaginator<Product>();
        var parameters = new PaginatorParameters { Page = 1, PageSize = 5 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => paginator.PaginateAsync(null, parameters));
    }

    [Fact]
    public async Task SimplePaginator_Paginate_ReturnsFirstPage_WhenPageIsZero()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        using var context = new FakeDbContext(options);
        SeedTestData(context);

        var paginator = new SimplePaginator<Product>();
        var parameters = new PaginatorParameters { Page = 0, PageSize = 5 };

        // Act
        var result = await paginator.PaginateAsync(context.Products, parameters);

        // Assert
        Assert.Equal(5, result.Items.Count());
        Assert.Equal(1, result.Page);
        Assert.Equal(5, result.PageSize);
        Assert.Equal(1, result.Items.First().Id);
        Assert.Equal(5, result.Items.Last().Id);
    }
}