using System.Linq;
using System.Threading.Tasks;
using CorePagination.Paginators.Common;
using CorePagination.Paginators.SizeAwarePaginator;
using CorePagination.Tests.Seeds;
using Microsoft.EntityFrameworkCore;
using FakeDbContext = CorePagination.Tests.DatabaseContexts.ApplicationDbContext;
using Xunit;
using CorePagination.Tests.Models;

public class SizeAwarePaginatorTests
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
    public async Task Paginate_ReturnsCorrectPageAndItemCount()
    {
        // Arrange
        var options = CreateInMemoryDatabaseOptions();
        using var context = new FakeDbContext(options);
        SeedTestData(context);

        var paginator = new SizeAwarePaginator<ProductTests>(); 
        int pageNumber = 2;
        int pageSize = 5;

        // Act
        var queryableData = context.Products.AsQueryable(); 
        var result = await paginator.PaginateAsync(queryableData, new PaginatorParameters { Page = pageNumber, PageSize = pageSize });

        // Assert
        Assert.Equal(pageSize, result.Items.Count());
        Assert.Equal(20, result.TotalItems);
        Assert.Equal(4, result.TotalPages); 

    }
}
