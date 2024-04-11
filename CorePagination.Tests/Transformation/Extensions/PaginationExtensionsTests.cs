﻿using CorePagination.Contracts;
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

namespace CorePagination.Tranformation.Transformers;
public class TransformersTests
{

    [Fact]
    public void SimpleUrlResultTransformer_Transform_WithValidPaginationResult_ReturnsUrlPaginationResult()
    {
        // Arrange
        var context = DatabaseSupport.SetupTestDatabase(10);
        var products = context.Products;
        var paginationResult = new Mock<IPaginationResult<UrlPaginationResult<ProductTests>>>();
        paginationResult.SetupGet(x => x.Items).Returns(products.Select(e=> new UrlPaginationResult<ProductTests>()));
        paginationResult.SetupGet(x => x.Page).Returns(1);
        paginationResult.SetupGet(x => x.PageSize).Returns(10);
        paginationResult.SetupGet(x => x.TotalItems).Returns(100);

        var transformer = new SimpleUrlResultTransformer<UrlPaginationResult<ProductTests>>();

        // Act
        var result = transformer.Transform(paginationResult.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Items.Count());
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(100, result.TotalItems);
    }

    [Fact]
    public void SimpleUrlResultTransformer_Transform_WithNullPaginationResult_ThrowsArgumentNullException()
    {
        // Arrange
        var transformer = new SimpleUrlResultTransformer<UrlPaginationResult<ProductTests>>();

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => transformer.Transform(null));
    }

    [Fact]
    public void SizeAwareUrlResultTransformer_Transform_WithValidPaginationResult_ReturnsUrlPaginationResult()
    {
        // Arrange
        var context = DatabaseSupport.SetupTestDatabase(10);
        var products = context.Products;
        var paginationResult = new Mock<IPaginationResult<UrlPaginationResult<ProductTests>>>();
        paginationResult.SetupGet(x => x.Items).Returns(products.Select(e=>new UrlPaginationResult<ProductTests>()));
        paginationResult.SetupGet(x => x.Page).Returns(1);
        paginationResult.SetupGet(x => x.PageSize).Returns(10);
        paginationResult.SetupGet(x => x.TotalItems).Returns(100);

        var transformer = new SizeAwareUrlResultTransformer<UrlPaginationResult<ProductTests>>("");

        // Act
        var result = transformer.Transform(paginationResult.Object);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(10, result.Items.Count());
        Assert.Equal(1, result.Page);
        Assert.Equal(10, result.PageSize);
        Assert.Equal(100, result.TotalItems);
    }

    [Fact]
    public void SizeAwareUrlResultTransformer_Transform_WithNullPaginationResult_ThrowsArgumentNullException()
    {
        // Arrange
        var transformer = new SizeAwareUrlResultTransformer<UrlPaginationResult<ProductTests>> ("");

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => transformer.Transform(null));
    }

    [Fact]
    public void SizeAwareUrlResultTransformer_Transform_WithIncludeTotalItems_ReturnsUrlPaginationResultWithTotalItems()
    {
        // Arrange
        var context = DatabaseSupport.SetupTestDatabase(10);
        var products = context.Products;
        var paginationResult = new Mock<IPaginationResult<UrlPaginationResult<ProductTests>>>();
        paginationResult.SetupGet(x => x.Items).Returns(products.Select(e => new UrlPaginationResult<ProductTests>() ));
        paginationResult.SetupGet(x => x.Page).Returns(1);
        paginationResult.SetupGet(x => x.PageSize).Returns(10);
        paginationResult.SetupGet(x => x.TotalItems).Returns(100);

        var transformer = new SizeAwareUrlResultTransformer<UrlPaginationResult<ProductTests>>("")
            .IncludeTotalItems();

        // Act
        var result = transformer.Transform(paginationResult.Object);

        // Assert
        Assert.Equal(100, result.TotalItems);
    }

    [Fact]
    public void SizeAwareUrlResultTransformer_Transform_WithIncludeTotalPages_ReturnsUrlPaginationResultWithTotalPages()
    {
        // Arrange
        var context = DatabaseSupport.SetupTestDatabase(10);
        var products = context.Products;
        var paginationResult = new Mock<IPaginationResult<SizeAwarePaginationResult<ProductTests>>>();
        paginationResult.SetupGet(x => x.Items).Returns(products.Select(e=>new SizeAwarePaginationResult<ProductTests>()));
        paginationResult.SetupGet(x => x.Page).Returns(1);
        paginationResult.SetupGet(x => x.PageSize).Returns(10);
        paginationResult.SetupGet(x => x.TotalItems).Returns(100);

        var transformer = new SizeAwareUrlResultTransformer<SizeAwarePaginationResult<ProductTests>>("")
            .IncludeTotalPages();

        // Act
        var result = transformer.Transform(paginationResult.Object);

        //Assert
        Assert.Equal(10, result.TotalItems / result.PageSize);
    }

    //[Fact]
    //public void CursorUrlResultTransformer_Transform_WithValidCursorPaginationResult_ReturnsCursorUrlPaginationResult()
    //{
    //    // Arrange
    //    var paginationResult = new CursorPaginationResult<int, int>
    //    {
    //        Items = Enumerable.Range(1, 10),
    //        CurrentCursor = 1,
    //        NextCursor = 11,
    //        HasMore = true
    //    };

    //    var transformer = new CursorUrlResultTransformer<int, int>("");

    //    // Act
    //    var result = transformer.Transform(paginationResult);

    //    // Assert
    //    Assert.NotNull(result);
    //    Assert.Equal(10, result.Items.Count());
    //    Assert.Equal(1, result.CurrentCursor);
    //    Assert.Equal(11, result.NextCursor);
    //    Assert.True(result.HasMore);
    //}

    //[Fact]
    //public void CursorUrlResultTransformer_Transform_WithInvalidPaginationResult_ThrowsArgumentException()
    //{
    //    // Arrange
    //    var paginationResult = new Mock<IPaginationResult<int>>();
    //    var transformer = new CursorUrlResultTransformer<int, int>("");

    //    // Act & Assert
    //    Assert.Throws<ArgumentException>(() => transformer.Transform(paginationResult.Object));
    //}

    //[Fact]
    //public void CursorUrlResultTransformer_Transform_WithIncludeCurrentCursor_ReturnsCursorUrlPaginationResultWithCurrentCursor()
    //{
    //    // Arrange
    //    var paginationResult = new CursorPaginationResult<int, int>
    //    {
    //        Items = Enumerable.Range(1, 10),
    //        CurrentCursor = 1,
    //        NextCursor = 11,
    //        HasMore = true
    //    };

    //    var transformer = new CursorUrlResultTransformer<int, int>("")
    //        .IncludeCurrentCursor();

    //    // Act
    //    var result = transformer.Transform(paginationResult);

    //    // Assert
    //    Assert.Equal(1, result.CurrentCursor);
    //}

    //[Fact]
    //public void CursorUrlResultTransformer_Transform_WithIncludeNextCursor_ReturnsCursorUrlPaginationResultWithNextCursor()
    //{
    //    // Arrange
    //    var paginationResult = new CursorPaginationResult<int, int>
    //    {
    //        Items = Enumerable.Range(1, 10),
    //        CurrentCursor = 1,
    //        NextCursor = 11,
    //        HasMore = true
    //    };

    //    var transformer = new CursorUrlResultTransformer<int, int>("")
    //        .IncludeNextCursor();

    //    // Act
    //    var result = transformer.Transform(paginationResult);

    //    // Assert
    //    Assert.Equal(11, result.NextCursor);
    //}
}