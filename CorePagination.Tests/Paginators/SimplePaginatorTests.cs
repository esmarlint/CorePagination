using System.Linq;
using Xunit;
using Microsoft.EntityFrameworkCore;
using CorePagination.Extensions;
using System.Threading.Tasks;
using FakeDbContext = CorePagination.Tests.Support.DatabaseContexts.ApplicationDbContext;
using Product = CorePagination.Tests.Support.Models.ProductTests;
using CorePagination.Paginators.SimplePaginator;
using CorePagination.Paginators.Common;
using CorePagination.Tests.Support.Seeds;
using CorePagination.Tests.Support.Models;
using CorePagination.Tests.Support.Utils;

namespace CorePagination.Tests.Paginators;

public class SimplePaginatorTests
{
    #region Paginate
    [Fact]
    public void Paginate_ShouldCorrectlyPaginateData()
    {
        using var context = DatabaseSupport.SetupTestDatabase(50);
        var data = context.Products;

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
        using var context = DatabaseSupport.SetupTestDatabase(10);

        var paginator = new SimplePaginator<ProductTests>();
        var parameters = new PaginatorParameters { Page = 5, PageSize = 10 };
        var paginationResult = paginator.Paginate(context.Products, parameters);

        Assert.NotNull(paginationResult);
        Assert.Empty(paginationResult.Items);
    }

    [Fact]
    public void Paginate_ShouldHandleEmptySource()
    {
        using var context = DatabaseSupport.SetupTestDatabase(0);

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
        using var context = DatabaseSupport.SetupTestDatabase(10);

        var paginator = new SimplePaginator<ProductTests>();
        var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };

        Assert.Throws<ArgumentOutOfRangeException>(() => paginator.Paginate(context.Products, parameters));
    }

    #endregion Paginate

    #region PaginateAsync
    [Fact]
    public async Task PaginateAsync_ShouldCorrectlyPaginateData()
    {
        using var context = DatabaseSupport.SetupTestDatabase(50);

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
        using var context = DatabaseSupport.SetupTestDatabase(10);

        var paginator = new SimplePaginator<ProductTests>();
        var parameters = new PaginatorParameters { Page = pageNumber, PageSize = pageSize };

        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => paginator.PaginateAsync(context.Products, parameters));
    }

    [Fact]
    public async Task PaginateAsync_ShouldHandleEmptySource()
    {
        using var context = DatabaseSupport.SetupTestDatabase(0);

        var paginator = new SimplePaginator<ProductTests>();
        var parameters = new PaginatorParameters { Page = 1, PageSize = 10 };
        var paginationResult = await paginator.PaginateAsync(context.Products, parameters);

        Assert.NotNull(paginationResult);
        Assert.Empty(paginationResult.Items);
    }

    #endregion PaianteAsync
}