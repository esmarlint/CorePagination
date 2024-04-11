using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using CorePagination.Paginators.SizeAwarePaginator;
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
    public class SizeAwareUrlResultTransformer
    {
        [Fact]
        public void SizeAwareUrlResultTransformer_Transform_WithIncludeTotalItems_ReturnsUrlPaginationResultWithTotalItems()
        {
            // Arrange
            var context = DatabaseSupport.SetupTestDatabase(10);
            var products = context.Products;
            var paginationResult = new Mock<IPaginationResult<UrlPaginationResult<ProductTests>>>();
            paginationResult.SetupGet(x => x.Items).Returns(products.Select(e => new UrlPaginationResult<ProductTests>()));
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
            paginationResult.SetupGet(x => x.Items).Returns(products.Select(e => new SizeAwarePaginationResult<ProductTests>()));
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
    }
}
