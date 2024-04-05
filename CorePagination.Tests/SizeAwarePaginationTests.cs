using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CorePagination.Tests.DatabaseContexts;
using CorePagination.Extensions;

namespace CorePagination.Tests.Paginators;

public class SizeAwarePaginationTests
{
    private readonly DbContextOptions<ApplicationDbContext> _options;

    public SizeAwarePaginationTests()
    {
        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabaseForSizeAware")
            .Options;

        using var context = new ApplicationDbContext(_options);
        if (!context.Products.Any())
        {
            for (int i = 1; i <= 100; i++)
            {
                context.Products.Add(new Models.ProductTests { Id = i, Name = $"Product {i}" });
            }
            context.SaveChanges();
        }
    }

    [Fact]
    public void TestSizeAwarePaginate_ReturnsExpectedResults()
    {
        using var context = new ApplicationDbContext(_options);

        var pageNumber = 2;
        var pageSize = 10;

        var expectedTotalCount = 100;
        var expectedFirstItemId = 11;

        var paginatedProducts = context.Products.Paginate(pageNumber, pageSize);

        Assert.NotNull(paginatedProducts);

        Assert.Equal(pageSize, paginatedProducts.Items.Count());
        Assert.Equal(expectedTotalCount, paginatedProducts.TotalItems);
        Assert.Equal(expectedFirstItemId, paginatedProducts.Items.First().Id);
    }
}
