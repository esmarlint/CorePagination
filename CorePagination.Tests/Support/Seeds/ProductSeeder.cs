﻿using CorePagination.Tests.Support.DatabaseContexts;
using CorePagination.Tests.Support.Models;
using System.Linq;

namespace CorePagination.Tests.Support.Seeds
{
    public static class ProductSeeder
    {
        public static void SeedProducts(ApplicationDbContext context, int count)
        {
            context.Products.AddRange(Enumerable.Range(1, count).Select(i => new ProductTests
            {
                Id = i,
                Name = $"Product {i}",
                Price = i * 10
            }));
            context.SaveChanges();
        }
    }
}