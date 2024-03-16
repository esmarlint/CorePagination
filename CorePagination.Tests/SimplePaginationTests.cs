using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CorePagination.Tests.DatabaseContexts;
using CorePagination.Extensions;

namespace CorePagination.Tests;

public class PaginationTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public PaginationTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        using var context = new ApplicationDbContext(_options);
        if (!context.Products.Any())
        {
            for (int i = 1; i <= 50; i++)
            {
                context.Products.Add(new CorePagination.Tests.Models.ProductTests { Id = i, Name = $"Product {i}" });
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
}