using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CorePagination.Tests.DatabaseContexts;
using CorePagination.Extensions;
using CorePagination.Tests.Models;

namespace CorePagination.Tests;

public class PaginationTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public PaginationTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "ExtendedTestDatabaseForSimplePagination")
            .Options;

        using var context = new ApplicationDbContext(_options);
        if (!context.Products.Any())
        {
            for (int i = 1; i <= 35; i++) 
            {
                context.Products.Add(new ProductTests { Id = i, Name = $"Product {i}" });
            }
            context.SaveChanges();
        }
    }

    [Fact]
    public void TestSimplePaginate_ReturnsExpectedResults()
    {
        using var context = new ApplicationDbContext(_options);

        var pageNumber = 1;
        var pageSize = 10;
        var paginatedProducts = context.Products.SimplePaginate(pageNumber, pageSize);

        Assert.Equal(pageSize, paginatedProducts.Items.Count());
        Assert.Equal("Product 1", paginatedProducts.Items.First().Name);
        Assert.Equal("Product 10", paginatedProducts.Items.Last().Name);
    }

    [Fact]
    public void TestSimplePaginate_ReturnsFirstPageCorrectly()
    {
        using var context = new ApplicationDbContext(_options);

        var pageNumber = 1;
        var pageSize = 10;

        var paginatedProducts = context.Products.SimplePaginate(pageNumber, pageSize);

        Assert.NotEmpty(paginatedProducts.Items);
        Assert.Equal(pageSize, paginatedProducts.Items.Count());
        Assert.Equal(1, paginatedProducts.Items.First().Id);
    }

    [Fact]
    public void TestSimplePaginate_HandlesPartialPageCorrectly()
    {
        using var context = new ApplicationDbContext(_options);

        var pageNumber = 4; 
        var pageSize = 10;

        var paginatedProducts = context.Products.SimplePaginate(pageNumber, pageSize);

        Assert.NotEmpty(paginatedProducts.Items);
        Assert.Equal(5, paginatedProducts.Items.Count()); 
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void TestSimplePaginate_WithInvalidPageNumber(int pageNumber)
    {
        using var context = new ApplicationDbContext(_options);
        var pageSize = 10;

        var paginatedProducts = context.Products.SimplePaginate(pageNumber, pageSize);

        Assert.NotEmpty(paginatedProducts.Items);
        Assert.Equal(pageSize, paginatedProducts.Items.Count());
        Assert.Equal(1, paginatedProducts.Items.First().Id); 
    }

}