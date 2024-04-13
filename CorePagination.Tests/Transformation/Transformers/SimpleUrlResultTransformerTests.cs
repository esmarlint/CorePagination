using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using CorePagination.Tests.Support.Models;
using CorePagination.Tests.Support.Utils;
using CorePagination.Tranformation.Transformers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePagination.Tests.Transformation.Transformers
{
    public class SimpleUrlResultTransformerTests
    {
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
            paginationResult.SetupGet(x => x.Items).Returns(products.Select(e => new UrlPaginationResult<ProductTests>()));
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
            var transformer = new SizeAwareUrlResultTransformer<UrlPaginationResult<ProductTests>>("");

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => transformer.Transform(null));
        }

        [Fact]
        public void SimpleUrlResultTransformer_Transform_WithValidPaginationResult_ReturnsUrlPaginationResult()
        {
            // Arrange
            var context = DatabaseSupport.SetupTestDatabase(10);
            var products = context.Products;
            var paginationResult = new Mock<IPaginationResult<UrlPaginationResult<ProductTests>>>();
            paginationResult.SetupGet(x => x.Items).Returns(products.Select(e => new UrlPaginationResult<ProductTests>()));
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
        public void Transform_WithBaseUrl_GeneratesCorrectUrls()
        {
            // Arrange
            var paginationResult = new PaginationResult<ProductTests>
            {
                Items = new List<ProductTests> { new ProductTests(), new ProductTests() },
                Page = 2,
                PageSize = 10,
                TotalItems = 30
            };
            var transformer = new SimpleUrlResultTransformer<ProductTests>("https://example.com/products");

            // Act
            var result = transformer.Transform(paginationResult);

            // Assert
            Assert.Equal("https://example.com/products?page=1", result.FirstPageUrl);
            Assert.Equal("https://example.com/products?page=1", result.PreviousUrl);
            Assert.Equal("https://example.com/products?page=2", result.CurrentUrl);
            Assert.Equal("https://example.com/products?page=3", result.NextUrl);
        }

        [Fact]
        public void Transform_WithEmptyBaseUrl_GeneratesRelativeUrls()
        {
            // Arrange
            var paginationResult = new PaginationResult<ProductTests>
            {
                Items = new List<ProductTests> { new ProductTests(), new ProductTests() },
                Page = 1,
                PageSize = 10,
                TotalItems = 30
            };
            var transformer = new SimpleUrlResultTransformer<ProductTests>("");

            // Act
            var result = transformer.Transform(paginationResult);

            // Assert
            Assert.Equal("?page=1", result.FirstPageUrl);
            Assert.Null(result.PreviousUrl);
            Assert.Equal("?page=1", result.CurrentUrl);
            Assert.Equal("?page=2", result.NextUrl);
        }

        [Fact]
        public void Transform_WithAdditionalParameters_IncludesParametersInUrls()
        {
            // Arrange
            var paginationResult = new PaginationResult<ProductTests>
            {
                Items = new List<ProductTests> { new ProductTests(), new ProductTests() },
                Page = 1,
                PageSize = 10,
                TotalItems = 30
            };
            var transformer = new SimpleUrlResultTransformer<ProductTests>("https://example.com/products")
                .AddParameter("sort", "name")
                .AddParameter("order", "asc");

            // Act
            var result = transformer.Transform(paginationResult);

            // Assert
            Assert.Contains("sort=name", result.FirstPageUrl);
            Assert.Contains("order=asc", result.FirstPageUrl);
        }
    }
}
