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
    }
}
