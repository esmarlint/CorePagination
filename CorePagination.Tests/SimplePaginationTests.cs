using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CorePagination.Tests.DatabaseContexts;
using CorePagination;

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
                context.Products.Add(new CorePagination.Tests.Models.ProductTets { Id = i, Name = $"Product {i}" });
            }
            context.SaveChanges();
        }
    }

}