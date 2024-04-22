using CorePagination.Contracts;
using CorePagination.Paginators.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePagination.Tranformation.Extensions;

namespace CorePagination.Tests.Transformation.Extensions
{
    public class PaginationTransformationExtensionsTest
    {
        [Fact]
        public void Transform_WithValidPaginationResult_ShouldApplyTransformation()
        {
            // Arrange
            var paginationResult = new PaginationResult<int>
            {
                Items = new[] { 1, 2, 3 },
                Page = 1,
                PageSize = 10,
                TotalItems = 3
            };

            // Act
            var transformedResult = paginationResult.Transform(result => new
            {
                Numbers = result.Items,
                result.Page,
                result.PageSize,
                Total = result.TotalItems
            });

            // Assert
            Assert.Equal(1, transformedResult.Page);
            Assert.Equal(10, transformedResult.PageSize);
            Assert.Equal(3, transformedResult.Total);
            Assert.Equal(new[] { 1, 2, 3 }, transformedResult.Numbers);
        }

        [Fact]
        public void Transform_WithNullPaginationResult_ShouldThrowArgumentNullException()
        {
            // Arrange
            IPaginationResult<int> paginationResult = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => paginationResult.Transform(result => new { }));
        }

        [Fact]
        public void TransformWithItems_WithValidPaginationResult_ShouldApplyTransformationAndIncludeItems()
        {
            // Arrange
            var paginationResult = new PaginationResult<int>
            {
                Items = new[] { 1, 2, 3 },
                Page = 1,
                PageSize = 10,
                TotalItems = 3
            };

            // Act
            var transformedResult = paginationResult.TransformWithItems(result => new PaginationResult<int>
            {
                Page = result.Page,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems
            });

            // Assert
            Assert.Equal(1, transformedResult.Page);
            Assert.Equal(10, transformedResult.PageSize);
            Assert.Equal(3, transformedResult.TotalItems);
            Assert.Equal(new[] { 1, 2, 3 }, transformedResult.Items);
        }

        [Fact]
        public void TransformWithItems_WithNullPaginationResult_ShouldThrowArgumentNullException()
        {
            // Arrange
            IPaginationResult<int> paginationResult = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => paginationResult.TransformWithItems(result => new PaginationResult<int>()));
        }
    }

}
