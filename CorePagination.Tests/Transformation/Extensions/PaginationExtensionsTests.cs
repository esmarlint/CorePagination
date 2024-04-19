using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using CorePagination.Paginators.CursorPaginator;
using CorePagination.Paginators.SizeAwarePaginator;
using CorePagination.Tests.Support.Models;
using CorePagination.Tests.Support.Utils;
using CorePagination.Tranformation.Transformers;
using Moq;
using System;
using System.Linq;
using Xunit;
using CorePagination.Tranformation.Extensions;

namespace CorePagination.Tests.Extensions;
public class TransformersTests
{

    [Fact]
    public void WithSimpleUrl_ReturnsUrlPaginationResult()
    {
        // Arrange
        var paginationResult = new PaginationResult<ProductTests>
        {
            Items = Enumerable.Range(1, 30).Select(product => new ProductTests()),
            Page = 1,
            PageSize = 10,
            TotalItems = 30
        };
        var baseUrl = "https://example.com/products";

        // Act
        var result = paginationResult.WithSimpleUrl(baseUrl);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UrlPaginationResult<ProductTests>>(result);
        Assert.Equal(baseUrl + "?page=1", result.FirstPageUrl);
        Assert.Equal(baseUrl + "?page=2", result.NextUrl);
    }

    [Fact]
    public void WithUrl_ReturnsUrlPaginationResult()
    {
        // Arrange
        var paginationResult = new PaginationResult<ProductTests>
        {
            Items = Enumerable.Range(1, 30).Select(product => new ProductTests()),
            Page = 1,
            PageSize = 10,
            TotalItems = 30
        };
        var baseUrl = "https://example.com/products";

        // Act
        var result = paginationResult.WithUrl(baseUrl);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<UrlPaginationResult<ProductTests>>(result);
        Assert.Equal(baseUrl + "?page=1&pageSize=10", result.FirstPageUrl);
        Assert.Equal(baseUrl + "?page=2&pageSize=10", result.NextPageUrl);
    }

    [Fact]
    public void WithUrl_ForCursorPagination_ReturnsCursorUrlPaginationResult()
    {
        // Arrange
        var paginationResult = new CursorPaginationResult<ProductTests, int>
        {
            Items = Enumerable.Range(1, 10).Select(product => new ProductTests()),
            CurrentCursor = 1,
            NextCursor = 11,
            PageSize = 10,
            Page = 1
        };
        var baseUrl = "https://example.com/products";

        // Act
        var result = paginationResult.WithUrl<ProductTests, int>(baseUrl);

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CursorUrlPaginationResult<ProductTests, int>>(result);
        Assert.Contains("currentCursor=1", result.CurrentUrl);
        Assert.Contains("nextCursor=11", result.CurrentUrl);
        Assert.Contains("page=1", result.CurrentUrl);
        Assert.Contains("pageSize=10", result.CurrentUrl);

    }

}