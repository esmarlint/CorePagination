using CorePagination.Extensions;
using CorePagination.Tests.Support.Models;
using CorePagination.Tests.Support.Seeds;
using CorePagination.Tests.Support.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FakeDbContext = CorePagination.Tests.Support.DatabaseContexts.ApplicationDbContext;

namespace CorePagination.Tests.Extensions
{
    public class PaginatorExtensionsTests
    {

        #region SimplePaginate

        [Fact]
        public void SimplePaginate_ShouldThrowArgumentNullException_WhenQueryIsNull()
        {
            var pageNumber = 1;
            var pageSize = 10;

            Assert.Throws<ArgumentNullException>(() => PaginatorExtensions.SimplePaginate<object>(null, pageNumber, pageSize));
        }

        [Theory]
        [InlineData(0, 10)] // pageNumber is less than 1
        [InlineData(1, 0)]  // pageSize is less than 1
        public void SimplePaginate_ShouldThrowArgumentOutOfRangeException_WhenPageNumberOrPageSizeIsInvalid(int pageNumber, int pageSize)
        {
            var query = new List<object>().AsQueryable();

            Assert.Throws<ArgumentOutOfRangeException>(() => query.SimplePaginate(pageNumber, pageSize));
        }

        [Fact]
        public void SimplePaginate_ShouldPaginateCorrectly()
        {
            using var context = DatabaseSupport.SetupTestDatabase(20);

            var data = context.Products;
            var pageNumber = 2;
            var pageSize = 10;

            var result = data.SimplePaginate(pageNumber, pageSize);

            Assert.Equal(pageSize, result.Items.Count());
            Assert.Equal(11, result.Items.First().Id); // Check that pagination starts correctly on page 2
        }

        [Fact]
        public async Task SimplePaginateAsync_ShouldThrowArgumentNullException_WhenQueryIsNull()
        {
            var pageNumber = 1;
            var pageSize = 10;

            await Assert.ThrowsAsync<ArgumentNullException>(() => PaginatorExtensions.SimplePaginateAsync<object>(null, pageNumber, pageSize));
        }

        [Theory]
        [InlineData(0, 10)] // pageNumber is less than 1
        [InlineData(1, 0)]  // pageSize is less than 1
        public async Task SimplePaginateAsync_ShouldThrowArgumentOutOfRangeException_WhenPageNumberOrPageSizeIsInvalid(int pageNumber, int pageSize)
        {
            var query = new List<object>().AsQueryable();

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => query.SimplePaginateAsync(pageNumber, pageSize));
        }

        [Fact]
        public async Task SimplePaginateAsync_ShouldPaginateCorrectly()
        {
            using var context = DatabaseSupport.SetupTestDatabase(30);
            var data = context.Products;
            var pageNumber = 2;
            var pageSize = 10;

            var result = await data.SimplePaginateAsync(pageNumber, pageSize);

            Assert.Equal(pageSize, result.Items.Count());
            Assert.Equal(11, result.Items.First().Id);// Check that pagination starts correctly on page 2
        }

        #endregion SimplePaginate

        #region Paginate

        [Fact]
        public void Paginate_ShouldThrowArgumentNullException_WhenQueryIsNull()
        {
            var pageNumber = 1;
            var pageSize = 10;

            Assert.Throws<ArgumentNullException>(() => PaginatorExtensions.Paginate<object>(null, pageNumber, pageSize));
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(1, 0)]
        public void Paginate_ShouldThrowArgumentOutOfRangeException_WhenPageNumberOrPageSizeIsInvalid(int pageNumber, int pageSize)
        {
            var query = new List<object>().AsQueryable();

            Assert.Throws<ArgumentOutOfRangeException>(() => query.Paginate(pageNumber, pageSize));
        }

        [Fact]
        public void Paginate_ShouldPaginateCorrectly()
        {
            using var context = DatabaseSupport.SetupTestDatabase(20);
            var data = context.Products;
            var pageNumber = 2;
            var pageSize = 10;

            var result = data.Paginate(pageNumber, pageSize);

            Assert.Equal(pageSize, result.Items.Count());
            Assert.Equal(20, result.TotalItems);
            Assert.Equal(2, result.TotalPages);
            Assert.Equal(11, result.Items.First().Id);
        }

        [Fact]
        public async Task PaginateAsync_ShouldThrowArgumentNullException_WhenQueryIsNull()
        {
            var pageNumber = 1;
            var pageSize = 10;

            await Assert.ThrowsAsync<ArgumentNullException>(() => PaginatorExtensions.PaginateAsync<object>(null, pageNumber, pageSize));
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(1, 0)]
        public async Task PaginateAsync_ShouldThrowArgumentOutOfRangeException_WhenPageNumberOrPageSizeIsInvalid(int pageNumber, int pageSize)
        {
            var query = new List<object>().AsQueryable();

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => query.PaginateAsync(pageNumber, pageSize));
        }

        [Fact]
        public async Task PaginateAsync_ShouldPaginateCorrectly()
        {
            using var context = DatabaseSupport.SetupTestDatabase(50);
            var data = context.Products;
            var pageNumber = 2;
            var pageSize = 10;

            var result = await data.PaginateAsync(pageNumber, pageSize);

            Assert.Equal(pageSize, result.Items.Count());
            Assert.Equal(50, result.TotalItems);
            Assert.Equal(5, result.TotalPages);
            Assert.Equal(11, result.Items.First().Id);
        }


        #endregion Paginate

        #region CursorPaginate
        [Fact]
        public void CursorPaginate_ShouldThrowArgumentNullException_WhenQueryIsNull()
        {
            Expression<Func<ProductTests, int>> keySelector = x => x.Id;
            var pageSize = 10;
            var currentCursor = 1;

            Assert.Throws<ArgumentNullException>(() => PaginatorExtensions.CursorPaginate<ProductTests, int>(null, keySelector, pageSize, currentCursor));
        }

        [Fact]
        public void CursorPaginate_ShouldThrowArgumentNullException_WhenKeySelectorIsNull()
        {
            var query = new List<ProductTests>().AsQueryable();
            var pageSize = 10;
            var currentCursor = 1;

            Assert.Throws<ArgumentNullException>(() => query.CursorPaginate<ProductTests, int>(null, pageSize, currentCursor));
        }

        [Fact]
        public void CursorPaginate_ShouldThrowArgumentOutOfRangeException_WhenPageSizeIsInvalid()
        {
            var query = new List<ProductTests>().AsQueryable();
            Expression<Func<ProductTests, int>> keySelector = x => x.Id;
            var currentCursor = 1;

            Assert.Throws<ArgumentOutOfRangeException>(() => query.CursorPaginate(keySelector, 0, currentCursor));
        }

        [Fact]
        public void CursorPaginate_ShouldPaginateCorrectly()
        {
            var data = Enumerable.Range(1, 50).Select(x => new ProductTests { Id = x }).AsQueryable();
            Expression<Func<ProductTests, int>> keySelector = x => x.Id;
            var pageSize = 10;
            var currentCursor = 20;

            var result = data.CursorPaginate(keySelector, pageSize, currentCursor);

            Assert.Equal(pageSize, result.Items.Count());
            Assert.Equal(21, result.Items.First().Id);
        }

        #endregion CursorPaginate

        #region CursorPaginateAsync
        [Fact]
        public async Task CursorPaginateAsync_ShouldThrowArgumentNullException_WhenQueryIsNull()
        {
            Expression<Func<ProductTests, int>> keySelector = x => x.Id;
            var pageSize = 10;
            var currentCursor = 1;

            await Assert.ThrowsAsync<ArgumentNullException>(() => PaginatorExtensions.CursorPaginateAsync<ProductTests, int>(null, keySelector, pageSize, currentCursor));
        }

        [Fact]
        public async Task CursorPaginateAsync_ShouldThrowArgumentNullException_WhenKeySelectorIsNull()
        {
            var query = new List<ProductTests>().AsQueryable();
            var pageSize = 10;
            var currentCursor = 1;

            await Assert.ThrowsAsync<ArgumentNullException>(() => query.CursorPaginateAsync<ProductTests, int>(null, pageSize, currentCursor));
        }

        [Fact]
        public async Task CursorPaginateAsync_ShouldThrowArgumentOutOfRangeException_WhenPageSizeIsInvalid()
        {
            var query = new List<ProductTests>().AsQueryable();
            Expression<Func<ProductTests, int>> keySelector = x => x.Id;
            var currentCursor = 1;

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => query.CursorPaginateAsync(keySelector, 0, currentCursor));
        }

        [Fact]
        public async Task CursorPaginateAsync_ShouldPaginateCorrectly()
        {
            using var context = DatabaseSupport.SetupTestDatabase(50);
            var data = context.Products;
            Expression<Func<ProductTests, int>> keySelector = x => x.Id;
            var pageSize = 10;
            var currentCursor = 20; // Assuming cursor is on value 20

            var result = await data.CursorPaginateAsync(keySelector, pageSize, currentCursor);

            Assert.Equal(pageSize, result.Items.Count());
            Assert.Equal(21, result.Items.First().Id); // The first value should be 21, as the cursor was on 20
        }

        #endregion CursorPaginateAsync

        [Fact]
public void WithUrl_NullPaginationResult_ThrowsArgumentNullException()
{
    // Arrange
    CursorPaginationResult<ProductTests, int> paginationResult = null;
    var baseUrl = "https://example.com/products";

    // Act & Assert
    Assert.Throws<ArgumentNullException>(() => paginationResult.WithUrl<ProductTests, int>(baseUrl));
}

[Fact]
public void WithUrl_EmptyBaseUrl_ThrowsArgumentException()
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
    var baseUrl = string.Empty;

    // Act & Assert
    Assert.Throws<ArgumentException>(() => paginationResult.WithUrl<ProductTests, int>(baseUrl));
}

[Fact]
public void WithUrl_IncludeOnlyCurrentCursor_GeneratesCorrectUrl()
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
    Assert.Equal(baseUrl + "?currentCursor=1", result.CurrentUrl);
}
    }
}
